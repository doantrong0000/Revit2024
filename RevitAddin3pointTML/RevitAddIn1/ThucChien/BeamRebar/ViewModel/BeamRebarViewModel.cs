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
using RevitAddIn1.ThucChien.BeamRebar.View;
using RevitAddIn1.ThucChien.BeamRebar.Model;

namespace RevitAddIn1.ThucChien.BeamRebar.ViewModel
{
    public class BeamRebarViewModel: ObservableObject
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
        private BeamRebarModel beamModel { get; set; }


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
                var column = uiDoc.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(), "'Select column").ToElement() as FamilyInstance;

          
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
        }
        void CreateXMainRebar()
        {
           
        }

        void CreateYMainRebar()
        {
         
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
