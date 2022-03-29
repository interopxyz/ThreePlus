using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoCylinder : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoCapsule class.
        /// </summary>
        public GH_GeoCylinder()
          : base("Cylinder Geometry", "Cylinder",
              "A Three JS Standard Geometry Cylinder",
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
            pManager.AddNumberParameter("Radius", "R", "The base radius", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Height", "H", "The cone height", GH_ParamAccess.item, 20);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Smooth", "S", "Is the cap smoothed", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Divisions", "D", "The number of radial divisions", GH_ParamAccess.item, 24);
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
            Plane plane = Plane.WorldXY;
            DA.GetData(0, ref plane);

            double radius = 10.0;
            DA.GetData(1, ref radius);

            double height = 20.0;
            DA.GetData(2, ref height);

            bool capsule = false;
            DA.GetData(3, ref capsule);

            int divisions = 24;
            DA.GetData(4, ref divisions);

            Shape shape = new Shape();
            if (capsule)
            {
                shape = Shape.CapsuleShape(plane, radius, height, divisions);
            }
            else
            {
                shape = Shape.CylinderShape(plane, radius, height, divisions);
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
                return Properties.Resources.Three_Shape_Cylinder_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fdd8ae15-0b71-45d5-8bd8-2c2520962e31"); }
        }
    }
}