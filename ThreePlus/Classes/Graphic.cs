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

        public Graphic(Rhino.DocObjects.ObjectAttributes attr) : base()
        {
            Rhino.DocObjects.Layer layer = Rhino.RhinoDoc.ActiveDoc.Layers[attr.LayerIndex];

            switch (attr.PlotColorSource)
            {
                case Rhino.DocObjects.ObjectPlotColorSource.PlotColorFromObject:
                    this.color = attr.PlotColor;
                    break;
                case Rhino.DocObjects.ObjectPlotColorSource.PlotColorFromLayer:
                    this.color = layer.PlotColor;
                    break;
            }
            this.color = attr.PlotColor;

            this.Colors = new List<Sd.Color> { this.color };

            switch(attr.PlotWeightSource)
            {
                case Rhino.DocObjects.ObjectPlotWeightSource.PlotWeightFromObject:
                    this.width = attr.PlotWeight;
                    break;
                case Rhino.DocObjects.ObjectPlotWeightSource.PlotWeightFromLayer:
                    this.width = layer.PlotWeight;
                    break;
            }

            if (this.width == 0) this.width = 1.0;

            int linetypeIndex = attr.LinetypeIndex;
            if (attr.LinetypeSource == Rhino.DocObjects.ObjectLinetypeSource.LinetypeFromLayer) linetypeIndex = layer.LinetypeIndex;

            Rhino.DocObjects.Linetype type = Rhino.RhinoDoc.ActiveDoc.Linetypes[linetypeIndex];

            if(linetypeIndex != -1)
            {
                double len0 = 0;
                bool test0 = false;
                type.GetSegment(0,out len0,out test0);

                if (type.PatternLength > 1)
                {
                double len1 = 0;
                bool test1 = false;
                type.GetSegment(1, out len1, out test1);

                    if (test0)
                    {
                        this.dashLength = len0;
                        this.gapLength = len1;
                    }
                    else
                    {
                        this.dashLength = len1;
                        this.gapLength = len0;
                    }
                }
                else
                {
                    this.dashLength = len0;
                    this.gapLength = len0;
                }

            }
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
