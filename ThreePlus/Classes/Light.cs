using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Light : MetaData
    {

        #region members

        public enum Types { Ambient, Point, Directional, Spot, Hemisphere}

        protected Types lightType = Types.Ambient;

        protected Sd.Color color = Sd.Color.White;
        protected Sd.Color colorB = Sd.Color.White;

        protected double intensity = 0.75;
        protected double distance = 0;
        protected double angle = 60;
        protected double penumbra = 1;
        protected double decay = 0;

        protected Rg.Point3d position = new Rg.Point3d(100, 100, 100);
        protected Rg.Point3d target = new Rg.Point3d(0, 0, 0);

        #endregion

        #region constructors

        public Light():base()
        {

        }

        public Light(Light light) : base(light)
        {
            this.lightType = light.lightType;

            this.color = light.color;
            this.colorB = light.colorB;

            this.intensity = light.intensity;
            this.distance = light.distance;
            this.decay = light.decay;
            this.angle = light.angle;
            this.penumbra = light.penumbra;

            this.position = new Rg.Point3d(light.position);
            this.target = new Rg.Point3d(light.target);
        }

        public static Light SpotLight(Rg.Point3d position, Rg.Point3d target, double intensity,double distance,double angle, double penumbra, double decay, Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Spot;

            light.position = new Rg.Point3d(position);
            light.target = new Rg.Point3d(target);

            light.intensity = intensity;
            light.distance = distance;
            light.angle = angle;
            light.penumbra = penumbra;
            light.decay = decay;

            light.color = color;

            return light;
        }

        public static Light DirectionalLight(Rg.Point3d position, Rg.Point3d target, double intensity, Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Directional;

            light.position = new Rg.Point3d(position);
            light.target = new Rg.Point3d(target);

            light.intensity = intensity;
            light.color = color;

            return light;
        }

        public static Light AmbientLight(double intensity, Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Ambient;

            light.intensity = intensity;
            light.color = color;

            return light;
        }

        public static Light HemisphereLight(double intensity, Sd.Color zenithColor, Sd.Color horizonColor)
        {
            Light light = new Light();

            light.lightType = Types.Hemisphere;

            light.intensity = intensity;
            light.color = zenithColor;
            light.colorB = horizonColor;

            return light;
        }

        public static Light PointLight(Rg.Point3d position, double intensity, double distance, double decay , Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Point;

            light.intensity = intensity;
            light.distance = distance;
            light.decay = decay;

            light.color = color;

            light.position = new Rg.Point3d(position);

            return light;
        }

        #endregion

        #region properties

        public virtual Sd.Color Color
        {
            get { return color; }
        }

        public virtual Sd.Color ColorB
        {
            get { return colorB; }
        }

        public virtual double Intensity
        {
            get { return intensity; }
        }

        public virtual double Distance
        {
            get { return distance; }
        }

        public virtual double Angle
        {
            get { return angle; }
        }

        public virtual double Penumbra
        {
            get { return penumbra; }
        }

        public virtual double Decay
        {
            get { return decay; }
        }

        public virtual Types LightType
        {
            get { return lightType; }
        }

        public virtual Rg.Point3d Position
        {
            get { return new Rg.Point3d(position); }
        }

        public virtual Rg.Point3d Target
        {
            get { return new Rg.Point3d(target); }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Light | ";
        }

        #endregion

    }
}
