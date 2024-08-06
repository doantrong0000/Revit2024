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
    public class FilterCeilingByClassInActiveViewCmd : ExternalCommand
    {
        public override void Execute()
        {
            var Ceilings = new FilteredElementCollector(Document,ActiveView.Id).OfClass(typeof(Ceiling))
                .ToElements();
            var categories = Ceilings.Select(x => x.Category.Name).Distinct().ToList();

            MessageBox.Show(string.Join(",",categories));
            MessageBox.Show(Ceilings.Count.ToString());
        }
    }
}
