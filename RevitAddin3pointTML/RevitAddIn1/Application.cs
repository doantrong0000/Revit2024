using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Commands;
using OfficeOpenXml;
using RevitAddIn1.Commands.Selection;

namespace RevitAddIn1
{
    // <summary>
    //     Application entry point
    // </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();

            //if you have a commercial license
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "RevitAddIn1");

            panel.AddPushButton<StartupCommand>("Execute")
                .SetImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}