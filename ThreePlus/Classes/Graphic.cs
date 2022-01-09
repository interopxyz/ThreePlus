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
        protected List<double> pattern = new List<double>();

        #endregion

        #region constructors

        public Graphic() : base()
        {

        }

        public Graphic(Graphic graphic) : base()
        {
            this.colors.Clear();
            this.Colors = graphic.colors;
            this.color = Sd.Color.FromArgb(graphic.color.A, graphic.color.R, graphic.color.G, graphic.color.B);

            this.pattern.Clear();
            foreach (double num in graphic.pattern) this.pattern.Add(num);
            this.width = graphic.width;
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

        public virtual List<double> Pattern
        {
            get { return pattern; }
            set
            {
                this.pattern.Clear();
                foreach (double num in value) this.pattern.Add(num);
            }
        }

        public virtual bool HasColors
        {
            get { return colors.Count > 0; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides



        #endregion

    }
}
