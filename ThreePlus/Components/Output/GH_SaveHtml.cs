using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
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
          : base("Save Html", "SaveHtml",
              "Export a standalone html and JavaScript file which can be loaded locally to view the scene.",
              Constants.ShortName, "Output")
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
            pManager.AddGenericParameter("Scene", "S", "A Three Plus Scene", GH_ParamAccess.item);
            pManager.AddTextParameter("Folder Path", "F", "The folderpath to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Folder Name", "N", "The new export folder name", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Target", "T", 
                "Select the scenario to open the scene" + System.Environment.NewLine
                + "0: Offline (Lg)" + System.Environment.NewLine
                + "Run locally and with no internet connection. Writes all the scene assets to the app file and copy the dependencies to a sub folder." + System.Environment.NewLine
                + "1: Single (Md)" + System.Environment.NewLine
                + "Runs locally, but requires an internet connection. Writes everything to a single html and references dependencies via cdn." + System.Environment.NewLine
                + "2: Local (Md)" + System.Environment.NewLine
                + "Runs locally, but requires an internet connection. Writes all the scene assets to the app file and references dependencies via cdn." + System.Environment.NewLine
                + "3: Server (Sm)" + System.Environment.NewLine 
                + "Requires a server or online hosting. Assets are saved seperately in an assets folder and references dependencies via cdn." + System.Environment.NewLine
                + "   (Note: Server output can be hosted on Amazon S3 static hosting, etc., but will not run locally due to to modern browsers \"Cross-Origin Resource Sharing (CORS)\" restrictions"
                , GH_ParamAccess.item, 2);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, the new file will be written or overwritten", GH_ParamAccess.item, false);
            pManager[4].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[3];
            paramA.AddNamedValue("Offline", 0);
            paramA.AddNamedValue("Single", 1);
            paramA.AddNamedValue("Local", 2);
            paramA.AddNamedValue("Server", 3);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML Path", "H", "The HTML file path location.", GH_ParamAccess.item);
            pManager.AddTextParameter("JavaScript Path", "J", "The JavaScript file path location.", GH_ParamAccess.item);
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

            bool local = true;
            bool assets = false;
            bool embed = false;
            int mode = 1;
            if (!DA.GetData(3, ref mode)) return;
            switch (mode)
            {
                case 0:
                    local = true;
                    assets = false;
                    break;
                case 1:
                    local = false;
                    assets = false;
                    embed = true;
                    break;
                case 2:
                    local = false;
                    assets = false;
                    break;
                case 3:
                    local = false;
                    assets = true;
                    break;
            }

            bool save = false;
            if (!DA.GetData(4, ref save)) return;

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
                string asset = parent + "assets" + "\\";

                Directory.CreateDirectory(parent);
                if (local)Directory.CreateDirectory(child);
                if (assets) Directory.CreateDirectory(asset);

                string html = scene.ToHtml(local,embed);
                string js = string.Empty;
                if (mode != 1) js = scene.ToJavascript(asset, assets);

                File.WriteAllText(parent + "index.html", html);
                if(mode!=1)File.WriteAllText(parent + "app.js", js);

                if(local)
                {
                File.WriteAllText(child + "three.min.js", Properties.Resources.three_min);
                File.WriteAllText(child + "OrbitControls.js", Properties.Resources.OrbitControls);
                File.WriteAllText(child + "VertexTangentsHelper.js", Properties.Resources.VertexTangentsHelper);
                File.WriteAllText(child + "VertexNormalsHelper.js", Properties.Resources.VertexNormalsHelper);

                //File.WriteAllText(child + "GeometryUtils.js", Properties.Resources.GeometryUtils);
                File.WriteAllText(child + "LineSegmentsGeometry.js", Properties.Resources.LineSegmentsGeometry);
                File.WriteAllText(child + "LineSegments2.js", Properties.Resources.LineSegments2);
                File.WriteAllText(child + "LineGeometry.js", Properties.Resources.LineGeometry);
                File.WriteAllText(child + "LineMaterial.js", Properties.Resources.LineMaterial);
                File.WriteAllText(child + "Line2.js", Properties.Resources.Line2);

                if (scene.AmbientOcclusion.HasAO)
                {
                    File.WriteAllText(child + "EffectComposer.js", Properties.Resources.EffectComposer);
                    File.WriteAllText(child + "CopyShader.js", Properties.Resources.CopyShader);
                    File.WriteAllText(child + "ShaderPass.js", Properties.Resources.ShaderPass);

                    File.WriteAllText(child + "SSAOPass.js", Properties.Resources.SSAOPass);
                    File.WriteAllText(child + "SimplexNoise.js", Properties.Resources.SimplexNoise);
                    File.WriteAllText(child + "SSAOShader.js", Properties.Resources.SSAOShader);
                }

                if (scene.Outline.HasOutline) File.WriteAllText(child + "OutlineEffect.js", Properties.Resources.OutlineEffect);
                if (scene.Environment.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
                {
                    File.WriteAllText(child + "LightProbeGenerator.js", Properties.Resources.LightProbeGenerator);
                    File.WriteAllText(child + "LightProbeHelper.js", Properties.Resources.LightProbeHelper);
                }
                if(scene.Sky.HasSky) File.WriteAllText(child + "Sky.js", Properties.Resources.Sky);
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
                return Properties.Resources.Three_Output_Html_File_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e5eae6d4-3943-4b60-9eec-a949ed5fcaf5"); }
        }

        public void PurgeDirectory(string path)
        {
            Directory.CreateDirectory(path);
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}