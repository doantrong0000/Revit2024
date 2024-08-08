using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Messaging;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1
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