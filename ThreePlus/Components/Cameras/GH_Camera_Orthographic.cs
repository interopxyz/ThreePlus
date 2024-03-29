﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Cameras
{
    public class GH_Camera_Orthographic : GH_Preview
    {
        /// <summary>
        /// Initializes a new instance of the GH_Camera_Orthographic class.
        /// </summary>
        public GH_Camera_Orthographic()
          : base("Camera Orthographic", "Ortho Cam",
              "Camera that uses orthographic projection.",
              Constants.ShortName, "Cameras")
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
            pManager.AddPointParameter("Position", "P", "The position of the camera", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddPointParameter("Target", "T", "The target of the camera", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Near", "N", "The camera frustum  near plane.", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Far", "F", "The camera frustum far plane.", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Camera Element", "C", "A Three Plus Camera Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Camera camera = new Camera();
            Point3d position = camera.Position;
            DA.GetData(0, ref position);

            Point3d target = camera.Target;
            DA.GetData(1, ref target);

            double nearPlane = camera.Near;
            DA.GetData(2, ref nearPlane);

            double farPlane = camera.Far;
            DA.GetData(3, ref farPlane);

            camera = new Camera(position, target, nearPlane, farPlane);

            DA.SetData(0, camera);
            prevCameras.Add(camera);
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
                return Properties.Resources.Three_Camera_Ortho_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("694ffbc6-aaec-47b9-b188-4ef928bf1456"); }
        }
    }
}