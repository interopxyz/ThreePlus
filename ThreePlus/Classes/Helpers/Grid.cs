using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Grid : SceneObject
    {

        #region members

        protected bool isPolar = false;

        bool show = false;

        protected double size = 10;
        protected double divisions = 10;

        protected Sd.Color axisColor = Sd.Color.Gray;
        protected Sd.Color gridColor = Sd.Color.DarkGray;

        #endregion

        #region constructors

        public Grid() : base()
        {

        }

        public Grid(double size, double divisions) : base()
        {
            this.show = true;

            this.size = size;
            this.divisions = divisions;
        }

        public Grid(double size, double divisions, Sd.Color axisColor, Sd.Color gridColor, bool isPolar) : base()
        {
            this.show = true;

            this.size = size;
            this.divisions = divisions;

            this.axisColor = axisColor;
            this.gridColor = gridColor;

            this.isPolar = isPolar;
        }

        public Grid(Grid grid) : base(grid)
        {
            this.show = grid.show;

            this.size = grid.size;
            this.isPolar = grid.isPolar;
            this.divisions = grid.divisions;

            this.axisColor = grid.axisColor;
            this.gridColor = grid.gridColor;
        }

        #endregion

        #region properties

        public virtual bool IsPolar
        {
            get { return isPolar; }
            set { isPolar = value; }
        }

        public virtual bool Show
        {
            get { return show; }
        }

        public virtual Sd.Color GridColor
        {
            get { return gridColor; }
        }

        public virtual Sd.Color AxisColor
        {
            get { return axisColor; }
        }

        public virtual double Size
        {
            get { return size; }
        }

        public virtual double Divisions
        {
            get { return divisions; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            string gridType = "Square";
            if (isPolar) gridType = "Polar";
            return "Grid | "+gridType;
        }

        #endregion

    }
}
