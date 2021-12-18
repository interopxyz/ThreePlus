using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Materials
{
    public class GH_MaterialPhysical : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_MaterialPhysical class.
        /// </summary>
        public GH_MaterialPhysical()
          : base("Physical Material", "Physical Material",
              "Description",
              Constants.ShortName, "Materials")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The material's color", GH_ParamAccess.item, Color.Wheat);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Roughness", "R", "How rough the material appears. 0.0 means a smooth mirror reflection, 1.0 means fully diffuse.", GH_ParamAccess.item, 1.0);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Metalness", "M", "How much the material is like a metal. Non-metallic materials such as wood or stone use 0.0, metallic use 1.0, with nothing (usually) in between.", GH_ParamAccess.item, 1.0);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Reflectivity", "R", "Degree of reflectivity, from 0.0 to 1.0.", GH_ParamAccess.item, 0.5);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Clearcoat", "C", "Represents the intensity of the clear coat layer, from 0.0 to 1.0.", GH_ParamAccess.item, 0.0);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Clearcoat Roughness", "CR", "Roughness of the clear coat layer, from 0.0 to 1.0.", GH_ParamAccess.item, 0.0);
            pManager[6].Optional = true;
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

            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            Model model = null;
            if (goo.CastTo<Model>(out model))
            {
                model = new Model(model);
            }
            else
            {
                model = goo.ToModel();
            }

            Color color = Color.Wheat;
            DA.GetData(1, ref color);

            double roughness = 1.0;
            DA.GetData(2, ref roughness);

            double metalness = 1.0;
            DA.GetData(3, ref metalness);

            double reflectivity = 0.5;
            DA.GetData(4, ref reflectivity);

            double clearcoat = 0.0;
            DA.GetData(5, ref clearcoat);

            double clearcoatReflectivity = 1.0;
            DA.GetData(6, ref clearcoatReflectivity);

            model.Material = Material.PhysicalMaterial(color, roughness, metalness,reflectivity,clearcoat,clearcoatReflectivity);

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
                return Properties.Resources.Three_Materials_Physical_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("44236e49-f3bb-44d7-8e6b-8ec37980db74"); }
        }
    }
}