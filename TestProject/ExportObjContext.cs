using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XY_Tools_Project
{
    class ExportObjContext : IExportContext
    {
        #region 定义需要使用的字段
        Document m_doc;
        Stack<Transform> m_TransformationStack = new Stack<Transform>();
        ElementId currentMaterialId;  // 传递材质ID
        Color currentColor;                  // 传递材质颜色
        double currentTransparency;          // 传递材质透明底
        Asset currentAsset;           // 传递材质贴图集合
        string textureFolder = "";          // 用于存储默认材质贴图的位置
        #endregion

        StreamWriter swObj = new StreamWriter(@"E:\test.obj");  //用于输出几何实体信息
        StreamWriter swMtl = new StreamWriter(@"E:\test.mtl");  // 用于输出材质信息
        ElementId currentId;
        int index = 0;

        const string strNewmtl = "\n" + "newmtl {0}\n" + "ka {1} {2} {3}\n" + "kd {1} {2} {3}\n" + "d {4}\n";
        #region 定义需要使用的属性
        #endregion


        #region 定义需要使用的方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="doc"></param>
        public ExportObjContext(Document doc)
        {
            m_doc = doc;
            m_TransformationStack.Push(Transform.Identity);
        }
        private Asset FindTextureAsset(AssetProperty ap)
        {
            Asset result = null;
            if (ap.Type == AssetPropertyType.Asset)
            {
                if (!IsTextureAsset(ap as Asset))
                {
                    for (int i = 0; i < (ap as Asset).Size; i++)
                    {
                        if (null != FindTextureAsset((ap as Asset)[i]))
                        {
                            result = FindTextureAsset((ap as Asset)[i]);
                            break;
                        }
                    }
                }
                else
                {
                    result = ap as Asset;
                }
                return result;
            }
            else
            {
                for (int j = 0; j < ap.NumberOfConnectedProperties; j++)
                {
                    if (null != FindTextureAsset(ap.GetConnectedProperty(j)))
                    {
                        result = FindTextureAsset(ap.GetConnectedProperty(j));
                    }
                }
                return result;
            }
        }
        private bool IsTextureAsset(Asset asset)
        {
            AssetProperty assetProprty = GetAssetProprty(asset, "assettype");
            if (assetProprty != null && (assetProprty as AssetPropertyString).Value == "texture")
            {
                return true;
            }
            return GetAssetProprty(asset, "unifiedbitmap_Bitmap") != null;
        }
        private AssetProperty GetAssetProprty(Asset asset, string propertyName)
        {
            for (int i = 0; i < asset.Size; i++)
            {
                if (asset[i].Name == propertyName)
                {
                    return asset[i];
                }
            }
            return null;
        }
        #endregion



        public void Finish()
        {
            //程序结束后关闭steamwriter
            swObj.Close();
            swMtl.Close();
        }

        public bool IsCanceled()
        {
            return false;
        }

        public RenderNodeAction OnElementBegin(ElementId elementId)
        {
            currentId = elementId;
            return RenderNodeAction.Proceed;
        }

        public void OnElementEnd(ElementId elementId)
        {
            
        }

        public RenderNodeAction OnFaceBegin(FaceNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnFaceEnd(FaceNode node)
        {
            
        }

        public RenderNodeAction OnInstanceBegin(InstanceNode node)
        {
            m_TransformationStack.Push(m_TransformationStack.Peek().Multiply(node.GetTransform()));
            return RenderNodeAction.Proceed;
        }

        public void OnInstanceEnd(InstanceNode node)
        {
            m_TransformationStack.Pop();
        }

        public void OnLight(LightNode node)
        {
            
        }

        public RenderNodeAction OnLinkBegin(LinkNode node)
        {
            m_TransformationStack.Push(m_TransformationStack.Peek().Multiply(node.GetTransform()));
            return RenderNodeAction.Proceed;
        }

        public void OnLinkEnd(LinkNode node)
        {
            m_TransformationStack.Pop();
        }

        public void OnMaterial(MaterialNode node)
        {
            if (currentMaterialId != node.MaterialId)
            {
                currentMaterialId = node.MaterialId;
                currentColor = node.Color;
                currentTransparency = node.Transparency;
                swMtl.Write(strNewmtl, currentMaterialId.IntegerValue.ToString(),
                (currentColor.Red / 256.0).ToString(), (currentColor.Green / 256.0).ToString(), (currentColor.Blue / 256.0).ToString(),
                currentTransparency);
                if (node.HasOverriddenAppearance)
                {
                    currentAsset = node.GetAppearanceOverride();
                }
                else
                {
                    currentAsset = node.GetAppearance();
                }
                //取得Asset中贴图信息
                // string textureFile = (FindTextureAsset(currentAsset as AssetProperty).FindByName("unifiedbitmap_Bitmap") as AssetPropertyString).Value.Split('|')[0];
                string textureFile = "";
                //用Asset中贴图信息和注册表里的材质库地址得到贴图文件所在位置
                string texturePath = Path.Combine(textureFolder, textureFile.Replace("/", "\\"));
                //写入贴图名称
                swMtl.Write("map_Kd " + Path.GetFileName(texturePath) + "\n");
                //如果贴图文件真实存在，就复制到相应位置
                if (File.Exists(texturePath))
                {
                    File.Copy(texturePath, Path.Combine("E:\\", Path.GetFileName(texturePath)), true);
                }
            }

        }

        public void OnPolymesh(PolymeshTopology node)
        {
            //把当前ElementId作为对象名写入文件
            swObj.Write("o " + currentId.IntegerValue.ToString() + "\n");

            //取得顶点坐标并进行位置转换
            Transform currentTransform = m_TransformationStack.Peek();
            IList<XYZ> points = node.GetPoints();
            points = points.Select(p => currentTransform.OfPoint(p)).ToList();

            //把顶点数据写入文件
            foreach (XYZ point in points)
            {
                swObj.Write("v " + point.X.ToString() + " " + point.Y.ToString() + " " + point.Z.ToString() + "\n");
            }

            //取得UV坐标
            IList<UV> uvs = node.GetUVs();

            //把UV数据写入文件
            foreach (UV uv in uvs)
            {
                swObj.Write("vt " + uv.U.ToString() + " " + uv.V.ToString() + " 0.0000\n");
            }
            // 把材质信息写入文件
            swObj.Write("usemtl " + currentMaterialId.IntegerValue.ToString() + "\n");

            //取得面
            IList<PolymeshFacet> facets = node.GetFacets();

            //把面数据写入文件
            foreach (PolymeshFacet facet in facets)
            {
                swObj.Write("f " + (facet.V1 + 1 + index).ToString() + "/" + (facet.V1 + 1 + index).ToString() + " " + (facet.V2 + 1 + index).ToString() + "/" + (facet.V2 + 1 + index).ToString() + " " + (facet.V3 + 1 + index).ToString() + "/" + (facet.V3 + 1 + index).ToString() + "\n");
            }
            index += node.NumberOfPoints;
        }

        public void OnRPC(RPCNode node)
        {
            
        }

        public RenderNodeAction OnViewBegin(ViewNode node)
        {
            return RenderNodeAction.Proceed;
        }

        public void OnViewEnd(ElementId elementId)
        {
            
        }

        public bool Start()
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey libraryPath = hklm.OpenSubKey("SOFTWARE\\Wow6432Node\\Autodesk\\ADSKAdvancedTextureLibrary\\1");
            textureFolder = libraryPath.GetValue("LibraryPaths").ToString();
            hklm.Close();
            libraryPath.Close();
            return true;
        }
    }
}
