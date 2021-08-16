using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components
{
    public class GH_Scene : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Scene class.
        /// </summary>
        public GH_Scene()
          : base("Scene", "Scene",
              "Description",
              Constants.ShortName, "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene Objects", "O", "Scene Objects including (Curves, Breps, Meshes, Lights, Cameras, Three Objects)", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene", "S", "Scene", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            Scene scene = new Scene();
            foreach (IGH_Goo goo in goos)
            {
                scene.Models.Add(goo.ToModel());
            }

            DA.SetData(0, scene);
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
            get { return new Guid("8eb51f10-c5c8-4e74-8c38-c6820ba815e6"); }
        }
    }
}