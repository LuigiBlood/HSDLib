using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSDLib.Common;

namespace HSDLib.KAR
{
    // grModel*
    public class KAR_GroundModel : IHSDNode
    {
        [FieldData(typeof(KAR_GroundModel_Main))]
        public KAR_GroundModel_Main MainModel { get; set; }

        [FieldData(typeof(KAR_GroundModel_Skybox))]
        public KAR_GroundModel_Skybox SkyboxModel { get; set; }

        [FieldData(typeof(KAR_GroundModel_Unk3))]
        public KAR_GroundModel_Unk3 Unk3 { get; set; }

        [FieldData(typeof(KAR_GroundModel_Unk4))]
        public KAR_GroundModel_Unk4 Unk4 { get; set; }
    }

    public class KAR_GroundModel_Main : IHSDNode
    {
        [FieldData(typeof(HSD_JOBJ))]
        public HSD_JOBJ JOBJ { get; set; }

        [FieldData(typeof(int))]
        public int Unk1 { get; set; }

        [FieldData(typeof(int))]
        public int Unk2 { get; set; }

        [FieldData(typeof(int))]
        public int Unk3 { get; set; }

        [FieldData(typeof(uint), Editable: false, Viewable: false)]
        public uint ModelUnkOffset { get; set; }

        public KAR_GroundModel_ModelUnk[] ModelUnkArray;   //There are 4 of them

        public override void Open(HSDReader Reader)
        {
            base.Open(Reader);

            ModelUnkArray = new KAR_GroundModel_ModelUnk[4];

            Reader.Seek(ModelUnkOffset);
            for (int i = 0; i < ModelUnkArray.Length; i++)
            {
                ModelUnkArray[i] = new KAR_GroundModel_ModelUnk();
                ModelUnkArray[i].Open(Reader);
            }
        }
    }

    public class KAR_GroundModel_Skybox : IHSDNode
    {
        [FieldData(typeof(HSD_JOBJ))]
        public HSD_JOBJ JOBJ { get; set; }

        [FieldData(typeof(int))]
        public int Unk { get; set; }
    }

    public class KAR_GroundModel_Unk3 : IHSDNode
    {

    }

    public class KAR_GroundModel_Unk4 : IHSDNode
    {

    }

    public class KAR_GroundModel_ModelUnk : IHSDNode
    {
        [FieldData(typeof(uint), Editable: false, Viewable: false)]
        public uint UnkArrayOffset { get; set; }

        [FieldData(typeof(short))]
        public short Amount { get; set; }

        [FieldData(typeof(short), Editable: false, Viewable: false)]
        public short Padding { get; set; }

        public short[] UnkArray;

        public override void Open(HSDReader Reader)
        {
            base.Open(Reader);
            uint Offset = Reader.Position();

            UnkArray = new short[Amount];

            Reader.Seek(UnkArrayOffset);
            for (int i = 0; i < UnkArray.Length; i++)
            {
                UnkArray[i] = Reader.ReadInt16();
            }

            Reader.Seek(Offset);
        }
    }

    // grModelMotion*
    public class KAR_GroundModelMotion : IHSDNode
    {

    }
}
