using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace RevitAddIn1.EditingCreating.CreateSheet1
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateSheetCmd :  ExternalCommand
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
