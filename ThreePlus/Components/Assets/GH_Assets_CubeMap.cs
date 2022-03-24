using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components.Assets
{
    public class GH_Assets_CubeMap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Assets_CubeMap class.
        /// </summary>
        public GH_Assets_CubeMap()
          : base("Cube Map", "CubeMap",
              "Create a CubeMap from six images",
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
            pManager.AddTextParameter("Name", "N", "An optional Name", GH_ParamAccess.item, "Custom");
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The intensity of a light probe", GH_ParamAccess.item, 1.0);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Positive X", "X+", "The positive X image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Negative X", "X-", "The negative X image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Positive Y", "Y+", "The positive Y image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Negative Y", "Y-", "The negative Y image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Positive Z", "Z+", "The positive Z image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Negative Z", "Z-", "The negative Z image", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CubeMap", "Img", "The selected CubeMap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "Custom";
            DA.GetData(0, ref name);

            double intensity = 1.0;
            bool hasIntensity = DA.GetData(1, ref intensity);

            string message = string.Empty;

            IGH_Goo posXg = null;
            if (!DA.GetData(2, ref posXg)) return;
            IGH_Goo negXg = null;
            if (!DA.GetData(3, ref negXg)) return;
            IGH_Goo posYg = null;
            if (!DA.GetData(4, ref posYg)) return;
            IGH_Goo negYg = null;
            if (!DA.GetData(5, ref negYg)) return;
            IGH_Goo posZg = null;
            if (!DA.GetData(6, ref posZg)) return;
            IGH_Goo negZg = null;
            if (!DA.GetData(7, ref negZg)) return;

            Sd.Bitmap posX;
            if (!posXg.TryGetBitmap(out posX, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }
            Sd.Bitmap negX;
            if (!negXg.TryGetBitmap(out negX, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }
            Sd.Bitmap posY;
            if (!posYg.TryGetBitmap(out posY, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }
            Sd.Bitmap negY;
            if (!negYg.TryGetBitmap(out negY, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }
            Sd.Bitmap posZ;
            if (!posZg.TryGetBitmap(out posZ, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }
            Sd.Bitmap negZ;
            if (!negZg.TryGetBitmap(out negZ, out message))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                return;
            }

            CubeMap cubeMap = new CubeMap(name, posX, negX, posY, negY, posZ, negZ);
            if (hasIntensity) cubeMap.Intensity = intensity;

            DA.SetData(0, cubeMap);
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
                return Properties.Resources.Three_Assets_Cube2_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("bc1e0aad-8a99-464f-a2a9-8acf3647d7e5"); }
        }
    }
}