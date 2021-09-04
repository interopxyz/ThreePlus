using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Lights
{
    public class GH_LightDirectional : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_LightDirectional class.
        /// </summary>
        public GH_LightDirectional()
          : base("Directional Light", "Directional Light",
              "Description",
              Constants.ShortName, "Lights")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color", "C", "The lights color", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddPointParameter("Position", "P", "The position of the light", GH_ParamAccess.item, new Point3d(100, 100, 100));
            pManager[1].Optional = true;
            pManager.AddPointParameter("Target", "T", "The target of the light", GH_ParamAccess.item, new Point3d(0, 0, 0));
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The light's strength/intensity.", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Light", "L", "A Directional light", GH_ParamAccess.item);
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

            Point3d target = new Point3d(0, 0, 0);
            DA.GetData(2, ref target);

            double intensity = 1;
            DA.GetData(3, ref intensity);

            Light light = Light.DirectionalLight(position, target, intensity, color);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("01ae5a7e-1b5c-4cb4-8a66-0febc1910356"); }
        }
    }
}