using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rd = Rhino.DocObjects;
using Sd = System.Drawing;

namespace ThreePlus
{
    public class Material : SceneObject
    {

        #region members

        public enum Types { None, Basic, Lambert, Standard, Phong, Toon, Physical, Normal, Depth, Shadow };

        protected bool isDefault = true;

        //
        protected Sd.Bitmap[] maps = new Sd.Bitmap[24];
        public string[] MapNames = new string[24];

        protected Types materialType = Types.Phong;

        protected Sd.Color color = Sd.Color.Wheat;
        protected bool transparent = false;

        protected double environment = 1.0;
        protected bool hasEnvironment = false;

        protected bool isWireframe = false;
        protected bool isFlatShaded = false;

        protected double ambient = 1.0;
        protected double shininess = 1.0;
        protected double reflectivity = 0.5;
        protected List<int> steps = new List<int>();

        protected double bumpIntensity = 1.0;
        protected double normalScale = 1.0;

        protected bool hasClearcoat = false;
        protected double clearcoat = 0.0;
        protected bool hasClearcoatRoughness = false;
        protected double clearcoatRoughness = 0.0;

        protected bool hasDisplacement = false;
        protected double displacementScale = 1.0;

        protected bool hasEmissive = false;
        protected Sd.Color emissiveColor = Sd.Color.White;
        protected double emissiveIntensity = 0.0;

        protected bool hasMetalness = false;
        protected double metalness = 0.5;

        protected bool hasOpacity = false;
        protected double opacity = 1.0;
        protected bool hasOpacityRoughness = false;
        protected double opacityRoughness = 0.0;

        protected bool hasRoughness = false;
        protected double roughness = 0.25;

        protected bool hasSheen = false;
        protected double sheen = 0.0;
        protected Sd.Color sheenColor = Sd.Color.White;
        protected bool hasSheenRoughness = false;
        protected double sheenRoughness = 0.0;

        protected bool hasVolumetrics = false;
        protected double transmission = 0.0;
        protected bool hasOpacityIor = false;
        protected double opacityIor = 1.5;
        protected double refractionRatio = 1.5;

        #endregion

        #region constructors

        public Material(bool isDefault = true) : base()
        {
            this.isDefault = isDefault;
            this.type = "MeshStandardMaterial";
            this.materialType = Types.Standard;
            this.color = Sd.Color.LightGray;
            this.shininess = 0.5;
            this.roughness = 0.5;
            this.metalness = 0.25;
            this.reflectivity = 0.125;
        }

        public Material(Rhino.DocObjects.Material material) : base()
        {
            this.isDefault = false;
            material.ToPhysicallyBased();

            Rhino.DocObjects.PhysicallyBasedMaterial pbr = material.PhysicallyBased;
            this.materialType = Types.Physical;

            Sd.Color clr = material.DiffuseColor;
            clr = Sd.Color.FromArgb((int)(255.0 * (1.0 - material.Transparency)), clr.R, clr.G, clr.B);

            SetTextureMap(clr, material.GetBitmap(Rd.TextureType.Diffuse));

            SetMetalnessMap(pbr.Metallic, material.GetBitmap(Rd.TextureType.PBR_Metallic));
            SetRoughness(pbr.Roughness, material.GetBitmap(Rd.TextureType.PBR_Roughness));
            SetAmbientOcclusion(material.GetIntensity(Rd.TextureType.PBR_AmbientOcclusion), material.GetBitmap(Rd.TextureType.PBR_AmbientOcclusion));
            SetEmissivity(pbr.Emission.L, pbr.Emission.ToDrawingColor(), material.GetBitmap(Rd.TextureType.PBR_Emission));

            SetSheen(pbr.Sheen, Sd.Color.White, material.GetBitmap(Rd.TextureType.PBR_Sheen));
            SetSheenRoughness(pbr.SheenTint, material.GetBitmap(Rd.TextureType.PBR_SheenTint));

            SetClearcoatMap(pbr.Clearcoat, material.GetBitmap(Rd.TextureType.PBR_Clearcoat));
            SetClearcoatRoughnessMap(pbr.ClearcoatRoughness, material.GetBitmap(Rd.TextureType.PBR_ClearcoatRoughness));
            SetClearcoatNormalMap(material.GetBitmap(Rd.TextureType.PBR_ClearcoatBump));

            SetOpacityMap(pbr.Opacity, material.GetBitmap(Rd.TextureType.Opacity));
            SetVolume(pbr.Subsurface, pbr.OpacityIOR, pbr.ReflectiveIOR, material.GetBitmap(Rd.TextureType.PBR_Subsurface));
            //SetOpacityIorMap(pbr.OpacityIOR, material.GetBitmap(Rd.TextureType.PBR_OpacityIor));

            SetNormalMap(material.GetBitmap(Rd.TextureType.Bump), material.GetIntensity(Rd.TextureType.Bump));
            SetBumpMap(material.GetBitmap(Rd.TextureType.Bump), material.GetIntensity(Rd.TextureType.Bump));
            SetDisplacementMap(material.GetBitmap(Rd.TextureType.PBR_Displacement), material.GetIntensity(Rd.TextureType.PBR_Displacement));

            this.reflectivity = 1.0;
        }

        public Material(Material material) : base(material)
        {
            this.isDefault = material.isDefault;
            
            for (int i = 0; i < material.maps.Length; i++) if(material.maps[i]!=null) this.maps[i] = material.maps[i];
                for (int i =0;i< material.maps.Length; i++) this.MapNames[i] = material.MapNames[i];

            this.materialType = material.materialType;

            this.color = material.color;
            this.transparent = material.transparent;
            this.environment = material.environment;
            this.hasEnvironment = material.hasEnvironment;

            this.isWireframe = material.isWireframe;
            this.isFlatShaded = material.isFlatShaded;

            this.shininess = material.shininess;
            this.reflectivity = material.reflectivity;
            this.steps = material.steps;

            this.bumpIntensity = material.bumpIntensity;

            this.hasClearcoat = material.hasClearcoat;
            this.hasClearcoatRoughness = material.hasClearcoatRoughness;
            this.clearcoat = material.clearcoat;
            this.clearcoatRoughness = material.clearcoatRoughness;

            this.hasDisplacement = material.hasDisplacement;
            this.displacementScale = material.displacementScale;

            this.hasEmissive = material.hasEmissive;
            this.emissiveColor = material.emissiveColor;
            this.emissiveIntensity = material.emissiveIntensity;

            this.hasMetalness = material.hasMetalness;
            this.metalness = material.metalness;

            this.normalScale = material.normalScale;

            this.hasOpacity = material.hasOpacity;
            this.opacity = material.opacity;

            this.hasOpacityRoughness = material.hasOpacityRoughness;
            this.opacityRoughness = material.opacityRoughness;

            this.hasRoughness = material.hasRoughness;
            this.roughness = material.roughness;

            this.hasSheen = material.hasSheen;
            this.hasSheenRoughness = material.hasSheenRoughness;
            this.sheen = material.sheen;
            this.sheenColor = material.sheenColor;
            this.sheenRoughness = material.sheenRoughness;

            this.hasVolumetrics = material.hasVolumetrics;
            this.transmission = material.transmission;
            this.hasOpacityIor = material.hasOpacityIor;
            this.opacityIor = material.opacityIor;
            this.refractionRatio = material.refractionRatio;
        }

        public static Material ShadowMaterial(Sd.Color color)
        {
            Material material = new Material(false);

            material.type = "MeshShadowMaterial";
            material.materialType = Types.Shadow;
            material.color = color;

            return material;
        }

        public static Material BasicMaterial(Sd.Color color)
        {
            Material material = new Material(false);

            material.type = "MeshBasicMaterial";
            material.materialType = Types.Basic;
            material.color = color;

            return material;
        }

        public static Material LambertMaterial(Sd.Color color)
        {
            Material material = new Material(false);

            material.type = "MeshLambertMaterial";
            material.materialType = Types.Lambert;
            material.color = color;

            return material;
        }

        public static Material PhongMaterial(Sd.Color color, double shininess)
        {
            Material material = new Material(false);

            material.type = "MeshPhongMaterial";
            material.materialType = Types.Phong;
            material.color = color;
            material.shininess = shininess;

            return material;
        }

        public static Material ToonMaterial(Sd.Color color, int steps)
        {
            Material material = new Material(false);

            material.type = "MeshToonMaterial";
            material.materialType = Types.Toon;
            material.color = color;

            List<int> series = new List<int>();
            for (int i = 0; i < steps; i++)
            {
                series.Add((int)((double)i / steps * 255.0));
            }

            material.steps = series;

            return material;
        }

        public static Material StandardMaterial(Sd.Color color, double roughness, double metalness)
        {
            Material material = new Material(false);

            material.type = "MeshStandardMaterial";
            material.materialType = Types.Standard;
            material.color = color;

            material.hasRoughness = true;
            material.roughness = roughness;

            material.hasMetalness = true;
            material.metalness = metalness;

            return material;
        }

        public static Material PhysicalMaterial(Sd.Color color, double roughness, double metalness, double sheen, Sd.Color sheenColor, double sheenRoughness, double reflectivity, double clearcoat, double clearcoatRoughness)
        {
            Material material = new Material(false);

            material.type = "MeshPhysicalMaterial";
            material.materialType = Types.Physical;
            material.color = color;
            material.reflectivity = reflectivity;

            material.hasRoughness = true;
            material.roughness = roughness;

            material.hasMetalness = true;
            material.metalness = metalness;

            material.sheen = sheen;
            material.sheenColor = sheenColor;
            material.sheenRoughness = sheenRoughness;

            material.clearcoat = clearcoat;
            material.clearcoatRoughness = clearcoatRoughness;

            return material;
        }

        public static Material NormalMaterial()
        {
            Material material = new Material(false);

            material.type = "MeshNormalMaterial";
            material.materialType = Types.Normal;

            return material;
        }

        public static Material DepthMaterial()
        {
            Material material = new Material(false);

            material.type = "MeshDepthMaterial";
            material.materialType = Types.Depth;

            return material;
        }


        #endregion

        #region properties

        public virtual bool IsDefault
        {
            get { return isDefault; }
        }

        public virtual Sd.Bitmap[] Maps
        {
            get { return maps; }
            }

        public virtual Types MaterialType
        {
            get { return materialType; }
        }

        public virtual bool Transparent
        {
            get { return transparent; }
        }

        public virtual bool IsWireframe
        {
            get { return isWireframe; }
            set { isWireframe = value; }
        }

        public virtual bool IsFlatShaded
        {
            get { return isFlatShaded; }
            set { isFlatShaded = value; }
        }

        public virtual double Shininess
        {
            get { return shininess; }
        }

        public virtual double Reflectivity
        {
            get { return reflectivity; }
        }

        public virtual List<int> Steps
        {
            get { return steps; }
        }

        #region 00 | DIFFUSE 
        public virtual Sd.Color DiffuseColor
        {
            get { return color; }
        }

        public virtual bool HasTextureMap
        {
            get { return (Maps[0]!=null); }
        }

        public virtual string TextureMapName
        {
            get { return MapNames[0]; }
        }

        public virtual Sd.Bitmap TextureMap
        {
            set { maps[0] = new Sd.Bitmap(value); }
            get 
            {
                if (maps[0] != null)
                {
                    return new Sd.Bitmap(maps[0]);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 01 | BUMP

        public virtual double BumpIntensity
        {
            get { return bumpIntensity; }
        }

        public virtual bool HasBumpMap
        {
            get { return BumpMap != null; }
        }

        public virtual string BumpMapName
        {
            get { return MapNames[1]; }
        }

        public virtual Sd.Bitmap BumpMap
        {
            set { maps[1] = new Sd.Bitmap(value); }
            get
            {
                if (maps[1] != null)
                {
                    return new Sd.Bitmap(maps[1]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 02 | NORMAL

        public virtual double NormalScale
        {
            get { return normalScale; }
        }

        public virtual bool HasNormalMap
        {
            get { return NormalMap != null; }
        }

        public virtual string NormalMapName
        {
            get { return MapNames[2]; }
        }

        public virtual Sd.Bitmap NormalMap
        {
            set { maps[2] = new Sd.Bitmap(value); }
            get
            {
                if (maps[2] != null)
                {
                    return new Sd.Bitmap(maps[2]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 03 | ENVIRONMENT

        public virtual double Environment
        {
            get { return environment; }
        }

        public virtual bool HasEnvironment
        {
            get { return hasEnvironment; }
        }

        public virtual bool HasEnvironmentMap
        {
            get { return OpacityMap != null; }
        }

        public virtual string EnvironmentMapName
        {
            get { return MapNames[3]; }
        }

        public virtual Sd.Bitmap EnvironmentMap
        {
            set { maps[3] = new Sd.Bitmap(value); }
            get
            {
                if (maps[3] != null)
                {
                    return new Sd.Bitmap(maps[3]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 04 | ROUGHNESS

        public virtual double Roughness
        {
            get { return roughness; }
        }

        public virtual bool HasRoughness
        {
            get { return hasRoughness; }
        }

        public virtual bool HasRoughnessMap
        {
            get { return RoughnessMap != null; }
        }

        public virtual string RoughnessMapName
        {
            get { return MapNames[4]; }
        }

        public virtual Sd.Bitmap RoughnessMap
        {
            set { maps[4] = new Sd.Bitmap(value); }
            get
            {
                if (maps[4] != null)
                {
                    return new Sd.Bitmap(maps[4]);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 05 | METALNESS

        public virtual double Metalness
        {
            get { return metalness; }
        }

        public virtual bool IsMetalic
        {
            get { return hasMetalness; }
        }

        public virtual bool HasMetalicMap
        {
            get { return MetalnessMap != null; }
        }

        public virtual string MetalnessMapName
        {
            get { return MapNames[5]; }
        }

        public virtual Sd.Bitmap MetalnessMap
        {
            set { maps[5] = new Sd.Bitmap(value); }
            get
            {
                if (maps[5] != null)
                {
                    return new Sd.Bitmap(maps[5]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 06 | EMISSIVE

        public virtual Sd.Color EmissiveColor
        {
            get { return emissiveColor; }
        }

        public virtual double EmissiveIntensity
        {
            get { return emissiveIntensity; }
        }

        public virtual bool IsEmissive
        {
            get { return hasEmissive; }
        }

        public virtual bool HasEmissiveMap
        {
            get { return EmissiveMap != null; }
        }

        public virtual string EmissiveMapName
        {
            get { return MapNames[6]; }
        }

        public virtual Sd.Bitmap EmissiveMap
        {
            set { maps[6] = new Sd.Bitmap(value); }
            get
            {
                if (maps[6] != null)
                {
                    return new Sd.Bitmap(maps[6]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 07 | DISPLACEMENT

        public virtual double DisplacementScale
        {
            get { return displacementScale; }
        }

        public virtual bool HasDisplacement
        {
            get { return hasDisplacement; }
        }

        public virtual string DisplacementMapName
        {
            get { return MapNames[7]; }
        }

        public virtual Sd.Bitmap DisplacementMap
        {
            set { maps[7] = new Sd.Bitmap(value); }
            get
            {
                if (maps[7] != null)
                {
                    return new Sd.Bitmap(maps[7]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 08 | AO

        public virtual double AmbientOcclusion
        {
            get { return ambient; }
        }

        public virtual bool HasAmbientOcclusionMap
        {
            get { return AmbientOcclusionMap != null; }
        }

        public virtual string AmbientOcclusionMapName
        {
            get { return MapNames[8]; }
        }

        public virtual Sd.Bitmap AmbientOcclusionMap
        {
            set { maps[8] = new Sd.Bitmap(value); }
            get
            {
                if (maps[8] != null)
                {
                    return new Sd.Bitmap(maps[8]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 09 | SHEEN

        public virtual double Sheen
        {
            get { return sheen; }
        }

        public virtual Sd.Color SheenColor
        {
            get { return sheenColor; }
        }

        public virtual bool HasSheen
        {
            get { return hasSheen; }
        }

        public virtual bool HasSheenMap
        {
            get { return SheenMap != null; }
        }

        public virtual string SheenMapName
        {
            get { return MapNames[9]; }
        }

        public virtual Sd.Bitmap SheenMap
        {
            set { maps[9] = new Sd.Bitmap(value); }
            get
            {
                if (maps[9] != null)
                {
                    return new Sd.Bitmap(maps[9]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 10 | SHEEN ROUGHNESS

        public virtual double SheenRoughness
        {
            get { return sheenRoughness; }
        }

        public virtual bool HasSheenRoughness
        {
            get { return hasSheenRoughness; }
        }

        public virtual bool HasSheenRoughnessMap
        {
            get { return SheenRoughnessMap != null; }
        }

        public virtual string SheenRoughnessMapName
        {
            get { return MapNames[10]; }
        }

        public virtual Sd.Bitmap SheenRoughnessMap
        {
            set { maps[10] = new Sd.Bitmap(value); }
            get
            {
                if (maps[10] != null)
                {
                    return new Sd.Bitmap(maps[10]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 11 | CLEARCOAT

        public virtual double Clearcoat
        {
            get { return clearcoat; }
        }

        public virtual bool HasClearcoat
        {
            get { return hasClearcoat; }
        }

        public virtual bool HasClearcoatMap
        {
            get { return ClearCoatMap != null; }
        }

        public virtual string ClearCoatMapName
        {
            get { return MapNames[11]; }
        }

        public virtual Sd.Bitmap ClearCoatMap
        {
            set { maps[11] = new Sd.Bitmap(value); }
            get
            {
                if (maps[11] != null)
                {
                    return new Sd.Bitmap(maps[11]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 12 | CLEARCOAT ROUGHNESS

        public virtual double ClearcoatRoughness
        {
            get { return clearcoatRoughness; }
        }

        public virtual bool HasClearcoatRoughness
        {
            get { return hasClearcoatRoughness; }
        }

        public virtual bool HasClearcoatRoughnessMap
        {
            get { return ClearCoatRoughnessMap != null; }
        }

        public virtual string ClearCoatRoughnessMapName
        {
            get { return MapNames[12]; }
        }

        public virtual Sd.Bitmap ClearCoatRoughnessMap
        {
            set { maps[12] = new Sd.Bitmap(value); }
            get
            {
                if (maps[12] != null)
                {
                    return new Sd.Bitmap(maps[12]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 13 | CLEARCOAT NORMAL

        public virtual bool HasClearcoatNormalMap
        {
            get { return ClearCoatNormalMap != null; }
        }

        public virtual string ClearCoatNormalMapName
        {
            get { return MapNames[13]; }
        }

        public virtual Sd.Bitmap ClearCoatNormalMap
        {
            set { maps[13] = new Sd.Bitmap(value); }
            get
            {
                if (maps[13] != null)
                {
                    return new Sd.Bitmap(maps[13]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 14 | OPACITY

        public virtual double Opacity
        {
            get { return opacity; }
        }

        public virtual bool IsTransparent
        {
            get
            {
                if (this.DiffuseColor.A < 255) return true;
                if (this.HasOpacityMap) return true;
                return false;
            }
        }

        public virtual bool HasOpacity
        {
            get { return hasOpacity; }
        }

        public virtual bool HasOpacityMap
        {
            get { return OpacityMap != null; }
        }

        public virtual string OpacityMapName
        {
            get { return MapNames[14]; }
        }

        public virtual Sd.Bitmap OpacityMap
        {
            set { maps[14] = new Sd.Bitmap(value); }
            get
            {
                if (maps[14] != null)
                {
                    return new Sd.Bitmap(maps[14]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 15 | OPACITY ROUGHNESS

        public virtual double OpacityRoughness
        {
            get { return opacityRoughness; }
        }

        public virtual bool HasOpacityRoughness
        {
            get { return hasOpacityRoughness; }
        }

        public virtual bool HasOpacityRoughnessMap
        {
            get { return OpacityRoughnessMap != null; }
        }

        public virtual string OpacityRoughnessMapName
        {
            get { return MapNames[15]; }
        }

        public virtual Sd.Bitmap OpacityRoughnessMap
        {
            set { maps[15] = new Sd.Bitmap(value); }
            get
            {
                if (maps[15] != null)
                {
                    return new Sd.Bitmap(maps[15]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 16 | OPACITY IOR

        public virtual double OpacityIor
        {
            get { return this.opacityIor; }
        }

        public virtual bool HasOpacityIor
        {
            get { return this.hasOpacityIor; }
        }

        public virtual bool HasOpacityIorMap
        {
            get { return OpacityIorMap != null; }
        }

        public virtual string OpacityIorMapName
        {
            get { return MapNames[16]; }
        }

        public virtual Sd.Bitmap OpacityIorMap
        {
            set { maps[16] = new Sd.Bitmap(value); }
            get
            {
                if (maps[16] != null)
                {
                    return new Sd.Bitmap(maps[16]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 17 | ANISOTROPIC

        public virtual bool HasAnisotropicMap
        {
            get { return AnisotropicMap != null; }
        }

        public virtual string AnisotropicMapName
        {
            get { return MapNames[17]; }
        }

        public virtual Sd.Bitmap AnisotropicMap
        {
            set { maps[17] = new Sd.Bitmap(value); }
            get
            {
                if (maps[17] != null)
                {
                    return new Sd.Bitmap(maps[17]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 18 | ANISOTROPIC ROTATION

        public virtual bool HasAnisotropicRotationMap
        {
            get { return AnisotropicRotationMap != null; }
        }

        public virtual string AnisotropicRotationMapName
        {
            get { return MapNames[18]; }
        }

        public virtual Sd.Bitmap AnisotropicRotationMap
        {
            set { maps[18] = new Sd.Bitmap(value); }
            get
            {
                if (maps[18] != null)
                {
                    return new Sd.Bitmap(maps[18]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 19 | SPECULAR

        public virtual bool HasSpecularMap
        {
            get { return SpecularMap != null; }
        }

        public virtual string SpecularMapName
        {
            get { return MapNames[19]; }
        }

        public virtual Sd.Bitmap SpecularMap
        {
            set { maps[19] = new Sd.Bitmap(value); }
            get
            {
                if (maps[19] != null)
                {
                    return new Sd.Bitmap(maps[19]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 20 | SPECULAR TINT

        public virtual bool HasSpecularTintMap
        {
            get { return SpecularTintMap != null; }
        }

        public virtual string SpecularTintMapName
        {
            get { return MapNames[20]; }
        }

        public virtual Sd.Bitmap SpecularTintMap
        {
            set { maps[20] = new Sd.Bitmap(value); }
            get
            {
                if (maps[20] != null)
                {
                    return new Sd.Bitmap(maps[20]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        //VOLUMETRICS
        #region 21 | TRANSMISSION

        public virtual bool HasTransmissionMap
        {
            get { return TransmissionMap != null; }
        }

        public virtual double Transmission
        {
            get { return transmission; }
        }

        public virtual string TransmissionMapName
        {
            get { return MapNames[21]; }
        }

        public virtual Sd.Bitmap TransmissionMap
        {
            set { maps[21] = new Sd.Bitmap(value); }
            get
            {
                if (maps[21] != null)
                {
                    return new Sd.Bitmap(maps[21]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 22 | SUBSURFACE

        public virtual bool HasSubSurfaceMap
        {
            get { return SubSurfaceMap != null; }
        }

        public virtual string SubSurfaceMapName
        {
            get { return MapNames[22]; }
        }

        public virtual Sd.Bitmap SubSurfaceMap
        {
            set { maps[22] = new Sd.Bitmap(value); }
            get
            {
                if (maps[22] != null)
                {
                    return new Sd.Bitmap(maps[22]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 23 | SUBSURFACE SCATTERING

        public virtual bool HasSubSurfaceScatterMap
        {
            get { return SubSurfaceScatterMap != null; }
        }

        public virtual string SubSurfaceScatterMapName
        {
            get { return MapNames[23]; }
        }

        public virtual Sd.Bitmap SubSurfaceScatterMap
        {
            set { maps[23] = new Sd.Bitmap(value); }
            get
            {
                if (maps[23] != null)
                {
                    return new Sd.Bitmap(maps[23]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region 24 | SUBSURFACE SCATTERING RADIUS

        public virtual bool HasSubSurfaceScatterRotationMap
        {
            get { return SubSurfaceScatterRotationMap != null; }
        }

        public virtual string SubSurfaceScatterRotationMapName
        {
            get { return MapNames[24]; }
        }

        public virtual Sd.Bitmap SubSurfaceScatterRotationMap
        {
            set { maps[24] = new Sd.Bitmap(value); }
            get
            {
                if (maps[24] != null)
                {
                    return new Sd.Bitmap(maps[24]);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        public virtual bool HasVolumetrics
        {
            get { return hasVolumetrics; }
        }

        public virtual double IOR
        {
            get { return opacityIor; }
        }

        public virtual double RefractionRatio
        {
            get { return refractionRatio; }
        }

        #endregion

        #region methods

        public void SetTextureMap(Sd.Color diffuseColor, Sd.Bitmap map = null)
        {
            this.color = diffuseColor;
            if (map != null)
            {
                this.color = Sd.Color.White;
                this.TextureMap = new Sd.Bitmap(map);
            }
        }

        public void SetBumpMap(Sd.Bitmap map, double intensity = 1.0)
        {
            this.bumpIntensity = intensity;
            if(map!=null) this.BumpMap = new Sd.Bitmap(map);
        }

        public void SetClearcoatMap(double clearcoat, Sd.Bitmap map = null)
        {
            this.hasClearcoat = true;
            this.clearcoat = clearcoat;
            if (map != null) this.ClearCoatMap = new Sd.Bitmap(map);
        }

        public void SetClearcoatRoughnessMap(double roughness, Sd.Bitmap map = null)
        {
            this.hasClearcoat = true;
            this.hasClearcoatRoughness = true;
            this.clearcoatRoughness = roughness;
            if (map != null) this.ClearCoatRoughnessMap = new Sd.Bitmap(map);
        }

        public void SetClearcoatNormalMap(Sd.Bitmap map = null)
        {
            this.hasClearcoat = (map != null); ;
            if (map != null) this.ClearCoatNormalMap = new Sd.Bitmap(map);
        }

        public void SetDisplacementMap(Sd.Bitmap map, double scale = 1.0)
        {
            this.hasDisplacement = (map != null);
            this.displacementScale = scale;
            if(map != null)this.DisplacementMap = new Sd.Bitmap(map);
        }

        public void SetEmissivity(double intensity, Sd.Color color, Sd.Bitmap map = null)
        {
            this.hasEmissive = true;

            this.emissiveIntensity = intensity;
            this.emissiveColor = color;
            if (map != null) this.EmissiveMap = new Sd.Bitmap(map);
        }

        public void SetMetalnessMap(double intensity = 1.0, Sd.Bitmap map = null)
        {
            this.hasMetalness = true;
            this.metalness = intensity;
            if (map != null) this.MetalnessMap = new Sd.Bitmap(map);
        }

        public void SetNormalMap(Sd.Bitmap map, double scale = 1.0)
        {
            this.normalScale = scale;
            if (map != null) this.NormalMap = new Sd.Bitmap(map);
        }

        public void SetOpacityMap(double opacity, Sd.Bitmap map = null)
        {
            this.hasOpacity = true;
            this.opacity = opacity;
            this.transparent = true;
            if (map != null) this.OpacityMap = new Sd.Bitmap(map);
        }

        public void SetOpacityRoughnessMap(double opacityRoughness, Sd.Bitmap map = null)
        {
            this.hasOpacityRoughness = true;
            this.opacityRoughness = opacityRoughness;
            if (map != null) this.OpacityRoughnessMap = new Sd.Bitmap(map);
        }

        public void SetOpacityIorMap(double opacityIor, Sd.Bitmap map = null)
        {
            this.hasOpacityIor = true;
            this.opacityIor = opacityIor;
            if (map != null) this.OpacityIorMap = new Sd.Bitmap(map);
        }

        public void SetRoughness(double roughness, Sd.Bitmap map = null)
        {
            this.hasRoughness = true;
            this.roughness = roughness;
            if (map != null) this.RoughnessMap = new Sd.Bitmap(map);
        }

        public void SetAmbientOcclusion(double intensity, Sd.Bitmap map = null)
        {
            this.ambient = intensity;
            if (map != null) this.AmbientOcclusionMap = new Sd.Bitmap(map);
        }

        public void SetSheen(double sheen, Sd.Color color, Sd.Bitmap map = null)
        {
            this.hasSheen = true;
            this.sheen = sheen;
            this.sheenColor = color;
            if (map != null) this.SheenMap = new Sd.Bitmap(map);
        }

        public void SetSheenRoughness(double roughness, Sd.Bitmap map = null)
        {
            this.hasSheen = true;
            this.hasSheenRoughness = true;
            this.sheenRoughness = roughness;
            if (map != null) this.SheenRoughnessMap = new Sd.Bitmap(map);
        }

        public void SetVolume(double transmission, double ior, double refractionRatio, Sd.Bitmap map = null)
        {
            this.hasVolumetrics = true;
            this.transmission = transmission;
            this.opacityIor = ior;
            this.refractionRatio = refractionRatio;
            if (map != null) this.TransmissionMap = new Sd.Bitmap(map);
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Material | "+ materialType.ToString();
        }

        #endregion

    }
}
