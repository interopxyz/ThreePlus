using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Lights
{
    public class GH_LightShadows : GH_Preview
    {
        /// <summary>
        /// Initializes a new instance of the GH_LightShadows class.
        /// </summary>
        public GH_LightShadows()
          : base("Add Shadows", "Shadows",
              "Apply to most lights to enable shadows",
              Constants.ShortName, "Lights")
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
            pManager.AddGenericParameter("Light / Sky", "L", "A Light Element or Sky Scene Element", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Samples", "S", "The shadow resolution samples", GH_ParamAccess.item, 20);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Threshold", "T", "The opacity threshold for casting shadows", GH_ParamAccess.item, 0.5);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light Element", "L", "The updated Three Plus Light Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            int samples = 20;
            DA.GetData(1, ref samples);

            double threshold = 0.5;
            DA.GetData(2, ref threshold);

            Light light = new Light();
            Sky sky = new Sky();

            if (goo.CastTo<Light>(out light))
            {
                light = new Light(light);
                light.SetShadow(samples, threshold);

                DA.SetData(0, light);
                prevLights.Add(light);
            }
            else if (goo.CastTo<Sky>(out sky))
            {
                if (sky.HasLight)
                {
                    sky = new Sky(sky);
                    light = new Light(sky.SunLight);
                    light.SetShadow(samples, threshold);

                    sky.SunLight = light;
                    prevLights.Add(light);
                }
                DA.SetData(0, sky);
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
                return Properties.Resources.Three_Light_Shadow_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("eb0d263e-43bb-43bc-b431-58e362c8665b"); }
        }
    }
}