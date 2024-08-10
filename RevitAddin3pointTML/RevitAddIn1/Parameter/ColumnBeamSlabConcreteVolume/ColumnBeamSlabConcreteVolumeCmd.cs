using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;

using Nice3point.Revit.Extensions;

namespace RevitAddIn1.Parameter.ColumnBeamSlabConcreteVolumeCmd
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ColumnBeamSlabConcreteVolumeCmd : ExternalCommand
    {
        public override void Execute()
        {
            var columnBeamSlabCategoryFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_Floors, BuiltInCategory.OST_StructuralColumns, BuiltInCategory.OST_StructuralFraming
            });
            var columnBeamSlabs = new FilteredElementCollector(Document, ActiveView.Id)
                .WherePasses(columnBeamSlabCategoryFilter)
                .WhereElementIsNotElementType()
                .ToElements();

            var totalV = columnBeamSlabs.Sum(x=>x.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble()*0.02831685);
            MessageBox.Show(columnBeamSlabs.Count.ToString());
            MessageBox.Show($"Tong the tich be tong {totalV} m3");
        }
    }
}
