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

        public enum Types { Basic, Lambert, Standard, Phong, Toon, Physical, Normal, Depth }

        protected Types materialType = Types.Basic;

        protected Sd.Color color = Sd.Color.Wheat;

        protected bool transparent = false;

        protected double opacity = 1;
        protected double shininess = 1;
        protected double roughness = 1;
        protected double metalness = 0.5;
        protected double reflectivity = 0.5;
        protected double clearcoat = 0.0;
        protected double clearcoatRoughness = 0.0;
        protected double emissive = 0;
        protected double emissiveIntensity = 1;
        protected int steps = 3;

        #endregion

        #region constructors

        public Material():base()
        {
            this.type = "MeshBasicMaterial";
        }

        public Material(Material material) : base(material)
        {
            this.materialType = material.materialType;

            this.color = material.color;

            this.transparent = material.transparent;

            this.opacity = material.opacity;
            this.shininess = material.shininess;
            this.roughness = material.roughness;
            this.metalness = material.metalness;
            this.reflectivity = material.reflectivity;
            this.clearcoat = material.clearcoat;
            this.clearcoatRoughness = material.clearcoatRoughness;
            this.emissive = material.emissive;
            this.emissiveIntensity = material.emissiveIntensity;
            this.steps = material.steps;
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
            material.steps = steps;

            return material;
        }

        public static Material StandardMaterial(Sd.Color color, double roughness, double metalness)
        {
            Material material = new Material();

            material.type = "MeshStandardMaterial";
            material.materialType = Types.Standard;
            material.color = color;
            material.roughness = roughness;
            material.metalness = metalness;

            return material;
        }

        public static Material PhysicalMaterial(Sd.Color color, double roughness, double metalness, double reflectivity, double clearcoat, double clearcoatRoughness)
        {
            Material material = new Material();

            material.type = "MeshPhysicalMaterial";
            material.materialType = Types.Physical;
            material.color = color;
            material.roughness = roughness;
            material.reflectivity = reflectivity;
            material.clearcoat = clearcoat;
            material.clearcoatRoughness = metalness;

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

        public virtual Types MaterialType
        {
            get { return materialType; }
        }

        public virtual Sd.Color Color
        {
            get { return color; }
        }

        public virtual bool Transparent
        {
            get { return transparent; }
        }

        public virtual double Opacity
        {
            get { return opacity; }
        }

        public virtual double Roughness
        {
            get { return roughness; }
        }

        public virtual double Shininess
        {
            get { return shininess; }
        }

        public virtual double Metalness
        {
            get { return metalness; }
        }

        public virtual double Reflectivity
        {
            get { return reflectivity; }
        }

        public virtual double Clearcoat
        {
            get { return clearcoat; }
        }

        public virtual double ClearcoatRoughness
        {
            get { return clearcoatRoughness; }
        }

        public virtual int Steps
        {
            get { return steps; }
        }

        public virtual double Emissive
        {
            get { return emissive; }
        }

        public virtual double EmissiveIntensity
        {
            get { return emissiveIntensity; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Material | "+ materialType.ToString();
        }

        #endregion

    }
}
