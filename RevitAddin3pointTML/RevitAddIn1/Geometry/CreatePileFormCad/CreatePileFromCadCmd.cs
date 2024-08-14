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
using Autodesk.Revit.Creation;
using RevitAddIn1.Parameter.RenameSheet.View;
using Document = Autodesk.Revit.DB.Document;
using RevitAddIn1.Parameter.RenameSheet.ViewModel;
using RevitAddIn1.Geometry.CreatePileFormCad.View;
using RevitAddIn1.Geometry.CreatePileFormCad.ViewModel;
using RevitAddIn1.Utils;


namespace RevitAddIn1.Geometry.CreatePileFormCad
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreatePileFromCadCmd: ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document =Document;
            var vm = new CreatePileFromCadViewModel(Document, UiDocument);
            var view = new CreatePileFromCadView() { DataContext = vm };

            vm.CreatePileFromCadView = view;
            view.ShowDialog();
        }
    }
}
