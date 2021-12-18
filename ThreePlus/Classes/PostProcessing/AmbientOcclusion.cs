using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class AmbientOcclusion : SceneObject
    {

        #region members

        protected bool hasAO = false;

        protected double radius = 16;
        protected double min = 0.001;
        protected double max = 0.5;

        #endregion

        #region constructors

        public AmbientOcclusion() : base()
        {

        }

        public AmbientOcclusion(double radius, double min, double max) : base()
        {
            this.hasAO = true;
            this.radius = radius;
            this.min = min;
            this.max = max;
        }

        public AmbientOcclusion(AmbientOcclusion ambientOcclusion) : base(ambientOcclusion)
        {
            this.hasAO = ambientOcclusion.hasAO;
            this.radius = ambientOcclusion.radius;
            this.min = ambientOcclusion.min;
            this.max = ambientOcclusion.max;
        }

        #endregion

        #region properties

        public virtual bool HasAO
        {
            get { return hasAO; }
        }

        public virtual double Radius
        {
            get { return radius; }
        }

        public virtual double MinDistance
        {
            get { return min; }
        }

        public virtual double MaxDistance
        {
            get { return max; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "SSAO | ";
        }

        #endregion

    }
}
