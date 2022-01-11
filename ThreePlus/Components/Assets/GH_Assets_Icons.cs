using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Assets
{
    public class GH_Assets_Icons : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Assets_Icons class.
        /// </summary>
        public GH_Assets_Icons()
          : base("Icon Images", "Icons",
              "A series of preselected icons",
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
            paramA.AddNamedValue("Circle", 0);
            paramA.AddNamedValue("Ring", 1);
            paramA.AddNamedValue("Square", 2);
            paramA.AddNamedValue("Diamond", 3);
            paramA.AddNamedValue("Triangle", 4);
            paramA.AddNamedValue("Cross", 5);
            paramA.AddNamedValue("Frame", 6);
            paramA.AddNamedValue("Marker", 7);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "Img", "The selected icon bitmap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int index = 0;
            DA.GetData(0, ref index);

            Bitmap bitmap = new Bitmap(Properties.Resources.Icons_Circle_01);
            switch (index)
            {
                case 1:
                    bitmap = new Bitmap(Properties.Resources.Icons_Ring_01);
                    break;
                case 2:
                    bitmap = new Bitmap(Properties.Resources.Icons_Square_01);
                    break;
                case 3:
                    bitmap = new Bitmap(Properties.Resources.Icons_Diamond_01);
                    break;
                case 4:
                    bitmap = new Bitmap(Properties.Resources.Icons_Triangle_01);
                    break;
                case 5:
                    bitmap = new Bitmap(Properties.Resources.Icons_Cross_01);
                    break;
                case 6:
                    bitmap = new Bitmap(Properties.Resources.Icons_Frame_01);
                    break;
                case 7:
                    bitmap = new Bitmap(Properties.Resources.Icons_Marker_01);
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
                return Properties.Resources.Three_Assets_Marker2_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9e2b625a-7620-4e72-ba7a-615cafcfbfeb"); }
        }
    }
}