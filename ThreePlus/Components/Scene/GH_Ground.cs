using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Classes
{
    public class GH_Ground : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ground class.
        /// </summary>
        public GH_Ground()
          : base("Ground", "Ground",
              "Description",
              Constants.ShortName, "Scene")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Size", "S", "The width / height of the ground plane.", GH_ParamAccess.item, 10000);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Z Offset", "Z", "The ground plane", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Ground", "G", "A Scene Ground object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double size = 10000.0;
            DA.GetData(0, ref size);

            double height = 0.0;
            DA.GetData(1, ref height);

            Ground ground = new Ground(size,height);

            DA.SetData(0, ground);
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
            get { return new Guid("de28cee7-0b99-4a9b-b648-1174f71d1dd2"); }
        }
    }
}