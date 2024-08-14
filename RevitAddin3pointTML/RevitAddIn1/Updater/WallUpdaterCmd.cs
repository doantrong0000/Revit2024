using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Updater
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class WallUpdaterCmd : ExternalCommand
    {
        public override void Execute()
        {
            // Register wall updater with Revit
            WallUpdater updater = new WallUpdater(UiApplication.ActiveAddInId);
            UpdaterRegistry.RegisterUpdater(updater);

            // Change Scope = any Wall element
            ElementClassFilter wallFilter = new ElementClassFilter(typeof(Wall));

            // Change type = element addition
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), wallFilter,
                Element.GetChangeTypeElementAddition());
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), wallFilter,
                Element.GetChangeTypeGeometry());

        }
    }
}