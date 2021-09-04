using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Camera:MetaData
    {

        #region members

        protected Rg.Point3d position = new Rg.Point3d(100,100,100);
        protected Rg.Point3d target = new Rg.Point3d(0, 0, 0);

        protected int fov = 50;
        protected double zoom = 1;
        protected double near = 0.1;
        protected double far = 10000;
        protected int focus = 10;
        protected double aspect = 1.678740157480315;
        protected int filmGauge = 35;
        protected int filmOffset = 0;

        #endregion

        #region constructors

        public Camera():base()
        {
            this.type = "PerspectiveCamera";
            this.name = "Camera";
        }

        public Camera(Camera camera) : base(camera)
        {
            this.position = new Rg.Point3d(camera.position);
            this.target = new Rg.Point3d(camera.target);

            this.fov = camera.fov;
            this.zoom = camera.zoom;
            this.near = camera.near;
            this.far = camera.far;
            this.focus = camera.focus;
            this.aspect = camera.aspect;
            this.filmGauge = camera.filmGauge;
            this.filmOffset = camera.filmOffset;
    }

        #endregion

        #region properties

        public virtual Rg.Point3d Position
        {
            get { return position; }
        }

        public virtual Rg.Point3d Target
        {
            get { return target; }
        }

        public virtual int FOV
        {
            get { return fov; }
        }
        public virtual double Zoom
        {
            get { return zoom; }
        }
        public virtual double Near
        {
            get { return near; }
        }
        public virtual double Far
        {
            get { return far; }
        }
        public virtual int Focus
        {
            get { return focus; }
        }
        public virtual double Aspect
        {
            get { return aspect; }
        }
        public virtual int FilmGauge
        {
            get { return filmGauge; }
        }
        public virtual int FilmOffset
        {
            get { return filmOffset; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Camera | ";
        }

        #endregion

    }
}
