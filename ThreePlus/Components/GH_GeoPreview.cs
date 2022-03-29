using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.Display;

namespace ThreePlus.Components
{
    public abstract class GH_GeoPreview : GH_Component
    {
        private BoundingBox prevBox = new BoundingBox();
        protected List<Model> prevModels = new List<Model>();

        /// <summary>
        /// Initializes a new instance of the GH_GeoPreview class.
        /// </summary>
        public GH_GeoPreview()
          : base("GH_GeoPreview", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_GeoPreview(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
        }

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            prevBox = BoundingBox.Unset;
            prevModels = new List<Model>();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override BoundingBox ClippingBox
        {
            get
            {
                return prevBox;
            }
        }

        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {

            foreach (Model model in prevModels)
            {
                Material mat = model.Material;
                Rd.DisplayMaterial material = new Rd.DisplayMaterial(mat.DiffuseColor);

                switch (model.GeoType)
                {
                    case Model.GeometryTypes.Shape:
                        if(model.Shape.PreviewMesh!=null) args.Display.DrawMeshShaded(model.Shape.PreviewMesh, material);
                        break;
                    case Model.GeometryTypes.Mesh:
                        args.Display.DrawMeshShaded(model.Mesh, material);
                        break;
                    case Model.GeometryTypes.Curve:
                        args.Display.DrawCurve(model.Curve, model.Graphic.Color, (int)model.Graphic.Width);
                        break;
                }

                prevBox.Union(model.BoundingBox);

            }

            // Set Display Override
            base.DrawViewportWires(args);
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
            get { return new Guid("160b0dbd-5d66-4bac-86bf-29a7be3ad0c4"); }
        }
    }
}