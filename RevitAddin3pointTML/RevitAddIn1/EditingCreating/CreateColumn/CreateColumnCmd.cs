using System.Data;
using System.Windows;
using Autodesk.Revit.Attributes;
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
    public class CreateColumnCmd : ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var pp = UiDocument.Selection.PickPoint("pick point");

            var CoulmnType = new FilteredElementCollector(Document).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns).FirstOrDefault(x => x.Id.Value == 12190) as FamilySymbol;
            var level0 = ActiveView.GenLevel;
                // CHON level can den la lv2
            var level1 = new FilteredElementCollector(Document).OfClass(typeof(Level)).FirstOrDefault(x => x.Name == "Level 2");


            using (var tx= new Transaction(Document, "Create Beam"))
            {
                tx.Start();

                FamilyInstance instance = Document.Create.NewFamilyInstance(pp, CoulmnType, level0, Autodesk.Revit.DB.Structure.StructuralType.Column);
                instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(level1.Id);
                instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(0.0);
                instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(0.01.MeetToFeet());


                tx.Commit();
            }
  
        }

 
    }
}