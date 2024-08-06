using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Extensions;
using RevitAddIn1.Parameter.RenameSheet.ViewModel;
using RevitAddIn1.Parameter.RenameSheet.View;
using Autodesk.Revit.Creation;

namespace RevitAddIn1.Parameter.RenameSheet
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class RenameSheetCmd : ExternalCommand
    {
        public override void Execute()
        {
     
            var vm = new RenameSheetViewModel(Document);
            var view = new RenameSheetView() { DataContext = vm };

            vm.RenameSheetView = view;
            view.ShowDialog();
        }
    }
}

