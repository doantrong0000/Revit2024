using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.LuyenTap.BeamRebar.ViewModel;
using RevitAddIn1.Utils;
using RevitAddIn1.LuyenTap.BeamRebar.View;

namespace RevitAddIn1.LuyenTap.BeamRebar
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class BeamRebarCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var vm = new BeamRebarViewModel(Document, UiDocument);
            var view = new BeamRebarView() { DataContext = vm };

            vm.BeamRebarView = view;
            view.ShowDialog();
        }
    }
}
