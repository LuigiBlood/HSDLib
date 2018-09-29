using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSDLib.KAR
{
    public class KAR_GroundData : IHSDNode
    {
        [FieldData(typeof(int))]
        public int Unk1 { get; set; }

        [FieldData(typeof(KAR_GroundData_StageNode))]
        public KAR_GroundData_StageNode StageData { get; set; }

        [FieldData(typeof(int))]
        public int Unk2 { get; set; }

        [FieldData(typeof(int))]
        public int Unk3 { get; set; }

        [FieldData(typeof(int))]
        public int Unk4 { get; set; }

        [FieldData(typeof(KAR_GroundData_LightNode))]
        public KAR_GroundData_LightNode LightData { get; set; }
    }

    public class KAR_GroundData_StageNode : IHSDNode
    {
        [FieldData(typeof(int))]
        public int Unk1 { get; set; }

        [FieldData(typeof(float))]
        public float Unk2 { get; set; }

        [FieldData(typeof(float))]
        public float StageScale { get; set; }

        [FieldData(typeof(float))]
        public float Unk3 { get; set; }

        [FieldData(typeof(float))]
        public float GravityX { get; set; }

        [FieldData(typeof(float))]
        public float GravityY { get; set; }

        [FieldData(typeof(float))]
        public float GravityZ { get; set; }

        [FieldData(typeof(int))]
        public int FogFlag { get; set; }

        [FieldData(typeof(float))]
        public float Unk4 { get; set; }

        [FieldData(typeof(float))]
        public float Unk5 { get; set; }

        [FieldData(typeof(float))]
        public float Unk6 { get; set; }

        [FieldData(typeof(float))]
        public float Unk7 { get; set; }

        [FieldData(typeof(float))]
        public float Unk8 { get; set; }

        [FieldData(typeof(float))]
        public float Unk9 { get; set; }

        [FieldData(typeof(float))]
        public float Unk10 { get; set; }

        [FieldData(typeof(float))]
        public float Unk11 { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Wall { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Destroyable { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Moving { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Unk1 { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Unk2 { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Unk3 { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Unk4 { get; set; }

        [FieldData(typeof(float))]
        public float Coeff_Unk5 { get; set; }

        [FieldData(typeof(float))]
        public float Minimap_Scale { get; set; }

        [FieldData(typeof(float))]
        public float Minimap_PlayerPositionAdjustX { get; set; }

        [FieldData(typeof(float))]
        public float Minimap_PlayerPositionAdjustY { get; set; }

        [FieldData(typeof(float))]
        public float Minimap_PlayerPositionAdjustZ { get; set; }

        [FieldData(typeof(int))]
        public int Unk12 { get; set; }

        [FieldData(typeof(int))]
        public int Unk13 { get; set; }

        [FieldData(typeof(int))]
        public int Unk14 { get; set; } // Is a pointer but not caring here

        [FieldData(typeof(int))]
        public int Unk15 { get; set; } // Is a pointer but not caring here

        [FieldData(typeof(int))]
        public int Flags { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostPadH { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostPadH_Unk { get; set; }

        [FieldData(typeof(float))]
        public float AccelerationTime_BoostPadH { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostPadL { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostPadL_Unk { get; set; }

        [FieldData(typeof(float))]
        public float AccelerationTime_BoostPadL { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostGateH { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostGateH_Unk { get; set; }

        [FieldData(typeof(float))]
        public float AccelerationTime_BoostGateH { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostGateL { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostGateL_Unk { get; set; }

        [FieldData(typeof(float))]
        public float AccelerationTime_BoostGateL { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostRing { get; set; }

        [FieldData(typeof(float))]
        public float Acceleration_BoostRing_Unk { get; set; }

        [FieldData(typeof(float))]
        public float AccelerationTime_BoostRing { get; set; }

        [FieldData(typeof(float))]
        public float Unk16 { get; set; }

        [FieldData(typeof(float))]
        public float Unk17 { get; set; }

        [FieldData(typeof(float))]
        public float Unk18 { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Xmin { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Ymin { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Zmin { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Xmax { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Ymax { get; set; }

        [FieldData(typeof(float))]
        public float AreaBound_Zmax { get; set; }

        [FieldData(typeof(int))]
        public int Unk19 { get; set; } // Is a pointer but not caring here
    }

    public class KAR_GroundData_LightNode : IHSDNode
    {

    }
}
