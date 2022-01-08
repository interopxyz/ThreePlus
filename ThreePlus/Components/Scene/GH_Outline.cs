using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Sd = System.Drawing;

namespace ThreePlus.Components
{
    public class GH_Outline : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Outline class.
        /// </summary>
        public GH_Outline()
          : base("Outline", "Outline",
              "Description",
              Constants.ShortName, "Scene")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //pManager.AddNumberParameter("Width", "W", "The outline width", GH_ParamAccess.item, 2);
            //pManager[0].Optional = true;
            //pManager.AddColourParameter("Color", "C", "The outline color", GH_ParamAccess.item, Sd.Color.Black);
            //pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Outline", "O", "The Outline Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double width = 2;
            //DA.GetData(0, ref width);
            Sd.Color color = Sd.Color.Black;
            //DA.GetData(1, ref color);

            Outline outline = new Outline(width,color);

            DA.SetData(0, outline);
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
                return Properties.Resources.Three_SceneOutline_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8f3b3ccf-8d39-420d-b07b-d596b17e4d24"); }
        }
    }
}