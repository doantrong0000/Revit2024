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
using RevitAddIn1.SelectionFilter;


namespace RevitAddIn1.EditingCreating.DimForGrid.ViewModel
{
    public class DimentionGridViewModel : ObservableObject
    {
        public List<DimensionType> DimensionTypes {  get; set; } = new List<DimensionType>();
        public DimensionType SelectedDimensionType { get; set; }

        public DimensionGridView DimentionGridView { get; set; }

        public double Distance { get; set; } = 5.0;

      
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        private Document doc;
        private UIDocument uiDoc;

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\HocRevitApi\\DimensionGrid.json";

        public DimentionGridViewModel(Document doc,UIDocument uiDoc)
        {
            this.uiDoc = uiDoc;
            this.doc = doc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            DimensionTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>().Where(x => x.StyleType == DimensionStyleType.Linear || x.StyleType == DimensionStyleType.LinearFixed).OrderBy(x => x.Name).ToList();


            SelectedDimensionType = DimensionTypes.FirstOrDefault();
            LoadData();
        }
        void Run()
        {
            DimentionGridView.Close();
            var allGrids = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(Grid)).Cast<Grid>().ToList();
            var allGridModels = allGrids.Select(x=>new GridModel(x)).ToList();
            using (var tx = new Transaction(doc, "Create Dim"))
            {
                tx.Start();
                while (true)
                {
                    Reference rf;
                    try
                    {
                        rf = uiDoc.Selection.PickObject(ObjectType.Element, new GridSelectionFilter(), "Select grid to Dim");
                    }
                    catch(Exception e) { break; }

                    var point =rf.GlobalPoint;
                    var grid = rf.ToElement() as Grid;

                    var selectedGridModel = new GridModel(grid);
                    var gridsToDim = allGridModels.Where(x=> x.IsHorizintalGrid ==  selectedGridModel.IsHorizintalGrid).ToList();

                    var ra = new ReferenceArray();
                    foreach (var gridModel in gridsToDim)
                    {
                        ra.Append(new Reference(gridModel.Grid));
                    }
                    var vector = XYZ.BasisX;
                    if (selectedGridModel.IsHorizintalGrid) 
                    {
                        vector= XYZ.BasisY;
                    }

                    var orderedGrids =  gridsToDim.OrderBy(x=> x.EP.DotProduct(vector)).ToList();
                    var gridModel1 = orderedGrids.FirstOrDefault();
                    var gridModel2 = orderedGrids.LastOrDefault();


                    var line = Line.CreateBound(rf.GlobalPoint, rf.GlobalPoint.Add(vector));
                    doc.Create.NewDimension(doc.ActiveView, line, ra);

                    var p = rf.GlobalPoint.Add(XYZ.BasisX * Distance.MmToFeet() * doc.ActiveView.Scale);
                    var line2 = Line.CreateBound(p, p.Add(vector));
                    var ra2 = new ReferenceArray(); 
                    ra2.Append( new Reference(gridModel1.Grid));
                    ra2.Append( new Reference(gridModel2.Grid));
                    doc.Create.NewDimension(doc.ActiveView, line2, ra2, SelectedDimensionType);
                }
             
                tx.Commit();
            }

            SaveData();
        }
        void Close()
        {

        }
        void SaveData()
        {
            var jsonData = new DimensionGridJsonModel()
            {
                SelectedDimensionTypeName = SelectedDimensionType.Name,
                Distance = Distance,
            };

            var jsonString = JsonSerializer.Serialize(jsonData);

            if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HocRevitApi"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HocRevitApi");
            }

            File.WriteAllText(path,jsonString);
         
        }
        void LoadData()
        {
            if (File.Exists(path)) 
            { 
                var text = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<DimensionGridJsonModel>(text);

                if (data != null)
                {
                    Distance= data.Distance;

                    SelectedDimensionType = DimensionTypes.FirstOrDefault(x=>x.Name== data.SelectedDimensionTypeName);
                }
            }
        }
    }
}
