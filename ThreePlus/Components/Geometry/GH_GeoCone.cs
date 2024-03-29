﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoCone : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_GeoCone class.
        /// </summary>
        public GH_GeoCone()
          : base("Cone Geometry", "Cone",
              "A Three JS Standard Geometry Cone",
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
            Plane plane = Plane.WorldXY;
            DA.GetData(0, ref plane);

            double radius = 10.0;
            DA.GetData(1, ref radius);

            double height = 20.0;
            DA.GetData(2, ref height);

            int divisions = 24;
            DA.GetData(3, ref divisions);

            Shape shape = Shape.ConeShape(plane, radius,height,divisions);
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
                return Properties.Resources.Three_Shape_Cone_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f6348bf0-24a7-4fff-93ac-13bef8a262e5"); }
        }
    }
}