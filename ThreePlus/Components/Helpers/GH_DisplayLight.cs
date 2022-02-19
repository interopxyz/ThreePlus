using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace ThreePlus.Components.Helpers
{
    public class GH_DisplayLight : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_DisplayLight class.
        /// </summary>
        public GH_DisplayLight()
          : base("Display Lights", "Display Light",
              "Apply to helper object to visualize light type specific properties.",
              Constants.ShortName, "Helpers")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Light", "L", "A light object", GH_ParamAccess.item);
            pManager.AddNumberParameter("Size", "S", "The preview size", GH_ParamAccess.item, 5);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Color", "C", "The preview color", GH_ParamAccess.item, Color.Gray);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light", "L", "An updated light object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            double size = 5;
            DA.GetData(1, ref size);

            Color color = Color.Gray;
            DA.GetData(2, ref color);

            Light light = new Light();
            Sky sky = new Sky();

            if (goo.CastTo<Light>(out light))
            {
                light = new Light(light);
                light.SetHelper(size, color);

                DA.SetData(0, light);
            }
            else if (goo.CastTo<Sky>(out sky))
            {
                if (sky.HasLight)
                {
                    sky = new Sky(sky);
                    light = new Light(sky.SunLight);
                    light.SetHelper(size, color);

                    sky.SunLight = light;
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
                return Properties.Resources.Three_Helper_Light_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("58CB6685-8A28-4903-AA82-FB1A301D340A"); }
        }
    }
}