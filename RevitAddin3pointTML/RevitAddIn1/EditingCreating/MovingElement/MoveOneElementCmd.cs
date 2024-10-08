﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.MovingElement
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class MoveElementCmd : ExternalCommand
    {
        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var column =UiDocument.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to move").ToElement();

            using (var tx= new Transaction(Document, "Move"))
            {
                tx.Start();
                // thay đổi đối tượng bằng cách đổi giá trị chuyền vào Ids
                ElementTransformUtils.MoveElement(Document, column.Id, new XYZ(10.MeetToFeet(), 10.MeetToFeet(), 0));
                tx.Commit();
            }
  
        }
    }
}