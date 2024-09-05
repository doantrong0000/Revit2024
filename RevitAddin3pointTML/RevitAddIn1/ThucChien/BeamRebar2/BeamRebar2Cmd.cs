using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ThucChien.BeamRebar2.View;
using RevitAddIn1.ThucChien.BeamRebar2.ViewModel;
using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitAddIn1.ThucChien.BeamRebar2Cmd
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class BeamRebar2Cmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var vm = new BeamRebar2ViewModel(Document, UiDocument);
            var view = new BeamRebar2View() { DataContext = vm };

            vm.BeamRebar2View = view;
            view.ShowDialog();
        }
    }
}
