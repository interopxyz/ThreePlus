using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components
{
    public class GH_TransformModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_TransformModel class.
        /// </summary>
        public GH_TransformModel()
          : base("Transform Model", "Xform Model",
              "Description",
              Constants.ShortName, "Transform")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.hidden; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddTransformParameter("Transform", "X", "The transforms to be applied to the model", GH_ParamAccess.list);
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

            List<Transform> transforms = new List<Transform>();
            DA.GetDataList(1, transforms);

            model.SetTransforms(transforms);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4dfaf548-cef9-48b3-911d-e5587c31cda6"); }
        }
    }
}