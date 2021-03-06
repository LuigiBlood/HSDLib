﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HSDLib.Animation;
using HSDLib.GX;

namespace HSDLib.Helpers
{
    public class FOBJFrameEncoder
    {
        public static HSD_FOBJ EncodeFrames(List<FOBJKey> Keys, byte TrackType)
        {
            HSD_FOBJ fobj = new HSD_FOBJ();
            fobj.AnimationType = TrackType;

            // perform quantization
            float vmax = -float.MaxValue, tmax = -float.MaxValue;
            bool vsigned = false, tsigned = false;
            foreach(FOBJKey key in Keys)
            {
                if (key.Value < 0) vsigned = true;
                if (key.Tan < 0) tsigned = true;
                vmax = Math.Max(Math.Abs(key.Value), vmax);
                tmax = Math.Max(Math.Abs(key.Tan), tmax);
            }
            GXAnimDataFormat t;
            fobj.ValueScale = (uint)QuantizeScaler(vmax, vsigned, out t);
            fobj.ValueFormat = t;
            fobj.TanScale = (uint)QuantizeScaler(tmax, tsigned, out t);
            fobj.TanFormat = t;
            
            MemoryStream o = new MemoryStream();
            using (HSDWriter Writer = new HSDWriter(o))
            {
                Writer.BigEndian = false;

                int time = 0;
                for(int i = 0;i < Keys.Count;)
                {
                    InterpolationType ip = Keys[i].InterpolationType;
                    int j;
                    for (j = 0; j < Keys.Count - i; j++)
                    {
                        if (Keys[i+j].InterpolationType != ip)
                            break;
                    }

                    int flag = (j << 4) | (int)ip;
                    Writer.ExtendedByte(flag);

                    for (int k = i; k < i + j; k++)
                    {
                        switch (Keys[k].InterpolationType)
                        {
                            case InterpolationType.Step:
                                WriteVal(Writer, Keys[k].Value, fobj.ValueFormat, fobj.ValueScale);
                                Writer.ExtendedByte((int)(Keys[k].Frame - time));
                                break;
                            case InterpolationType.Linear:
                                WriteVal(Writer, Keys[k].Value, fobj.ValueFormat, fobj.ValueScale);
                                Writer.ExtendedByte((int)(Keys[k].Frame - time));
                                break;
                            case InterpolationType.HermiteValue:
                                WriteVal(Writer, Keys[k].Value, fobj.ValueFormat, fobj.ValueScale);
                                Writer.ExtendedByte((int)(Keys[k].Frame - time));
                                break;
                            case InterpolationType.Hermite:
                                WriteVal(Writer, Keys[k].Value, fobj.ValueFormat, fobj.ValueScale);
                                WriteVal(Writer, Keys[k].Tan, fobj.TanFormat, fobj.TanScale);
                                Writer.ExtendedByte((int)(Keys[k].Frame - time));
                                break;
                            case InterpolationType.HermiteCurve:
                                WriteVal(Writer, Keys[k].Tan, fobj.TanFormat, fobj.TanScale);
                                break;
                            case InterpolationType.Constant:
                                WriteVal(Writer, Keys[k].Value, fobj.ValueFormat, fobj.ValueScale);
                                break;
                            default:
                                throw new Exception("end");
                        }
                        time = (int)Keys[k].Frame;
                    }

                    i += j;
                }
            }
            fobj.Data = o.GetBuffer();
            o.Close();
            return fobj;
        }

        //TODO: Increase Accuracy
        public static int QuantizeScaler(float max, bool Signed, out GXAnimDataFormat Type)
        {
            float error = 0.00001f;
            if (Signed)
            {
                for (int i = 0; i < 0x1F; i++)
                {
                    float Estimated = (float)(((sbyte)(Math.Pow(2, i) * max)) / Math.Pow(2, i));
                    if (Math.Abs(max - Estimated) < error)
                    {
                        Type = GXAnimDataFormat.SByte;
                        return (int)Math.Pow(2, i);
                    }
                }
                for (int i = 0; i < 0x1F; i++)
                {
                    float Estimated = (float)(((short)(Math.Pow(2, i) * max)) / Math.Pow(2, i));
                    if (Math.Abs(max - Estimated) < error)
                    {
                        Console.WriteLine(max + " " + Estimated);
                        Type = GXAnimDataFormat.Short;
                        return (int)Math.Pow(2, i);
                    }
                }
            }
            else
            {

                for (int i = 0; i < 0x1F; i++)
                {
                    float Estimated = (float)(((byte)(Math.Pow(2, i) * max)) / Math.Pow(2, i));
                    if (Math.Abs(max - Estimated) < error)
                    {
                        Type = GXAnimDataFormat.Byte;
                        return (int)Math.Pow(2, i);
                    }
                }
                for (int i = 0; i < 0x1F; i++)
                {
                    float Estimated = (float)(((ushort)(Math.Pow(2, i) * max)) / Math.Pow(2, i));
                    if (Math.Abs(max - Estimated) < error)
                    {
                        Type = GXAnimDataFormat.UShort;
                        return (int)Math.Pow(2, i);
                    }
                }
            }
            Type = GXAnimDataFormat.Float;
            return (int)Math.Pow(2, 1);
        }

        private static void WriteVal(HSDWriter d, float Value, GXAnimDataFormat Format, float Scale)
        {
            switch (Format)
            {
                case GXAnimDataFormat.Float:
                    d.Write(Value);
                    break;
                case GXAnimDataFormat.Short:
                    d.Write((short)(Value * Scale));
                    break;
                case GXAnimDataFormat.UShort:
                    d.Write((ushort)(Value * Scale));
                    break;
                case GXAnimDataFormat.SByte:
                    d.Write((sbyte)(Value * Scale));
                    break;
                case GXAnimDataFormat.Byte:
                    d.Write((byte)(Value * Scale));
                    break;
                default:
                    throw new Exception("Unknown GXAnimDataFormat " + Format.ToString());
            }
        }

    }
}
