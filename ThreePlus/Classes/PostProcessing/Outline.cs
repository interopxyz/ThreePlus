using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sd = System.Drawing;

namespace ThreePlus
{
    public class Outline : SceneObject
    {

        #region members

        protected bool hasOutline = false;

        protected double width = 2;
        protected Sd.Color color = Sd.Color.Black;

        #endregion

        #region constructors

        public Outline() : base()
        {

        }

        public Outline(double width, Sd.Color color) : base()
        {
            this.hasOutline = true;
            this.width= width;
            this.color = color;
        }

        public Outline(Outline outline) : base(outline)
        {
            this.hasOutline = outline.hasOutline;
            this.width = outline.width;
            this.color = outline.color;
        }

        #endregion

        #region properties

        public virtual bool HasOutline
        {
            get { return hasOutline; }
        }

        public virtual double Width
        {
            get { return width; }
        }

        public virtual Sd.Color Color
        {
            get { return color; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Outline | ";
        }

        #endregion

    }
}
