using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using RevitAddIn1.SelectionFilter;

using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitAddIn1.ThucChien.BeamRebar2.View;
using System.Data.Common;
using RevitAddIn1.ThucChien.BeamRebar2.Model;

namespace RevitAddIn1.ThucChien.BeamRebar2.ViewModel
{
    public class BeamRebar2ViewModel : ObservableObject
    {
        public BeamRebar2View BeamRebar2View { get; set; }

    

        public int SoLuongThepChinh
        {
            get => _soLuongThepChinh;
            set
            {
                if (value == _soLuongThepChinh) return;
                _soLuongThepChinh = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType DuongKinhThepChinh
        {
            get => _duongKinhThepChinh1;
            set
            {
                if (Equals(value, _duongKinhThepChinh1)) return;
                _duongKinhThepChinh1 = value;
                OnPropertyChanged();
            }
        }

        public int SoLuongThepGiaCuongLopDuoi { get; set; } = 2;

        public RebarBarType DuongKinhThepGiaCuongLopDuoi
        {
            get => _duongKinhThepGiaCuongLopDuoi;
            set
            {
                if (Equals(value, _duongKinhThepGiaCuongLopDuoi)) return;
                _duongKinhThepGiaCuongLopDuoi = value;
                OnPropertyChanged();
            }
        }

        public int SoLuongThepGiaCuongLopTren { get; set; } = 2;

        public RebarBarType DuongKinhThepGiaCuongLopTren
        {
            get => _duongKinhThepGiaCuongLopTren;
            set
            {
                if (Equals(value, _duongKinhThepGiaCuongLopTren)) return;
                _duongKinhThepGiaCuongLopTren = value;
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
       
        private List<RebarBarType> _diameters;
        private int _strirrupSpacing = 200;
        private RebarBarType _strirrupDiameter;
        private int _soLuongThepChinh;
        private int _duongKinhThepChinh;
        private RebarBarType _duongKinhThepChinh1;
        private RebarBarType _duongKinhThepGiaCuongLopTren;
        private RebarBarType _duongKinhThepGiaCuongLopDuoi;

        public double Cover { get; set; } = 30.MmToFeet();
        private BeamRebarModel2 beamModel { get; set; }


        public BeamRebar2ViewModel(Document doc, UIDocument uiDoc)
        {
            this.uiDoc = uiDoc;
            this.doc = doc;

            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            Diameters = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .OrderBy(x => x.Name).ToList();

            DuongKinhThepChinh = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 14);
            DuongKinhThepGiaCuongLopDuoi = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 14);
            DuongKinhThepGiaCuongLopTren = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 14);
            StrirrupDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() < 12);
            LoadData();
        }
        void Run()
        {
            BeamRebar2View.Close();
            try
            {
                var beam = uiDoc.Selection.PickObject(ObjectType.Element, new BeamSelectionFilter(), "'Select beam")
                    .ToElement() as FamilyInstance;
                var columns = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_StructuralColumns).Cast<FamilyInstance>().ToList();

                beamModel = new BeamRebarModel2(beam, columns);

            }
            catch (Exception e)
            {
                MessageBox.Show("Caceled");
                return;
            }



            using (var tx = new Transaction(doc, "Create Rebar"))
            {
                tx.Start();
                CreateStirrup();
                CreateMainTopRebar();
                CreateMainBotRebar();
                tx.Commit();
            }


        }

        void CreateStirrup()
        {

            var shape = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .First(x => x.Name == "M_T1");

            var o1 = beamModel.StartPointTop.Add(beamModel.Direction * 50.MmToFeet()).Add(XYZ.BasisZ * -(beamModel.H- Cover)).Add(-beamModel.XVector*Cover);

            var rebar = Rebar.CreateFromRebarShape(doc, shape , StrirrupDiameter, beamModel.Beam, o1,beamModel.XVector, beamModel.ZVector);

            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
            shapeDrivenAccessor.ScaleToBox(o1, -beamModel.XVector * (beamModel.B -2*Cover),beamModel.ZVector*(beamModel.H-2*Cover));

            var normalSide = rebar.GetShapeDrivenAccessor().Normal.DotProduct(beamModel.Direction) >  0;
            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(StrirrupSpacing.MmToFeet(), (beamModel.StartPointTop - beamModel.EndPointTop).GetLength() - 100.MmToFeet(), normalSide, true, true);

            doc.Regenerate();
        }
        void CreateMainTopRebar()
        {
          // top Bar 

            var p1 = beamModel.StartPointTop.Add(XYZ.BasisZ*-(Cover+ StrirrupDiameter.BarNominalDiameter+ DuongKinhThepChinh.BarNominalDiameter / 2)).Add(-XYZ.BasisX * (Cover + StrirrupDiameter.BarNominalDiameter + DuongKinhThepChinh.BarNominalDiameter / 2));

            var p0 = p1.Add(-beamModel.Direction * 30 * DuongKinhThepChinh.BarNominalDiameter);
            var p2 = p1.Add(beamModel.Direction*(beamModel.Length+30*DuongKinhThepChinh.BarNominalDiameter)).Add(beamModel.Direction*30* DuongKinhThepChinh.BarNominalDiameter);

            var curve = Line.CreateBound(p0, p2);
            var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, DuongKinhThepChinh, null, null, beamModel.Beam,
                    -beamModel.XVector,
                    new List<Curve>() {curve }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            var spacing =  (beamModel.B- (2*Cover + 2* StrirrupDiameter.BarNominalDiameter +DuongKinhThepChinh.BarNominalDiameter))/ (SoLuongThepChinh - 1);

            for (int i = 1; i < SoLuongThepChinh; i++)
            {
                ElementTransformUtils.CopyElement(doc, rebar.Id,- beamModel.XVector * spacing * i);
            }

        }

        void CreateMainBotRebar()
        {
            // bot Bar 
            var p1 = beamModel.StartPointTop.Add(XYZ.BasisZ * -(beamModel.H-( Cover + StrirrupDiameter.BarNominalDiameter + DuongKinhThepChinh.BarNominalDiameter / 2))).Add(-XYZ.BasisX * (Cover + StrirrupDiameter.BarNominalDiameter + DuongKinhThepChinh.BarNominalDiameter / 2));

            var p0 = p1.Add(-beamModel.Direction * 30 * DuongKinhThepChinh.BarNominalDiameter);
            var p2 = p1.Add(beamModel.Direction * (beamModel.Length + 30 * DuongKinhThepChinh.BarNominalDiameter)).Add(beamModel.Direction * 30 * DuongKinhThepChinh.BarNominalDiameter);

            var curve = Line.CreateBound(p0, p2);
            var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, DuongKinhThepChinh, null, null, beamModel.Beam,
                    -beamModel.XVector,
                    new List<Curve>() { curve }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);

            var spacing = (beamModel.B - (2 * Cover + 2 * StrirrupDiameter.BarNominalDiameter + DuongKinhThepChinh.BarNominalDiameter)) / (SoLuongThepChinh - 1);

            for (int i = 1; i < SoLuongThepChinh; i++)
            {
                ElementTransformUtils.CopyElement(doc, rebar.Id, -beamModel.XVector * spacing * i);
            }


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
