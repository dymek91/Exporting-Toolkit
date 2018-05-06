using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CryEngine
{
    class Chunk_Node_824
    {
        public Chunk header;
        uint size = 204;

        public byte[] name = new byte[64];
        public uint objectID;
        public int parentID;
        uint nChildren;
        public uint matID;
        uint _obsoleteA_;
        float[] tm0 = new float[4];
        float[] tm1 = new float[4];
        float[] tm2 = new float[4];
        float[] tm3 = new float[4];
        float[] _obsoleteB_ = new float[3];
        float[] _obsoleteC_ = new float[4];
        float[] _obsoleteD_ = new float[3];
        public int pos_cont_id;
        public int rot_cont_id;
        public int scl_cont_id;
        uint PropStrLen;
        byte[] propStr;

        public byte[] serialized;

        public Chunk_Node_824(byte[] content)
        {
            //Stream stream = new MemoryStream(content);
            using (MemoryStream stream = new MemoryStream(content))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    for (int i = 0; i < 64; i++)
                        name[i] = br.ReadByte();
                    objectID = br.ReadUInt32();
                    parentID = br.ReadInt32();
                    nChildren = br.ReadUInt32();
                    matID = br.ReadUInt32();
                    _obsoleteA_ = br.ReadUInt32();
                    for (int i = 0; i < 4; i++)
                        tm0[i] = br.ReadSingle();
                    for (int i = 0; i < 4; i++)
                        tm1[i] = br.ReadSingle();
                    for (int i = 0; i < 4; i++)
                        tm2[i] = br.ReadSingle();
                    for (int i = 0; i < 4; i++)
                        tm3[i] = br.ReadSingle();
                    for (int i = 0; i < 3; i++)
                        _obsoleteB_[i] = br.ReadSingle();
                    for (int i = 0; i < 4; i++)
                        _obsoleteC_[i] = br.ReadSingle();
                    for (int i = 0; i < 3; i++)
                        _obsoleteD_[i] = br.ReadSingle();
                    pos_cont_id = br.ReadInt32();
                    rot_cont_id = br.ReadInt32();
                    scl_cont_id = br.ReadInt32();
                    PropStrLen  = br.ReadUInt32();
                    size = size + PropStrLen;
                    propStr = new byte[PropStrLen];
                    for (int i = 0; i < PropStrLen; i++)
                        propStr[i] = br.ReadByte();
                }
            }
            
        }
        public void Serialize()
        {
            serialized = new byte[size];
            using (MemoryStream stream = new MemoryStream(serialized))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                { 
                    for (int i = 0; i < 64; i++)
                        bw.Write(name[i]);
                    bw.Write(objectID);
                    bw.Write(parentID);
                    bw.Write(nChildren);
                    bw.Write(matID);
                    bw.Write(_obsoleteA_);
                    for (int i = 0; i < 4; i++)
                        bw.Write(tm0[i]);
                    for (int i = 0; i < 4; i++)
                        bw.Write(tm1[i]);
                    for (int i = 0; i < 4; i++)
                        bw.Write(tm2[i]);
                    for (int i = 0; i < 4; i++)
                        bw.Write(tm3[i]);
                    for (int i = 0; i < 3; i++)
                        bw.Write(_obsoleteB_[i]);
                    for (int i = 0; i < 4; i++)
                        bw.Write(_obsoleteC_[i]);
                    for (int i = 0; i < 3; i++)
                        bw.Write(_obsoleteD_[i]);
                    bw.Write(pos_cont_id);
                    bw.Write(rot_cont_id);
                    bw.Write(scl_cont_id);
                    bw.Write(PropStrLen);
                    bw.Write(propStr);
                }
            }
        }
    }
}
