using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Materials
{
    public class GH_MaterialPhong : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_MaterialPhong class.
        /// </summary>
        public GH_MaterialPhong()
          : base("Phong Material", "Phong Material",
              "A material for shiny surfaces with specular highlights.",
              Constants.ShortName, "Materials")
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
            pManager.AddGenericParameter("Model Element", "M", "A Three Plus Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The material color", GH_ParamAccess.item, Color.Wheat);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Shininess", "S", "The material shininess value (0.0 flat to 1.0 shiny)", GH_ParamAccess.item, 1.0);
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

            double shininess = 1.0;
            DA.GetData(2, ref shininess);

            model.Material = Material.PhongMaterial(color,shininess);

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
                return Properties.Resources.Three_Materials_Phong_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("77b54dfe-6c2a-4f0b-81fe-41eb32cf4a44"); }
        }
    }
}