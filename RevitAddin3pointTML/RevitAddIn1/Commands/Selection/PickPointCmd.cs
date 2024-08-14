﻿using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Commands.Selection
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class PickPointCmd : ExternalCommand
    {
        public override void Execute()
        {
            try
            {
                var point = UiDocument.Selection.PickPoint(ObjectSnapTypes.Nearest,"Pick point");
              
                MessageBox.Show(point.ToString());
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban da huy chon", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
  
        }
    }
}