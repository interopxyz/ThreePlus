using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThreePlus.Components.Output
{
    public class GH_SaveHtml : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SaveHtml class.
        /// </summary>
        public GH_SaveHtml()
          : base("SaveHtml", "SaveHtml",
              "Description",
              Constants.ShortName, "Output")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene", "S", "Scene", GH_ParamAccess.item);
            pManager.AddTextParameter("Folder Path", "F", "The folderpath to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Folder Name", "N", "The new export folder name", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, the new file will be writter or overwritten", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML path", "H", "The html text", GH_ParamAccess.item);
            pManager.AddTextParameter("JS path", "J", "The javascript text", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Scene scene = new Scene();
            if (!DA.GetData(0, ref scene)) return;

            string path = "C:\\Users\\Public\\Documents\\";
            bool hasPath = DA.GetData(1, ref path);

            string name = DateTime.UtcNow.ToString("yyyy-dd-M_HH-mm-ss");
            bool hasName = DA.GetData(2, ref name);

            scene.Name = name;

            bool save = false;
            if (!DA.GetData(3, ref save)) return;

            if (!hasPath)
            {
                if (this.OnPingDocument().FilePath != null)
                {
                    path = Path.GetDirectoryName(this.OnPingDocument().FilePath) + "\\";
                }
            }

            if (save)
            {

                string filepath = path + name;


            if (!Directory.Exists(path))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The provided folder path does not exist. Please verify this is a valid path.");
                return;
            }

                string parent = path + "\\" + name + "\\";
                string child = parent + "js" + "\\";

                Directory.CreateDirectory(parent);
                Directory.CreateDirectory(child);

            string html = scene.ToHtml(true);
            string js = scene.ToJavascript();

                File.WriteAllText(parent + "index.html", html);
                File.WriteAllText(parent + "app.js", js);

                File.WriteAllText(child + "three.js", Properties.Resources.three);
                File.WriteAllText(child + "OrbitControls.js", Properties.Resources.OrbitControls);
                File.WriteAllText(child + "VertexTangentsHelper.js", Properties.Resources.VertexTangentsHelper);
                File.WriteAllText(child + "VertexNormalsHelper.js", Properties.Resources.VertexNormalsHelper);

                if (scene.AmbientOcclusion.HasAO)
                {
                    File.WriteAllText(child + "EffectComposer.js", Properties.Resources.EffectComposer);
                    File.WriteAllText(child + "CopyShader.js", Properties.Resources.CopyShader);
                    File.WriteAllText(child + "ShaderPass.js", Properties.Resources.ShaderPass);

                    File.WriteAllText(child + "SSAOPass.js", Properties.Resources.SSAOPass);
                    File.WriteAllText(child + "SimplexNoise.js", Properties.Resources.SimplexNoise);
                    File.WriteAllText(child + "SSAOShader.js", Properties.Resources.SSAOShader);
                }

                DA.SetData(0, parent + "index.html");
                DA.SetData(1, parent + "app.js");
            }


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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e5eae6d4-3943-4b60-9eec-a949ed5fcaf5"); }
        }
    }
}