using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoSphere : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoSphere class.
        /// </summary>
        public GH_GeoSphere()
          : base("Sphere Geometry", "Sphere",
              "A Three JS Standard Geometry Sphere",
              Constants.ShortName, "Shapes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "The plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Radius", "R", "The radius", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("U Divisions", "U", "The number of divisions about the axis", GH_ParamAccess.item, 24);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("V Divisions", "V", "The number of divisions about the circle", GH_ParamAccess.item, 24);
            pManager[3].Optional = true;
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
            Plane plane = Plane.WorldXY;
            DA.GetData(0, ref plane);

            double radius = 10.0;
            DA.GetData(1, ref radius);

            int u = 24;
            DA.GetData(2, ref u);

            int v = 24;
            DA.GetData(3, ref v);

            Shape shape = Shape.SphereShape(plane, radius, u, v);
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
                return Properties.Resources.Three_Shape_Sphere_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8d0952f5-2fe4-4dae-8b37-094f39bdc9b8"); }
        }
    }
}