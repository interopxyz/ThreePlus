using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoTorusKnot : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoTorusKnot class.
        /// </summary>
        public GH_GeoTorusKnot()
          : base("Torus Knot Geometry", "Torus Knot",
              "A Three JS Standard Geometry Torus Knot",
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
            pManager.AddNumberParameter("Radius", "R", "The radius", GH_ParamAccess.item, 5);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Thickness", "T", "The torus thickness", GH_ParamAccess.item, 2.5);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("P", "P", "How many times the geometry winds around its axis of rotational symmetry", GH_ParamAccess.item, 2);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Q", "Q", "How many times the geometry winds around a circle in the interior of the torus", GH_ParamAccess.item, 3);
            pManager[4].Optional = true;
            pManager.AddIntegerParameter("U Divisions", "U", "The number of divisions about the axis", GH_ParamAccess.item, 72);
            pManager[5].Optional = true;
            pManager.AddIntegerParameter("V Divisions", "V", "The number of divisions along the axis", GH_ParamAccess.item, 24);
            pManager[6].Optional = true;
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

            double radius = 5;
            DA.GetData(1, ref radius);

            double thickness = 2.5;
            DA.GetData(2, ref thickness);

            int p = 2;
            DA.GetData(3, ref p);

            int q = 3;
            DA.GetData(4, ref q);

            int divisionsU = 72;
            DA.GetData(5, ref divisionsU);

            int divisionsV = 24;
            DA.GetData(6, ref divisionsV);

            Shape shape = Shape.TorusKnotShape(plane, radius, thickness, p, q, divisionsU, divisionsV);
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
                return Properties.Resources.Three_Shape_TorusKnot_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2da590bd-c42b-4958-b315-2d0fa3c1bc43"); }
        }
    }
}