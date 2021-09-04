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

        public enum Types { Basic, Lambert, Phong, Toon, Physical, Normal, Depth }

        protected Types materialType = Types.Basic;

        protected Sd.Color color = Sd.Color.Wheat;

        protected bool transparent = false;

        protected double opacity = 1;
        protected double roughness = 1;
        protected double metalness = 0.5;
        protected double emissive = 0;
        protected double emissiveIntensity = 1;

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
            this.roughness = material.roughness;
            this.metalness = material.metalness;
            this.emissive = material.emissive;
            this.emissiveIntensity = material.emissiveIntensity;
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
        public virtual double Metalness
        {
            get { return metalness; }
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
            return "Material | ";
        }

        #endregion

    }
}
