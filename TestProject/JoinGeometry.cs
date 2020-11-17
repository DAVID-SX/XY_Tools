using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

            //获取结构基础
            FilteredElementCollector foundationCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFoundation).OfClass(typeof(FamilyInstance));
            List<Element> foundationList = foundationCollector.ToList();

            //获取建筑墙与结构墙
            List<Element> archiWallList = new List<Element>();
            List<Element> struWallList = new List<Element>();
            FilteredElementCollector wallCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall));
            foreach (var elem in wallCollector)
            {
                Wall wall = elem as Wall;
                foreach (Parameter para in wall.Parameters)
                {
                    if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiWallList.Add(wall);
                    else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struWallList.Add(wall);
                }
            }

            //获取建筑柱和结构柱
            FilteredElementCollector archiColumnCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).OfClass(typeof(FamilyInstance));
            List<Element> archiColumnList = archiColumnCollector.ToList();
            FilteredElementCollector struColumnCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns).OfClass(typeof(FamilyInstance));
            List<Element> struColumnList = struColumnCollector.ToList();

            //获取梁
            FilteredElementCollector beamCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).OfClass(typeof(FamilyInstance));
            List<Element> struBeamList = beamCollector.ToList();

            //获取建筑板和结构板
            FilteredElementCollector FloorCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).OfClass(typeof(Floor));
            List<Element> archiFloorList = new List<Element>();
            List<Element> struFloorList = new List<Element>();
            foreach (var elem in FloorCollector)
            {
                Floor floor = elem as Floor;
                foreach (Parameter para in floor.Parameters)
                {
                    if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 0)) archiFloorList.Add(floor);
                    else if ((para.Definition.Name == "结构" || para.Definition.Name == "Structural") && (para.AsInteger() == 1)) struFloorList.Add(floor);
                }
            }

            //获取常规模型
            FilteredElementCollector genericModelCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_GenericModel).OfClass(typeof(FamilyInstance));
            List<Element> genericModelList = genericModelCollector.ToList();


            //获取标高
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).OfClass(typeof(Level));
            List<Element> levelList = levelCollector.ToList();
            foreach (Element elem in levelList)
            {
                cache.LevelNameList.Add(elem.Name);
            }

            //string result = "结构基础：  " + foundationList.Count().ToString() + "\n" +
            //    "结构墙：  " + struWallList.Count().ToString() + "\n" +
            //    "结构柱：  " + struColumnList.Count().ToString() + "\n" +
            //    "结构梁：  " + struBeamList.Count().ToString() + "\n" +
            //    "结构板：  " + struFloorList.Count().ToString() + "\n" +
            //    "结构梯：  " + "\n" +
            //    "常规模型：  " + genericModelList.Count().ToString() + "\n" +
            //    "建筑墙：  " + archiWallList.Count().ToString() + "\n" +
            //    "建筑柱：  " + archiColumnList.Count().ToString() + "\n" +
            //    "建筑板：  " + archiFloorList.Count().ToString() + "\n";
            //TaskDialog.Show("统计结果", result);



            JoinGeometry_Window joinGeometry_Window = new JoinGeometry_Window(cache);
            joinGeometry_Window.ShowDialog();

            if (joinGeometry_Window.isClickConfirm == true)
            {
                Transaction trans = new Transaction(doc, "扣减模型");
                trans.Start();
                ///结构构件扣减建筑构件
                //结构基础切建筑构件
                JoinModel(doc, foundationList, archiWallList);
                JoinModel(doc, foundationList, archiWallList);
                JoinModel(doc, foundationList, archiFloorList);
                //结构墙切建筑构件
                JoinModel(doc, struWallList, archiWallList);
                JoinModel(doc, struWallList, archiWallList);
                JoinModel(doc, struWallList, archiFloorList);
                //结构柱切建筑构件
                JoinModel(doc, struColumnList, archiWallList);
                JoinModel(doc, struColumnList, archiWallList);
                JoinModel(doc, struColumnList, archiFloorList);
                //结构梁切建筑构件
                JoinModel(doc, struBeamList, archiWallList);
                JoinModel(doc, struBeamList, archiWallList);
                JoinModel(doc, struBeamList, archiFloorList);
                //结构板切建筑构件
                JoinModel(doc, struFloorList, archiWallList);
                JoinModel(doc, struFloorList, archiWallList);
                JoinModel(doc, struFloorList, archiFloorList);
                ///判断窗体返回值进行扣减
                //结构基础的相关扣减
                if (joinGeometry_Window.JGJC_JGQ) JoinModel(doc, foundationList, struWallList);
                if (joinGeometry_Window.JGJC_JGZ) JoinModel(doc, foundationList, struColumnList);
                if (joinGeometry_Window.JGJC_JGL) JoinModel(doc, foundationList, struBeamList);
                if (joinGeometry_Window.JGJC_JGB) JoinModel(doc, foundationList, struFloorList);
                if (joinGeometry_Window.JGJC_CGMX) JoinModel(doc, foundationList, genericModelList);
                //结构墙的相关扣减
                if (joinGeometry_Window.JGQ_JGJC) JoinModel(doc, struWallList, foundationList);
                if (joinGeometry_Window.JGQ_JGZ) JoinModel(doc, struWallList, struColumnList);
                if (joinGeometry_Window.JGQ_JGL) JoinModel(doc, struWallList, struBeamList);
                if (joinGeometry_Window.JGQ_JGB) JoinModel(doc, struWallList, struBeamList);
                if (joinGeometry_Window.JGQ_CGMX) JoinModel(doc, struWallList, genericModelList);
                //结构柱的相关扣减
                if (joinGeometry_Window.JGZ_JGJC) JoinModel(doc, struColumnList, foundationList);
                if (joinGeometry_Window.JGZ_JGQ) JoinModel(doc, struColumnList, struWallList);
                if (joinGeometry_Window.JGZ_JGL) JoinModel(doc, struColumnList, struBeamList);
                if (joinGeometry_Window.JGZ_JGB) JoinModel(doc, struColumnList, struFloorList);
                if (joinGeometry_Window.JGZ_CGMX) JoinModel(doc, struColumnList, genericModelList);
                //结构梁的相关扣减
                if (joinGeometry_Window.JGL_JGJC) JoinModel(doc, struBeamList, foundationList);
                if (joinGeometry_Window.JGL_JGQ) JoinModel(doc, struBeamList, struWallList);
                if (joinGeometry_Window.JGL_JGZ) JoinModel(doc, struBeamList, struColumnList);
                if (joinGeometry_Window.JGL_JGB) JoinModel(doc, struBeamList, struFloorList);
                if (joinGeometry_Window.JGL_CGMX) JoinModel(doc, struBeamList, genericModelList);
                //结构板的相关扣减
                if (joinGeometry_Window.JGB_JGJC) JoinModel(doc, struFloorList, foundationList);
                if (joinGeometry_Window.JGB_JGQ) JoinModel(doc, struFloorList, struWallList);
                if (joinGeometry_Window.JGB_JGZ) JoinModel(doc, struFloorList, struColumnList);
                if (joinGeometry_Window.JGB_JGL) JoinModel(doc, struFloorList, struBeamList);
                if (joinGeometry_Window.JGB_CGMX) JoinModel(doc, struFloorList, genericModelList);
                //常规模型的相关扣减
                if (joinGeometry_Window.CGMX_JGJC) JoinModel(doc, genericModelList, foundationList);
                if (joinGeometry_Window.CGMX_JGQ) JoinModel(doc, genericModelList, struWallList);
                if (joinGeometry_Window.CGMX_JGZ) JoinModel(doc, genericModelList, struColumnList);
                if (joinGeometry_Window.CGMX_JGL) JoinModel(doc, genericModelList, struBeamList);
                if (joinGeometry_Window.CGMX_JGB) JoinModel(doc, genericModelList, struFloorList);
                if (joinGeometry_Window.CGMX_JZQ) JoinModel(doc, genericModelList, archiWallList);
                if (joinGeometry_Window.CGMX_JZZ) JoinModel(doc, genericModelList, archiColumnList);
                if (joinGeometry_Window.CGMX_JZB) JoinModel(doc, genericModelList, archiFloorList);
                //建筑墙的相关扣减
                if (joinGeometry_Window.JZQ_JZZ) JoinModel(doc, archiWallList, archiColumnList);
                if (joinGeometry_Window.JZQ_JZB) JoinModel(doc, archiWallList, archiFloorList);
                if (joinGeometry_Window.JZQ_CGMX) JoinModel(doc, archiWallList, genericModelList);
                //建筑柱的相关扣减
                if (joinGeometry_Window.JZZ_JZQ) JoinModel(doc, archiColumnList, archiWallList);
                if (joinGeometry_Window.JZZ_JZB) JoinModel(doc, archiColumnList, archiFloorList);
                if (joinGeometry_Window.JZZ_CGMX) JoinModel(doc, archiColumnList, genericModelList);
                //建筑板的相关扣减
                if (joinGeometry_Window.JZB_JZQ) JoinModel(doc, archiFloorList, archiWallList);
                if (joinGeometry_Window.JZB_JZZ) JoinModel(doc, archiFloorList, archiColumnList);
                if (joinGeometry_Window.JZB_CGMX) JoinModel(doc, archiFloorList, genericModelList);
                trans.Commit();
            }
            return Result.Succeeded;
        }

        /// <summary>
        /// 定义用列表A的构件扣减列表B的构件的方法
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        public void JoinModel(Document doc, List<Element> listA, List<Element> listB)//用a扣b
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
