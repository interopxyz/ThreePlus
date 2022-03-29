using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components.Geometry
{
    public class GH_GeoBox : GH_GeoPreview
    {
        /// <summary>
        /// Initializes a new instance of the GH_Primative_Box class.
        /// </summary>
        public GH_GeoBox()
          : base("Box Geometry", "Box",
              "A Three JS Standard Geometry Box",
              Constants.ShortName, "Shapes")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBoxParameter("Box", "B", "Base Box", GH_ParamAccess.item, new Box(Plane.WorldXY,new Interval(-5,5),new Interval(-5,5), new Interval(-5, 5)));
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("X Count", "X", "Face count in the X direction", GH_ParamAccess.item, 1);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Y Count", "Y", "Face count in the Y direction", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Z Count", "Z", "Face count in the Z direction", GH_ParamAccess.item, 1);
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
            Box box = Box.Unset;
            DA.GetData(0, ref box);

            int x = 1;
            DA.GetData(1, ref x);

            int y = 1;
            DA.GetData(2, ref y);

            int z = 1;
            DA.GetData(3, ref z);

            Shape shape = Shape.BoxShape(box, x,y,z);
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
                return Properties.Resources.Three_Shape_Box_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6452750e-9669-4375-bed8-76b4358a93e1"); }
        }
    }
}