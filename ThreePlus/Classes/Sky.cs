using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Sky: SceneObject
    {

        #region members

        protected double turbidity = 10.0;
        protected double rayleigh = 3.0;
        protected double altitude = 2.0;
        protected double azimuth = 180.0;
        protected double exposure = 0.5;
        protected double coefficient = 0.005;
        protected double directional = 0.7;

        protected bool environment = true;

        protected Light light = new Light();

        protected bool hasSky = false;
        protected bool hasLight = false;

        #endregion

        #region constructors

        public Sky() : base()
        {
        }
        public Sky(Sky sky) : base(sky)
        {
            this.turbidity = sky.turbidity;
            this.rayleigh = sky.rayleigh;
            this.altitude = sky.altitude;
            this.azimuth = sky.azimuth;
            this.exposure = sky.exposure;
            this.coefficient = sky.coefficient;
            this.directional = sky.directional;

            this.environment = sky.environment;

            this.hasSky = sky.hasSky;

            this.hasLight = sky.hasLight;
            this.light = new Light(sky.light);

        }

        public Sky(double altitude, double azimuth, double turbidity, double rayleigh, double exposure, double coefficient, double directional, bool environment)
        {
            this.turbidity = turbidity;
            this.rayleigh = rayleigh;
            this.altitude = altitude;
            this.azimuth = azimuth;
            this.exposure = exposure;
            this.coefficient = coefficient;
            this.directional = directional;

            this.environment = environment;

            this.hasSky = true;
        }

        #endregion

        #region properties

        public virtual bool HasSky
        {
            get { return hasSky; }
        }

        public virtual double Turbidity
        {
            get { return turbidity; }
        }

        public virtual double Rayleigh
        {
            get { return rayleigh; }
        }

        public virtual double Altitude
        {
            get { return altitude; }
        }

        public virtual double Azimuth
        {
            get { return azimuth; }
        }

        public virtual double Exposure
        {
            get { return exposure; }
        }

        public virtual double Coefficient
        {
            get { return coefficient; }
        }

        public virtual double Directional
        {
            get { return directional; }
        }

        public virtual bool Environment
        {
            get { return environment; }
        }

        public virtual bool HasLight
        {
            get { return hasLight; }
        }

        public virtual Light SunLight
        {
            get { return new Light(this.light); }
            set { light = new Light(value); }
        }

        #endregion

        #region methods

        public void SetLight(Sd.Color color)
        {
            this.hasLight = true;

            double i = azimuth/180.0 * Math.PI;
            double j = Math.PI/2.0-altitude /180.0 * Math.PI;
            double r = 10000;

            this.light = Light.DirectionalLight(new Rg.Point3d(r * Math.Cos(i) * Math.Sin(j), r * Math.Sin(i) * Math.Sin(j), r * Math.Cos(j)), new Rg.Point3d(0, 0, 0), color.A / 255.0, color);
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Sky | alt: " + Math.Round(this.altitude,3).ToString()+", azm: "+ Math.Round(this.azimuth, 3).ToString();
        }


        #endregion


    }
}
