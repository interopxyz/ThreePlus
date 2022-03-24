using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components.Output
{
    public class GH_BitmapSerialize : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_BitmapSerialize class.
        /// </summary>
        public GH_BitmapSerialize()
          : base("Serialize Bitmap", "SerBmp",
              "Serialize a Bitmap to a byte array string. Useful for internalizing a Bitmsp in a definition.",
              Constants.ShortName, "Output")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "A System.Drawing.Bitmap image object or full file path to an image.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "The serialized Bitmap text.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Warning: This component creates large single strings. Do not plug the output into a panel to preview.");

            IGH_Goo goo = null;
            bool hasImage = DA.GetData(0, ref goo);
            Sd.Bitmap bitmap = null;
            string message = string.Empty;
            if (hasImage)
            {
                if (!goo.TryGetBitmap(out bitmap, out message))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                    return;
                }
            }

            DA.SetData(0,bitmap.ToStr());
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
                return Properties.Resources.Three_Output_SerializeBitmap_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dfd29b1b-19fa-4ed5-a5bf-b856d5e81874"); }
        }
    }
}