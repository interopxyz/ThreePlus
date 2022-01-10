using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components
{
    public class GH_GraphicsPoints : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Graphics_Points class.
        /// </summary>
        public GH_GraphicsPoints()
          : base("Point Cloud", "Cloud",
              "Description",
              Constants.ShortName, "Materials")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "A list of points to populate a cloud", GH_ParamAccess.list);
            pManager.AddColourParameter("Colors", "C", "A corresponding list of colors", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Size", "S", "The size of the point cloud visualization", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Bitmap", "Img", "A bitmap image", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Threshold", "T", "The opacity threshold.", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "The new point cloud Model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PointCloud cloud = new PointCloud();

            List<Point3d> points = new List<Point3d>();
            if(!DA.GetDataList(0, points))return ;

            List<Sd.Color> colors = new List<Sd.Color>();
            DA.GetDataList(1, colors);
            if (colors.Count < 1) colors.Add(Sd.Color.Black);

            double scale = 1.0;
            DA.GetData(2, ref scale);

            double threshold = 0.6;
            DA.GetData(4, ref threshold);

            IGH_Goo goo = null;
            bool hasImage = DA.GetData(3, ref goo);
            Sd.Bitmap bitmap = null;
            string message = string.Empty;
            if (hasImage)
            {
                if (!goo.TryGetBitmap(out bitmap, out message))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);
                    return;
                }
                cloud = new PointCloud(points, colors, scale, threshold, bitmap);
            }
            else
            {
            cloud = new PointCloud(points, colors, scale,threshold);
            }

            Model model = new Model(cloud);

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
                return Properties.Resources.Three_Graphics_Points_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2a109928-eb7c-441e-b638-8ddfb9fc2881"); }
        }
    }
}