using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Helpers
{
    public class GH_Grid : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Grid class.
        /// </summary>
        public GH_Grid()
          : base("Grid", "Grid",
              "The GridHelper is an object to define grids. Grids are two-dimensional arrays of lines.",
              Constants.ShortName, "Helpers")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Size", "S", "The size of the grid", GH_ParamAccess.item, 100);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Divisions", "D", "The number of divisions in the grid", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Axis Color", "A", "The axis color of the grid", GH_ParamAccess.item, Color.Gray);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Grid Color", "G", "The grid line color", GH_ParamAccess.item, Color.DarkGray);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Polar", "P", "If true the grid will be displayed in a polar configuration", GH_ParamAccess.item, false);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Grid", "G", "The grid object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double size = 100;
            DA.GetData(0, ref size);

            double divisions = 10;
            DA.GetData(1, ref divisions);

            Color axisColor = Color.Gray;
            DA.GetData(2, ref axisColor);

            Color gridColor = Color.DarkGray;
            DA.GetData(3, ref gridColor);

            bool isPolar = false;
            DA.GetData(4, ref isPolar);

            Grid grid = new Grid(size, divisions, axisColor, gridColor,isPolar);

            DA.SetData(0, grid);

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
                return Properties.Resources.Three_Helper_Grid_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9fc244af-79ee-410c-9c5d-ab651e741f6d"); }
        }
    }
}