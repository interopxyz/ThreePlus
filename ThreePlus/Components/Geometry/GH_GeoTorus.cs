using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoTorus : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoTorus class.
        /// </summary>
        public GH_GeoTorus()
          : base("Torus Geometry", "Torus",
              "A Three JS Standard Geometry Torus",
              Constants.ShortName, "Shapes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "The base plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Radius", "R", "The radius", GH_ParamAccess.item, 7);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Thickness", "T", "The torus thickness", GH_ParamAccess.item, 3);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("U Divisions", "U", "The number of divisions about the axis", GH_ParamAccess.item, 24);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("V Divisions", "V", "The number of divisions along the axis", GH_ParamAccess.item, 24);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model Element", "M", "A Three Plus Model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane plane = Plane.Unset;
            DA.GetData(0, ref plane);

            double radius = 7;
            DA.GetData(1, ref radius);

            double thickness = 3;
            DA.GetData(2, ref thickness);

            int divisionsU = 24;
            DA.GetData(3, ref divisionsU);

            int divisionsV = 24;
            DA.GetData(4, ref divisionsV);

            Shape shape = Shape.TorusShape(plane,radius,thickness,divisionsU,divisionsV);
            Model model = new Model(shape);

            prevModels.Add(model);
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
                return Properties.Resources.Three_Shape_Torus_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("09d71035-8a74-4487-bb1c-61707627bb53"); }
        }
    }
}