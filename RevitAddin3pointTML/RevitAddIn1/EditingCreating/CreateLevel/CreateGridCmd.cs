using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.CreateLevel
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateLevelCmd : ExternalCommand
    {

        public override void Execute()
        {
            DocumentUtils.Document = Document;
          
            using (var tx = new Transaction(Document, "Create Level"))
            {
                tx.Start();
                CreateLevel(Document, 7.MeetToFeet());
                CreateLevel(Document, 9.MeetToFeet());
                tx.Commit();
            }

        }

        Level CreateLevel(Autodesk.Revit.DB.Document document,double elevation)
        {
         

            // Begin to create a level
            Level level = Level.Create(document, elevation);
            if (null == level)
            {
                throw new Exception("Create a new level failed.");
            }

          

            return level;
        }



    }
}