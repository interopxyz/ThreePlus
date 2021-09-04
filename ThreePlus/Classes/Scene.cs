using System;
using System.Collections.Generic;
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

        public Camera Camera = new Camera();
        public Grid Grid = new Grid();
        public Axes Axes = new Axes();

        public List<Model> Models = new List<Model>();
        public List<Light> Lights = new List<Light>();
        public List<Script> Scripts = new List<Script>();

        public Sd.Color Background = Sd.Color.White;

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

            this.Background = scene.Background;
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
