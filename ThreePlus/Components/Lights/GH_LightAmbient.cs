using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Sd = System.Drawing;

namespace ThreePlus.Components.Lights
{
    public class GH_LightAmbient : GH_Preview
    {

        /// <summary>
        /// Initializes a new instance of the GH_LightAmbient class.
        /// </summary>
        public GH_LightAmbient()
          : base("Ambient Light", "Ambient Light",
              "This light globally illuminates all objects in the scene equally.",
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
            pManager.AddColourParameter("Color", "C", "The lights color", GH_ParamAccess.item, Sd.Color.White);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The light's strength/intensity.", GH_ParamAccess.item, 1);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light", "L", "An Ambient light", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Sd.Color color = Sd.Color.White;
            DA.GetData(0, ref color);

            double intensity = 1;
            DA.GetData(1, ref intensity);

            Light light = Light.AmbientLight(intensity, color);

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
                return Properties.Resources.Three_Light_Ambient_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d1f0bbc5-7e1b-4608-b9c1-f16ea122f7aa"); }
        }
    }
}