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

        protected Sd.Color color = Sd.Color.LightGray;
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

        public Material(Sd.Color color):base()
        {
            this.type = "MeshBasicMaterial";
            this.color = color;
        }

        public Material(Material material) : base(material)
        {
            this.color = material.color;
        }

        #endregion

        #region properties

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
