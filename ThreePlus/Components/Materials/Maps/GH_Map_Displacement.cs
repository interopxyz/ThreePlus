using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Displacement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Displacement class.
        /// </summary>
        public GH_Map_Displacement()
          : base("Displacement Map", "Displacement",
              "Apply a Displacement Map to a material",
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
            pManager.AddGenericParameter("Transmission Map", "Img", "The displacement map affects the position of the mesh's vertices. Unlike other maps which only affect the light and shade of the material the displaced vertices can cast shadows, block other objects, and otherwise act as real geometry. The displacement texture is an image where the value of each pixel (white being the highest) is mapped against, and repositions, the vertices of the mesh." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Scale", "S", "How much the displacement map affects the mesh (where black is no displacement, and white is maximum displacement). Without a displacement map set, this value is not applied 0-1.", GH_ParamAccess.item);
            pManager[2].Optional = true;
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

            double intensity = model.Material.DisplacementScale;
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
            model.Material.SetDisplacementMap(bitmap, intensity);

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
                return Properties.Resources.Three_MaterialMaps_Displacement_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5632eda6-54c1-4dfc-bae8-68d05acf569c"); }
        }
    }
}