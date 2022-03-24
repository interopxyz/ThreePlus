using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Volume : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Volume class.
        /// </summary>
        public GH_Map_Volume()
          : base("Volumetric Maps", "Volumetric",
              "Apply a Volumetric Map to a material",
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
            pManager.AddGenericParameter("Transmission Map", "Tm", "The red channel of this texture is multiplied against .transmission, for per-pixel control over optical transparency." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Transmission", "T", "The effect factor between 0-1.0.", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("IOR", "I", "Index-of-refraction for non-metallic materials, from 1.0-2.333. Default is 1.5.", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Refraction Ratio", "R", "The index of refraction (IOR) of air (approximately 1) divided by the index of refraction of the material from 0-1.0.", GH_ParamAccess.item);
            pManager[4].Optional = true;
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

            double transmission = model.Material.Transmission;
            DA.GetData(2, ref transmission);

            double ior = model.Material.IOR;
            DA.GetData(3, ref ior);

            double refraction = model.Material.RefractionRatio;
            DA.GetData(4, ref refraction);

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
            model.Material.SetVolume(transmission,ior,refraction, bitmap);

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
                return Properties.Resources.Three_MaterialMaps_Volume_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("87c5230d-9715-41a6-8a74-75a28bb62a8b"); }
        }
    }
}