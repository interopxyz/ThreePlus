using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Emissive : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Emissive class.
        /// </summary>
        public GH_Map_Emissive()
          : base("Emissive Map", "Emissive",
              "Apply a Emissive Map to a material",
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
            pManager.AddGenericParameter("Model", "M", "The Model with a Material to update", GH_ParamAccess.item);
            pManager.AddGenericParameter("Emissive Map", "Img", "Set emisssive (glow) map. The emissive map color is modulated by the emissive color and the emissive intensity. If you have an emissive map, be sure to set the emissive color to something other than black." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Emissive Intensity", "I", "Intensity of the emissive light. Modulates the emissive color 0-1.", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Emissive Color", "C", "Emissive (light) color of the material, essentially a solid color unaffected by other lighting.", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "The updated Model", GH_ParamAccess.item);
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

            double val = model.Material.EmissiveIntensity;
            DA.GetData(2, ref val);

            Sd.Color color = model.Material.EmissiveColor;
            DA.GetData(3, ref color);

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
                    model.Material.SetEmissivity(val,color,bitmap);


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
                return Properties.Resources.Three_MaterialMaps_Emissive_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("feae7cad-3630-4f3e-9e34-35eedbce872f"); }
        }
    }
}