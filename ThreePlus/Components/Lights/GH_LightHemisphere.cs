using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Lights
{
    public class GH_LightHemisphere : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_LightHemisphere class.
        /// </summary>
        public GH_LightHemisphere()
          : base("Hemisphere Light", "Hemisphere Light",
              "Description",
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
            pManager.AddColourParameter("Zenith Color", "Z", "The lights color at the Zenith", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddColourParameter("Horizon Color", "H", "The lights color at the Horizon", GH_ParamAccess.item, Color.White);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The light's strength/intensity.", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light", "L", "A Hemisphere light", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color zenith = Color.White;
            DA.GetData(0, ref zenith);

            Color horizon = Color.White;
            DA.GetData(1, ref horizon);

            double intensity = 1;
            DA.GetData(2, ref intensity);

            Light light = Light.HemisphereLight(intensity, zenith,horizon);

            DA.SetData(0, light);
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
                return Properties.Resources.Three_Light_Hemisphere_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("645f506e-2e76-4ced-8fc9-fdb0f0f23c7d"); }
        }
    }
}