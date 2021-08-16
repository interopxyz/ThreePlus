using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Output
{
    public class GH_ToHtml : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ToHtml class.
        /// </summary>
        public GH_ToHtml()
          : base("ToHtml", "ToHtml",
              "Description",
              Constants.ShortName, "Output")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene", "S", "Scene", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "H", "The html text", GH_ParamAccess.item);
            pManager.AddTextParameter("Javascript", "J", "The javascript text", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Scene scene = new Scene();
            if (!DA.GetData(0, ref scene)) return;

            DA.SetData(0, scene.ToHtml(false));
            DA.SetData(1, scene.ToJavascript());
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
            get { return new Guid("9655eb45-8d33-4633-9988-e012c6ff9eab"); }
        }
    }
}