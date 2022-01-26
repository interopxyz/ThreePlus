using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Edges : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Edges class.
        /// </summary>
        public GH_Map_Edges()
          : base("Show Edges", "Edges",
              "Display Edges on a mesh",
              Constants.ShortName, "Materials")
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
            pManager.AddGenericParameter("Model", "M", "A Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The edge color", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Width", "W", "The edge display width", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Threshold", "T", "The cutoff angle threshold for edge display", GH_ParamAccess.item);
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

            Sd.Color color = model.Graphic.Color;
            DA.GetData(1, ref color);

            double weight = model.Graphic.Width;
            DA.GetData(2, ref weight);

            double threshold = model.EdgeThreshold;
            DA.GetData(3, ref threshold);

            model.SetEdges(threshold,color,weight);

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
            get { return new Guid("0D9B657A-61DC-4A6D-9048-9DCA75CBBE74"); }
        }
    }
}