﻿using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nice3point.Revit.Toolkit.External;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace RevitAddIn1.Commands.Filter
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class FilterWallByClassAndCategoryCmd : ExternalCommand
    {
        public override void Execute()
        {
            var wallsByClass = new FilteredElementCollector(Document)
                .OfClass(typeof(Wall))
                .OfCategory(BuiltInCategory.OST_CurtainWallPanels)
                .ToElements();

            var categories = wallsByClass.Select(x => x.Category.Name).Distinct().ToList();

            MessageBox.Show(string.Join(",",categories));
            MessageBox.Show(wallsByClass.Count.ToString());
        }
    }
}
