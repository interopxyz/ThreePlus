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
        public Ground Ground = new Ground();

        public AmbientOcclusion AmbientOcclusion = new AmbientOcclusion();

        public Camera Camera = new Camera();
        public Grid Grid = new Grid();
        public Axes Axes = new Axes();

        public List<Model> Models = new List<Model>();
        public List<Light> Lights = new List<Light>();
        public List<Script> Scripts = new List<Script>();

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
            foreach (Light light in scene.Lights)
            {
                this.Lights.Add(new Light(light));
            }
            foreach (Script script in scene.Scripts)
            {
                this.Scripts.Add(new Script(script));
            }

            this.Environment = new Environment(scene.Environment);
            this.Atmosphere = new Atmosphere(scene.Atmosphere);
            this.Ground = new Ground(scene.Ground);

            this.AmbientOcclusion = new AmbientOcclusion(scene.AmbientOcclusion);
    }

        #endregion

        #region properties

        #endregion

        #region methods

        public string CompileToJS()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(Properties.Resources.ThreeJsHeader);

            return output.ToString();
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Image(m:" + this.Models.Count + " l:" + this.Lights.Count+ ")";
        }

        #endregion

    }
}
