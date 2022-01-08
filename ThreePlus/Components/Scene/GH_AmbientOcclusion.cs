using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components
{
    public class GH_AmbientOcclusion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_AmbientOcclusion class.
        /// </summary>
        public GH_AmbientOcclusion()
          : base("Ambient Occlusion", "AO",
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
            pManager.AddNumberParameter("Radius", "R", "The radius", GH_ParamAccess.item, 16);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Min Distance", "C", "The min distance", GH_ParamAccess.item, 0.001);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Max Distance", "F", "The max distance", GH_ParamAccess.item, 0.5);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Ambient Occlusion", "A", "The Ambient Occlusion Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double radius = 16;
            DA.GetData(0, ref radius);
            double min = 0.001;
            DA.GetData(1, ref min);
            double max = 0.5;
            DA.GetData(2, ref max);

            AmbientOcclusion ambientOcclusion= new AmbientOcclusion(radius,min,max);

            DA.SetData(0, ambientOcclusion);
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
                return Properties.Resources.Three_SceneAO_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6f7ad539-56ad-474d-9e00-d28bb210a49b"); }
        }
    }
}