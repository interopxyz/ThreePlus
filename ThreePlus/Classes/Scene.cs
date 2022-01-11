using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd=System.Drawing;

namespace ThreePlus
{
    public class Scene:MetaData
    {

        #region members

        public Settings Settings = new Settings();
        public Environment Environment = new Environment();
        public Atmosphere Atmosphere = new Atmosphere();
        public Outline Outline = new Outline();

        public AmbientOcclusion AmbientOcclusion = new AmbientOcclusion();

        public Camera Camera = new Camera();
        public Grid Grid = new Grid();
        public Axes Axes = new Axes();

        public List<Model> Models = new List<Model>();
        protected List<Light> lights = new List<Light>();
        public List<Script> Scripts = new List<Script>();

        protected bool hasShadows = false;

        #endregion

        #region constructors

        public Scene():base()
        {
            type = "Scene";
            name = "Scene";
        }

        public Scene(Scene scene):base(scene)
        {
            this.Camera = new Camera(scene.Camera);
            this.Grid = new Grid(scene.Grid);
            this.Axes = new Axes(scene.Axes);

            foreach(Model model in scene.Models)
            {
                this.Models.Add(new Model(model));
            }
            foreach (Light light in scene.lights)
            {
                this.lights.Add(new Light(light));
            }
            foreach (Script script in scene.Scripts)
            {
                this.Scripts.Add(new Script(script));
            }

            this.Environment = new Environment(scene.Environment);
            this.Atmosphere = new Atmosphere(scene.Atmosphere);

            this.AmbientOcclusion = new AmbientOcclusion(scene.AmbientOcclusion);
            this.Outline = scene.Outline;

            this.hasShadows = scene.hasShadows;
    }

        #endregion

        #region properties

        public virtual bool HasCurves
        {
            get
            {
                foreach (Model model in this.Models) if (model.IsCurve) return true;
                return false;
            }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }

        public bool HasShadows
        {
            get { return hasShadows; }
        }

        #endregion

        #region methods

        public string CompileToJS()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(Properties.Resources.ThreeJsHeader);

            return output.ToString();
        }

        public void AddLight(Light light)
        {
            if (light.HasShadow) hasShadows = true;
                lights.Add(new Light(light));
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Image(m:" + this.Models.Count + " l:" + this.lights.Count+ ")";
        }

        #endregion

    }
}
