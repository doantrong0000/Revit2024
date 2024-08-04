using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HelloWorld1
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorldCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)

        {
            var a = 1 + 1;

            MessageBox.Show("Hello World");
            return Result.Succeeded;
        }
    }
}