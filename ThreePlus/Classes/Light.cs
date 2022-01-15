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

        protected bool hasHelper = false;
        protected Sd.Color helperColor = Sd.Color.Gray;
        protected double helperSize = 5;

        protected bool hasShadow = false;
        protected int shadowSamples = 20;

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

            this.hasHelper = light.hasHelper;
            this.helperColor = light.helperColor;
            this.helperSize = light.helperSize;

            this.hasShadow = light.hasShadow;
            this.shadowSamples = light.shadowSamples;

        }

        public static Light SpotLight(Rg.Point3d position, Rg.Point3d target, double intensity,double distance,double angle, double penumbra, double decay, Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Spot;
            light.objectType = "SpotLight";

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
            light.objectType = "DirectionalLight";

            light.position = new Rg.Point3d(position);
            light.target = new Rg.Point3d(target);

            light.intensity = intensity;
            light.color = color;

            return light;
        }

        public static Light AmbientLight(double intensity, Sd.Color color)
        {
            Light light = new Light();
            light.objectType = "AmbientLight";

            light.lightType = Types.Ambient;

            light.intensity = intensity;
            light.color = color;

            return light;
        }

        public static Light HemisphereLight(double intensity, Sd.Color zenithColor, Sd.Color horizonColor)
        {
            Light light = new Light();

            light.lightType = Types.Hemisphere;
            light.objectType = "HemisphereLight";
            light.position = new Rg.Point3d(0, 0, 1);

            light.intensity = intensity;
            light.color = zenithColor;
            light.colorB = horizonColor;

            return light;
        }

        public static Light PointLight(Rg.Point3d position, double intensity, double distance, double decay , Sd.Color color)
        {
            Light light = new Light();

            light.lightType = Types.Point;
            light.objectType = "PointLight";

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

        public virtual bool HasHelper
        {
            get { return hasHelper; }
        }

        public virtual double HelperSize
        {
            get { return helperSize; }
        }

        public virtual Sd.Color HelperColor
        {
            get { return helperColor; }
        }

        public bool HasShadow
        {
            get { return hasShadow; }
        }

        #endregion

        #region methods

        public void SetShadow(int samples)
        {
            hasShadow = true;
            this.shadowSamples = samples;
        }

        public void SetHelper(double size, Sd.Color color)
        {
            this.hasHelper = true;
            this.helperSize = size;
            this.helperColor = color;
        }
        #endregion

        #region overrides

        public override string ToString()
        {
            return "Light | "+this.LightType.ToString();
        }

        #endregion

    }
}
