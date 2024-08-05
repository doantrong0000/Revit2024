using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nice3point.Revit.Toolkit.External;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace RevitAddIn1.Commands.Filter
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class FilterColumnByClassAndCategoryCmd : ExternalCommand
    {
        public override void Execute()
        {
            var columnByClassAndCategory = new FilteredElementCollector(Document, ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .OfClass(typeof(FamilyInstance))
                .ToElements();

          
            MessageBox.Show(columnByClassAndCategory.Count.ToString());
        }
    }
}
