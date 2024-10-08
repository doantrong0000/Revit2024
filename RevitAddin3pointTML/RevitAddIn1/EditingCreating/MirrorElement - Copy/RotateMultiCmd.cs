﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.EditingCreating.MirrorElement___Copy
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class RotateMultiCmd : ExternalCommand
    {

        public override void Execute()
        {
          DocumentUtils.Document = Document;

            var column =UiDocument.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(), "Select Column to Rotate").ToElement();

            using (var tx= new Transaction(Document, "Rotate"))
            {
                tx.Start();
                var lc = column.Location as LocationPoint;
                XYZ point1 = new XYZ(lc.Point.X, lc.Point.Y, 0);
                XYZ point2 = new XYZ(lc.Point.X, lc.Point.Y, 30);
                Line axis = Line.CreateBound(point1, point2);
                ElementTransformUtils.RotateElement(Document, column.Id, axis, Math.PI / 3.0);
                tx.Commit();
            }
  
        }

 
    }
}