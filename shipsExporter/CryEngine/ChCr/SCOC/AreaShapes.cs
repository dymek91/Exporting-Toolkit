using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace CryEngine
{
    class AreaShapes
    {
        ///// <summary>
        ///// 0x0000 0090 - visarea;
        ///// 0x0000 0093 - 3d_visarea
        ///// </summary>
        ///// <summary>
        ///// 0x0000 0038 - portal;
        ///// 0x0000 0018 - childportal_3d_visarea
        ///// </summary>
        ///// 

        List<AreaShape_Portal_Type18> portalType18 = new List<AreaShape_Portal_Type18>();
        List<AreaShape_Portal_Type38> portalType38 = new List<AreaShape_Portal_Type38>();
        List<AreaShape_VisArea_Type90> visAreaType90 = new List<AreaShape_VisArea_Type90>();
        List<AreaShape_VisArea_Type91> visAreaType91 = new List<AreaShape_VisArea_Type91>();
        List<AreaShape_VisArea_Type93> visAreaType93 = new List<AreaShape_VisArea_Type93>();
        List<AreaShape_VisArea_TypeB0> visAreaTypeB0 = new List<AreaShape_VisArea_TypeB0>();

        int visAreasCount;
        int portalsCount;

        List<Portal> portals = new List<Portal>();
        List<VisArea> visAreas = new List<VisArea>();

        List<PrefabObject> prefabObjects = new List<PrefabObject>();

        public AreaShapes(byte[] areaShapesStream,int visCount,int porCount)
        {
            visAreasCount = visCount;
            portalsCount = porCount;

            using (MemoryStream stream = new MemoryStream(areaShapesStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int streamSize = areaShapesStream.Length;
                    int iter = 0;
                    while (br.BaseStream.Position < streamSize)
                    {
                        if (iter >= visAreasCount + portalsCount) break;
                        bool cancel = false;
                        br.ReadUInt32();
                        string areaShapeName = new string(br.ReadChars(32)).TrimEnd('\0');
                        uint type = br.ReadUInt32();
                        switch (type)
                        {
                            case AreaShape_Portal_Type18.type:
                                portalType18.Add(new AreaShape_Portal_Type18(br, areaShapeName));
                                break;
                            case AreaShape_Portal_Type38.type:
                                portalType38.Add(new AreaShape_Portal_Type38(br, areaShapeName));
                                break;
                            case AreaShape_VisArea_Type90.type:
                                visAreaType90.Add(new AreaShape_VisArea_Type90(br, areaShapeName));
                                break;
                            case AreaShape_VisArea_Type91.type:
                                visAreaType91.Add(new AreaShape_VisArea_Type91(br, areaShapeName));
                                break;
                            case AreaShape_VisArea_Type93.type:
                                visAreaType93.Add(new AreaShape_VisArea_Type93(br, areaShapeName));
                                break;
                            case AreaShape_VisArea_TypeB0.type:
                                visAreaTypeB0.Add(new AreaShape_VisArea_TypeB0(br, areaShapeName));
                                break;
                            default:
                                cancel = true;
                                Console.WriteLine("Unknown AreaShape type: {0}", type);
                                Console.WriteLine("Canceling reading AreaShapes from current chunk");
                               // Console.Read();
                               // throw new System.ArgumentException("Unknown AreaShape type", "original");
                                break;
                        }
                        if (cancel) break;
                        iter++;
                    }
                }
            }
            CombinePortals();
            CombineVisAreas();
            MakePrefabObjects();
        }
        public List<Portal> GetPortals()
        {
            return portals;
        }
        public List<VisArea> GetVisAreas()
        {
            return visAreas;
        }
        public List<PrefabObject> GetPrefabObjects()
        {
            return prefabObjects;
        }
        void CombinePortals()
        {
            foreach(AreaShape_Portal_Type18 portal in portalType18)
            {
                portals.Add(new Portal(portal.GetName(),portal.GetPosition(),portal.GetRotation(),portal.GetHeight(),portal.GetPoints(), AreaShape_Portal_Type18.type));
            }
            foreach (AreaShape_Portal_Type38 portal in portalType38)
            {
                portals.Add(new Portal(portal.GetName(), portal.GetPosition(), portal.GetRotation(), portal.GetHeight(), portal.GetPoints(), AreaShape_Portal_Type38.type));
            }
        }
        void CombineVisAreas()
        {
            foreach (AreaShape_VisArea_Type90 visarea in visAreaType90)
            {
                visAreas.Add(new VisArea(visarea.GetName(), visarea.GetPosition(), visarea.GetRotation(), visarea.GetHeight(), visarea.GetPoints(), AreaShape_VisArea_Type90.type));
            }
            foreach (AreaShape_VisArea_Type91 visarea in visAreaType91)
            {
                visAreas.Add(new VisArea(visarea.GetName(), visarea.GetPosition(), visarea.GetRotation(), visarea.GetHeight(), visarea.GetPoints(), AreaShape_VisArea_Type91.type));
            }
            foreach (AreaShape_VisArea_Type93 visarea in visAreaType93)
            {
                visAreas.Add(new VisArea(visarea.GetName(), visarea.GetPosition(), visarea.GetRotation(), visarea.GetHeight(), visarea.GetPoints(), AreaShape_VisArea_Type93.type));
            }
            foreach (AreaShape_VisArea_TypeB0 visarea in visAreaTypeB0)
            {
                visAreas.Add(new VisArea(visarea.GetName(), visarea.GetPosition(), visarea.GetRotation(), visarea.GetHeight(), visarea.GetPoints(), AreaShape_VisArea_TypeB0.type));
            }
        }
        void MakePrefabObjects()
        {
            foreach (VisArea sc in visAreas)
            {
                if (sc.GetVisAreaType() == 0x00000090 || sc.GetVisAreaType() == 0x00000091 || sc.GetVisAreaType() == 0x000000B0)
                {
                    PrefabObject prefabObj = new PrefabObject();
                    prefabObj.Name = sc.GetName();
                    prefabObj.SetPos(sc.GetPosition());
                    prefabObj.SetRotate(sc.GetRotation());
                    prefabObj.Height = sc.GetHeight().ToString();
                    prefabObj.Type = "VisArea";
                    prefabObj.DisplayFilled = "1";

                    XElement elPoints = new XElement("Points");
                    foreach (Point3D pt in sc.GetPoints())
                    {
                        XElement elPoint = new XElement("Point");
                        elPoint.Add(new XAttribute("Pos", pt.Pos));
                        elPoints.Add(elPoint);
                    }
                    prefabObj.Points = elPoints;
                    prefabObjects.Add(prefabObj);
                }
            }
            foreach (Portal sc in portals)
            {
                if (sc.GetPortalType() == 0x00000038)
                {
                    PrefabObject prefabObj = new PrefabObject();
                    prefabObj.Name = sc.GetName();
                    prefabObj.SetPos(sc.GetPosition());
                    prefabObj.SetRotate(sc.GetRotation());
                    prefabObj.Height = sc.GetHeight().ToString();
                    prefabObj.Type = "Portal";
                    prefabObj.DisplayFilled = "1";
                    prefabObj.LightBlending = "0";
                    prefabObj.LightBlendValue = "0";

                    XElement elPoints = new XElement("Points");
                    foreach (Point3D pt in sc.GetPoints())
                    {
                        XElement elPoint = new XElement("Point");
                        elPoint.Add(new XAttribute("Pos", pt.Pos));
                        elPoints.Add(elPoint);
                    }
                    prefabObj.Points = elPoints;
                    prefabObjects.Add(prefabObj);
                }
            }
        }
        public PrefabObject GetPrefabObjByName(string name)
        {
            PrefabObject probj = new PrefabObject();
            foreach (PrefabObject pr in prefabObjects)
            {
                if (pr.Name.ToLower() == name.ToLower()) probj = pr;
            }
            return probj;
        }
    }
}
