using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components
{
    public class GH_Atmosphere : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Atmosphere class.
        /// </summary>
        public GH_Atmosphere()
          : base("Atmosphere", "Atmosphere",
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
            pManager.AddColourParameter("Color", "C", "The fog color", GH_ParamAccess.item,Color.White);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Density", "D", "The fog density", GH_ParamAccess.item, 0.5);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Atmosphere", "A", "A Scene Atmosphere object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color color = Color.White;
            if (!DA.GetData(0, ref color)) return;

            double density = 0.5;
            DA.GetData(1, ref density);

            Atmosphere atmosphere = new Atmosphere(color,density);

            DA.SetData(0, atmosphere);
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
            get { return new Guid("e31a4845-4ee7-41e0-ac09-f320c818eac1"); }
        }
    }
}