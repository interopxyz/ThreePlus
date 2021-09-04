using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Axes : SceneObject
    {

        #region members

        bool show = false;

        protected double scale = 10;

        protected Sd.Color xAxis = Sd.Color.Red;
        protected Sd.Color yAxis = Sd.Color.Green;
        protected Sd.Color zAxis = Sd.Color.Blue;

        #endregion

        #region constructors

        public Axes() : base()
        {

        }

        public Axes(double scale) : base()
        {
            this.show = true;

            this.scale = scale;
        }

        public Axes(double scale, Sd.Color xAxis, Sd.Color yAxis, Sd.Color zAxis) : base()
        {
            this.show = true;

            this.scale = scale;

            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.zAxis = zAxis;
        }

        public Axes(Axes axes) : base(axes)
        {
            this.show = axes.show;

            this.scale = axes.scale;

            this.xAxis = axes.xAxis;
            this.yAxis = axes.yAxis;
            this.zAxis = axes.zAxis;
        }

        #endregion

        #region properties

        public virtual bool Show
        {
            get { return show; }
        }

        public virtual Sd.Color XAxis
        {
            get { return xAxis; }
        }

        public virtual Sd.Color YAxis
        {
            get { return yAxis; }
        }

        public virtual Sd.Color ZAxis
        {
            get { return zAxis; }
        }

        public virtual double Scale
        {
            get { return scale; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Axes | ";
        }

        #endregion

    }
}
