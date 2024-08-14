using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.CreateBeam
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateBeamCmd : ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("p1");
            var p2 = UiDocument.Selection.PickPoint("p2");
            var curve = Line.CreateBound(p1, p2);

            var BeamType = new FilteredElementCollector(Document).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming).FirstOrDefault(x=> x.Id.Value == 84121) as FamilySymbol;

            var level = ActiveView.GenLevel;


            using (var tx= new Transaction(Document, "Create Beam"))
            {
                tx.Start();

                FamilyInstance instance = Document.Create.NewFamilyInstance(curve, BeamType, level, Autodesk.Revit.DB.Structure.StructuralType.Beam);

                instance.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE).Set(0.01.MeetToFeet());

                tx.Commit();
            }
  
        }

 
    }
}