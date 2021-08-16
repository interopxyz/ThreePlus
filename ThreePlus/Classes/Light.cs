using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Light : MetaData
    {

        #region members

        protected Sd.Color color = Sd.Color.White;
        protected double intensity = 0.75;
        protected double distance = 0;
        protected double decay = 0;

        #endregion

        #region constructors

        public Light():base()
        {

        }

        public Light(Light light) : base(light)
        {
            this.color = light.color;
            this.intensity = light.intensity;
            this.distance = light.distance;
            this.decay = light.decay;
        }

        #endregion

        #region properties

        public virtual Sd.Color Color
        {
            get { return color; }
        }
        public virtual double Intensity
        {
            get { return intensity; }
        }
        public virtual double Distance
        {
            get { return distance; }
        }
        public virtual double Decay
        {
            get { return decay; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Light | ";
        }

        #endregion

    }
}
