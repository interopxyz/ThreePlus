using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class TangentDisplay : SceneObject
    {

        #region members

        bool show = false;

        protected double size = 10;
        protected double width = 1;

        protected Sd.Color color = Sd.Color.Magenta;

        #endregion

        #region constructors

        public TangentDisplay() : base()
        {

        }

        public TangentDisplay(double size, double width, Sd.Color color) : base()
        {
            this.show = true;

            this.size = size;
            this.width = width;
            this.color = color;
        }

        public TangentDisplay(TangentDisplay tangentDisplay) : base(tangentDisplay)
        {
            this.show = tangentDisplay.show;

            this.size = tangentDisplay.size;
            this.width = tangentDisplay.width;

            this.color = tangentDisplay.color;
        }

        #endregion

        #region properties

        public virtual bool Show
        {
            get { return show; }
        }

        public virtual Sd.Color Color
        {
            get { return color; }
        }

        public virtual double Size
        {
            get { return size; }
        }

        public virtual double Width
        {
            get { return width; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Tangent | ";
        }

        #endregion

    }
}