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

            //获取结构基础
            FilteredElementCollector foundationCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFoundation).OfClass(typeof(FamilyInstance));
            List<Element> foundationList = foundationCollector.ToList();

            //获取建筑墙与结构墙
            List<Wall> archiWallList = new List<Wall>();
            List<Wall> struWallList = new List<Wall>();
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
            List<Element> struColumnList = archiColumnCollector.ToList();

            //获取梁
            FilteredElementCollector beamCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming).OfClass(typeof(FamilyInstance));
            List<Element> beamList = beamCollector.ToList();

            //获取建筑板和结构板
            FilteredElementCollector FloorCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).OfClass(typeof(Floor));
            List<Floor> archiFloorList = new List<Floor>();
            List<Floor> struFloorList = new List<Floor>();
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

            //获取楼梯
            //FilteredElementCollector stairCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Stairs).OfClass(typeof());


            JoinGeometry_Window joinGeometry_Window = new JoinGeometry_Window();
            joinGeometry_Window.ShowDialog();








            return Result.Succeeded;
        }

    }
}
