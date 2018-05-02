using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    //SC exclusive
    class Objects
    {
        string[] filesPaths;

        List<Object_Type1> objectType1 = new List<Object_Type1>();
        List<Object_Type7> objectType7 = new List<Object_Type7>();
        List<Object_Type10> objectType10 = new List<Object_Type10>();

        public Objects(byte[] objectsStream, string[] objFilesPaths)
        {
            filesPaths = objFilesPaths;

            using (MemoryStream stream = new MemoryStream(objectsStream))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int streamSize = objectsStream.Length;
                    int type1Iter = 0;
                    while(br.BaseStream.Position<streamSize)
                    {
                        bool cancel = false;
                        uint type = br.ReadUInt32();
                        switch(type)
                        {
                            case Object_Type1.type:
                                objectType1.Add(new Object_Type1(br, filesPaths));
                                type1Iter++;
                                break;
                            case Object_Type7.type:
                                objectType7.Add(new Object_Type7(br));
                                break;
                            case Object_Type10.type:
                                objectType10.Add(new Object_Type10(br));
                                break;
                            default:
                                cancel = true;
                                Console.WriteLine("Unknown Object type: {0}",type);
                                Console.WriteLine("Canceling reading objects from current chunk");
                                //Console.Read();
                                //throw new System.ArgumentException("Unknown Object type", "original");
                                break;
                        }
                        if (cancel) break; 
                    }
                }
            }
        }
        public List<Object_Type1> GetObject_Type1()
        {
            return objectType1;
        }
        public int GetFilesCount()
        {
            return filesPaths.Length;
        }
        public string GetFilePath(int id)
        {
            return filesPaths[id];
            //if (id < filesPaths.Count())
            //{
            //    return filesPaths[id];
            //}
            //else
            //{
            //    return "";
            //}
        }
    }
}
