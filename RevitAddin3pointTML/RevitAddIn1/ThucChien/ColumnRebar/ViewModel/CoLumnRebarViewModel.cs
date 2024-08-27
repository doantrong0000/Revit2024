using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using RevitAddIn1.Geometry.CreatePileFormCad.Model;
using RevitAddIn1.Geometry.CreatePileFormCad.View;
using RevitAddIn1.SelectionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitAddIn1.ThucChien.ColumnRebar.Model;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.ColumnRebar.ViewModel
{
    public class ColumnRebarViewModel : ObservableObject
    {
        public ColumnRebarView ColumnRebarView { get; set; }
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
    private ColumnRebarModel columnModel { get; set; }


        public ColumnRebarViewModel(Document doc, UIDocument uiDoc)
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

            XDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() >20);
            YDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            StrirrupDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() < 12);
            LoadData();
        }
        void Run()
        {
            ColumnRebarView.Close();
            try
            {
                var column = uiDoc.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(), "'Select column").ToElement() as FamilyInstance;

                columnModel = new ColumnRebarModel(column);
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
            var o1= columnModel.D.Add(columnModel.XVector * Cover).Add(columnModel.YVector * Cover);

            o1 = new XYZ(o1.X, o1.Y, columnModel.BotElevation +Cover +StrirrupDiameter.BarModelDiameter/2);

            var rebar =Rebar.CreateFromRebarShape(doc, shaper, StrirrupDiameter, columnModel.Column, o1, columnModel.XVector,
                columnModel.YVector);

            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
            shapeDrivenAccessor.ScaleToBox(o1, columnModel.XVector*(columnModel.Width-2*Cover),columnModel.YVector*(columnModel.Height-2*Cover));

            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(StrirrupSpacing.MmToFeet(), (columnModel.TopElevation-columnModel.BotElevation) -2*Cover- StrirrupDiameter.BarModelDiameter, true, true,true);
        }
        void CreateXMainRebar()
        {
        var spacing2Rebars = (columnModel.Width-2*Cover - 2* StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter)/(
            NumberOfXRebar - 1);


        //Top layer 
        var topRebars = new List<Rebar>();

            for (int i = 0; i < NumberOfXRebar; i++)
            {
                var o2 = columnModel.A.Add(
                        columnModel.XVector * (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                    .Add(-columnModel.YVector *
                         (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));

                o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);
                o2 = o2.Add(columnModel.XVector * i * spacing2Rebars);

                var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                var line20 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 20 * XDiameter.BarNominalDiameter)));
                var line30 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 30 * XDiameter.BarNominalDiameter)));
                if (i % 2 == 0)
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                        columnModel.XVector,
                        new List<Curve>() { line20 }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                    topRebars.Add(rebar);
                }
                else
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                        columnModel.XVector,
                        new List<Curve>() { line30 }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                    topRebars.Add(rebar);
                }

            }

            ElementTransformUtils.CopyElements(doc, topRebars.Select(x=>x.Id).ToList(),columnModel.YVector * -1 *(columnModel.Height -2*Cover- 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter));
          
        }

        void CreateYMainRebar()
        {
            var spacing2Rebars = (columnModel.Height - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter) / (
         NumberOfYRebar - 1);


            //Left layer 
            var topRebars = new List<Rebar>();

            
            if (NumberOfYRebar > 2)
            {
                for (int i = 0; i < NumberOfYRebar; i++)
                {
                    if (i == 0 || i == NumberOfYRebar - 1)
                    {
                        continue;
                    }
                    var o2 = columnModel.A.Add(
                            columnModel.XVector * (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                        .Add(-columnModel.YVector *
                             (Cover + StrirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));

                    o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);
                    o2 = o2.Add(columnModel.YVector*-1 * i * spacing2Rebars);

                    var columnHeight = columnModel.TopElevation - columnModel.BotElevation;

                    var line20 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 20 * XDiameter.BarNominalDiameter)));
                    var line30 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 30 * XDiameter.BarNominalDiameter)));
                    if (i % 2 == 0)
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                            columnModel.XVector,
                            new List<Curve>() { line20 }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                        topRebars.Add(rebar);
                    }
                    else
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                            columnModel.XVector,
                            new List<Curve>() { line30 }, RebarHookOrientation.Left, RebarHookOrientation.Left, true, true);
                        topRebars.Add(rebar);
                    }
                }
            }
            ElementTransformUtils.CopyElements(doc, topRebars.Select(x => x.Id).ToList(), columnModel.XVector  * (columnModel.Width - 2 * Cover - 2 * StrirrupDiameter.BarNominalDiameter - XDiameter.BarNominalDiameter));
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
