using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XY_Tools
{
    [Transaction(TransactionMode.Manual)]
    class TabManager : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RibbonControl ribbonControl = ComponentManager.Ribbon;
            foreach (RibbonTab tab in ribbonControl.Tabs)
            {
                if (!tab.IsContextualTab && !tab.IsMergedContextualTab && tab.KeyTip == null)
                {
                    //tab.IsVisible = !tab.IsVisible;
                    Autodesk.Revit.UI.TaskDialog.Show("result", tab.Name);
                }
                //Autodesk.Revit.UI.TaskDialog.Show("result", tab.Name);
            }
            return Result.Succeeded;
        }
    }
}
