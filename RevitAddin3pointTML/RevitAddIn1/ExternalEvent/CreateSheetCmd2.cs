using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using OfficeOpenXml;
using RevitAddIn1.EditingCreating.CreateSheet1.View;
using RevitAddIn1.EditingCreating.CreateSheet1.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateSheetCmd2 :  ExternalCommand
    {
        public override void Execute()
        {
            var externalEvent = DocumentUtils.ExternalEvent;
            // if you have a commercial license
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var vm = new CreateSheetViewModel(Document);
            var view = new CreateSheetView() { DataContext = vm };

            vm.CreateSheetView = view;
            view.ShowDialog();

        }
    }
}
