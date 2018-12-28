using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSDLib.Common;
using HSDLib.GX;
using HSDLib.Helpers;
using System.IO;
using System.Text.RegularExpressions;
using OpenTK.Graphics.OpenGL;
using HSDLib.Helpers.TriangleConverter;
using System.Globalization;
using System.Drawing;

namespace HALSysDATViewer.Modeling
{
    public class SMDTriangle
    {
        public string Material;
        public GXVertex v1, v2, v3;
    }

    public class SMD
    {
        public HSD_JOBJ RootJOBJ;
        public List<SMDTriangle> Triangles;
        public List<HSD_JOBJWeight> BoneWeightList = new List<HSD_JOBJWeight>();

        public SMD()
        {
            Triangles = new List<SMDTriangle>();
        }

        public SMD(string fname)
        {
            Read(fname);
        }

        public void Read(string fname)
        {
            StreamReader reader = File.OpenText(fname);
            string line;

            string current = "";
            string previous = "";

            RootJOBJ = new HSD_JOBJ();
            Triangles = new List<SMDTriangle>();
            Dictionary<int, HSD_JOBJ> BoneList = new Dictionary<int, HSD_JOBJ>();
            Dictionary<int, List<SMDTriangle>> TriList = new Dictionary<int, List<SMDTriangle>>();
            Dictionary<int, int> ParentBoneList = new Dictionary<int, int>();

            List<int> RootList = new List<int>();

            int time = 0;

            //List<HSD_JOBJ> JOBJBoneList;
            while ((line = reader.ReadLine()) != null)
            {
                line = Regex.Replace(line, @"\s+", " ");
                string[] args = line.Replace(";", "").TrimStart().Split(' ');

                if (args[0].Equals("triangles") || args[0].Equals("end") || args[0].Equals("skeleton") || args[0].Equals("nodes"))
                {
                    previous = current;
                    current = args[0];

                    if (current.Equals("end") && previous.Equals("nodes"))
                    {
                        foreach (KeyValuePair<int, int> bone in ParentBoneList)
                        {
                            int id = bone.Key;
                            int ParentIndex = bone.Value;
                            if (ParentIndex != -1)
                            {
                                BoneList[ParentIndex].AddChild(BoneList[id]);
                            }
                        }
                    }

                    continue;
                }

                if (current.Equals("nodes"))
                {
                    int id = int.Parse(args[0]);
                    HSD_JOBJ b = new HSD_JOBJ();
                    //b.Text = args[1].Replace('"', ' ').Trim();
                    int s = 2;
                    while (args[s].Contains("\""))
                        s++;
                    int ParentIndex = int.Parse(args[s]);
                    ParentBoneList.Add(id, ParentIndex);
                    if (ParentIndex == -1)
                    {
                        RootList.Add(id);
                        //RootJOBJ = b;
                        b.Flags |= JOBJ_FLAG.SKELETON_ROOT | JOBJ_FLAG.CLASSICAL_SCALING | JOBJ_FLAG.ENVELOPE_MODEL | JOBJ_FLAG.TEXEDGE | JOBJ_FLAG.ROOT_TEXEDGE;
                    }
                    else
                    {
                        b.Flags |= JOBJ_FLAG.SKELETON | JOBJ_FLAG.CLASSICAL_SCALING | JOBJ_FLAG.TEXEDGE;
                    }
                    BoneList.Add(id, b);
                }

                if (current.Equals("skeleton"))
                {
                    if (args[0].Contains("time"))
                        time = int.Parse(args[1]);
                    else
                    {
                        if (time == 0)
                        {
                            HSD_JOBJ b = BoneList[int.Parse(args[0])];
                            b.Transforms = new HSD_Transforms()
                            {
                                TX = float.Parse(args[1], CultureInfo.InvariantCulture),
                                TY = float.Parse(args[2], CultureInfo.InvariantCulture),
                                TZ = float.Parse(args[3], CultureInfo.InvariantCulture),
                                RX = float.Parse(args[4], CultureInfo.InvariantCulture),
                                RY = float.Parse(args[5], CultureInfo.InvariantCulture),
                                RZ = float.Parse(args[6], CultureInfo.InvariantCulture),
                                SX = 1f,
                                SY = 1f,
                                SZ = 1f,
                            };
                        }
                    }
                    //JOBJBoneList = RootJOBJ.DepthFirstList;
                }

                if (current.Equals("triangles"))
                {
                    string meshName = args[0];
                    if (args[0].Equals(""))
                        continue;

                    SMDTriangle t = new SMDTriangle();
                    Triangles.Add(t);
                    t.Material = meshName;

                    List<SMDTriangle> SMDTriList = new List<SMDTriangle>();
                    for (int j = 0; j < 3; j++)
                    {
                        line = reader.ReadLine();
                        line = Regex.Replace(line, @"\s+", " ");
                        args = line.Replace(";", "").TrimStart().Split(' ');

                        int parent = int.Parse(args[0]);
                        
                        if (!TriList.Keys.Contains(parent))
                        {
                            SMDTriList = new List<SMDTriangle>();
                            TriList.Add(parent, SMDTriList);
                        }
                        else
                        {
                            SMDTriList = TriList[parent];
                        }

                        GXVertex vert = new GXVertex();
                        vert.Pos = new GXVector3(float.Parse(args[1], CultureInfo.InvariantCulture),
                            float.Parse(args[2], CultureInfo.InvariantCulture),
                            float.Parse(args[3], CultureInfo.InvariantCulture));
                        vert.Nrm = new GXVector3(float.Parse(args[4], CultureInfo.InvariantCulture),
                            float.Parse(args[5], CultureInfo.InvariantCulture),
                            float.Parse(args[6], CultureInfo.InvariantCulture));
                        vert.TEX0 = new GXVector2(float.Parse(args[7], CultureInfo.InvariantCulture),
                            float.Parse(args[8], CultureInfo.InvariantCulture));
                        if (args.Length > 9)
                        {
                            // eww, gross, please fix later
                            /*
                            int wCount = int.Parse(args[9]);
                            int w = 10;
                            HSD_JOBJWeight bw = new HSD_JOBJWeight();
                            for (int i = 0; i < wCount; i++)
                            {
                                int bone = (int.Parse(args[w++]));
                                float weight = (float.Parse(args[w++]));
                                bw.JOBJs.Add(BoneList[bone]);
                                bw.Weights.Add(weight);
                            }
                            int mtxid = BoneWeightList.IndexOf(bw);
                            if(mtxid == -1)
                            {
                                mtxid = BoneWeightList.Count;
                                BoneWeightList.Add(bw);
                            }
                            vert.PMXID = (ushort)(mtxid * 3);
                            */
                        }
                        switch (j)
                        {
                            case 0: t.v1 = vert; break;
                            case 1: t.v2 = vert; break;
                            case 2: t.v3 = vert; break;
                        }
                    }
                    SMDTriList.Add(t);
                }
            }

            // Steps:
            // 1 - Get all vertices
            // 2 - Use GroupPrimitives
            // 3 - Use the list to make the DisplayList
            // 4 - Done

            //Process each material to avoid duplicates
            Dictionary<string, HSD_MOBJ> Materials = new Dictionary<string, HSD_MOBJ>();
            List<string> MatNameList = new List<string>();
            foreach (int id in TriList.Keys)
            {
                foreach (SMDTriangle SMDTri in TriList[id])
                {
                    if (!MatNameList.Contains(SMDTri.Material))
                        MatNameList.Add(SMDTri.Material);
                }
            }

            foreach (string MatName in MatNameList)
            {
                string _matPath = Path.GetDirectoryName(fname) + Path.DirectorySeparatorChar + MatName + ".png";
                Console.WriteLine(_matPath);
                Image _matBitmap = Image.FromFile(_matPath);

                HSD_MOBJ _mat = new HSD_MOBJ()
                {
                    RenderFlags = RENDER_MODE.ALPHA_COMPAT | RENDER_MODE.DIFFSE_MAT | RENDER_MODE.TEX0 | RENDER_MODE.XLU,

                    Textures = new HSD_TOBJ()
                    {
                        TexMapID = GXTexMapID.GX_TEXMAP0,
                        GXTexGenSrc = 4,
                        Transform = new HSD_Transforms()
                        {
                            RX = 0,
                            RY = 0,
                            RZ = 0,
                            SX = 1,
                            SY = 1,
                            SZ = 1,
                            TX = 0,
                            TY = 0,
                            TZ = 0,
                        },
                        WrapS = GXWrapMode.REPEAT,
                        WrapT = GXWrapMode.REPEAT,
                        WScale = 1,
                        HScale = 1,
                        Flags = TOBJ_FLAGS.COORD_UV | TOBJ_FLAGS.LIGHTMAP_DIFFUSE | TOBJ_FLAGS.COLORMAP_ALPHA_MASK,
                        //TOBJ_FLAGS.COLORMAP_ALPHA_MASK for formats with alpha
                        //TOBJ_FLAGS.COLORMAP_BLEND for formats without alpha
                        Blending = 1,
                        MagFilter = GXTexFilter.GX_LINEAR,
                        ImageData = new HSD_Image()
                        {
                            Width = (ushort)_matBitmap.Width,
                            Height = (ushort)_matBitmap.Height,
                            Format = GXTexFmt.RGB565,
                            Mipmap = 0,
                            MaxLOD = 0,
                            MinLOD = 0,

                            /*Data = new byte[]
                            {
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,

                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                                0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,  0xFF, 0xFF,
                            },*/
                        },

                        /*Tlut = new HSD_Tlut()
                        {
                            GXTlut = 0,
                            Format = GXTlutFmt.RGB5A3,
                        },*/

                        /*TEV = new HSD_TOBJ_TEV
                        {
                            color_op = 0,   //GX_TEV_ADD
                            color_bias = 0, //GX_TB_ZERO
                            color_clamp = 0,    //GX_TC_LINEAR

                            color_a = 15,   //GX_CC_ZERO
                            color_b = 15,   //GX_CC_ZERO
                            color_c = 15,   //GX_CC_ZERO
                            color_d = 8,    //GX_CC_TEXC

                            alpha_op = 0,   //GX_TEV_ADD
                            alpha_bias = 0, //GX_TB_ZERO

                            alpha_a = 7,    //GX_CA_ZERO
                            alpha_b = 7,    //GX_CA_ZERO
                            alpha_c = 7,    //GX_CA_ZERO
                            alpha_d = 6,    //GX_CA_KONST

                            konst = 0xFFFFFFFF, //WHITE
                            tev0 = 0,
                            tev1 = 0,
                            active = 0,
                        },*/
                    },

                    MaterialColor = new HSD_MCOBJ()
                    {
                        Alpha = 1f,
                        Shininess = 50f,

                        SPC_A = 255,
                        SPC_R = 255,
                        SPC_G = 255,
                        SPC_B = 255,

                        DIF_A = 255,
                        DIF_R = 255,
                        DIF_G = 255,
                        DIF_B = 255,

                        AMB_A = 255,
                        AMB_R = 255,
                        AMB_G = 255,
                        AMB_B = 255,
                    },
                };

                byte[] dummy;
                //_mat.Textures.ImageData.Data = TPL.ConvertToTextureMelee(_matBitmap, (int)TPL_TextureFormat.RGBA8, (int)TPL_PaletteFormat.None, out _mat.Textures.Tlut.Data);
                _mat.Textures.ImageData.Data = TPL.ConvertToTextureMelee(_matBitmap, (int)TPL_TextureFormat.RGB565, (int)TPL_PaletteFormat.None, out dummy);
                //_mat.Textures.Tlut.ColorCount = (ushort)(_mat.Textures.Tlut.Data.Length / 2);

                Materials.Add(MatName, _mat);
            }

            //Process each joint seperately
            foreach (int id in TriList.Keys)
            {
                TriangleConverter triConverter = new TriangleConverter(true, 52, 2, true);
                int newTriPointCount = 0;
                int newTriFaceCount = 0;

                List<GXVertex> vertexList = new List<GXVertex>();
                foreach (SMDTriangle SMDTri in TriList[id])
                {
                    vertexList.Add(SMDTri.v1);
                    vertexList.Add(SMDTri.v2);
                    vertexList.Add(SMDTri.v3);
                }

                HSD_JOBJ _joint = BoneList[id];
                HSD_POBJ _poly = new HSD_POBJ();
                HSD_DOBJ _display = new HSD_DOBJ();
                HSD_MOBJ _mat = Materials[TriList[id][0].Material];

                List<PrimitiveGroup> _primitivesConv = triConverter.GroupPrimitives(vertexList.ToArray(), out newTriPointCount, out newTriFaceCount);
                List<GXPrimitiveGroup> _prim = new List<GXPrimitiveGroup>();
                HSD_AttributeGroup _attr = new HSD_AttributeGroup();
                GXDisplayList _dlist = new GXDisplayList();

                //--Manage Vertices
                //Optimize Vertices

                vertexList.Clear();
                foreach (SMDTriangle SMDTri in TriList[id])
                {
                    if (!vertexList.Contains(SMDTri.v1))
                        vertexList.Add(SMDTri.v1);
                    if (!vertexList.Contains(SMDTri.v2))
                        vertexList.Add(SMDTri.v2);
                    if (!vertexList.Contains(SMDTri.v3))
                        vertexList.Add(SMDTri.v3);
                }

                //Add all vertex data
                GXVertexBuffer vPos = new GXVertexBuffer()
                {
                    Name = GXAttribName.GX_VA_POS,
                    AttributeType = GXAttribType.GX_INDEX16,
                    CompCount = GXCompCnt.PosXYZ,
                    CompType = GXCompType.Float,
                    Scale = 0,
                    Stride = 4 * 3,
                };
                GXVertexBuffer vNrm = new GXVertexBuffer()
                {
                    Name = GXAttribName.GX_VA_NRM,
                    AttributeType = GXAttribType.GX_INDEX16,
                    CompCount = GXCompCnt.NrmXYZ,
                    CompType = GXCompType.Float,
                    Scale = 0,
                    Stride = 4 * 3,
                };
                GXVertexBuffer vTex = new GXVertexBuffer()
                {
                    Name = GXAttribName.GX_VA_TEX0,
                    AttributeType = GXAttribType.GX_INDEX16,
                    CompCount = GXCompCnt.TexST,
                    CompType = GXCompType.Float,
                    Scale = 0,
                    Stride = 4 * 2,
                };

                List<float> vPosArray = new List<float>();
                List<float> vNrmArray = new List<float>();
                List<float> vTexArray = new List<float>();

                foreach (GXVertex v in vertexList)
                {
                    vPosArray.Add(v.Pos.X);
                    vPosArray.Add(v.Pos.Y);
                    vPosArray.Add(v.Pos.Z);

                    vNrmArray.Add(v.Nrm.X);
                    vNrmArray.Add(v.Nrm.Y);
                    vNrmArray.Add(v.Nrm.Z);

                    vTexArray.Add(v.TEX0.X);
                    vTexArray.Add(v.TEX0.Y);
                }

                //Convert to the proper endian
                List<byte> byteArray = new List<byte>();
                foreach (float f in vPosArray)
                {
                    byteArray.Add(BitConverter.GetBytes(f)[3]);
                    byteArray.Add(BitConverter.GetBytes(f)[2]);
                    byteArray.Add(BitConverter.GetBytes(f)[1]);
                    byteArray.Add(BitConverter.GetBytes(f)[0]);
                }
                vPos.DataBuffer = byteArray.ToArray();

                byteArray.Clear();
                foreach (float f in vNrmArray)
                {
                    byteArray.Add(BitConverter.GetBytes(f)[3]);
                    byteArray.Add(BitConverter.GetBytes(f)[2]);
                    byteArray.Add(BitConverter.GetBytes(f)[1]);
                    byteArray.Add(BitConverter.GetBytes(f)[0]);
                }
                vNrm.DataBuffer = byteArray.ToArray();

                byteArray.Clear();
                foreach (float f in vTexArray)
                {
                    byteArray.Add(BitConverter.GetBytes(f)[3]);
                    byteArray.Add(BitConverter.GetBytes(f)[2]);
                    byteArray.Add(BitConverter.GetBytes(f)[1]);
                    byteArray.Add(BitConverter.GetBytes(f)[0]);
                }
                vTex.DataBuffer = byteArray.ToArray();

                _attr.Attributes.Add(vPos);
                _attr.Attributes.Add(vNrm);
                _attr.Attributes.Add(vTex);

                //Process each primitive
                foreach (PrimitiveGroup g in _primitivesConv)
                {
                    if (g._triangles.Count != 0)
                    {
                        GXPrimitiveGroup p = new GXPrimitiveGroup();
                        p.PrimitiveType = GXPrimitiveType.Triangles;
                        p.Count = (ushort)g._triangles.Count;
                        p.Indices = new GXIndexGroup[p.Count * _attr.Attributes.Count];
                        ushort idx = 0;

                        //convert each Primitive to proper GX Primitive
                        foreach (PointTriangle point in g._triangles)
                        {
                            foreach (GXVertex _vtx in point.Points)
                            {
                                p.Indices[idx] = new GXIndexGroup();
                                p.Indices[idx].Indices = new ushort[_attr.Attributes.Count];
                                for (int i = 0; i < _attr.Attributes.Count; i++)
                                    p.Indices[idx].Indices[i] = (ushort)Array.IndexOf(vertexList.ToArray(), _vtx);
                                idx++;
                            }
                        }
                        _prim.Add(p);
                    }

                    if (g._tristrips.Count != 0)
                    {
                        //Convert each Primitive to proper GX Primitive
                        foreach (PointTriangleStrip point in g._tristrips)
                        {
                            GXPrimitiveGroup p = new GXPrimitiveGroup();
                            p.PrimitiveType = GXPrimitiveType.TriangleStrip;
                            List<GXIndexGroup> _indices = new List<GXIndexGroup>();
                            foreach (GXVertex _vtx in point.Points)
                            {
                                GXIndexGroup _indice = new GXIndexGroup();
                                _indice.Indices = new ushort[_attr.Attributes.Count];
                                for (int i = 0; i < _attr.Attributes.Count; i++)
                                    _indice.Indices[i] = (ushort)Array.IndexOf(vertexList.ToArray(), _vtx);
                                _indices.Add(_indice);
                            }
                            p.Indices = _indices.ToArray();
                            p.Count = (ushort)(_indices.Count / _attr.Attributes.Count);
                            _prim.Add(p);
                        }
                    }
                }
                _poly.VertexAttributes = _attr;
                _dlist.Primitives = _prim;
                _poly.DisplayListBuffer = _dlist.ToBuffer(_attr);
                _display.POBJ = _poly;
                _display.MOBJ = _mat;
                _joint.DOBJ = _display;
            }

            //Process RootJOBJ here
            if (RootList.Count == 1)
            {
                RootJOBJ = BoneList[RootList[0]];
            }

            //There may be a case of more than one root bones, but we won't do anything about it yet

            reader.Close();
        }

        public GXVertex[] GetVertices()
        {
            List<GXVertex> Vertices = new List<GXVertex>(Triangles.Count * 3);
            foreach (SMDTriangle t in Triangles)
            {
                Vertices.Add(t.v3);
                Vertices.Add(t.v2);
                Vertices.Add(t.v1);
            }
            return Vertices.ToArray();
        }

        public List<HSD_JOBJWeight> PMXIDCreate(ref GXVertex[] Verts)
        {
            List<HSD_JOBJWeight> BoneWeightList = new List<HSD_JOBJWeight>();
            List<HSD_JOBJ> BoneList = RootJOBJ.DepthFirstList;

            for(int i = 0; i < Verts.Length; i++)
            {
                // map Bone is
                HSD_JOBJWeight bw = new HSD_JOBJWeight();
                /*if(Verts[i].W1 > 0)
                {
                    bw.JOBJs.Add(BoneList[Verts[i].B1]);
                    bw.Weights.Add(Verts[i].W1);
                }
                if (Verts[i].W2 > 0)
                {
                    bw.JOBJs.Add(BoneList[Verts[i].B2]);
                    bw.Weights.Add(Verts[i].W2);
                }
                if (Verts[i].W3 > 0)
                {
                    bw.JOBJs.Add(BoneList[Verts[i].B3]);
                    bw.Weights.Add(Verts[i].W3);
                }
                int index = BoneWeightList.IndexOf(bw);
                if (index != -1)
                {
                } else
                {
                    index = BoneWeightList.Count;
                    BoneWeightList.Add(bw);
                }
                Verts[i].PMXID = (ushort)index;*/
            }

            return BoneWeightList;
        }

        public void PrimitiveRender()
        {
            GL.Begin(PrimitiveType.Triangles);

            foreach(SMDTriangle tri in Triangles)
            {
                GL.Vertex3(tri.v1.Pos.X, tri.v1.Pos.Y, tri.v1.Pos.Z);
                GL.Vertex3(tri.v2.Pos.X, tri.v2.Pos.Y, tri.v2.Pos.Z);
                GL.Vertex3(tri.v3.Pos.X, tri.v3.Pos.Y, tri.v3.Pos.Z);
            }

            GL.End();
        }

        /*public void Save(string FileName)
        {
            StringBuilder o = new StringBuilder();

            o.AppendLine("version 1");

            if (Bones != null)
            {
                o.AppendLine("nodes");
                for (int i = 0; i < Bones.bones.Count; i++)
                    o.AppendLine("  " + i + " \"" + Bones.bones[i].Text + "\" " + Bones.bones[i].parentIndex);
                o.AppendLine("end");

                o.AppendLine("skeleton");
                o.AppendLine("time 0");
                for (int i = 0; i < Bones.bones.Count; i++)
                {
                    Bone b = Bones.bones[i];
                    o.AppendFormat("{0} {1} {2} {3} {4} {5} {6}\n", i, b.position[0], b.position[1], b.position[2], b.rotation[0], b.rotation[1], b.rotation[2]);
                }
                o.AppendLine("end");
            }

            if (Triangles != null)
            {
                o.AppendLine("triangles");
                foreach (SMDTriangle tri in Triangles)
                {
                    o.AppendLine(tri.Material);
                    WriteVertex(o, tri.v1);
                    WriteVertex(o, tri.v2);
                    WriteVertex(o, tri.v3);
                }
                o.AppendLine("end");
            }

            File.WriteAllText(FileName, o.ToString());
        }

        private void WriteVertex(StringBuilder o, SMDVertex v)
        {
            o.AppendFormat("{0} {1} {2} {3} {4} {5} {6} {7} {8} ",
                        v.Parent,
                        v.P.X, v.P.Y, v.P.Z,
                        v.N.X, v.N.Y, v.N.Z,
                        v.UV.X, v.UV.Y);
            if (v.Weights == null)
            {
                o.AppendLine("0");
            }
            else
            {
                string weights = v.Weights.Length + "";
                for (int i = 0; i < v.Weights.Length; i++)
                {
                    weights += " " + v.Bones[i] + " " + v.Weights[i];
                }
                o.AppendLine(weights);
            }*/
    }
}
