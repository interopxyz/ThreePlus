using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using System.Drawing;

namespace ThreePlus.Components.Helpers
{
    public class GH_Axes : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Axes class.
        /// </summary>
        public GH_Axes()
          : base("Axes", "Axes",
              "Description",
              Constants.ShortName, "Helper")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Scale", "S", "The scale of the axes", GH_ParamAccess.item, 10);
            pManager[0].Optional = true;
            pManager.AddColourParameter("X Axis", "X", "The X axis color", GH_ParamAccess.item, Color.Red);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Y Axis", "Y", "The Y axis color", GH_ParamAccess.item, Color.Green);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Z Axis", "Z", "The X axis color", GH_ParamAccess.item, Color.Blue);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Axes", "A", "The Axes object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double scale = 10;
            DA.GetData(0, ref scale);

            Color xAxis = Color.Red;
            DA.GetData(1, ref xAxis);

            Color yAxis = Color.Green;
            DA.GetData(2, ref yAxis);

            Color zAxis = Color.Blue;
            DA.GetData(3, ref zAxis);

            Axes axes = new Axes(scale, xAxis, yAxis, zAxis);

            DA.SetData(0, axes);

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
                return Properties.Resources.Three_Helper_Axis_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d2daa7b2-7ff9-4f38-9551-366ade7372f9"); }
        }
    }
}