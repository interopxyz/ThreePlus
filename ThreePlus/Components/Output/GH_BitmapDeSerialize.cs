using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using Sd = System.Drawing;

namespace ThreePlus.Components.Output
{
    public class GH_BitmapDeSerialize : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_BitmapDeSerialize class.
        /// </summary>
        public GH_BitmapDeSerialize()
          : base("DeSerialize Bitmap", "DeSerBmp",
              "DeSerialize a byte array string representation of a Bitmap to a System.Drawing.Bitmap. Useful for restoring an internalized Bitmap.",
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
            pManager.AddTextParameter("Serialized Bitmap", "T", "Serialized bitmap text", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "A System.Drawing.Bitmap image object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string txt = string.Empty;
            if (!DA.GetData(0, ref txt)) return;

            Sd.Bitmap bitmap = null;

            byte[] bytes = Convert.FromBase64String(txt);
            MemoryStream memoryStream = new MemoryStream(bytes);

            memoryStream.Position = 0;

            bitmap = (Sd.Bitmap)Sd.Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            bytes = null;

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
                return Properties.Resources.Three_Output_DeSerializeBitmap_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("723b1ae2-6120-4dd8-beee-f7000dd15c5a"); }
        }
    }
}