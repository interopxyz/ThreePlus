using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using Sd = System.Drawing;

namespace ThreePlus.Components.Materials.Maps
{
    public class GH_Map_Clearcoat : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Map_Clearcoat class.
        /// </summary>
        public GH_Map_Clearcoat()
          : base("Clearcoat Map", "Clearcoat",
              "Apply Clearcoat Maps to a material",
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
            pManager.AddGenericParameter("Clearcoat Map", "Cm", "The red channel of this texture is multiplied against .clearcoat, for per-pixel control over a coating's intensity." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Clearcoat", "C", "Represents the intensity of the clear coat layer 0-1.", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Roughness Map", "Rm", "The green channel of this texture is multiplied against the Clearcoat Roughness, for per-pixel control over a coating's roughness." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Roughness", "R", "Roughness of the clear coat layer 0-1.", GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager.AddGenericParameter("Normal Map", "Nm", "Can be used to enable independent normals for the clear coat layer." + System.Environment.NewLine + "(A System Drawing Bitmap or full filepath to an image.)", GH_ParamAccess.item);
            pManager[5].Optional = true;
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

            double sheen = model.Material.NormalScale;
            DA.GetData(2, ref sheen);

            IGH_Goo gooA = null;
            bool hasImageA = DA.GetData(1, ref gooA);
            Sd.Bitmap bitmapA = null;
            string messageA = string.Empty;
            if (hasImageA)
            {
                if (!gooA.TryGetBitmap(out bitmapA, out messageA))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, messageA);
                    return;
                }
            }

            model.Material.SetClearcoatMap(sheen, bitmapA);

            double roughness = model.Material.NormalScale;
            DA.GetData(4, ref roughness);

            IGH_Goo gooB = null;
            bool hasImageB = DA.GetData(3, ref gooB);
            Sd.Bitmap bitmapB = null;
            string messageB = string.Empty;
            if (hasImageB)
            {
                if (!gooB.TryGetBitmap(out bitmapB, out messageB))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, messageB);
                    return;
                }
            }

            model.Material.SetClearcoatRoughnessMap(roughness, bitmapB);

            IGH_Goo gooC = null;
            bool hasImageC = DA.GetData(5, ref gooC);
            Sd.Bitmap bitmapC = null;
            string messageC = string.Empty;
            if (hasImageC)
            {
                if (!gooC.TryGetBitmap(out bitmapC, out messageC))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, messageC);
                    return;
                }
            }

            model.Material.SetClearcoatNormalMap(bitmapC);

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
                return Properties.Resources.Three_MaterialMaps_Clearcoat3;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ed065431-6416-45c7-b959-0043f24ae9d2"); }
        }
    }
}