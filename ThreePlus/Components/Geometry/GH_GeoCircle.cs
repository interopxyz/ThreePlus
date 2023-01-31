using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoCircle : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoCircle class.
        /// </summary>
        public GH_GeoCircle()
          : base("Circle Geometry", "Circle",
              "A Three JS Standard Geometry Circle",
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
            pManager.AddNumberParameter("Outer Radius", "O", "The outer radius", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Inner Radius", "I", "The inner radius", GH_ParamAccess.item, 4);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Divisions", "D", "The number of radial divisions", GH_ParamAccess.item, 24);
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
            Plane plane = Plane.Unset;
            DA.GetData(0, ref plane);

            double radius = 10;
            DA.GetData(1, ref radius);

            double thickness = 4;
            DA.GetData(2, ref thickness);

            int divisions = 24;
            DA.GetData(3, ref divisions);

            Shape shape = new Shape();

            if(thickness==0)
            {
                shape = Shape.CircleShape(plane, radius,divisions);
            }
            else
            {
                shape = Shape.RingShape(plane, radius,thickness, divisions);
            }

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
                return Properties.Resources.Three_Shape_Circle_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ae4cd0c9-4b17-471b-ac45-372fa1d646d1"); }
        }
    }
}