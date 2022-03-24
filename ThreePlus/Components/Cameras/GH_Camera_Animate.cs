using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Cameras
{
    public class GH_Camera_Animate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Camera_Animate class.
        /// </summary>
        public GH_Camera_Animate()
          : base("Camera Animate", "Move Cam",
              "Animate between sequential camera positions",
              Constants.ShortName, "Cameras")
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
            pManager.AddGenericParameter("Camera", "C", "A camera object", GH_ParamAccess.item);
            pManager.AddLineParameter("Positions", "P", "A series of lines specifying the position and target of the camera", GH_ParamAccess.list);
            pManager.AddNumberParameter("Speed", "S", "The animation speed multiplier", GH_ParamAccess.item, 1.0);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Camera Element", "C", "The updated Three Plus Camera Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Camera camera = new Camera();
            if (!DA.GetData(0, ref camera)) return;
            camera = new Camera(camera);

            List<Line> lines = new List<Line>();
            DA.GetDataList(1, lines);

            double speed = 1.0;
            DA.GetData(2, ref speed);

            camera.SetTweens(lines,speed);

            DA.SetData(0, camera);
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
                return Properties.Resources.Three_Camera_Orbit_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7d7e0f2e-b101-4a99-8cc8-0d16bb445c81"); }
        }
    }
}