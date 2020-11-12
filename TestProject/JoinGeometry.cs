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
            FilteredElementCollector columnCollector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Columns).OfClass(typeof(FamilyInstance));








            return Result.Succeeded;
        }
    }
}
