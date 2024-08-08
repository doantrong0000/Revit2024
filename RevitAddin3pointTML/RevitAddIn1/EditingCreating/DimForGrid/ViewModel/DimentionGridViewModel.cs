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

        public DimentionGridViewModel(Document doc)
        {
            this.doc = doc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            DimensionTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>().Where(x => x.StyleType == DimensionStyleType.Linear || x.StyleType == DimensionStyleType.LinearFixed).OrderBy(x => x.Name).ToList();


            if (DimensionTypes == null)
            {
                TaskDialog.Show("Error", "The list is null, which should not happen.");
            }
            else if (!DimensionTypes.Any())
            {
                TaskDialog.Show("Info", "The list is empty. No matching DimensionTypes found.");
            }
            else
            {
                TaskDialog.Show("Success", "DimensionTypes retrieved successfully.");
            }

            SelectedDimensionType = DimensionTypes.FirstOrDefault();
        }
        void Run()
        {

        }
        void Close()
        {

        }
    }
}
