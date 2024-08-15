using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Geometry.CreatePileFormCad.View;
using RevitAddIn1.Geometry.CreatePileFormCad.ViewModel;
using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.ThucChien.ColumnRebar.ViewModel;

namespace RevitAddIn1.ThucChien.ColumnRebar
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ColumnRebarCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var vm = new ColumnRebarViewModel(Document, UiDocument);
            var view = new ColumnRebarView() { DataContext = vm };

            vm.ColumnRebarView = view;
            view.ShowDialog();
        }
    }
}
