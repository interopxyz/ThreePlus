using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Sd = System.Drawing;

namespace ThreePlus.Components.Graphics
{
    public class GH_GraphicsPaths : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Graphics class.
        /// </summary>
        public GH_GraphicsPaths()
          : base("Graphics", "Graphics",
              "A material for adding curve stroke appearance properties. ",
              Constants.ShortName, "Materials")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Three Plus Model or Curve", GH_ParamAccess.item);
            pManager.AddColourParameter("Colors", "C", "The graphic colors. A single color or list of colors corresponding to each control point.", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Width", "W", "The stroke width.",GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Dash", "D", "The dashed stroke solid.", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Gap", "G", "The dashed stroke gap.", GH_ParamAccess.item);
            pManager[4].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model Element", "M", "The updated Three Plus Model Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            Model model = null;
            if (goo.CastTo<Model>(out model))
            {
                model = new Model(model);
            }
            else
            {
                model = goo.ToModel();
            }

            List<Sd.Color> colors = new List<Sd.Color>();
            bool hasColors = DA.GetDataList(1, colors);

            double width = model.Graphic.Width;
            if(DA.GetData(2, ref width)) model.Graphic.Width = width;

            double dash = model.Graphic.DashLength;
            if(DA.GetData(3, ref dash)) model.Graphic.DashLength= dash;

            double gap = model.Graphic.GapLength;
            if(DA.GetData(4, ref gap)) model.Graphic.GapLength = gap;

            if (model.IsCurve)
            {
                if (hasColors)
                {
                    if (colors.Count > 1) { 
                    int countA = colors.Count;
                    int countB = model.Curve.Points.Count;
                    for (int i = countA; i < countB;i++)
                    {
                            int j = i % countA;
                        colors.Add(colors[j]);
                    }
                        model.Graphic.Colors = colors;
                    }
                    else
                    {
                        model.Graphic.Color = colors[0];
                    }
                }
            }

            DA.SetData(0, model);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.Three_Graphics_Stroke_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("B2A69216-F8D1-4632-86E2-D0440A8C4F6D"); }
        }
    }
}