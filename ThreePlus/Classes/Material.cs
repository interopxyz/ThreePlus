using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Material : SceneObject
    {

        #region members

        public enum Types { None, Basic, Lambert, Standard, Phong, Toon, Physical, Normal, Depth, Shadow }

        protected Sd.Bitmap[] maps = new Sd.Bitmap[14];
        public string[] MapNames = new string[14];

        protected Types materialType = Types.Phong;

        protected Sd.Color color = Sd.Color.Wheat;
        protected bool transparent = false;

        protected bool isWireframe = false;
        protected bool isFlatShaded = false;

        protected double shininess = 1;
        protected double reflectivity = 0.5;
        protected List<int> steps = new List<int>();

        protected bool hasBumpMap = false;
        protected double bumpIntensity = 1.0;

        protected bool hasClearcoat = false;
        protected bool hasClearcoatRoughness = false;
        protected double clearcoat = 0.0;
        protected double clearcoatRoughness = 0.0;

        protected bool hasDisplacement = false;
        protected double displacementScale = 1;

        protected bool hasEmissive = false;
        protected Sd.Color emissiveColor = Sd.Color.White;
        protected double emissiveIntensity = 1.0;

        protected bool isMetalic = false;
        protected double metalness = 0.5;

        protected bool hasNormalMap = false;
        protected double normalScale = 0.5;

        protected bool hasOpacityMap = false;
        protected double opacity = 1;

        protected bool hasRoughness = false;
        protected double roughness = 1;

        protected bool hasSheen = false;
        protected bool hasSheenRoughness = false;
        protected double sheen = 0.0;
        protected Sd.Color sheenColor = Sd.Color.White;
        protected double sheenRoughness = 0.0;

        protected bool hasVolumetrics = false;
        protected double transmission = 0.0;
        protected double ior = 1.5;
        protected double refractionRatio = 1.5;

        #endregion

        #region constructors

        public Material() : base()
        {
            this.type = "MeshBasicMaterial";
        }

        public Material(Material material) : base(material)
        {
            
            for (int i = 0; i < 14; i++) if(material.maps[i]!=null) this.maps[i] = material.maps[i];
                for (int i =0;i<14;i++) this.MapNames[i] = material.MapNames[i];

            this.materialType = material.materialType;

            this.color = material.color;
            this.transparent = material.transparent;

            this.isWireframe = material.isWireframe;
            this.isFlatShaded = material.isFlatShaded;

            this.shininess = material.shininess;
            this.reflectivity = material.reflectivity;
            this.steps = material.steps;

            this.hasBumpMap = material.hasBumpMap;
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

            this.isMetalic = material.isMetalic;
            this.metalness = material.metalness;

            this.hasNormalMap = material.hasNormalMap;
            this.normalScale = material.normalScale;

            this.hasOpacityMap = material.hasOpacityMap;
            this.opacity = material.opacity;

            this.hasRoughness = material.hasRoughness;
            this.roughness = material.roughness;

            this.hasSheen = material.hasSheen;
            this.hasSheenRoughness = material.hasSheenRoughness;
            this.sheen = material.sheen;
            this.sheenColor = material.sheenColor;
            this.sheenRoughness = material.sheenRoughness;

            this.hasVolumetrics = material.hasVolumetrics;
            this.transmission = material.transmission;
            this.ior = material.ior;
            this.refractionRatio = material.refractionRatio;
        }

        public static Material ShadowMaterial(Sd.Color color)
        {
            Material material = new Material();

            material.type = "MeshShadowMaterial";
            material.materialType = Types.Shadow;
            material.color = color;

            return material;
        }

        public static Material BasicMaterial(Sd.Color color)
        {
            Material material = new Material();

            material.type = "MeshBasicMaterial";
            material.materialType = Types.Basic;
            material.color = color;

            return material;
        }

        public static Material LambertMaterial(Sd.Color color)
        {
            Material material = new Material();

            material.type = "MeshLambertMaterial";
            material.materialType = Types.Lambert;
            material.color = color;

            return material;
        }

        public static Material PhongMaterial(Sd.Color color, double shininess)
        {
            Material material = new Material();

            material.type = "MeshPhongMaterial";
            material.materialType = Types.Phong;
            material.color = color;
            material.shininess = shininess;

            return material;
        }

        public static Material ToonMaterial(Sd.Color color, int steps)
        {
            Material material = new Material();

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
            Material material = new Material();

            material.type = "MeshStandardMaterial";
            material.materialType = Types.Standard;
            material.color = color;
            material.hasRoughness = true;
            material.roughness = roughness;
            material.metalness = metalness;
            material.hasRoughness = true;

            return material;
        }

        public static Material PhysicalMaterial(Sd.Color color, double roughness, double metalness, double sheen, Sd.Color sheenColor, double sheenRoughness, double reflectivity, double clearcoat, double clearcoatRoughness)
        {
            Material material = new Material();

            material.type = "MeshPhysicalMaterial";
            material.materialType = Types.Physical;
            material.color = color;
            material.hasRoughness = true;
            material.roughness = roughness;
            material.reflectivity = reflectivity;
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
            Material material = new Material();

            material.type = "MeshNormalMaterial";
            material.materialType = Types.Normal;

            return material;
        }

        public static Material DepthMaterial()
        {
            Material material = new Material();

            material.type = "MeshDepthMaterial";
            material.materialType = Types.Depth;

            return material;
        }


        #endregion

        #region properties

        public virtual Sd.Bitmap[] Maps
        {
            get { return maps; }
            }

        public virtual Types MaterialType
        {
            get { return materialType; }
        }

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

        //BUMP MAP
        public virtual bool HasBumpMap
        {
            get { return hasBumpMap; }
        }

        public virtual double BumpIntensity
        {
            get { return bumpIntensity; }
        }

        public virtual string BumpMapName
        {
            get { return MapNames[1]; }
        }

        public virtual Sd.Bitmap BumpMap
        {
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

        //CLEARCOAT
        public virtual bool HasClearcoat
        {
            get { return hasClearcoat; }
        }

        public virtual bool HasClearcoatMap
        {
            get { return ClearCoatMap != null; }
        }

        public virtual double Clearcoat
        {
            get { return clearcoat; }
        }

        public virtual string ClearCoatMapName
        {
            get { return MapNames[2]; }
        }

        public virtual Sd.Bitmap ClearCoatMap
        {
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

        public virtual bool HasClearcoatNormalMap
        {
            get { return ClearCoatNormalMap!=null; }
        }

        public virtual string ClearCoatNormalMapName
        {
            get { return MapNames[3]; }
        }

        public virtual Sd.Bitmap ClearCoatNormalMap
        {
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

        public virtual bool HasClearcoatRoughness
        {
            get { return hasClearcoatRoughness; }
        }

        public virtual bool HasClearcoatRoughnessMap
        {
            get { return ClearCoatRoughnessMap != null; }
        }

        public virtual double ClearcoatRoughness
        {
            get { return clearcoatRoughness; }
        }

        public virtual string ClearCoatRoughnessMapName
        {
            get { return MapNames[4]; }
        }

        public virtual Sd.Bitmap ClearCoatRoughnessMap
        {
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

        //DISPLACEMENT
        public virtual bool HasDisplacement
        {
            get { return hasDisplacement; }
        }

        public virtual double DisplacementScale
        {
            get { return displacementScale; }
        }

        public virtual string DisplacementMapName
        {
            get { return MapNames[5]; }
        }

        public virtual Sd.Bitmap DisplacementMap
        {
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

        //EMISSIVE
        public virtual bool IsEmissive
        {
            get { return hasEmissive; }
        }

        public virtual Sd.Color EmissiveColor
        {
            get { return emissiveColor; }
        }

        public virtual double EmissiveIntensity
        {
            get { return emissiveIntensity; }
        }

        public virtual string EmissiveMapName
        {
            get { return MapNames[6]; }
        }

        public virtual Sd.Bitmap EmissiveMap
        {
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

        //METALNESS
        public virtual bool IsMetalic
        {
            get { return isMetalic; }
        }

        public virtual bool HasMetalicMap
        {
            get { return MetalnessMap!=null; }
        }

        public virtual double Metalness
        {
            get { return metalness; }
        }

        public virtual string MetalnessMapName
        {
            get { return MapNames[7]; }
        }

        public virtual Sd.Bitmap MetalnessMap
        {
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

        //NORMALS
        public virtual bool HasNormalMap
        {
            get { return hasNormalMap; }
        }

        public virtual double NormalScale
        {
            get { return normalScale; }
        }

        public virtual string NormalMapName
        {
            get { return MapNames[8]; }
        }

        public virtual Sd.Bitmap NormalMap
        {
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

        //OPACITY
        public virtual bool HasOpacity
        {
            get { return hasOpacityMap; }
        }

        public virtual bool HasOpacityMap
        {
            get { return OpacityMap!=null; }
        }

        public virtual double Opacity
        {
            get { return opacity; }
        }

        public virtual string OpacityMapName
        {
            get { return MapNames[9]; }
        }

        public virtual Sd.Bitmap OpacityMap
        {
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

        public virtual bool IsTransparent
        {
            get 
            { 
                if (this.DiffuseColor.A < 255) return true;
                if (this.HasOpacityMap) return true;
                return false;
            }
        }

        //ROUGHNESS
        public virtual bool HasRoughness
        {
            get { return hasRoughness; }
        }

        public virtual bool HasRoughnessMap
        {
            get { return RoughnessMap != null; }
        }

        public virtual double Roughness
        {
            get { return roughness; }
        }

        public virtual string RoughnessMapName
        {
            get { return MapNames[10]; }
        }

        public virtual Sd.Bitmap RoughnessMap
        {
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

        //SHEEN
        public virtual bool HasSheen
        {
            get { return hasSheen; }
        }

        public virtual bool HasSheenMap
        {
            get { return SheenColorMap != null; }
        }

        public virtual bool HasSheenRoughness
        {
            get { return hasSheenRoughness; }
        }

        public virtual bool HasSheenRoughnessMap
        {
            get { return SheenRoughnessMap != null; }
        }

        public virtual double Sheen
        {
            get { return sheen; }
        }

        public virtual Sd.Color SheenColor
        {
            get { return sheenColor; }
        }

        public virtual string SheenColorMapName
        {
            get { return MapNames[11]; }
        }

        public virtual Sd.Bitmap SheenColorMap
        {
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

        public virtual double SheenRoughness
        {
            get { return sheenRoughness; }
        }

        public virtual string SheenRoughnessMapName
        {
            get { return MapNames[12]; }
        }

        public virtual Sd.Bitmap SheenRoughnessMap
        {
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

        //VOLUMETRICS
        public virtual bool HasVolumetrics
        {
            get { return hasVolumetrics; }
        }

        public virtual double Transmission
        {
            get { return transmission; }
        }

        public virtual double IOR
        {
            get { return ior; }
        }

        public virtual double RefractionRatio
        {
            get { return refractionRatio; }
        }

        public virtual string TransmissionMapName
        {
            get { return MapNames[13]; }
        }

        public virtual Sd.Bitmap TransmissionMap
        {
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

        //MAPS

        public virtual bool HasEmissiveMap
        {
            get { return EmissiveMap != null; }
        }

        #endregion

        #region methods

        public void SetTextureMap(Sd.Color diffuseColor, Sd.Bitmap map = null)
        {
            this.color = diffuseColor;
            if(map!=null) this.maps[0] = new Sd.Bitmap(map);
        }

        public void SetBumpMap(Sd.Bitmap map, double intensity = 1.0)
        {
            this.hasBumpMap = (map != null); ;
            this.bumpIntensity = intensity;
            if(map!=null) this.maps[1] = new Sd.Bitmap(map);
        }

        public void SetClearcoatMap(double clearcoat, Sd.Bitmap map = null)
        {
            this.hasClearcoat = true;
            this.clearcoat = clearcoat;
            if (map != null) maps[2] = new Sd.Bitmap(map);
        }

        public void SetClearcoatRoughnessMap(double roughness, Sd.Bitmap map = null)
        {
            this.hasClearcoat = true;
            this.hasClearcoatRoughness = true;
            this.clearcoatRoughness = roughness;
            if (map != null) this.maps[4] = new Sd.Bitmap(map);
        }

        public void SetClearcoatNormalMap(Sd.Bitmap map = null)
        {
            this.hasClearcoat = (map != null); ;
            this.maps[3] = new Sd.Bitmap(map);
        }

        public void SetDisplacementMap(Sd.Bitmap map, double scale = 1.0)
        {
            this.hasDisplacement = (map != null);
            this.displacementScale = scale;
            if(map != null)this.maps[5] = new Sd.Bitmap(map);
        }

        public void SetEmissivity(double intensity, Sd.Color color, Sd.Bitmap map = null)
        {
            this.hasEmissive = true;

            this.emissiveIntensity = intensity;
            this.emissiveColor = color;
            if (map != null) this.maps[6] = new Sd.Bitmap(map);
        }

        public void SetMetalnessMap(double intensity = 1.0, Sd.Bitmap map = null)
        {
            this.isMetalic = true;
            this.metalness = intensity;
            if (map != null) this.maps[7] = new Sd.Bitmap(map);
        }

        public void SetNormalMap(Sd.Bitmap map, double scale = 1.0)
        {
            this.hasNormalMap = (map != null);
            this.normalScale = scale;
            this.maps[8] = new Sd.Bitmap(map);
        }

        public void SetOpacityMap(double opacity, Sd.Bitmap map = null)
        {
            this.hasOpacityMap = true;
            this.opacity = opacity;
            this.transparent = true;
            if (map != null) this.maps[9] = new Sd.Bitmap(map);
        }

        public void SetRoughness(double roughness, Sd.Bitmap map = null)
        {
            this.hasRoughness = true;
            this.roughness = roughness;
            if (map != null) this.maps[10] = new Sd.Bitmap(map);
        }

        public void SetSheen(double sheen, Sd.Color color, Sd.Bitmap map = null)
        {
            this.hasSheen = true;
            this.sheen = sheen;
            this.sheenColor = color;
            if (map != null) this.maps[11] = new Sd.Bitmap(map);
        }

        public void SetSheenRoughness(double roughness, Sd.Bitmap map = null)
        {
            this.hasSheen = true;
            this.hasSheenRoughness = true;
            this.sheenRoughness = roughness;
            if (map != null) this.maps[12] = new Sd.Bitmap(map);
        }

        public void SetVolume(double transmission, double ior, double refractionRatio, Sd.Bitmap map = null)
        {
            this.hasVolumetrics = true;
            this.transmission = transmission;
            this.ior = ior;
            this.refractionRatio = refractionRatio;
            if (map != null) this.maps[13] = new Sd.Bitmap(map);
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
