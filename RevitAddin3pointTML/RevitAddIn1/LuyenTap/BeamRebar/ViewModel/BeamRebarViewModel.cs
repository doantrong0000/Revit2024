using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.ThucChien.ColumnRebar.Model;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitAddIn1.LuyenTap.BeamRebar.View;
using System.Data.Common;
using RevitAddIn1.LuyenTap.BeamRebar.Model;

namespace RevitAddIn1.LuyenTap.BeamRebar.ViewModel
{
    public class BeamRebarViewModel : ObservableObject
    {
        public BeamRebarView BeamRebarView { get; set; }
        public int NumberOfXRebar
        {
            get => _numberOfXRebar;
            set
            {
                if (value == _numberOfXRebar) return;
                _numberOfXRebar = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfYRebar
        {
            get => _numberOfYRebar;
            set
            {
                if (value == _numberOfYRebar) return;
                _numberOfYRebar = value;
                OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        public RebarBarType XDiameter
        {
            get => _xDiameter;
            set
            {
                if (Equals(value, _xDiameter)) return;
                _xDiameter = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType YDiameter
        {
            get => _yDiameter;
            set
            {
                if (Equals(value, _yDiameter)) return;
                _yDiameter = value;
                OnPropertyChanged();
            }
        }

        public List<RebarBarType> Diameters
        {
            get => _diameters;
            set
            {
                if (Equals(value, _diameters)) return;
                _diameters = value;
                OnPropertyChanged();
            }
        }

        public int StrirrupSpacing
        {
            get => _strirrupSpacing;
            set
            {
                if (value == _strirrupSpacing) return;
                _strirrupSpacing = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType StrirrupDiameter
        {
            get => _strirrupDiameter;
            set
            {
                if (Equals(value, _strirrupDiameter)) return;
                _strirrupDiameter = value;
                OnPropertyChanged();
            }
        }

        public int D { get; set; } = 20;

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public RelayCommand LoadCadCmd { get; set; }

        private Document doc;
        private UIDocument uiDoc;
        private int _numberOfXRebar = 4;
        private int _numberOfYRebar = 4;
        private RebarBarType _xDiameter;
        private RebarBarType _yDiameter;
        private List<RebarBarType> _diameters;
        private int _strirrupSpacing = 200;
        private RebarBarType _strirrupDiameter;

        public double Cover { get; set; } = 30.MmToFeet();
        private BeamRebarModelY beamModel { get; set; }


        public BeamRebarViewModel(Document doc, UIDocument uiDoc)
        {
            this.uiDoc = uiDoc;
            this.doc = doc;
            LoadCadCmd = new RelayCommand(LoadCad);
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            Diameters = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .OrderBy(x => x.Name).ToList();

            XDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            YDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            StrirrupDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() < 12);
            LoadData();
        }
        void Run()
        {
            BeamRebarView.Close();
            try
            {
                var beam = uiDoc.Selection.PickObject(ObjectType.Element, new BeamSelectionFilter(), "'Select beam").ToElement() as FamilyInstance;

                beamModel = new BeamRebarModelY(beam);

            }
            catch (Exception e)
            {
                MessageBox.Show("Caceled");
                return;
            }



            using (var tx = new Transaction(doc, "Create pile"))
            {
                tx.Start();
                CreateStirrup();
                CreateXMainRebar();
                CreateYMainRebar();


                tx.Commit();
            }


        }

        void CreateStirrup()
        {

            var shaper = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .First(x => x.Name == "M_T6");

            double totalLength = beamModel.beamCurve.Length;

            var o1 = beamModel.D.Add(beamModel.YVector * Cover).Add(beamModel.ZVector * Cover);

            o1 = o1.Add(-beamModel.XVector * (totalLength / 2 + Cover));

            var rebar = Rebar.CreateFromRebarShape(doc, shaper, StrirrupDiameter, beamModel.Beam, o1, beamModel.XVector,
                beamModel.ZVector);

            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();

            shapeDrivenAccessor.ScaleToBox(o1, beamModel.YVector * (beamModel.Width - 2 * Cover), beamModel.ZVector * (beamModel.Height - 2 * Cover));


            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(StrirrupSpacing.MmToFeet(), totalLength - 2 * Cover - StrirrupDiameter.BarModelDiameter, true, true, true);


        }
        void CreateXMainRebar()
        {
            var spacing2Rebars = (beamModel.Width - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter) / (
            NumberOfXRebar - 1);


            // Top layer
            var topRebars = new List<Rebar>();

            for (int i = 0; i < NumberOfXRebar; i++)
            {
                var o2 = beamModel.A.Add(
                        beamModel.YVector * (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                    .Add(-beamModel.ZVector *
                         (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));
                var beamLength = beamModel.beamCurve.Length;

                o2 = o2.Add(-beamModel.XVector * (beamLength / 2 + Cover));
                o2 = o2.Add(beamModel.YVector * i * spacing2Rebars);


                var line = Line.CreateBound(o2, o2.Add(beamModel.XVector * beamLength));

                if (i % 2 == 0)
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, beamModel.Beam,
                        beamModel.YVector,
                        new List<Curve>() { line }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                    topRebars.Add(rebar);
                }
                else
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, beamModel.Beam,
                        beamModel.YVector,
                        new List<Curve>() { line }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                    topRebars.Add(rebar);
                }

            }

            ElementTransformUtils.CopyElements(doc, topRebars.Select(x => x.Id).ToList(), beamModel.ZVector * -1 * (beamModel.Height - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter));

        }

        void CreateYMainRebar()
        {
            var spacing2Rebars = (beamModel.Height - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter) / (
         NumberOfYRebar - 1);


            //Left layer 
            var leftRebars = new List<Rebar>();


            if (NumberOfYRebar > 2)
            {
                for (int i = 0; i < NumberOfYRebar; i++)
                {
                    if (i == 0 || i == NumberOfYRebar - 1)
                    {
                        continue;
                    }
                    var o2 = beamModel.A.Add(
                            beamModel.YVector * (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                        .Add(-beamModel.ZVector *
                             (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));
                    var beamLength = beamModel.beamCurve.Length;

                    o2 = o2.Add(beamModel.XVector * (beamLength / 2 + Cover));
                    o2 = o2.Add(beamModel.ZVector * -1 * i * spacing2Rebars);



                    var line = Line.CreateBound(o2, o2.Add(XYZ.BasisY * beamLength));


                    if (i % 2 == 0)
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, YDiameter, null, null, beamModel.Beam,
                            beamModel.ZVector,
                            new List<Curve>() { line }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                        leftRebars.Add(rebar);
                    }
                    else
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, YDiameter, null, null, beamModel.Beam,
                            beamModel.ZVector,
                            new List<Curve>() { line }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                        leftRebars.Add(rebar);
                    }
                }
            }
            ElementTransformUtils.CopyElements(doc, leftRebars.Select(x => x.Id).ToList(), beamModel.YVector * (beamModel.Width - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter));
        }
        void LoadCad()
        {

        }
        void Close()
        {

        }
        void SaveData()
        {

        }
        void LoadData()
        {

        }
    }
}
