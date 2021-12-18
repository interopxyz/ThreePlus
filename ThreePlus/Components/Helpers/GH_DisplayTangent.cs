using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Helpers
{
    public class GH_DisplayTangent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_DisplayTangent class.
        /// </summary>
        public GH_DisplayTangent()
          : base("Display Tangents", "Tangents",
              "Description",
              Constants.ShortName, "Helper")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Model or Curve, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddNumberParameter("Size", "S", "The length of the vector line", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Width", "D", "The width of the vector line", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Color", "C", "The color of the vector line", GH_ParamAccess.item, Color.Magenta);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "The updated Model", GH_ParamAccess.item);
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

            double size = 10;
            DA.GetData(1, ref size);

            double width = 1;
            DA.GetData(2, ref width);

            Color color = Color.Magenta;
            DA.GetData(3, ref color);

            model.Tangents = new TangentDisplay(size, width, color);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("436c2ce7-0fbb-442c-9e0f-e16279ea1d1d"); }
        }
    }
}