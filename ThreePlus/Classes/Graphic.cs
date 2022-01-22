using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Graphic
    {
        #region members

        protected List<Sd.Color> colors = new List<Sd.Color>();
        protected Sd.Color color = Sd.Color.Black;

        protected double width = 1.0;
        protected double dashLength = 0.0;
        protected double gapLength = 1.0;

        #endregion

        #region constructors

        public Graphic() : base()
        {

        }

        public Graphic(Graphic graphic) : base()
        {
            this.Colors = graphic.colors;
            this.color = Sd.Color.FromArgb(graphic.color.A, graphic.color.R, graphic.color.G, graphic.color.B);

            this.width = graphic.width;
            this.dashLength = graphic.dashLength;
            this.gapLength = graphic.gapLength;
        }

        #endregion

        #region properties

        public virtual List<Sd.Color> Colors
        {
            get { return colors; }
            set
            {
                this.colors.Clear();
                foreach (Sd.Color clr in value) this.colors.Add(clr);
            }
        }

        public virtual Sd.Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public virtual bool HasColors
        {
            get { return colors.Count > 0; }
        }

        public virtual double Width
        {
            get { return width; }
            set { width = value; }
        }

        public virtual double DashLength
        {
            get { return dashLength; }
            set { dashLength = value; }
        }

        public virtual double GapLength
        {
            get { return gapLength; }
            set { gapLength = value; }
        }

        public virtual bool HasDash
        {
            get { return dashLength != 0.0; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides



        #endregion

    }
}
