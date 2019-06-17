using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSDLib.GX;
using HSDLib.Helpers;

namespace HSDLib.Common
{
    public class HSD_SOBJ : IHSDNode
    {
        [FieldData(typeof(HSD_SceneModelSet))]
        public HSD_SceneModelSet joints { get; set; }

        [FieldData(typeof(uint))]
        public uint cameraOffset { get; set; }

        [FieldData(typeof(uint))]
        public uint lightOffset { get; set; }

        [FieldData(typeof(uint))]
        public uint fogOffset { get; set; }
    }

    public class HSD_SceneModelSet : IHSDNode
    {
        [FieldData(typeof(HSD_PointerArray<HSD_JOBJ>))]
        public HSD_PointerArray<HSD_JOBJ> jointArray { get; set; }
    }
}
