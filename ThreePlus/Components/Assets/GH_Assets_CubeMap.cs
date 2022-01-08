using Grasshopper.Kernel;
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
            pManager.AddTextParameter("Name", "N", "An optional Name", GH_ParamAccess.item, "Custom");
            pManager[0].Optional = true;
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

            Sd.Bitmap posX = null;
            if (!DA.GetData(1, ref posX))return;
            Sd.Bitmap negX = null;
            if (!DA.GetData(2, ref negX)) return;
            Sd.Bitmap posY = null;
            if (!DA.GetData(3, ref posY)) return;
            Sd.Bitmap negY = null;
            if (!DA.GetData(4, ref negY)) return;
            Sd.Bitmap posZ = null;
            if (!DA.GetData(5, ref posZ)) return;
            Sd.Bitmap negZ = null;
            if (!DA.GetData(6, ref negZ)) return;

            CubeMap cubeMap = new CubeMap(name, posX, negX, posY, negY, posZ, negZ);

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