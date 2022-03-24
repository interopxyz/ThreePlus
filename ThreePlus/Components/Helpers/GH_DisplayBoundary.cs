using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace ThreePlus.Components.Helpers
{
    public class GH_DisplayBoundary : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_DisplayBoundary class.
        /// </summary>
        public GH_DisplayBoundary()
          : base("Display Boundary", "Display Bounds",
              "Helper object to graphically show the world-axis-aligned bounding box around an object.",
              Constants.ShortName, "Helpers")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model Element", "M", "A Model Element, Curve, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The preview color", GH_ParamAccess.item, Color.Gray);
            pManager[1].Optional = true;
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

            Color color = Color.Gray;
            DA.GetData(1, ref color);

            model.SetHelper(color);

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
                return Properties.Resources.Three_Helper_Boundary_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("384DC19D-E2B9-467D-B6F0-A408497F68CD"); }
        }
    }
}