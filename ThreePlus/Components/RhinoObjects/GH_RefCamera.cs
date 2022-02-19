using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefCamera : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefCamera class.
        /// </summary>
        public GH_RefCamera()
          : base("Reference View", "RefView",
              "Reference a Rhino document's camera by Name",
              Constants.ShortName, "Doc")
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
            pManager.AddTextParameter("Named View", "V", "The name of a view in this scene", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Description", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string viewName = string.Empty;
            DA.GetData(0, ref viewName);

            int index = Rhino.RhinoDoc.ActiveDoc.NamedViews.FindByName(viewName);
            Rhino.DocObjects.ViewInfo viewInfo = Rhino.RhinoDoc.ActiveDoc.NamedViews[index];

            if (viewInfo != null)
            {
                Rhino.DocObjects.ViewportInfo view = viewInfo.Viewport;
                Camera camera = new Camera();
                if (view.IsParallelProjection)
                {
                    camera = new Camera(view.CameraLocation, view.TargetPoint, view.FrustumNear/100.00, view.FrustumFar * 100.00);
                }
                else
                {
                    camera = new Camera(view.CameraLocation, view.TargetPoint, (int)view.Camera35mmLensLength, view.FrustumNear / 100.00, view.FrustumFar*100.00);
                }

                DA.SetData(0, camera);
            }
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
                return Properties.Resources.Three_Reference_Camera_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("02538de6-18eb-41a2-8cf0-8e9436b3b315"); }
        }
    }
}