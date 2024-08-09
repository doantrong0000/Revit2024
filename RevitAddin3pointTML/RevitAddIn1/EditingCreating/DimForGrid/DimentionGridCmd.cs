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
using RevitAddIn1.EditingCreating.DimForGrid;
using Autodesk.Revit.Creation;
using RevitAddIn1.EditingCreating.DimForGrid.View;
using RevitAddIn1.EditingCreating.DimForGrid.Model;
using RevitAddIn1.EditingCreating.DimForGrid.ViewModel;
using RevitAddIn1.EditingCreating;


namespace RevitAddIn1.EditingCreating.DimForGrid
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class DimensionGridCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var vm = new DimentionGridViewModel(Document, UiDocument);
            var view = new DimensionGridView() { DataContext = vm };

            vm.DimentionGridView = view;

            view.ShowDialog();
        }
    }
}