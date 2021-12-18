using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Atmosphere : SceneObject
    {

        #region members

        protected Sd.Color color = Sd.Color.White;
        protected double density = 0.0;

        #endregion

        #region constructors

        public Atmosphere() : base()
        {

        }

        public Atmosphere(Atmosphere atmosphere) : base()
        {
            this.color = atmosphere.color;
            this.density = atmosphere.density;
        }

        public Atmosphere(Sd.Color color, double density) : base()
        {
            this.color = color;
            this.density = density;
        }

        #endregion

        #region properties

        public virtual Sd.Color Color
        {
            get { return color; }
        }

        public virtual double Density
        {
            get { return density; }
        }

        public virtual bool HasFog
        {
            get { return (density > 0.0); }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Atmosphere | ";
        }

        #endregion


    }
}
