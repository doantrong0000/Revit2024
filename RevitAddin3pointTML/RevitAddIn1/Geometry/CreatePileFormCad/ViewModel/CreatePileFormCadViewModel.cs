using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using RevitAddIn1.EditingCreating.DimForGrid.View;
using RevitAddIn1.EditingCreating.DimForGrid.Model;
using RevitAddIn1.EditingCreating.DimForGrid;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitAddIn1.Utils;
using Autodesk.Revit.UI.Selection;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RevitAddIn1.Geometry.CreatePileFormCad.View;
using System.Windows.Shapes;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using MoreLinq;
using RevitAddIn1.Geometry.CreatePileFormCad.Model;


namespace RevitAddIn1.Geometry.CreatePileFormCad
{
    public class CreatePileFromCadViewModel : ObservableObject
    {
        public List<CadCurveModel> CadCurveModels { get; set; }= new List<CadCurveModel>();

        public List<String> Layers
        {
            get => _layers;
            set
            {
                if (Equals(value, _layers)) return;
                _layers = value;
                OnPropertyChanged();
            }
        }

        public String SelectedLayer

        {
            get => _selectedLayer;
            set
            {
                if (value == _selectedLayer) return;
                _selectedLayer = value;
                OnPropertyChanged();
            }
        }

        public List<FamilySymbol> PileSymbols { get; set; } = new List<FamilySymbol>();
        public FamilySymbol SelectedPileSymbol { get; set; }
        public CreatePileFromCadView CreatePileFromCadView { get; set; }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HocRevitApi\\PileCad.json";

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public RelayCommand LoadCadCmd { get; set; }

        private Document doc;
        private UIDocument uiDoc;
        private List<string> _layers = new List<string>();
        private string _selectedLayer;


        public CreatePileFromCadViewModel(Document doc, UIDocument uiDoc)
        {
            this.uiDoc = uiDoc;
            this.doc = doc;
            LoadCadCmd= new RelayCommand(LoadCad);
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            PileSymbols = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFoundation).Cast<FamilySymbol>()
                .OrderBy(x => x.Name).ToList();

            SelectedPileSymbol = PileSymbols.FirstOrDefault();
            LoadData();
        }
        void Run()
        {
         CreatePileFromCadView.Close();

         var cadPileCurves = CadCurveModels.Where(x => x.Layer == SelectedLayer).ToList();
         using (var tx = new Transaction(doc, "Create pile"))
         {
             tx.Start();
             foreach (var cadPileCurve in cadPileCurves)
             {
                 var arc = cadPileCurve.Curve as Arc;
                 if (arc != null)
                 {
                     var center = arc.Center;
                     var radius = arc.Radius;

                     doc.Create.NewFamilyInstance(center, SelectedPileSymbol, doc.ActiveView.GenLevel,
                         StructuralType.Footing);
                 }
             }

             tx.Commit();
         }
     

        }
        void LoadCad()
        {
            CreatePileFromCadView.Hide();
           var cadLink =  uiDoc.Selection.PickObject(ObjectType.Element,new CadLinkSelectionFilter(), "Select Cad Link").ToElement();
           var allArcs = new List<Arc>();
           var geometryElement = cadLink.get_Geometry(new Options());
           foreach (var geoOjb in geometryElement)
           {
               if (geoOjb is GeometryInstance geometryInstance)
               {
                   var ge = geometryInstance.GetInstanceGeometry();
                   var arcs = ge.Where(x => x is Arc).Cast<Arc>().ToList();
                   allArcs.AddRange(arcs);
               }
           }

           CadCurveModels = allArcs.Select(x => new CadCurveModel(x)).ToList();
           Layers = CadCurveModels.Select(x => x.Layer).DistinctBy(x => x).OrderBy(x=>x).ToList();

           SelectedLayer = Layers.FirstOrDefault(); 

            CreatePileFromCadView.ShowDialog();
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

