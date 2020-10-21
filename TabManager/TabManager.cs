using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TabManagerProject
{
    [Transaction(TransactionMode.Manual)]
    class TabManager : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Cache cache = new Cache();

            // 获取功能页信息并将其存放至列表中
            IList<RibbonTab> tabList = ComponentManager.Ribbon.Tabs;
            //List<string> tabNameList = new List<string>();
            //List<bool> tabBoolList = new List<bool>();
            foreach (RibbonTab tab in tabList)
            {
                if (!tab.IsContextualTab && !tab.IsMergedContextualTab && tab.KeyTip == null)
                {
                    //tab.IsVisible = !tab.IsVisible;
                    cache.TabNameList.Add(tab.Name);
                    //cache.TabValueList.Add(true);
                }
            }
            Autodesk.Revit.UI.TaskDialog.Show("result", cache.TabNameList.Count.ToString());
            // 初始化窗体
            TabManagerWindow wpf = new TabManagerWindow(commandData, cache);

            wpf.ShowDialog();
            return Result.Succeeded;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
