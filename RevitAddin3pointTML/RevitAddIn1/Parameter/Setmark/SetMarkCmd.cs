using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;

using Nice3point.Revit.Extensions;

namespace RevitAddIn1.Parameter.Setmark
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class SetMarkCmd : ExternalCommand
    {
        public override void Execute()
        {
            var columnRfs = UiDocument.Selection.PickObjects(ObjectType.Element, "Select columns");
            var columns =  columnRfs.Select(x=>Document.GetElement(x)).ToList();

            var markC1 = "test";

            using (var tx = new Transaction(Document, "Set mark"))
            {
                tx.Start();

                columns.ForEach(x =>
                {
                    x.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(markC1);
                    x.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("tesstt");
                    x.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(0.328);
                });

                tx.Commit();
            }
        }
    }
}
