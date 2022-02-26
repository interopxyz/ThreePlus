using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Materials
{
    public class GH_MaterialShadow : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_MaterialShadow class.
        /// </summary>
        public GH_MaterialShadow()
          : base("Shadow Material", "Shadow Material",
              "This material can receive shadows, but otherwise is completely transparent.",
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
            pManager.AddGenericParameter("Model", "M", "A Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The shadows color", GH_ParamAccess.item, Color.Transparent);
            pManager[1].Optional = true;
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

            Color color = Color.Transparent;
            DA.GetData(1, ref color);

            model.Material = Material.ShadowMaterial(color);

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
                return Properties.Resources.Three_Materials_Shadow_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e0576127-d6b1-4f7a-a489-146d0c68537e"); }
        }
    }
}