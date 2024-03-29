﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components
{
    public class GH_LightPoint : GH_Preview
    {
        /// <summary>
        /// Initializes a new instance of the GH_Light class.
        /// </summary>
        public GH_LightPoint()
          : base("Point Light", "Point Light",
              "A light that gets emitted from a single point in all directions. A common use case for this is to replicate the light emitted from a bare lightbulb.",
              Constants.ShortName, "Lights")
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
            pManager.AddColourParameter("Color", "C", "The light color.", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddPointParameter("Position", "P", "The light position.", GH_ParamAccess.item, new Point3d(100, 100, 100));
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The light strength/intensity.", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Distance", "D", "The light maximum range.", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Decay Factor", "F", "The amount the light dims along the distance of the light.", GH_ParamAccess.item, 1);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light Element", "L", "A Three Plus Light Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color color = Color.White;
            DA.GetData(0, ref color);

            Point3d position = new Point3d(100, 100, 100);
            DA.GetData(1, ref position);

            double intensity = 1;
            DA.GetData(2, ref intensity);

            double distance = 0;
            DA.GetData(3, ref distance);

            double decay = 1;
            DA.GetData(4, ref decay);

            Light light = Light.PointLight(position, intensity, distance, decay, color);

            DA.SetData(0, light);
            prevLights.Add(light);
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
                return Properties.Resources.Three_Light_Point_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2471da93-962d-428b-8fcc-2ee1b6be35dd"); }
        }
    }
}