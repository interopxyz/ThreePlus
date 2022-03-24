using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;

namespace ThreePlus.Components.Output
{
    public class GH_SaveJson : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SaveJson class.
        /// </summary>
        public GH_SaveJson()
          : base("Save Json", "SaveJson",
              "Save a Json file in the Three.js Object Scene format 4 compatible with https://threejs.org/editor/",
              Constants.ShortName, "Output")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene", "S", "A Three Plus Scene", GH_ParamAccess.item);
            pManager.AddTextParameter("Folder Path", "F", "The folderpath to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("File Name", "N", "The new file name", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, the new file will be written or overwritten", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("JSON Path", "J", "The JSON file path location.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Scene scene = new Scene();
            if (!DA.GetData(0, ref scene)) return;

            if(scene.ContainsDirectionalLights) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Due to limitations in the object model, the JSON export will normalize the target of Directional Lights to the scene origin");
            if (scene.ContainsSpotLights) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Due to limitations in the object model, the JSON export will normalize the target of Spot Lights to the scene origin.");

            string path = "C:\\Users\\Public\\Documents\\";
            bool hasPath = DA.GetData(1, ref path);

            string name = DateTime.UtcNow.ToString("yyyy-dd-M_HH-mm-ss");
            DA.GetData(2, ref name);

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

            if(path[path.Length-1] != '\\')path+="\\";

            if (save)
            {

                string filepath = path + name;

                if (!Directory.Exists(path))
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The provided folder path does not exist. Please verify this is a valid path.");
                    return;
                }

                string json = scene.ToJson();
                File.WriteAllText(filepath + ".json", json);

                DA.SetData(0, path + ".json");
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
                return Properties.Resources.Three_Output_Json_File_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f66092ca-e857-4fc6-8cf2-16a6426f04e5"); }
        }
    }
}