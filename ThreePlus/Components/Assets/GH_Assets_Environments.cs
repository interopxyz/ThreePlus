using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Assets
{
    public class GH_Assets_Environments : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Assets_Environments class.
        /// </summary>
        public GH_Assets_Environments()
          : base("Environment Maps", "EnvMaps",
              "A series of preselected public domain environment maps sourced from: https://polyhaven.com/",
              Constants.ShortName, "Assets")
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
            pManager.AddIntegerParameter("Index", "I", "The index of the default bitmap", GH_ParamAccess.item, 0);
            pManager[0].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[0];
            paramA.AddNamedValue("Small Studio", 0);
            paramA.AddNamedValue("Country Hall Studio", 1);
            paramA.AddNamedValue("Loft Photo Studio", 2);
            paramA.AddNamedValue("Autoshop", 3);
            paramA.AddNamedValue("Spruit Sunrise", 4);
            paramA.AddNamedValue("Venice Sunset", 5);
            paramA.AddNamedValue("Chalk Quarry", 6);
            paramA.AddNamedValue("Pink Sunrise", 7);
            paramA.AddNamedValue("Sky on Fire", 8);
            paramA.AddNamedValue("Snowy Park", 9);
            paramA.AddNamedValue("Satara Night", 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "Img", "The selected environment bitmap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int index = 0;
            DA.GetData(0, ref index);
            
            Bitmap bitmap = new Bitmap(Properties.Resources.polyhaven_studio_small_03);
            switch (index)
            {
                case 1:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_studio_country_hall);
                    break;
                case 2:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_photo_studio_loft_hall);
                    break;
                case 3:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_autoshop);
                    break;
                case 4:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_spruit_sunrise);
                    break;
                case 5:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_venice_sunset);
                    break;
                case 6:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_sunset_in_the_chalk_quarry);
                    break;
                case 7:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_pink_sunrise);
                    break;
                case 8:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_the_sky_is_on_fire);
                    break;
                case 9:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_snowy_park);
                    break;
                case 10:
                    bitmap = new Bitmap(Properties.Resources.polyhaven_satara_night);
                    break;
            }

            DA.SetData(0, bitmap);
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
                return Properties.Resources.Three_Light_Probe_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("808ff243-5137-4d2e-bff2-6e77461bc33b"); }
        }
    }
}