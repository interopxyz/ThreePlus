using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Normal : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Normal class.
        /// </summary>
        public GH_Map_Normal()
          : base("Normal Map", "Normal",
              "Apply a Normal Map to a material",
              Constants.ShortName, "Materials")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model Element", "M", "A Three Plus Model Element to update", GH_ParamAccess.item);
            pManager.AddGenericParameter("Normal Map", "Nm", "The texture to create a normal map. The RGB values affect the surface normal for each pixel fragment and change the way the color is lit. Normal maps do not change the actual shape of the surface, only the lighting." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Normal Scale", "S", "How much the normal map affects the material 0-1.", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model Element", "M", "The updated Three Plus Model Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Model model = new Model();
            if (!DA.GetData(0, ref model)) return;
            model = new Model(model);

            double intensity = model.Material.NormalScale;
            DA.GetData(2, ref intensity);

            IGH_Goo goo = null;
            bool hasImage = DA.GetData(1, ref goo);
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
            model.Material.SetNormalMap(bitmap, intensity);

            DA.SetData(0, model);
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
                return Properties.Resources.Three_MaterialMaps_Normal_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("19a136cd-67dc-4dcb-b5cb-95ef191aeba2"); }
        }
    }
}