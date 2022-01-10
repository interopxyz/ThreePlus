using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class PointCloud
    {
        #region members

        //public enum CloudTypes { Point,Sprites}
        //protected CloudTypes cloudType = CloudTypes.Point;

        public string MapName = string.Empty;
        protected Sd.Bitmap map = null;

        protected List<Rg.Point3d> points = new List<Rg.Point3d>();
        protected double scale = 1.0;
        protected double threshold = 0.6;
        protected List<Sd.Color> colors = new List<Sd.Color>();

        #endregion

        #region constructors

        public PointCloud()
        {

        }

        public PointCloud(PointCloud pointCloud)
        {
            //this.cloudType = pointCloud.cloudType;

            this.points = pointCloud.points;

            this.colors = pointCloud.colors;

            this.scale = pointCloud.scale;
            this.threshold = pointCloud.threshold;

            this.map = pointCloud.Map;
            this.MapName = pointCloud.MapName;
        }

        public PointCloud(Rg.Point3d point, Sd.Color color, double scale = 1, double threshold = 0.6)
        {
            //this.cloudType = CloudTypes.Point;

            this.points.Add(point);
            this.colors.Add(color);

            this.scale = scale;
            this.threshold = threshold;
        }

        public PointCloud(List<Rg.Point3d> points, List<Sd.Color> colors, double scale, double threshold = 0.6)
        {
            //this.cloudType = CloudTypes.Point;

            int countA = points.Count;
            int countB = colors.Count;

            this.points = points;
            for (int i = 0; i < countA; i++) this.colors.Add(colors[i % countB]);

            this.scale = scale;
            this.threshold = threshold;
        }

        public PointCloud(List<Rg.Point3d> points, List<Sd.Color> colors, double scale, double threshold, Sd.Bitmap bitmap)
        {
            //this.cloudType = CloudTypes.Sprites;

            int countA = points.Count;
            int countB = colors.Count;

            this.points = points;
            for (int i = 0; i < countA; i++) this.colors.Add(colors[i % countB]);

            this.scale = scale;
            this.threshold = threshold;

            if(bitmap!=null)this.map = new Sd.Bitmap(bitmap);
        }
        #endregion

        #region properties

        public virtual List<Rg.Point3d> Points
        {
            get { return this.points; }
        }

        public virtual List<Sd.Color> Colors
        {
            get { return this.colors; }
        }

        public virtual double Scale
        {
            get { return scale; }
        }

        public virtual double Threshold
        {
            get { return threshold; }
        }

        public virtual Sd.Bitmap Map
        {
            get 
            { 
                if(map!=null) return new Sd.Bitmap(map);
                return null;
            }
        }

        public virtual bool HasBitmap
        {
            get { return map != null; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "PointCloud | " + points.Count + " Points";
        }

        #endregion
    }
}
