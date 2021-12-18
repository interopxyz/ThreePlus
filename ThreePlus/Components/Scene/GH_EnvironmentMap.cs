using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components
{
    public class GH_EnvironmentMap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_EnvironmentMap class.
        /// </summary>
        public GH_EnvironmentMap()
          : base("Environment Map", "EnvMap",
              "Description",
              Constants.ShortName, "Scene")
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
            pManager.AddColourParameter("Background Color", "C", "The background color is an image is not used.", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Bitmap", "I", "A bitmap image", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Is Background", "B", "If true the image will be used as the background", GH_ParamAccess.item, true);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Is Environment", "E", "If true the image will be used as the reflection environment for physical based materials", GH_ParamAccess.item, true);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Environment", "E", "The Environment Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color color = Color.White;
            DA.GetData(0, ref color);

            Bitmap envMap = null;
            DA.GetData(1, ref envMap);

            bool hasBackground = true;
            DA.GetData(2, ref hasBackground);

            bool hasEnvironment = true;
            DA.GetData(3, ref hasEnvironment);

            Environment environment = new Environment();
            if (envMap != null)
            {
            environment = new Environment(envMap);
            environment.IsBackground = hasBackground;
            environment.IsEnvironment = hasEnvironment;
            }
                environment.Background = color;

            DA.SetData(0, environment);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0bdac5df-b7aa-411c-93ac-2c020bd53d6f"); }
        }
    }
}