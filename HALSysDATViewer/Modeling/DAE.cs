using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HSDLib.Common;
using HSDLib.GX;
using HSDLib.Helpers;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using OpenTK;
using System.Globalization;

namespace HALSysDATViewer.Modeling
{
    public partial class DAE : Form
    {
        public HSD_JOBJ RootJOBJ;

        public DAE(string path)
        {
            using (DecoderShell shell = DecoderShell.Import(path))
            {
                //Get all images
                Dictionary<string, HSD_Image> images = new Dictionary<string, HSD_Image>();

                //Dummy Image
                HSD_Image image_dummy = new HSD_Image()
                {
                    Width = 16,
                    Height = 16,
                    Format = GXTexFmt.RGB565,
                    Mipmap = 0,
                    MaxLOD = 0,
                    MinLOD = 0,

                    Data = new byte[]
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
                    },
                };

                HSD_TOBJ dummy_TOBJ = new HSD_TOBJ()
                {
                    Flags = TOBJ_FLAGS.COORD_UV | TOBJ_FLAGS.LIGHTMAP_DIFFUSE | TOBJ_FLAGS.COLORMAP_ALPHA_MASK,
                    Blending = 1f,
                    GXTexGenSrc = 4,
                    MagFilter = GXTexFilter.GX_LINEAR,
                    TexMapID = GXTexMapID.GX_TEXMAP0,
                    NameOffset = 0,
                    Transform = new HSD_Transforms()
                    {
                        TX = 0,
                        TY = 0,
                        TZ = 0,
                        SX = 1,
                        SY = 1,
                        SZ = 1,
                        RX = 0,
                        RY = 0,
                        RZ = 0,
                    },
                    WrapS = GXWrapMode.REPEAT,
                    WrapT = GXWrapMode.REPEAT,
                    WScale = 1,
                    HScale = 1,
                    ImageData = image_dummy,
                };

                foreach (ImageEntry img in shell._images)
                {
                    string name = img._path != null ?
                        Path.GetFileNameWithoutExtension(img._path) :
                        img._name != null ? img._name : img._id;

                    Image _matBitmap = Image.FromFile(Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + img._path);
                    byte[] dummy;

                    HSD_Image image = new HSD_Image()
                    {
                        Format = GXTexFmt.RGB565,
                        MinLOD = 0,
                        MaxLOD = 0,
                        Mipmap = 0,
                        Height = (ushort)_matBitmap.Height,
                        Width = (ushort)_matBitmap.Width,

                        Data = TPL.ConvertToTextureMelee(_matBitmap, (int)TPL_TextureFormat.RGB565, (int)TPL_PaletteFormat.None, out dummy),
                    };

                    images.Add(img._id, image);
                }

                //Get all materials
                Dictionary<string, HSD_MOBJ> materials = new Dictionary<string, HSD_MOBJ>();

                foreach (MaterialEntry material in shell._materials)
                {
                    foreach (EffectEntry effect in shell._effects)
                    {
                        if (effect._id == material._effect)
                        {
                            HSD_MOBJ _mat = new HSD_MOBJ()
                            {
                                NameOffset = 0,
                                RenderFlags = RENDER_MODE.XLU | RENDER_MODE.ALPHA_COMPAT | RENDER_MODE.DIFFSE_MAT,
                                MaterialColor = new HSD_MCOBJ()
                                {
                                    AMB_R = 255,
                                    AMB_G = 255,
                                    AMB_B = 255,
                                    AMB_A = 255,
                                    DIF_R = 255,
                                    DIF_G = 255,
                                    DIF_B = 255,
                                    DIF_A = 255,
                                    SPC_R = 255,
                                    SPC_G = 255,
                                    SPC_B = 255,
                                    SPC_A = 255,
                                    Alpha = 1f,
                                    Shininess = 50f,
                                },
                            };
                            
                            foreach (LightEffectEntry light in effect._shader._effects)
                            {
                                if (light._color != null)
                                {
                                    switch (light._type)
                                    {
                                        case (LightEffectType.ambient):
                                            _mat.MaterialColor.AMB_R = light._color.R;
                                            _mat.MaterialColor.AMB_G = light._color.G;
                                            _mat.MaterialColor.AMB_B = light._color.B;
                                            _mat.MaterialColor.AMB_A = light._color.A;
                                            break;
                                        case (LightEffectType.diffuse):
                                            _mat.MaterialColor.DIF_R = light._color.R;
                                            _mat.MaterialColor.DIF_G = light._color.G;
                                            _mat.MaterialColor.DIF_B = light._color.B;
                                            _mat.MaterialColor.DIF_A = light._color.A;
                                            break;
                                        case (LightEffectType.specular):
                                            _mat.MaterialColor.SPC_R = light._color.R;
                                            _mat.MaterialColor.SPC_G = light._color.G;
                                            _mat.MaterialColor.SPC_B = light._color.B;
                                            _mat.MaterialColor.SPC_A = light._color.A;
                                            break;
                                    }
                                }

                                if (light._texture != null)
                                {
                                    //Find Sampler2D
                                    EffectNewParam base_sampler2d = new EffectNewParam();
                                    foreach (EffectNewParam newparam in effect._newParams)
                                    {
                                        if (newparam._sid == light._texture)
                                        {
                                            base_sampler2d = newparam;
                                        }
                                    }

                                    //Find Surface
                                    foreach (EffectNewParam newparam in effect._newParams)
                                    {
                                        if (newparam._sid == base_sampler2d._sampler2D._source)
                                        {
                                            _mat.RenderFlags |= RENDER_MODE.TEX0;
                                            _mat.Textures = new HSD_TOBJ()
                                            {
                                                Flags = TOBJ_FLAGS.COORD_UV | TOBJ_FLAGS.LIGHTMAP_DIFFUSE | TOBJ_FLAGS.COLORMAP_ALPHA_MASK,
                                                Blending = 1f,
                                                GXTexGenSrc = 4,
                                                MagFilter = GXTexFilter.GX_LINEAR,
                                                TexMapID = GXTexMapID.GX_TEXMAP0,
                                                NameOffset = 0,
                                                Transform = new HSD_Transforms()
                                                {
                                                    TX = 0,
                                                    TY = 0,
                                                    TZ = 0,
                                                    SX = 1,
                                                    SY = 1,
                                                    SZ = 1,
                                                    RX = 0,
                                                    RY = 0,
                                                    RZ = 0,
                                                },
                                                WrapS = GXWrapMode.CLAMP,
                                                WrapT = GXWrapMode.CLAMP,
                                                WScale = 1,
                                                HScale = 1,
                                            };

                                            if (images.Count == 0)
                                            {
                                                _mat.Textures.ImageData = image_dummy;
                                            }
                                            else
                                            {
                                                _mat.Textures.ImageData = images[newparam._path];
                                            }
                                        }
                                    }
                                }
                            }

                            if (_mat.Textures == null)
                            {
                                _mat.RenderFlags |= RENDER_MODE.TEX0;
                                _mat.Textures = dummy_TOBJ;
                            }

                            _mat.MaterialColor.Shininess = effect._shader._shininess;
                            //_mat.MaterialColor.Alpha = effect._shader._transparency;
                            materials.Add(material._id, _mat);
                        }
                    }
                }

                //Get all models
                Dictionary<string, HSD_POBJ> polygons = new Dictionary<string, HSD_POBJ>();

                foreach (GeometryEntry geometry in shell._geometry)
                {
                    List<GXVertexBuffer> buffers = new List<GXVertexBuffer>();
                    GXDisplayList dlist = new GXDisplayList()
                    {
                        Primitives = new List<GXPrimitiveGroup>(),
                    };
                    HSD_POBJ _poly = new HSD_POBJ()
                    {
                        Flags = POBJ_FLAG.SKIN,
                        VertexAttributes = new HSD_AttributeGroup(),
                    };

                    foreach (PrimitiveEntry _prim in geometry._primitives)
                    {
                        int colorVTX = -1;
                        foreach (InputEntry _input in _prim._inputs)
                        {
                            GXVertexBuffer buffer = new GXVertexBuffer()
                            {
                                AttributeType = GXAttribType.GX_INDEX16,
                                Scale = 0,
                            };

                            switch (_input._semantic)
                            {
                                case SemanticType.VERTEX:
                                    //hacky, should check vertices instead to make sure
                                    buffer.Name = GXAttribName.GX_VA_POS;
                                    break;
                                case SemanticType.NORMAL:
                                    buffer.Name = GXAttribName.GX_VA_NRM;
                                    break;
                                case SemanticType.TEXCOORD:
                                    buffer.Name = GXAttribName.GX_VA_TEX0 + _input._set;
                                    break;
                                case SemanticType.COLOR:
                                    continue;
                                    buffer.Name = GXAttribName.GX_VA_CLR0;
                                    colorVTX = buffers.Count;
                                    break;
                                default:
                                    continue;
                            }

                            //Get Source
                            foreach (SourceEntry source in geometry._sources)
                            {
                                string compare = "";
                                if (_input._semantic == SemanticType.VERTEX)
                                {
                                    compare = geometry._verticesInput._source;
                                }
                                else
                                {
                                    compare = _input._source;
                                }

                                if (source._id == compare)
                                {
                                    //Data Type and Stride
                                    switch (source._arrayType)
                                    {
                                        case (SourceType.Float):
                                            buffer.CompType = GXCompType.Float;
                                            buffer.Stride = (ushort)(source._accessorStride * 4);
                                            break;
                                        case (SourceType.Int):
                                            buffer.CompType = GXCompType.Int16;
                                            buffer.Stride = (ushort)(source._accessorStride * 2);
                                            break;
                                    }

                                    //CompCount
                                    switch (_input._semantic)
                                    {
                                        case SemanticType.VERTEX:
                                            if (source._accessorStride == 2)
                                                buffer.CompCount = GXCompCnt.PosXY;
                                            else if (source._accessorStride == 3)
                                                buffer.CompCount = GXCompCnt.PosXYZ;
                                            break;
                                        case SemanticType.NORMAL:
                                            if (source._accessorStride == 3)
                                                buffer.CompCount = GXCompCnt.NrmXYZ;
                                            break;
                                        case SemanticType.TEXCOORD:
                                            if (source._accessorStride == 1)
                                                buffer.CompCount = GXCompCnt.TexS;
                                            else if (source._accessorStride == 2)
                                                buffer.CompCount = GXCompCnt.TexST;
                                            break;
                                        case SemanticType.COLOR:
                                            if (source._accessorStride == 3)
                                                buffer.CompCount = GXCompCnt.ClrRGB;
                                            else if (source._accessorStride == 4)
                                                buffer.CompCount = GXCompCnt.ClrRGBA;
                                            break;
                                    }

                                    //Data
                                    List<byte> byteArray = new List<byte>();
                                    switch (source._arrayType)
                                    {
                                        case (SourceType.Float):
                                            foreach (float f in (float[])source._arrayData)
                                            {
                                                byteArray.Add(BitConverter.GetBytes(f)[3]);
                                                byteArray.Add(BitConverter.GetBytes(f)[2]);
                                                byteArray.Add(BitConverter.GetBytes(f)[1]);
                                                byteArray.Add(BitConverter.GetBytes(f)[0]);
                                            }
                                            break;
                                        case (SourceType.Int):
                                            //Not supported for now
                                            break;
                                    }
                                    buffer.DataBuffer = byteArray.ToArray();
                                }
                            }

                            //Insert
                            buffers.Insert(_input._offset, buffer);
                        }
                        //Put all inputs into VertexAttributes
                        _poly.VertexAttributes.Attributes = buffers;

                        //Manage Primitives
                        if (_prim._type == ColladaPrimitiveType.triangles
                            || _prim._type == ColladaPrimitiveType.tristrips
                            || _prim._type == ColladaPrimitiveType.trifans
                            || _prim._type == ColladaPrimitiveType.polylist)
                        {
                            GXPrimitiveGroup p = new GXPrimitiveGroup();

                            //Primitive Type
                            switch (_prim._type)
                            {
                                case (ColladaPrimitiveType.tristrips):
                                    p.PrimitiveType = GXPrimitiveType.TriangleStrip;
                                    break;
                                case (ColladaPrimitiveType.trifans):
                                    p.PrimitiveType = GXPrimitiveType.TriangleFan;
                                    break;
                                case (ColladaPrimitiveType.triangles):
                                default:
                                    p.PrimitiveType = GXPrimitiveType.Triangles;
                                    break;
                            }

                            p.Count = (ushort)_prim._pointCount;
                            p.Indices = new GXIndexGroup[_prim._pointCount];

                            for (int i = 0; i < p.Indices.Length; i++)
                                p.Indices[i] = new GXIndexGroup()
                                {
                                    Indices = new ushort[_prim._entryStride],
                                };

                            //Add each index for each point
                            foreach (PrimitiveFace face in _prim._faces)
                            {
                                for (int j = 0; j < face._pointIndices.Length; j++)
                                {
                                    p.Indices[j / _prim._entryStride].Indices[j % (_prim._entryStride)] = face._pointIndices[j];
                                }
                            }
                            dlist.Primitives.Add(p);
                        }
                    }
                    //Convert to Display List Buffer
                    _poly.DisplayListBuffer = dlist.ToBuffer(_poly.VertexAttributes);

                    //Add POBJ to the list
                    polygons.Add(geometry._id, _poly);
                }

                RootJOBJ = new HSD_JOBJ()
                {
                    Flags = JOBJ_FLAG.ENVELOPE_MODEL | JOBJ_FLAG.SKELETON_ROOT | JOBJ_FLAG.CLASSICAL_SCALING | JOBJ_FLAG.ROOT_TEXEDGE | JOBJ_FLAG.TEXEDGE,
                    Transforms = new HSD_Transforms()
                    {
                        TX = 0,
                        TY = 0,
                        TZ = 0,
                        RX = 0,
                        RY = 0,
                        RZ = 0,
                        SX = 1,
                        SY = 1,
                        SZ = 1,
                    },
                };

                foreach (SceneEntry scene in shell._scenes)
                {
                    foreach (NodeEntry node in scene._nodes)
                    {
                        ProcessNode(shell, RootJOBJ, node, materials, polygons);
                    }
                }
            }
        }

        private void ProcessNode(DecoderShell shell, HSD_JOBJ joint, NodeEntry node, Dictionary<string, HSD_MOBJ> materials, Dictionary<string, HSD_POBJ> polygons)
        {
            HSD_JOBJ jointChild = new HSD_JOBJ();
            foreach (NodeEntry nodeChild in node._children)
            {
                ProcessNode(shell, jointChild, nodeChild, materials, polygons);
            }

            //Get first instance of a mesh
            InstanceEntry mesh = new InstanceEntry();
            bool meshFound = false;
            string materialUrl = "";
            string meshUrl = "";
            foreach (InstanceEntry instance in node._instances)
            {
                mesh = instance;
                if (instance._type == InstanceType.Geometry)
                {
                    meshFound = true;
                    materialUrl = mesh._material._target;
                    meshUrl = mesh._url;
                    break;
                }
                else if (instance._type == InstanceType.Controller)
                {
                    foreach (SkinEntry skin in shell._skins)
                    {
                        if (skin._id == mesh._url)
                        {
                            meshFound = true;
                            materialUrl = mesh._material._target;
                            meshUrl = skin._skinSource;
                            break;
                        }
                    }

                    if (meshFound)
                        break;
                }
            }

            //Get Matrix
            Vector3 translate = node._matrix.ExtractTranslation();
            Quaternion rotate = node._matrix.ExtractRotation();
            Vector3 scale = node._matrix.ExtractScale();
            jointChild.Transforms = new HSD_Transforms()
            {
                TX = translate.X,
                TY = translate.Y,
                TZ = translate.Z,
                SX = scale.X,
                SY = scale.Y,
                SZ = scale.Z,
                RX = rotate.X * rotate.W,
                RY = rotate.Y * rotate.W,
                RZ = rotate.Z * rotate.W,
            };

            jointChild.ROBJOffset = 0;
            jointChild.NameOffset = 0;
            jointChild.Flags = JOBJ_FLAG.CLASSICAL_SCALING | JOBJ_FLAG.SKELETON | JOBJ_FLAG.TEXEDGE;
            if (meshFound)
            {
                jointChild.DOBJ = new HSD_DOBJ()
                {
                    MOBJ = materials[materialUrl],
                    POBJ = polygons[meshUrl],
                };
            }
            joint.AddChild(jointChild);
        }

        private class DecoderShell : IDisposable
        {
            //From BrawlLib
            internal List<ImageEntry> _images = new List<ImageEntry>();
            internal List<MaterialEntry> _materials = new List<MaterialEntry>();
            internal List<EffectEntry> _effects = new List<EffectEntry>();
            internal List<GeometryEntry> _geometry = new List<GeometryEntry>();
            internal List<SkinEntry> _skins = new List<SkinEntry>();
            internal List<NodeEntry> _nodes = new List<NodeEntry>();
            internal List<SceneEntry> _scenes = new List<SceneEntry>();
            internal int _v1, _v2, _v3;

            public static DecoderShell Import(string path)
            {
                XmlDocument reader = new XmlDocument();
                reader.Load(path);
                return new DecoderShell(reader);
            }
            ~DecoderShell() { Dispose(); }
            public void Dispose()
            {
                foreach (GeometryEntry geo in _geometry)
                    geo.Dispose();
            }

            private void Output(string message)
            {
                MessageBox.Show(message);
            }

            private DecoderShell(XmlDocument reader)
            {
                if (reader.DocumentElement.Name == "COLLADA")
                    ParseMain(reader.DocumentElement);
            }

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (SceneEntry scene in _scenes)
                    foreach (NodeEntry node in scene._nodes)
                        if ((n = FindNodeInternal(name, node)) != null)
                            return n;
                return null;
            }
            internal static NodeEntry FindNodeInternal(string name, NodeEntry node)
            {
                NodeEntry e;

                if (node._name == name || node._sid == name || node._id == name)
                    return node;

                foreach (NodeEntry n in node._children)
                    if ((e = FindNodeInternal(name, n)) != null)
                        return e;

                return null;
            }

            private void ParseMain(XmlElement element)
            {
                if (element.HasAttribute("version"))
                {
                    string v = (string)element.Attributes["version"].Value;
                    string[] s = v.Split('.');
                    int.TryParse(s[0], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v1);
                    int.TryParse(s[1], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v2);
                    int.TryParse(s[2], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v3);
                }
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("asset"))
                        ParseAsset(childElement);
                    else if (childElement.Name.Equals("library_images"))
                        ParseLibImages(childElement);
                    else if (childElement.Name.Equals("library_materials"))
                        ParseLibMaterials(childElement);
                    else if (childElement.Name.Equals("library_effects"))
                        ParseLibEffects(childElement);
                    else if (childElement.Name.Equals("library_geometries"))
                        ParseLibGeometry(childElement);
                    else if (childElement.Name.Equals("library_controllers"))
                        ParseLibControllers(childElement);
                    else if (childElement.Name.Equals("library_visual_scenes"))
                        ParseLibScenes(childElement);
                    else if (childElement.Name.Equals("library_nodes"))
                        ParseLibNodes(childElement);
                }
            }

            public float _scale = 1;
            private void ParseAsset(XmlElement element)
            {
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("unit"))
                        if (childElement.HasAttribute("meter"))
                            float.TryParse((string)childElement.Attributes["meter"].Value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out _scale);
                }
            }

            private void ParseLibImages(XmlElement element)
            {
                ImageEntry img;
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("image"))
                    {
                        img = new ImageEntry();
                        if (childElement.HasAttribute("id"))
                            img._id = (string)childElement.Attributes["id"].Value;
                        if (childElement.HasAttribute("name"))
                            img._name = (string)childElement.Attributes["name"].Value;

                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            img._path = null;
                            if (childChildElement.Name.Equals("init_from"))
                            {
                                if (_v2 < 5)
                                    img._path = childChildElement.InnerText;
                                else
                                    foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                    {
                                        if (childChildChildElement.Name.Equals("ref"))
                                            img._path = childChildChildElement.InnerText;
                                    }
                            }
                        }

                        _images.Add(img);
                    }
                }
            }

            private void ParseLibMaterials(XmlElement element)
            {
                MaterialEntry mat;
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("material"))
                    {
                        mat = new MaterialEntry();
                        if (childElement.HasAttribute("id"))
                            mat._id = (string)childElement.Attributes["id"].Value;
                        if (childElement.HasAttribute("name"))
                            mat._name = (string)childElement.Attributes["name"].Value;

                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("instance_effect"))
                                if (childChildElement.HasAttribute("url"))
                                    mat._effect = childChildElement.Attributes["url"].Value[0] == '#' ? (string)(childChildElement.Attributes["url"].Value.Substring(1)) : (string)childChildElement.Attributes["url"].Value;
                        }

                        _materials.Add(mat);
                    }
                }
            }

            private void ParseLibEffects(XmlElement element)
            {
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("effect"))
                        _effects.Add(ParseEffect(childElement));
                }
            }

            private EffectEntry ParseEffect(XmlElement element)
            {
                EffectEntry eff = new EffectEntry();

                if (element.HasAttribute("id"))
                    eff._id = (string)element.Attributes["id"].Value;
                if (element.HasAttribute("name"))
                    eff._name = (string)element.Attributes["name"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    //Only common is supported
                    if (childElement.Name.Equals("profile_COMMON"))
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("newparam"))
                                eff._newParams.Add(ParseNewParam(childChildElement));
                            else if (childChildElement.Name.Equals("technique"))
                            {
                                foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                {
                                    if (childChildChildElement.Name.Equals("phong"))
                                        eff._shader = ParseShader(childChildChildElement, ShaderType.phong);
                                    else if (childChildChildElement.Name.Equals("lambert"))
                                        eff._shader = ParseShader(childChildChildElement, ShaderType.lambert);
                                    else if (childChildChildElement.Name.Equals("blinn"))
                                        eff._shader = ParseShader(childChildChildElement, ShaderType.blinn);
                                }
                            }
                        }
                }
                return eff;
            }
            private EffectNewParam ParseNewParam(XmlElement element)
            {
                EffectNewParam p = new EffectNewParam();

                if (element.HasAttribute("sid"))
                    p._sid = (string)element.Attributes["sid"].Value;
                if (element.HasAttribute("id"))
                    p._id = (string)element.Attributes["id"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("surface"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            p._path = null;
                            if (childChildElement.Name.Equals("init_from"))
                            {
                                if (_v2 < 5)
                                    p._path = childChildElement.InnerText;
                                else
                                    foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                    {
                                        if (childChildChildElement.Name.Equals("ref"))
                                            p._path = childChildChildElement.InnerText;
                                    }
                            }
                        }
                    }
                    else if (childElement.Name.Equals("sampler2D"))
                        p._sampler2D = ParseSampler2D(childElement);
                }

                return p;
            }

            private EffectSampler2D ParseSampler2D(XmlElement element)
            {
                EffectSampler2D s = new EffectSampler2D();

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("source"))
                        s._source = childElement.InnerText;
                    else if (childElement.Name.Equals("instance_image"))
                    {
                        if (childElement.HasAttribute("url"))
                            s._url = childElement.Attributes["url"].Value[0] == '#' ? (string)(childElement.Attributes["url"].Value.Substring(1)) : (string)childElement.Attributes["url"].Value;
                    }
                    else if (childElement.Name.Equals("wrap_s"))
                        s._wrapS = childElement.InnerText;
                    else if (childElement.Name.Equals("wrap_t"))
                        s._wrapT = childElement.InnerText;
                    else if (childElement.Name.Equals("minfilter"))
                        s._minFilter = childElement.InnerText;
                    else if (childElement.Name.Equals("magfilter"))
                        s._magFilter = childElement.InnerText;
                }

                return s;
            }
            private EffectShaderEntry ParseShader(XmlElement element, ShaderType type)
            {
                EffectShaderEntry s = new EffectShaderEntry();
                s._type = type;
                float v;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("ambient"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.ambient));
                    else if (childElement.Name.Equals("diffuse"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.diffuse));
                    else if (childElement.Name.Equals("emission"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.emission));
                    else if (childElement.Name.Equals("reflective"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.reflective));
                    else if (childElement.Name.Equals("specular"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.specular));
                    else if (childElement.Name.Equals("transparent"))
                        s._effects.Add(ParseLightEffect(childElement, LightEffectType.transparent));
                    else if (childElement.Name.Equals("shininess"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("float"))
                                s._shininess = float.Parse(childChildElement.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                    else if (childElement.Name.Equals("reflectivity"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("float"))
                                s._reflectivity = float.Parse(childChildElement.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                    else if (childElement.Name.Equals("transparency"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("float"))
                                s._transparency = float.Parse(childChildElement.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                }

                return s;
            }

            private LightEffectEntry ParseLightEffect(XmlElement element, LightEffectType type)
            {
                LightEffectEntry eff = new LightEffectEntry();
                eff._type = type;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("color"))
                        eff._color = ParseColor(childElement);
                    else if (childElement.Name.Equals("texture"))
                    {
                        if (childElement.HasAttribute("texture"))
                            eff._texture = (string)childElement.Attributes["texture"].Value;
                        if (childElement.HasAttribute("texcoord"))
                            eff._texCoord = (string)childElement.Attributes["texcoord"].Value;
                    }
                }

                return eff;
            }
            private void ParseLibGeometry(XmlElement element)
            {
                GeometryEntry geo;
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("geometry"))
                    {
                        geo = new GeometryEntry();
                        if (childElement.HasAttribute("id"))
                            geo._id = (string)childElement.Attributes["id"].Value;
                        if (childElement.HasAttribute("name"))
                            geo._name = (string)childElement.Attributes["name"].Value;

                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("mesh"))
                            {
                                foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                {
                                    if (childChildChildElement.Name.Equals("source"))
                                        geo._sources.Add(ParseSource(childChildChildElement));
                                    else if (childChildChildElement.Name.Equals("vertices"))
                                    {
                                        if (childChildChildElement.HasAttribute("id"))
                                            geo._verticesId = (string)childChildChildElement.Attributes["id"].Value;

                                        foreach (XmlElement childChildChildChildElement in childChildChildElement.ChildNodes)
                                        {
                                            if (childChildChildChildElement.Name.Equals("input"))
                                                geo._verticesInput = ParseInput(childChildChildChildElement);
                                        }
                                    }
                                    else if (childChildChildElement.Name.Equals("polygons"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.polygons));
                                    else if (childChildChildElement.Name.Equals("polylist"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.polylist));
                                    else if (childChildChildElement.Name.Equals("triangles"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.triangles));
                                    else if (childChildChildElement.Name.Equals("tristrips"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.tristrips));
                                    else if (childChildChildElement.Name.Equals("trifans"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.trifans));
                                    else if (childChildChildElement.Name.Equals("lines"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.lines));
                                    else if (childChildChildElement.Name.Equals("linestrips"))
                                        geo._primitives.Add(ParsePrimitive(childChildChildElement, ColladaPrimitiveType.linestrips));
                                }
                            }
                        }
                        _geometry.Add(geo);
                    }
                }
            }
            private PrimitiveEntry ParsePrimitive(XmlElement element, ColladaPrimitiveType type)
            {
                PrimitiveEntry prim = new PrimitiveEntry() { _type = type };
                PrimitiveFace p;
                int val;
                int stride = 0, elements = 0;

                switch (type)
                {
                    case ColladaPrimitiveType.trifans:
                    case ColladaPrimitiveType.tristrips:
                    case ColladaPrimitiveType.triangles:
                        stride = 3;
                        break;
                    case ColladaPrimitiveType.lines:
                    case ColladaPrimitiveType.linestrips:
                        stride = 2;
                        break;
                    case ColladaPrimitiveType.polygons:
                    case ColladaPrimitiveType.polylist:
                        stride = 4;
                        break;
                }

                if (element.HasAttribute("material"))
                    prim._material = (string)element.Attributes["material"].Value;
                if (element.HasAttribute("count"))
                    prim._entryCount = int.Parse((string)element.Attributes["count"].Value);

                prim._faces.Capacity = prim._entryCount;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("input"))
                    {
                        prim._inputs.Add(ParseInput(childElement));
                        elements++;
                    }
                    else if (childElement.Name.Equals("p"))
                    {
                        List<ushort> indices = new List<ushort>(stride * elements);

                        p = new PrimitiveFace();
                        //p._pointIndices.Capacity = stride * elements;
                        indices.AddRange(Array.ConvertAll<string, ushort>(childElement.InnerText.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries), ushort.Parse));

                        p._pointCount = indices.Count / elements;
                        p._pointIndices = indices.ToArray();

                        switch (type)
                        {
                            case ColladaPrimitiveType.trifans:
                            case ColladaPrimitiveType.tristrips:
                            case ColladaPrimitiveType.polygons:
                            case ColladaPrimitiveType.polylist:
                                p._faceCount = p._pointCount - 2;
                                break;

                            case ColladaPrimitiveType.triangles:
                                p._faceCount = p._pointCount / 3;
                                break;

                            case ColladaPrimitiveType.lines:
                                p._faceCount = p._pointCount / 2;
                                break;

                            case ColladaPrimitiveType.linestrips:
                                p._faceCount = p._pointCount - 1;
                                break;
                        }

                        prim._faceCount += p._faceCount;
                        prim._pointCount += p._pointCount;
                        prim._faces.Add(p);
                    }
                }

                prim._entryStride = elements;

                return prim;
            }

            private InputEntry ParseInput(XmlElement element)
            {
                InputEntry inp = new InputEntry();

                if (element.HasAttribute("id"))
                    inp._id = (string)element.Attributes["id"].Value;
                if (element.HasAttribute("name"))
                    inp._name = (string)element.Attributes["name"].Value;
                if (element.HasAttribute("semantic"))
                    inp._semantic = (SemanticType)Enum.Parse(typeof(SemanticType), (string)element.Attributes["semantic"].Value, true);
                if (element.HasAttribute("set"))
                    inp._set = int.Parse((string)element.Attributes["set"].Value);
                if (element.HasAttribute("offset"))
                    inp._offset = int.Parse((string)element.Attributes["offset"].Value);
                if (element.HasAttribute("source"))
                    inp._source = element.Attributes["source"].Value[0] == '#' ? (string)(element.Attributes["source"].Value.Substring(1)) : (string)element.Attributes["source"].Value;

                return inp;
            }

            private SourceEntry ParseSource(XmlElement element)
            {
                SourceEntry src = new SourceEntry();

                if (element.HasAttribute("id"))
                    src._id = (string)element.Attributes["id"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("float_array"))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Float;

                            if (childElement.HasAttribute("id"))
                                src._arrayId = (string)childElement.Attributes["id"].Value;
                            if (childElement.HasAttribute("count"))
                                src._arrayCount = int.Parse((string)childElement.Attributes["count"].Value);

                            float[] buffer = ConvertToFloatArray(childElement.InnerText.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                            src._arrayData = buffer;
                        }
                    }
                    else if (childElement.Name.Equals("int_array"))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Int;

                            if (childElement.HasAttribute("id"))
                                src._arrayId = (string)childElement.Attributes["id"].Value;
                            if (childElement.HasAttribute("count"))
                                src._arrayCount = int.Parse((string)childElement.Attributes["count"].Value);

                            int[] buffer = Array.ConvertAll<string, int>(childElement.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
                            src._arrayData = buffer;
                        }
                    }
                    else if (childElement.Name.Equals("Name_array"))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Name;

                            if (childElement.HasAttribute("id"))
                                src._arrayId = (string)childElement.Attributes["id"].Value;
                            if (childElement.HasAttribute("count"))
                                src._arrayCount = int.Parse((string)childElement.Attributes["count"].Value);

                            string[] list = childElement.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            src._arrayData = list;
                            src._arrayDataString = childElement.Value;
                        }
                    }
                    else if (childElement.Name.Equals("technique_common"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("accessor"))
                            {
                                if (childChildElement.HasAttribute("source"))
                                    src._accessorSource = childChildElement.Attributes["source"].Value[0] == '#' ? (string)(childChildElement.Attributes["source"].Value.Substring(1)) : (string)childChildElement.Attributes["source"].Value;
                                if (childChildElement.HasAttribute("count"))
                                    src._accessorCount = int.Parse((string)childChildElement.Attributes["count"].Value);
                                if (childChildElement.HasAttribute("stride"))
                                    src._accessorStride = int.Parse((string)childChildElement.Attributes["stride"].Value);

                                //Ignore params
                            }
                        }
                    }
                }

                return src;
            }

            private void ParseLibControllers(XmlElement element)
            {
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("controller"))
                    {
                        string id = null;
                        if (childElement.HasAttribute("id"))
                            id = (string)childElement.Attributes["id"].Value;

                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("skin"))
                                _skins.Add(ParseSkin(childChildElement, id));
                        }
                    }
                }
            }

            private SkinEntry ParseSkin(XmlElement element, string id)
            {
                SkinEntry skin = new SkinEntry();
                skin._id = id;

                if (element.HasAttribute("source"))
                    skin._skinSource = element.Attributes["source"].Value[0] == '#' ? (string)(element.Attributes["source"].Value.Substring(1)) : (string)element.Attributes["source"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("bind_shape_matrix"))
                        skin._bindMatrix = ParseMatrix(childElement);
                    else if (childElement.Name.Equals("source"))
                        skin._sources.Add(ParseSource(childElement));
                    else if (childElement.Name.Equals("joints"))
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("input"))
                                skin._jointInputs.Add(ParseInput(childChildElement));
                        }
                    else if (childElement.Name.Equals("vertex_weights"))
                    {
                        if (childElement.HasAttribute("count"))
                            skin._weightCount = int.Parse((string)childElement.Attributes["count"].Value);

                        skin._weights = new int[skin._weightCount][];

                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("input"))
                                skin._weightInputs.Add(ParseInput(childChildElement));
                            else if (childChildElement.Name.Equals("vcount"))
                            {
                                string[] vcount_array = childChildElement.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int val = int.Parse(vcount_array[i]);
                                    skin._weights[i] = new int[val * skin._weightInputs.Count];
                                }
                            }
                            else if (childChildElement.Name.Equals("v"))
                            {
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int[] weights = skin._weights[i];

                                    int[] buffer = Array.ConvertAll<string, int>(childChildElement.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

                                    for (int x = 0; x < weights.Length; x++)
                                        weights[x] = buffer[x];
                                }
                            }
                        }
                    }
                }

                return skin;
            }

            private void ParseLibNodes(XmlElement element)
            {
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("node"))
                        _nodes.Add(ParseNode(childElement));
                }
            }

            private void ParseLibScenes(XmlElement element)
            {
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("visual_scene"))
                        _scenes.Add(ParseScene(childElement));
                }
            }

            private SceneEntry ParseScene(XmlElement element)
            {
                SceneEntry sc = new SceneEntry();

                if (element.HasAttribute("id"))
                    sc._id = (string)element.Attributes["id"].Value;

                if (element.HasAttribute("name"))
                    sc._name = (string)element.Attributes["name"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("node"))
                        sc._nodes.Add(ParseNode(childElement));
                }

                return sc;
            }

            private NodeEntry ParseNode(XmlElement element)
            {
                NodeEntry node = new NodeEntry();

                if (element.HasAttribute("id"))
                    node._id = (string)element.Attributes["id"].Value;
                if (element.HasAttribute("name"))
                    node._name = (string)element.Attributes["name"].Value;
                if (element.HasAttribute("sid"))
                    node._sid = (string)element.Attributes["sid"].Value;
                if (element.HasAttribute("type"))
                    node._type = (NodeType)Enum.Parse(typeof(NodeType), (string)element.Attributes["type"].Value, true);

                Matrix4 m = Matrix4.Identity;
                Matrix4 mInv = Matrix4.Identity;
                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("matrix"))
                    {
                        Matrix4 matrix = ParseMatrix(childElement);
                        m *= matrix;
                        //mInv *= matrix.Invert();
                    }
                    else if (childElement.Name.Equals("rotate"))
                    {
                        Vector4 v = ParseVec4(childElement);
                        m *= Matrix4.CreateRotationX(v.X * v.W) + Matrix4.CreateRotationY(v.Y * v.W) + Matrix4.CreateRotationZ(v.Z * v.W);
                        //mInv *= Matrix.ReverseRotationMatrix(v._x * v._w, v._y * v._w, v._z * v._w);
                    }
                    else if (childElement.Name.Equals("scale"))
                    {
                        Vector3 scale = ParseVec3(childElement);
                        m *= Matrix4.CreateScale(scale);
                        //mInv *= Matrix.ScaleMatrix(1.0f / scale);
                    }
                    else if (childElement.Name.Equals("translate"))
                    {
                        Vector3 translate = ParseVec3(childElement);
                        m *= Matrix4.CreateTranslation(translate);
                        //mInv *= Matrix.TranslationMatrix(-translate);
                    }
                    else if (childElement.Name.Equals("node"))
                        node._children.Add(ParseNode(childElement));
                    else if (childElement.Name.Equals("instance_controller"))
                        node._instances.Add(ParseInstance(childElement, InstanceType.Controller));
                    else if (childElement.Name.Equals("instance_geometry"))
                        node._instances.Add(ParseInstance(childElement, InstanceType.Geometry));
                    else if (childElement.Name.Equals("instance_node"))
                        node._instances.Add(ParseInstance(childElement, InstanceType.Node));
                    else if (childElement.Name.Equals("extra"))
                    {
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("technique"))
                            {
                                foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                {
                                    if (childChildChildElement.Name.Equals("visibility"))
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                node._matrix = m;
                node._invMatrix = mInv;
                return node;
            }

            private InstanceEntry ParseInstance(XmlElement element, InstanceType type)
            {
                InstanceEntry c = new InstanceEntry();
                c._type = type;

                if (element.HasAttribute("url"))
                    c._url = element.Attributes["url"].Value[0] == '#' ? (string)(element.Attributes["url"].Value.Substring(1)) : (string)element.Attributes["url"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("skeleton"))
                        c.skeletons.Add(childElement.InnerText[0] == '#' ? (string)(childElement.InnerText.Substring(1)) : (string)childElement.InnerText);

                    if (childElement.Name.Equals("bind_material"))
                        foreach (XmlElement childChildElement in childElement.ChildNodes)
                        {
                            if (childChildElement.Name.Equals("technique_common"))
                                foreach (XmlElement childChildChildElement in childChildElement.ChildNodes)
                                {
                                    if (childChildChildElement.Name.Equals("instance_material"))
                                        c._material = ParseMatInstance(childChildChildElement);
                                }
                        }
                }

                return c;
            }

            private InstanceMaterial ParseMatInstance(XmlElement element)
            {
                InstanceMaterial mat = new InstanceMaterial();

                if (element.HasAttribute("symbol"))
                    mat._symbol = (string)element.Attributes["symbol"].Value;
                if (element.HasAttribute("target"))
                    mat._target = element.Attributes["target"].Value[0] == '#' ? (string)(element.Attributes["target"].Value.Substring(1)) : (string)element.Attributes["target"].Value;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name.Equals("bind_vertex_input"))
                        mat._vertexBinds.Add(ParseVertexInput(childElement));
                }
                return mat;
            }
            private VertexBind ParseVertexInput(XmlElement element)
            {
                VertexBind v = new VertexBind();

                if (element.HasAttribute("semantic"))
                    v._semantic = (string)element.Attributes["semantic"].Value;
                if (element.HasAttribute("input_semantic"))
                    v._inputSemantic = (string)element.Attributes["input_semantic"].Value;
                if (element.HasAttribute("input_set"))
                    v._inputSet = int.Parse((string)element.Attributes["input_set"].Value);

                return v;
            }

            private Matrix4 ParseMatrix(XmlElement element)
            {
                float[] pM = ConvertToFloatArray(element.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                Matrix4 m = new Matrix4(pM[0], pM[1], pM[2], pM[3],
                    pM[4], pM[5], pM[6], pM[7],
                    pM[8], pM[9], pM[10], pM[11],
                    pM[12], pM[13], pM[14], pM[15]);

                return m;
            }
            private Color ParseColor(XmlElement element)
            {
                float[] f = ConvertToFloatArray(element.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                byte[] p = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    if (i >= f.Length)
                        p[i] = 255;
                    else
                        p[i] = (byte)(f[i] * 255.0f + 0.5f);
                }
                return Color.FromArgb(p[3], p[0], p[1], p[2]);
            }
            private Vector3 ParseVec3(XmlElement element)
            {
                float[] f = ConvertToFloatArray(element.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                float[] p = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    if (i >= f.Length)
                        p[i] = 0;
                    else
                        p[i] = f[i];
                }
                return new Vector3(p[0], p[1], p[2]);
            }
            private Vector4 ParseVec4(XmlElement element)
            {
                float[] f = ConvertToFloatArray(element.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                float[] p = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    if (i >= f.Length)
                        p[i] = 0;
                    else
                        p[i] = f[i];
                }
                return new Vector4(p[0], p[1], p[2], p[3]);
            }
        }

        private static float[] ConvertToFloatArray(string[] array)
        {
            float[] f = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
                f[i] = float.Parse(array[i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
            return f;
        }

        private class ColladaEntry : IDisposable
        {
            internal string _id, _name, _sid;
            internal object _node;

            ~ColladaEntry() { Dispose(); }
            public virtual void Dispose() { GC.SuppressFinalize(this); }
        }
        private class ImageEntry : ColladaEntry
        {
            internal string _path;
        }
        private class MaterialEntry : ColladaEntry
        {
            internal string _effect;
        }
        private class EffectEntry : ColladaEntry
        {
            internal EffectShaderEntry _shader;
            internal List<EffectNewParam> _newParams = new List<EffectNewParam>();
        }
        private class GeometryEntry : ColladaEntry
        {
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<PrimitiveEntry> _primitives = new List<PrimitiveEntry>();

            internal int _faces, _lines;

            internal string _verticesId;
            internal InputEntry _verticesInput;

            public override void Dispose()
            {
                foreach (SourceEntry p in _sources)
                    p.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SourceEntry : ColladaEntry
        {
            internal SourceType _arrayType;
            internal string _arrayId;
            internal int _arrayCount;
            internal object _arrayData; //Parser must dispose!
            internal string _arrayDataString;

            internal string _accessorSource;
            internal int _accessorCount;
            internal int _accessorStride;

            public override void Dispose()
            {
                _arrayData = null;
                GC.SuppressFinalize(this);
            }
        }
        private class InputEntry : ColladaEntry
        {
            internal SemanticType _semantic;
            internal int _set;
            internal int _offset;
            internal string _source;
            internal int _outputOffset;
        }
        private class PrimitiveEntry
        {
            internal ColladaPrimitiveType _type;

            internal string _material;
            internal int _entryCount;
            internal int _entryStride;
            internal int _faceCount, _pointCount;

            internal List<InputEntry> _inputs = new List<InputEntry>();

            internal List<PrimitiveFace> _faces = new List<PrimitiveFace>();
        }
        private class PrimitiveFace
        {
            internal int _pointCount;
            internal int _faceCount;
            internal ushort[] _pointIndices;
        }
        private class SkinEntry : ColladaEntry
        {
            internal string _skinSource;
            internal Matrix4 _bindMatrix = new Matrix4();

            //internal Matrix _bindShape;
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<InputEntry> _jointInputs = new List<InputEntry>();

            internal List<InputEntry> _weightInputs = new List<InputEntry>();
            internal int _weightCount;
            internal int[][] _weights;

            public override void Dispose()
            {
                foreach (SourceEntry src in _sources)
                    src.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SceneEntry : ColladaEntry
        {
            internal List<NodeEntry> _nodes = new List<NodeEntry>();

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (NodeEntry node in _nodes)
                    if ((n = DecoderShell.FindNodeInternal(name, node)) != null)
                        return n;
                return null;
            }
        }
        private class NodeEntry : ColladaEntry
        {
            internal NodeType _type = NodeType.NONE;
            internal Matrix4 _matrix = new Matrix4();
            internal Matrix4 _invMatrix = new Matrix4();
            internal List<NodeEntry> _children = new List<NodeEntry>();
            internal List<InstanceEntry> _instances = new List<InstanceEntry>();
        }
        private enum InstanceType
        {
            Controller,
            Geometry,
            Node
        }
        private class InstanceEntry : ColladaEntry
        {
            internal InstanceType _type;
            internal string _url;
            internal InstanceMaterial _material;
            internal List<string> skeletons = new List<string>();
        }
        private class InstanceMaterial : ColladaEntry
        {
            internal string _symbol, _target;
            internal List<VertexBind> _vertexBinds = new List<VertexBind>();
        }
        private class VertexBind : ColladaEntry
        {
            internal string _semantic;
            internal string _inputSemantic;
            internal int _inputSet;
        }
        private class EffectSampler2D
        {
            public string _source;
            public string _url;
            public string _wrapS, _wrapT;
            public string _minFilter, _magFilter;
        }
        private class EffectNewParam : ColladaEntry
        {
            public string _path;
            public EffectSampler2D _sampler2D;
        }
        private class EffectShaderEntry : ColladaEntry
        {
            internal ShaderType _type;
            internal float _shininess, _reflectivity, _transparency;
            internal List<LightEffectEntry> _effects = new List<LightEffectEntry>();
        }
        private class LightEffectEntry : ColladaEntry
        {
            internal LightEffectType _type;
            internal Color _color;

            internal string _texture;
            internal string _texCoord;
        }
        private enum ShaderType
        {
            None,
            phong,
            lambert,
            blinn
        }
        private enum LightEffectType
        {
            None,
            ambient,
            diffuse,
            emission,
            reflective,
            specular,
            transparent
        }
        private enum ColladaPrimitiveType
        {
            None,
            polygons,
            polylist,
            triangles,
            trifans,
            tristrips,
            lines,
            linestrips
        }
        private enum SemanticType
        {
            None,
            POSITION,
            VERTEX,
            NORMAL,
            TEXCOORD,
            COLOR,
            WEIGHT,
            JOINT,
            INV_BIND_MATRIX,
            TEXTANGENT,
            TEXBINORMAL
        }
        private enum SourceType
        {
            None,
            Float,
            Int,
            Name
        }
        private enum NodeType
        {
            NODE,
            JOINT,
            NONE
        }
    }
}
