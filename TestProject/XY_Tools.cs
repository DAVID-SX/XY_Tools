using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace XY_Tools_Project
{
    [Transaction(TransactionMode.Manual)]
    class XY_Tools : IExternalApplication
    {
        //定义只读属性，该属性为本插件的Tab面板名称
        private string tabName = "XY工具集";
        public string TabName
        {
            get { return tabName; }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //【01】创建一个RibbonTab
            application.CreateRibbonTab(tabName);
            //【02】在刚才的RibbonTab中创建RibbonPanel
            RibbonPanel uiManageRibbonPanel = application.CreateRibbonPanel("XY工具集", "界面管理工具集");
            RibbonPanel modelManageRibbonPanel = application.CreateRibbonPanel("XY工具集", "模型操作工具集");
            //【03】指定程序集的名称以及使用的类名
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            //获取程序集的路径（相对路径）
            //namespaceName.className
            string tabManagerClass = "XY_Tools_Project.TabManager";
            string joinModelClass = "XY_Tools_Project.JoinGeometry";
            //【04-01】创建pushbutton
            PushButtonData tabManagerPushButtonData = new PushButtonData("InnerNameRevit", "插件管理器", assemblyPath, tabManagerClass);
            PushButtonData joinModelPushButtonData = new PushButtonData("InnerNameRevit2", "扣减模型", assemblyPath, joinModelClass);
            //new PushButtonData("在程序内部的名称，必须唯一", "在按钮上显示的名称", 程序集dll的路径, 命名空间以及类名)
            //【04-02】将pushbuttom添加到RibbonPanel中
            PushButton tabManagerPushButton = uiManageRibbonPanel.AddItem(tabManagerPushButtonData) as PushButton;
            PushButton joinModelPushButton = modelManageRibbonPanel.AddItem(joinModelPushButtonData) as PushButton;
            //【04-03】给按钮设置一个图片(大图标一般是32px，小图标一般是16px,格式可以是ico或者png）
            //设置图片路径（右击项目名称>>添加>>文件夹>>新建文件夹并将图片拖动到文件夹中>>右击图片名称>>属性>>生成操作>>Resource
            //UIDemo指的是命名空间名
            tabManagerPushButton.LargeImage = new BitmapImage(new Uri("pack://application:,,,/XY_Tools_Project;component/pic/TabManager.png", UriKind.Absolute));
            joinModelPushButton.LargeImage = new BitmapImage(new Uri("pack://application:,,,/XY_Tools_Project;component/pic/JoinGeometry.png", UriKind.Absolute));
            //【04-04】给按钮设置一个默认提示信息
            tabManagerPushButton.ToolTip = "管理Revit插件面板的显隐性";
            joinModelPushButton.ToolTip = "根据定义的条件对模型的重叠部分进行处理";
            //【06】返回程序执行结果
            return Result.Succeeded;
        }
    }
}
