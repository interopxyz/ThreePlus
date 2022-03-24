using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Lights
{
    public class GH_LightSpot : GH_Preview
    {
        /// <summary>
        /// Initializes a new instance of the GH_LightSpot class.
        /// </summary>
        public GH_LightSpot()
          : base("Spot Light", "Spot Light",
              "This light gets emitted from a single point in one direction, along a cone that increases in size the further from the light it gets.",
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
            pManager.AddColourParameter("Color", "C", "The light color", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddPointParameter("Position", "P", "The light position", GH_ParamAccess.item, new Point3d(100, 100, 100));
            pManager[1].Optional = true;
            pManager.AddPointParameter("Target", "T", "The light target", GH_ParamAccess.item, new Point3d(0, 0, 0));
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The light strength/intensity.", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Decay Factor", "F", "The amount the light dims along the distance of the light.", GH_ParamAccess.item, 1);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Angle", "A", "Maximum angle of light dispersion from its direction.", GH_ParamAccess.item, Math.PI/3.0);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Penumbra", "P", "Percent of the spotlight cone that is attenuated due to penumbra.", GH_ParamAccess.item, 0);
            pManager[6].Optional = true;
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

            Point3d target = new Point3d(0, 0, 0);
            DA.GetData(2, ref target);

            double intensity = 1;
            DA.GetData(3, ref intensity);

            double distance = position.DistanceTo(target);

            double decay = 1;
            DA.GetData(4, ref decay);

            double angle = Math.PI/3.0;
            DA.GetData(5, ref angle);

            double penumbra = 0;
            DA.GetData(6, ref penumbra);

            Light light = Light.SpotLight(position, target, intensity, distance, angle, penumbra, decay, color);

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
                return Properties.Resources.Three_Light_Spot_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c4f88363-a8f4-45a3-beaf-0593beaf3268"); }
        }
    }
}