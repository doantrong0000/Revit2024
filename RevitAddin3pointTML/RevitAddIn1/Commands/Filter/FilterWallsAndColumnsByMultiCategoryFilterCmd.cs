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
    public class FilterWallsAndColumnsByMultiCategoryFilterCmd : ExternalCommand
    {
        public override void Execute()
        {
            var wallAndColumnCategoryFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_Walls, BuiltInCategory.OST_StructuralColumns
            });
            var wallsAndColumns = new FilteredElementCollector(Document, ActiveView.Id)
                .WherePasses(wallAndColumnCategoryFilter)
                .WhereElementIsNotElementType()
                .ToElements();
          

            MessageBox.Show(wallsAndColumns.Count.ToString());
        }
    }
}
