using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components
{
    public class GH_Scene : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Scene class.
        /// </summary>
        public GH_Scene()
          : base("Scene", "Scene",
              "Compiles scene elements into a single scene.",
              Constants.ShortName, "Scene")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Elements including Models, Lights, Cameras, Helpers, Scene Elements, and Geometry (Curves, Breps, Meshes)", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Click Modes", "C", "Adds click events to model elements", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[1];
            paramA.AddNamedValue("None", 0);
            paramA.AddNamedValue("Display Data", 1);
            paramA.AddNamedValue("Open Link", 2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene", "S", "A Three Plus Scene", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            int click = 0;
            DA.GetData(1, ref click);

            Scene scene = new Scene();

            Model model = null;
            Grid grid = new Grid();
            Axes axes = new Axes();
            Light light = new Light();
            Camera camera = new Camera();
            Environment environment = new Environment();
            Atmosphere atmosphere = new Atmosphere();
            Sky sky = new Sky();
            AmbientOcclusion ambientOcclusion = new AmbientOcclusion();
            Outline outline = new Outline();

            foreach (IGH_Goo goo in goos)
            {
                if (goo.CastTo<Model>(out model))
                {
                    scene.Models.Add(new Model(model));
                }
                else if (goo.CastTo<Grid>(out grid))
                {
                    scene.Grid = new Grid(grid);
                }
                else if (goo.CastTo<Axes>(out axes))
                {
                    scene.Axes = new Axes(axes);
                }
                else if (goo.CastTo<Light>(out light))
                {
                    scene.AddLight(new Light(light));
                }
                else if (goo.CastTo<Camera>(out camera))
                {
                    scene.Cameras.Add(new Camera(camera));
                }
                else if (goo.CastTo<Environment>(out environment))
                {
                    scene.Environment = new Environment(environment);
                }
                else if (goo.CastTo<Sky>(out sky))
                {
                    scene.Sky = new Sky(sky);
                    if (sky.HasLight) scene.AddLight(sky.SunLight);
                }
                else if (goo.CastTo<Atmosphere>(out atmosphere))
                {
                    scene.Atmosphere = new Atmosphere(atmosphere);
                }
                else if (goo.CastTo<AmbientOcclusion>(out ambientOcclusion))
                {
                    scene.AmbientOcclusion = new AmbientOcclusion(ambientOcclusion);
                }
                else if (goo.CastTo<Outline>(out outline))
                {
                    scene.Outline = new Outline(outline);
                }
                else
                {
                    scene.Models.Add(goo.ToModel());
                }
            }
            scene.ClickEvent = (Scene.ClickEvents)click;

            DA.SetData(0, scene);
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
                return Properties.Resources.Three_SceneCompile_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8eb51f10-c5c8-4e74-8c38-c6820ba815e6"); }
        }
    }
}