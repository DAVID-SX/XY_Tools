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

namespace TestProject
{
    [Transaction(TransactionMode.Manual)]
    class TabManager : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Cache cache = new Cache();

            // 获取功能页信息并将其存放至列表中
            IList<RibbonTab> tabList = ComponentManager.Ribbon.Tabs;
            List<RibbonTab> addinTabList = new List<RibbonTab>();
            foreach (RibbonTab tab in tabList)
            {
                if (!tab.IsContextualTab && !tab.IsMergedContextualTab && tab.KeyTip == null)
                {
                    cache.TabNameList.Add(tab.Name);
                    //cache.TabValueList.Add(true);
                    addinTabList.Add(tab);
                }
            }

            // 初始化窗体
            TabManagerWindow wpf = new TabManagerWindow(commandData, cache);
            wpf.ShowDialog();
            if (wpf._cache.TabValueList.Count != 0)
            {
                for (int i = 0; i < addinTabList.Count; i++)
                {
                    addinTabList[i].IsVisible = wpf._cache.TabValueList[i + 1];
                }
            }
            return Result.Succeeded;
        }
    }
}
