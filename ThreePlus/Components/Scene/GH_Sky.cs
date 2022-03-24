using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ThreePlus.Components
{
    public class GH_Sky : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Sky class.
        /// </summary>
        public GH_Sky()
          : base("Sky", "Sky",
              "Adds a generated sky box map to a scene based on location and atmospheric properties with an optional coordinated directional light.",
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
            pManager.AddNumberParameter("Altitude", "A", "The altitude value.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Azimuth", "Z", "The azimuth value.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Turbidity", "T", "The turbidity value.", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Rayleigh", "R", "The Rayleigh value.", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Exposure", "E", "The Exposure value.", GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Coefficient", "C", "The Coefficient value.", GH_ParamAccess.item);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Directional", "D", "The Directional value.", GH_ParamAccess.item);
            pManager[6].Optional = true;
            pManager.AddBooleanParameter("Environment", "E", "If true, the environment will illuminate the scene", GH_ParamAccess.item);
            pManager[7].Optional = true;
            pManager.AddColourParameter("Sun Color", "S", "An optional sun color. If a color is provided a directional light representing the sun will be created. If empty, no directional lighting will be added.", GH_ParamAccess.item);
            pManager[8].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene Element", "E", "A Sky Three Plus Scene Modifier Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double altitude = 2.0;
            DA.GetData(0, ref altitude);
            double azimuth = 180.0;
            DA.GetData(1, ref azimuth);
            double turbidity = 10.0;
            DA.GetData(2, ref turbidity);
            double rayleigh = 3.0;
            DA.GetData(3, ref rayleigh);
            double exposure = 0.5;
            DA.GetData(4, ref exposure);
            double coefficient = 0.005;
            DA.GetData(5, ref coefficient);
            double directional = 0.7;
            DA.GetData(6, ref directional);
            bool environment = true;
            DA.GetData(7, ref environment);

            Color color = Color.Black;
            bool hasLight = DA.GetData(8, ref color);

            Sky sky = new Sky(altitude,azimuth,turbidity,rayleigh,exposure,coefficient,directional,environment);

            if (hasLight) sky.SetLight(color);

            DA.SetData(0, sky);
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
                return Properties.Resources.Three_SceneSky_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7f89d433-9268-480f-98dc-fe52202ea7ef"); }
        }
    }
}