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
            pManager.AddTextParameter("File Name", "N", "The filename for the Svg export", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, the new file will be writter or overwritten", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
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

            bool save = false;
            if (!DA.GetData(3, ref save)) return;

            if (!hasPath)
            {
                if (this.OnPingDocument().FilePath != null)
                {
                    path = Path.GetDirectoryName(this.OnPingDocument().FilePath) + "\\";
                }
            }

            string filepath = path + name;


            if (!Directory.Exists(path))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The provided folder path does not exist. Please verify this is a valid path.");
                return;
            }

            Directory.CreateDirectory(path+"\\"+name);
            path = path + "\\" + name + "\\";

            string html = scene.ToHtml(false);
            string js = scene.ToJavascript();

            if (save)
            {
                File.WriteAllText(path +"index.html", html);
                File.WriteAllText(path + "app.js", js);
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