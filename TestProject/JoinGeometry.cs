using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XY_Tools_Project
{
    [Transaction(TransactionMode.Manual)]
    class JoinGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //获取文档
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            //初始化缓存系统
            Cache cache = new Cache();
            ///分类收集项目中的元素
            FilteredElementCollector foundationCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralFoundation).OfClass(typeof(FamilyInstance));//结构基础收集器
            FilteredElementCollector wallCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall));//墙收集器
            FilteredElementCollector archiColumnCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Columns).OfClass(typeof(FamilyInstance));//建筑柱收集器
            FilteredElementCollector struColumnCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns).OfClass(typeof(FamilyInstance));//结构柱收集器
            FilteredElementCollector beamCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralFraming).OfClass(typeof(FamilyInstance));//结构梁收集器
            FilteredElementCollector FloorCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Floors).OfClass(typeof(Floor));//楼板收集器
            FilteredElementCollector genericModelCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_GenericModel).OfClass(typeof(FamilyInstance));//常规模型收集器
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels).OfClass(typeof(Level));//标高收集器
            ///将项目中所有的标高名称加载到LevelNameList中，在窗体加载时将以列表内容创建标高复选框
            List<Element> levelList = levelCollector.ToList();
            foreach (Element elem in levelList) cache.LevelNameList.Add(elem.Name);

            ///加载窗体
            JoinGeometry_Window window = new JoinGeometry_Window(cache);
            window.ShowDialog();
            ///用户在对话框中单击确定时执行扣减任务
            if (window.IsClickConfirm)
            {
                ///用户需要扣减全部模型时的扣减方法
                if (window.JoinAllModel == true)
                {
                    //将需要扣减的模型分别放入列表中
                    List<Element> foundationList = foundationCollector.ToList();
                    List<Element> archiWallList = new List<Element>();
                    List<Element> struWallList = new List<Element>();
                    foreach (var elem in wallCollector)
                    {
                        foreach (Parameter para in elem.Parameters)
                        {
                            if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiWallList.Add(elem);
                            else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struWallList.Add(elem);
                        }
                    }
                    List<Element> archiColumnList = archiColumnCollector.ToList();
                    List<Element> struColumnList = struColumnCollector.ToList();
                    List<Element> struBeamList = beamCollector.ToList();
                    List<Element> archiFloorList = new List<Element>();
                    List<Element> struFloorList = new List<Element>();
                    foreach (var elem in FloorCollector)
                    {
                        foreach (Parameter para in elem.Parameters)
                        {
                            if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiFloorList.Add(elem);
                            else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struFloorList.Add(elem);
                        }
                    }
                    List<Element> genericModelList = genericModelCollector.ToList();

                    ///开启事务进行扣减
                    Transaction trans = new Transaction(doc, "扣减全部模型");
                    trans.Start();
                    ///结构构件扣减建筑构件
                    # region
                    //结构基础切建筑构件
                    JoinModel(doc, foundationList, archiWallList);
                    JoinModel(doc, foundationList, archiColumnList);
                    JoinModel(doc, foundationList, archiFloorList);
                    //结构墙切建筑构件
                    JoinModel(doc, struWallList, archiWallList);
                    JoinModel(doc, struWallList, archiColumnList);
                    JoinModel(doc, struWallList, archiFloorList);
                    //结构柱切建筑构件
                    JoinModel(doc, struColumnList, archiWallList);
                    JoinModel(doc, struColumnList, archiColumnList);
                    JoinModel(doc, struColumnList, archiFloorList);
                    //结构梁切建筑构件
                    JoinModel(doc, struBeamList, archiWallList);
                    JoinModel(doc, struBeamList, archiColumnList);
                    JoinModel(doc, struBeamList, archiFloorList);
                    //结构板切建筑构件
                    JoinModel(doc, struFloorList, archiWallList);
                    JoinModel(doc, struFloorList, archiColumnList);
                    JoinModel(doc, struFloorList, archiFloorList);
                    #endregion
                    ///根据窗体返回值结构构件之间的扣减
                    # region
                    //结构基础的相关扣减
                    if (window.JGJC_JGQ) JoinModel(doc, foundationList, struWallList);
                    if (window.JGJC_JGZ) JoinModel(doc, foundationList, struColumnList);
                    if (window.JGJC_JGL) JoinModel(doc, foundationList, struBeamList);
                    if (window.JGJC_JGB) JoinModel(doc, foundationList, struFloorList);
                    if (window.JGJC_CGMX) JoinModel(doc, foundationList, genericModelList);
                    //结构墙的相关扣减
                    if (window.JGQ_JGJC) JoinModel(doc, struWallList, foundationList);
                    if (window.JGQ_JGZ) JoinModel(doc, struWallList, struColumnList);
                    if (window.JGQ_JGL) JoinModel(doc, struWallList, struBeamList);
                    if (window.JGQ_JGB) JoinModel(doc, struWallList, struFloorList);
                    if (window.JGQ_CGMX) JoinModel(doc, struWallList, genericModelList);
                    //结构柱的相关扣减
                    if (window.JGZ_JGJC) JoinModel(doc, struColumnList, foundationList);
                    if (window.JGZ_JGQ) JoinModel(doc, struColumnList, struWallList);
                    if (window.JGZ_JGL) JoinModel(doc, struColumnList, struBeamList);
                    if (window.JGZ_JGB) JoinModel(doc, struColumnList, struFloorList);
                    if (window.JGZ_CGMX) JoinModel(doc, struColumnList, genericModelList);
                    //结构梁的相关扣减
                    if (window.JGL_JGJC) JoinModel(doc, struBeamList, foundationList);
                    if (window.JGL_JGQ) JoinModel(doc, struBeamList, struWallList);
                    if (window.JGL_JGZ) JoinModel(doc, struBeamList, struColumnList);
                    if (window.JGL_JGB) JoinModel(doc, struBeamList, struFloorList);
                    if (window.JGL_CGMX) JoinModel(doc, struBeamList, genericModelList);
                    //结构板的相关扣减
                    if (window.JGB_JGJC) JoinModel(doc, struFloorList, foundationList);
                    if (window.JGB_JGQ) JoinModel(doc, struFloorList, struWallList);
                    if (window.JGB_JGZ) JoinModel(doc, struFloorList, struColumnList);
                    if (window.JGB_JGL) JoinModel(doc, struFloorList, struBeamList);
                    if (window.JGB_CGMX) JoinModel(doc, struFloorList, genericModelList);
                    //常规模型的相关扣减
                    if (window.CGMX_JGJC) JoinModel(doc, genericModelList, foundationList);
                    if (window.CGMX_JGQ) JoinModel(doc, genericModelList, struWallList);
                    if (window.CGMX_JGZ) JoinModel(doc, genericModelList, struColumnList);
                    if (window.CGMX_JGL) JoinModel(doc, genericModelList, struBeamList);
                    if (window.CGMX_JGB) JoinModel(doc, genericModelList, struFloorList);
                    if (window.CGMX_JZQ) JoinModel(doc, genericModelList, archiWallList);
                    if (window.CGMX_JZZ) JoinModel(doc, genericModelList, archiColumnList);
                    if (window.CGMX_JZB) JoinModel(doc, genericModelList, archiFloorList);
                    //建筑墙的相关扣减
                    if (window.JZQ_JZZ) JoinModel(doc, archiWallList, archiColumnList);
                    if (window.JZQ_JZB) JoinModel(doc, archiWallList, archiFloorList);
                    if (window.JZQ_CGMX) JoinModel(doc, archiWallList, genericModelList);
                    //建筑柱的相关扣减
                    if (window.JZZ_JZQ) JoinModel(doc, archiColumnList, archiWallList);
                    if (window.JZZ_JZB) JoinModel(doc, archiColumnList, archiFloorList);
                    if (window.JZZ_CGMX) JoinModel(doc, archiColumnList, genericModelList);
                    //建筑板的相关扣减
                    if (window.JZB_JZQ) JoinModel(doc, archiFloorList, archiWallList);
                    if (window.JZB_JZZ) JoinModel(doc, archiFloorList, archiColumnList);
                    if (window.JZB_CGMX) JoinModel(doc, archiFloorList, genericModelList);
                    #endregion
                    trans.Commit();
                }
                //用户需要选择模型进行扣减时的扣减方法
                else if (window.JoinPickedModel == true)
                {
                    TaskDialog.Show("提示！！！", "请选择需要扣减的模型，选择完成后点击屏幕左上角（或者屏幕下方）的确认按钮！");
                    IList<Reference> referList = uidoc.Selection.PickObjects(ObjectType.Element);
                    //定义存放元素的列表
                    List<Element> foundationList = new List<Element>();
                    List<Element> archiWallList = new List<Element>();
                    List<Element> struWallList = new List<Element>();
                    List<Element> archiColumnList = new List<Element>();
                    List<Element> struColumnList = new List<Element>();
                    List<Element> struBeamList = new List<Element>();
                    List<Element> archiFloorList = new List<Element>();
                    List<Element> struFloorList = new List<Element>();
                    List<Element> genericModelList = new List<Element>();
                    //向元素列表中添加元素
                    foreach (var item in referList)
                    {
                        Element elem = doc.GetElement(item);
                        if (elem.Category.Name == "结构基础" || elem.Category.Name == "Structural Foundations") foundationList.Add(elem);
                        else if (elem.Category.Name == "墙" || elem.Category.Name == "Walls")
                        {
                            foreach (Parameter para in elem.Parameters)
                            {
                                if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiWallList.Add(elem);
                                else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struWallList.Add(elem);
                            }
                        }
                        else if (elem.Category.Name == "柱" || elem.Category.Name == "Columns") archiColumnList.Add(elem);
                        else if (elem.Category.Name == "结构柱" || elem.Category.Name == "Structural Columns") struColumnList.Add(elem);
                        else if (elem.Category.Name == "结构框架" || elem.Category.Name == "Structural Framing") struBeamList.Add(elem);
                        else if (elem.Category.Name == "楼板" || elem.Category.Name == "Floors")
                        {
                            foreach (Parameter para in elem.Parameters)
                            {
                                if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiFloorList.Add(elem);
                                else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struFloorList.Add(elem);
                            }
                        }
                    }
                    ///开始扣减
                    Transaction trans = new Transaction(doc, "扣减选择的模型");
                    trans.Start();
                    ///结构构件扣减建筑构件
                    //结构基础切建筑构件
                    JoinModel(doc, foundationList, archiWallList);
                    JoinModel(doc, foundationList, archiColumnList);
                    JoinModel(doc, foundationList, archiFloorList);
                    //结构墙切建筑构件
                    JoinModel(doc, struWallList, archiWallList);
                    JoinModel(doc, struWallList, archiColumnList);
                    JoinModel(doc, struWallList, archiFloorList);
                    //结构柱切建筑构件
                    JoinModel(doc, struColumnList, archiWallList);
                    JoinModel(doc, struColumnList, archiColumnList);
                    JoinModel(doc, struColumnList, archiFloorList);
                    //结构梁切建筑构件
                    JoinModel(doc, struBeamList, archiWallList);
                    JoinModel(doc, struBeamList, archiColumnList);
                    JoinModel(doc, struBeamList, archiFloorList);
                    //结构板切建筑构件
                    JoinModel(doc, struFloorList, archiWallList);
                    JoinModel(doc, struFloorList, archiColumnList);
                    JoinModel(doc, struFloorList, archiFloorList);
                    ///判断窗体返回值进行扣减
                    //结构基础的相关扣减
                    if (window.JGJC_JGQ) JoinModel(doc, foundationList, struWallList);
                    if (window.JGJC_JGZ) JoinModel(doc, foundationList, struColumnList);
                    if (window.JGJC_JGL) JoinModel(doc, foundationList, struBeamList);
                    if (window.JGJC_JGB) JoinModel(doc, foundationList, struFloorList);
                    if (window.JGJC_CGMX) JoinModel(doc, foundationList, genericModelList);
                    //结构墙的相关扣减
                    if (window.JGQ_JGJC) JoinModel(doc, struWallList, foundationList);
                    if (window.JGQ_JGZ) JoinModel(doc, struWallList, struColumnList);
                    if (window.JGQ_JGL) JoinModel(doc, struWallList, struBeamList);
                    if (window.JGQ_JGB) JoinModel(doc, struWallList, struFloorList);
                    if (window.JGQ_CGMX) JoinModel(doc, struWallList, genericModelList);
                    //结构柱的相关扣减
                    if (window.JGZ_JGJC) JoinModel(doc, struColumnList, foundationList);
                    if (window.JGZ_JGQ) JoinModel(doc, struColumnList, struWallList);
                    if (window.JGZ_JGL) JoinModel(doc, struColumnList, struBeamList);
                    if (window.JGZ_JGB) JoinModel(doc, struColumnList, struFloorList);
                    if (window.JGZ_CGMX) JoinModel(doc, struColumnList, genericModelList);
                    //结构梁的相关扣减
                    if (window.JGL_JGJC) JoinModel(doc, struBeamList, foundationList);
                    if (window.JGL_JGQ) JoinModel(doc, struBeamList, struWallList);
                    if (window.JGL_JGZ) JoinModel(doc, struBeamList, struColumnList);
                    if (window.JGL_JGB) JoinModel(doc, struBeamList, struFloorList);
                    if (window.JGL_CGMX) JoinModel(doc, struBeamList, genericModelList);
                    //结构板的相关扣减
                    if (window.JGB_JGJC) JoinModel(doc, struFloorList, foundationList);
                    if (window.JGB_JGQ) JoinModel(doc, struFloorList, struWallList);
                    if (window.JGB_JGZ) JoinModel(doc, struFloorList, struColumnList);
                    if (window.JGB_JGL) JoinModel(doc, struFloorList, struBeamList);
                    if (window.JGB_CGMX) JoinModel(doc, struFloorList, genericModelList);
                    //常规模型的相关扣减
                    if (window.CGMX_JGJC) JoinModel(doc, genericModelList, foundationList);
                    if (window.CGMX_JGQ) JoinModel(doc, genericModelList, struWallList);
                    if (window.CGMX_JGZ) JoinModel(doc, genericModelList, struColumnList);
                    if (window.CGMX_JGL) JoinModel(doc, genericModelList, struBeamList);
                    if (window.CGMX_JGB) JoinModel(doc, genericModelList, struFloorList);
                    if (window.CGMX_JZQ) JoinModel(doc, genericModelList, archiWallList);
                    if (window.CGMX_JZZ) JoinModel(doc, genericModelList, archiColumnList);
                    if (window.CGMX_JZB) JoinModel(doc, genericModelList, archiFloorList);
                    //建筑墙的相关扣减
                    if (window.JZQ_JZZ) JoinModel(doc, archiWallList, archiColumnList);
                    if (window.JZQ_JZB) JoinModel(doc, archiWallList, archiFloorList);
                    if (window.JZQ_CGMX) JoinModel(doc, archiWallList, genericModelList);
                    //建筑柱的相关扣减
                    if (window.JZZ_JZQ) JoinModel(doc, archiColumnList, archiWallList);
                    if (window.JZZ_JZB) JoinModel(doc, archiColumnList, archiFloorList);
                    if (window.JZZ_CGMX) JoinModel(doc, archiColumnList, genericModelList);
                    //建筑板的相关扣减
                    if (window.JZB_JZQ) JoinModel(doc, archiFloorList, archiWallList);
                    if (window.JZB_JZZ) JoinModel(doc, archiFloorList, archiColumnList);
                    if (window.JZB_CGMX) JoinModel(doc, archiFloorList, genericModelList);
                    trans.Commit();
                }
                //用户需要按照楼层进行扣减时的扣减方法
                else if (window.JoinFloorModel == true)
                {
                    //定义存放元素的列表
                    List<Element> foundationList = new List<Element>();
                    List<Element> archiWallList = new List<Element>();
                    List<Element> struWallList = new List<Element>();
                    List<Element> archiColumnList = new List<Element>();
                    List<Element> struColumnList = new List<Element>();
                    List<Element> struBeamList = new List<Element>();
                    List<Element> archiFloorList = new List<Element>();
                    List<Element> struFloorList = new List<Element>();
                    List<Element> genericModelList = new List<Element>();
                    //根据条件向每个列表中存放元素
                    foreach (string floorName in window.FloorToJoin)
                    {
                        //结构基础
                        foreach (Element elem in foundationCollector)
                        {
                            if (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_"))
                            {
                                foundationList.Add(elem);
                            }
                        }
                        //建筑墙和结构墙
                        foreach (Element elem in wallCollector)
                        {
                            foreach (Parameter para in elem.Parameters)
                            {
                                if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0) //确定是建筑墙
                                    && (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_")))                             //确定标高满足要求
                                    archiWallList.Add(elem);
                                else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1) //确认是结构墙
                                    && (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_")))                                 //确认标高满足要求
                                    struWallList.Add(elem);
                            }
                        }
                        //建筑柱
                        foreach (Element elem in archiColumnCollector)
                        {
                            if (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_"))
                            {
                                archiColumnList.Add(elem);
                            }
                        }
                        //结构柱
                        foreach (Element elem in struColumnCollector)
                        {
                            if (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_"))
                            {
                                struColumnList.Add(elem);
                            }
                        }
                        //结构梁
                        foreach (Element elem in beamCollector)
                        {
                            ;
                            if ((elem as FamilyInstance).Host.Name == floorName.Replace("__", "_"))
                            {
                                struBeamList.Add(elem);
                            }
                        }
                        //建筑板和结构板
                        foreach (Element elem in FloorCollector)
                        {
                            foreach (Parameter para in elem.Parameters)
                            {
                                if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0) //确定是建筑板
                                    && (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_")))                             //确定标高满足要求
                                    archiFloorList.Add(elem);
                                else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1) //确认是结构板
                                    && (doc.GetElement(elem.LevelId).Name == floorName.Replace("__", "_")))                                 //确认标高满足要求
                                    struFloorList.Add(elem);
                            }
                        }
                        //常规模型
                        foreach (Element elem in genericModelCollector)
                        {
                            genericModelList.Add(elem);
                        }
                    }

                    ///开始扣减
                    Transaction trans = new Transaction(doc, "根据楼层扣减模型");
                    trans.Start();
                    ///结构构件扣减建筑构件
                    //结构基础切建筑构件
                    JoinModel(doc, foundationList, archiWallList);
                    JoinModel(doc, foundationList, archiColumnList);
                    JoinModel(doc, foundationList, archiFloorList);
                    //结构墙切建筑构件
                    JoinModel(doc, struWallList, archiWallList);
                    JoinModel(doc, struWallList, archiColumnList);
                    JoinModel(doc, struWallList, archiFloorList);
                    //结构柱切建筑构件
                    JoinModel(doc, struColumnList, archiWallList);
                    JoinModel(doc, struColumnList, archiColumnList);
                    JoinModel(doc, struColumnList, archiFloorList);
                    //结构梁切建筑构件
                    JoinModel(doc, struBeamList, archiWallList);
                    JoinModel(doc, struBeamList, archiColumnList);
                    JoinModel(doc, struBeamList, archiFloorList);
                    //结构板切建筑构件
                    JoinModel(doc, struFloorList, archiWallList);
                    JoinModel(doc, struFloorList, archiColumnList);
                    JoinModel(doc, struFloorList, archiFloorList);
                    ///判断窗体返回值进行扣减
                    //结构基础的相关扣减
                    if (window.JGJC_JGQ) JoinModel(doc, foundationList, struWallList);
                    if (window.JGJC_JGZ) JoinModel(doc, foundationList, struColumnList);
                    if (window.JGJC_JGL) JoinModel(doc, foundationList, struBeamList);
                    if (window.JGJC_JGB) JoinModel(doc, foundationList, struFloorList);
                    if (window.JGJC_CGMX) JoinModel(doc, foundationList, genericModelList);
                    //结构墙的相关扣减
                    if (window.JGQ_JGJC) JoinModel(doc, struWallList, foundationList);
                    if (window.JGQ_JGZ) JoinModel(doc, struWallList, struColumnList);
                    if (window.JGQ_JGL) JoinModel(doc, struWallList, struBeamList);
                    if (window.JGQ_JGB) JoinModel(doc, struWallList, struFloorList);
                    if (window.JGQ_CGMX) JoinModel(doc, struWallList, genericModelList);
                    //结构柱的相关扣减
                    if (window.JGZ_JGJC) JoinModel(doc, struColumnList, foundationList);
                    if (window.JGZ_JGQ) JoinModel(doc, struColumnList, struWallList);
                    if (window.JGZ_JGL) JoinModel(doc, struColumnList, struBeamList);
                    if (window.JGZ_JGB) JoinModel(doc, struColumnList, struFloorList);
                    if (window.JGZ_CGMX) JoinModel(doc, struColumnList, genericModelList);
                    //结构梁的相关扣减
                    if (window.JGL_JGJC) JoinModel(doc, struBeamList, foundationList);
                    if (window.JGL_JGQ) JoinModel(doc, struBeamList, struWallList);
                    if (window.JGL_JGZ) JoinModel(doc, struBeamList, struColumnList);
                    if (window.JGL_JGB) JoinModel(doc, struBeamList, struFloorList);
                    if (window.JGL_CGMX) JoinModel(doc, struBeamList, genericModelList);
                    //结构板的相关扣减
                    if (window.JGB_JGJC) JoinModel(doc, struFloorList, foundationList);
                    if (window.JGB_JGQ) JoinModel(doc, struFloorList, struWallList);
                    if (window.JGB_JGZ) JoinModel(doc, struFloorList, struColumnList);
                    if (window.JGB_JGL) JoinModel(doc, struFloorList, struBeamList);
                    if (window.JGB_CGMX) JoinModel(doc, struFloorList, genericModelList);
                    //常规模型的相关扣减
                    if (window.CGMX_JGJC) JoinModel(doc, genericModelList, foundationList);
                    if (window.CGMX_JGQ) JoinModel(doc, genericModelList, struWallList);
                    if (window.CGMX_JGZ) JoinModel(doc, genericModelList, struColumnList);
                    if (window.CGMX_JGL) JoinModel(doc, genericModelList, struBeamList);
                    if (window.CGMX_JGB) JoinModel(doc, genericModelList, struFloorList);
                    if (window.CGMX_JZQ) JoinModel(doc, genericModelList, archiWallList);
                    if (window.CGMX_JZZ) JoinModel(doc, genericModelList, archiColumnList);
                    if (window.CGMX_JZB) JoinModel(doc, genericModelList, archiFloorList);
                    //建筑墙的相关扣减
                    if (window.JZQ_JZZ) JoinModel(doc, archiWallList, archiColumnList);
                    if (window.JZQ_JZB) JoinModel(doc, archiWallList, archiFloorList);
                    if (window.JZQ_CGMX) JoinModel(doc, archiWallList, genericModelList);
                    //建筑柱的相关扣减
                    if (window.JZZ_JZQ) JoinModel(doc, archiColumnList, archiWallList);
                    if (window.JZZ_JZB) JoinModel(doc, archiColumnList, archiFloorList);
                    if (window.JZZ_CGMX) JoinModel(doc, archiColumnList, genericModelList);
                    //建筑板的相关扣减
                    if (window.JZB_JZQ) JoinModel(doc, archiFloorList, archiWallList);
                    if (window.JZB_JZZ) JoinModel(doc, archiFloorList, archiColumnList);
                    if (window.JZB_CGMX) JoinModel(doc, archiFloorList, genericModelList);
                    trans.Commit();
                }
            }
            ///程序执行情况返回值
            return Result.Succeeded;
        }

        /// <summary>
        /// 定义用列表A的构件扣减列表B的构件的方法
        /// </summary>
        public void JoinModel(Document doc, List<Element> listA, List<Element> listB)
        {
            for (int i = 0; i < listA.Count; i++)
            {
                for (int j = 0; j < listB.Count; j++)
                {
                    if (JoinGeometryUtils.AreElementsJoined(doc, listA[i], listB[j]))//判断a、b是否连接（例：结构柱扣梁中，a、b在这里代表的是结构柱与梁）
                    {
                        JoinGeometryUtils.UnjoinGeometry(doc, listA[i], listB[j]);//a、b若已经连接则取消连接
                    }
                    try
                    {
                        JoinGeometryUtils.JoinGeometry(doc, listA[i], listB[j]);//连接a、b
                        if (!JoinGeometryUtils.IsCuttingElementInJoin(doc, listA[i], listB[j]))//判断扣减效果是否为a扣减b（前面的会扣减后面的）
                        {
                            JoinGeometryUtils.SwitchJoinOrder(doc, listA[i], listB[j]);//否则的话则转换为a扣减b
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
    }
}
