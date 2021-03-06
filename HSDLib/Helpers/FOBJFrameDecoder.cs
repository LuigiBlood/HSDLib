﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSDLib.Animation;
using System.IO;

namespace HSDLib.Helpers
{
    public class FOBJKey
    {
        public float Frame;
        public float Value;
        public float Tan;
        public InterpolationType InterpolationType;
    }

    public class FOBJFrameDecoder : IDisposable
    {
        public HSD_FOBJ FOBJ { get; set; }
        private HSDReader Reader;
        private MemoryStream Stream;

        public FOBJFrameDecoder(HSD_FOBJ FOBJ)
        {
            Stream = new MemoryStream(FOBJ.Data);
            Reader = new HSDReader(Stream);
            this.FOBJ = FOBJ;
        }

        public List<FOBJKey> GetKeys(float FrameCount = -1)
        {
            List<FOBJKey> Keys = new List<FOBJKey>();
            int clock = 0;
            Reader.Seek(0);
            while (Reader.Position() < FOBJ.Data.Length)
            {
                int type = Reader.ExtendedByte();
                InterpolationType interpolation = (InterpolationType)((type) & 0x0F);
                int numOfKey = ((type >> 4)) + 1;
                if (interpolation == 0) break;

                for (int i = 0; i < numOfKey; i++)
                {
                    double value = 0;
                    double tan = 0;
                    int time = 0;

                    switch (interpolation)
                    {
                        case InterpolationType.Step:
                            value = ReadVal(Reader, FOBJ.ValueFormat, FOBJ.ValueScale);
                            time = Reader.ExtendedByte();
                            break;
                        case InterpolationType.Linear:
                            value = ReadVal(Reader, FOBJ.ValueFormat, FOBJ.ValueScale);
                            time = Reader.ExtendedByte();
                            break;
                        case InterpolationType.HermiteValue:
                            value = ReadVal(Reader, FOBJ.ValueFormat, FOBJ.ValueScale);
                            time = Reader.ExtendedByte();
                            break;
                        case InterpolationType.Hermite:
                            value = ReadVal(Reader, FOBJ.ValueFormat, FOBJ.ValueScale);
                            tan = ReadVal(Reader, FOBJ.TanFormat, FOBJ.TanScale);
                            time = Reader.ExtendedByte();
                            break;
                        case InterpolationType.HermiteCurve:
                            tan = ReadVal(Reader, FOBJ.TanFormat, FOBJ.TanScale);
                            break;
                        case InterpolationType.Constant:
                            value = ReadVal(Reader, FOBJ.ValueFormat, FOBJ.ValueScale);
                            break;
                        default:
                            throw new Exception("Unknown Interpolation Type " + interpolation.ToString("X"));
                    }

                    FOBJKey kf = new FOBJKey();
                    kf.InterpolationType = interpolation;
                    kf.Value = (float)value;
                    kf.Frame = clock;
                    kf.Tan = (float)(tan);
                    Keys.Add(kf);
                    clock += time;
                }
            }
            return Keys;
        }
        
        private static double ReadVal(HSDReader d, GXAnimDataFormat Format, float Scale)
        {
            d.BigEndian = false;
            switch (Format)
            {
                case GXAnimDataFormat.Float:
                    return d.ReadSingle();
                case GXAnimDataFormat.Short:
                    return d.ReadInt16() / (double)Scale;
                case GXAnimDataFormat.UShort:
                    return d.ReadUInt16() / (double)Scale;
                case GXAnimDataFormat.SByte:
                    return d.ReadSByte() / (double)Scale;
                case GXAnimDataFormat.Byte:
                    return d.ReadByte() / (double)Scale;
                default:
                    return 0;
            }
        }

        /*public float GetValueAt(float Time)
        {
            List<FOBJKey> Keys = GetKeys();
            
            for(int i = 0; i < Keys.Count; i++)
            {
                if(Time > Keys.Count)
                {

                }
            }
        }*/

        public void Dispose()
        {
            Reader.Close();
            Stream.Close();
        }
    }
}
