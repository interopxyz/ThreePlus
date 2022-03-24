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
              "Physically based rendering (PBR) materials use a physically correct shading to create a material that will react 'correctly' under all lighting scenarios",
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
            pManager.AddGenericParameter("Model Element", "M", "A Three Plus Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The material's color", GH_ParamAccess.item, Color.Wheat);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Roughness", "X", "How rough the material appears. 0.0 means a smooth mirror reflection, 1.0 means fully diffuse.", GH_ParamAccess.item, 0.1);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Reflectivity", "R", "Degree of reflectivity, from 0.0 to 1.0.", GH_ParamAccess.item, 0.5);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Metalness", "M", "How much the material is like a metal. Non-metallic materials such as wood or stone use 0.0, metallic use 1.0, with nothing (usually) in between.", GH_ParamAccess.item, 0.5);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Sheen", "S", "The intensity of the sheen layer, from 0.0 to 1.0.", GH_ParamAccess.item, 0.0);
            pManager[5].Optional = true;
            pManager.AddColourParameter("Sheen Color", "Sc", "The color of the sheen layer", GH_ParamAccess.item, Color.White);
            pManager[6].Optional = true;
            pManager.AddNumberParameter("Clearcoat", "L", "Represents the intensity of the clear coat layer, from 0.0 to 1.0.", GH_ParamAccess.item, 0.0);
            pManager[7].Optional = true;
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

            double roughness = 0.1;
            DA.GetData(2, ref roughness);

            double reflectivity = 0.5;
            DA.GetData(3, ref reflectivity);

            double metalness = 0.5;
            DA.GetData(4, ref metalness);

            double sheen = 0.0;
            DA.GetData(5, ref sheen);

            Color sheenColor = Color.White;
            DA.GetData(6, ref sheenColor);

            double clearcoat = 0.0;
            DA.GetData(7, ref clearcoat);

            model.Material = Material.PhysicalMaterial(color, roughness, metalness,sheen, sheenColor, 0.1, reflectivity, clearcoat,0.9);

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