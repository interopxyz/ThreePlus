using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ThreePlus.Components
{
    public class GH_EnvironmentMap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_EnvironmentMap class.
        /// </summary>
        public GH_EnvironmentMap()
          : base("Environment", "Env",
              "Adds an environment map to a scene for reflections and background as well as a light probe for box maps.",
              Constants.ShortName, "Scene")
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
            pManager.AddColourParameter("Background Color", "C", "The background color if an image is not used.", GH_ParamAccess.item, Color.White);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Image", "Img", "A Bitmap, CubeMap, or file path to an image.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Is Background", "B", "If true the image will be used as the background", GH_ParamAccess.item, true);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Is Environment", "E", "If true the image will be used as the reflection environment for physical and standard materials", GH_ParamAccess.item, true);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Is Illumination", "L", "If true the image will be used as a light probe illuminating the scene. NOTE: This will only work for CubeMaps.", GH_ParamAccess.item, true);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scene Element", "E", "An Environment Three Plus Scene Modifier Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color color = Color.White;
            DA.GetData(0, ref color);

            Environment environment = new Environment(color);

            IGH_Goo goo = null;
            bool hasImage = DA.GetData(1, ref goo);

            bool hasBackground = true;
            DA.GetData(2, ref hasBackground);

            bool hasEnvironment = true;
            DA.GetData(3, ref hasEnvironment);

            bool hasIllumination = true;
            DA.GetData(4, ref hasIllumination);

            Bitmap bitmap = null;
            CubeMap cubeMap = null;
            string filePath = string.Empty;
            if (hasImage)
            {
                goo.CastTo<string>(out filePath);
                if (goo.CastTo<CubeMap>(out cubeMap))
                {
                    environment = new Environment(cubeMap);
                }
                else if (goo.CastTo<Bitmap>(out bitmap))
                {
                    environment = new Environment(bitmap);
                }
                else if (File.Exists(filePath))
                {
                    if (!filePath.GetBitmapFromFile(out bitmap))
                    {
                        if (!Path.HasExtension(filePath))
                        {
                            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This is not a valid file path. This file does not have a valid bitmap extension"); this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This is not a valid file path. This file does not have a valid bitmap extension");
                            return;
                        }
                        else
                        {
                            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This is not a valid bitmap file type. The extension " + Path.GetExtension(filePath) + " is not a supported bitmap format");
                            return;
                        }
                    }
                    environment = new Environment(bitmap);
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "This is not a valid ThreePlus CubeMap, System Drawing Bitmap, or File Path to a valid Image file");
                }
                environment.IsBackground = hasBackground;
                environment.IsEnvironment = hasEnvironment;
                environment.IsIllumination = hasIllumination;
                environment.Background = color;
            }

            DA.SetData(0, environment);
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
                return Properties.Resources.Three_SceneEnvironment_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0bdac5df-b7aa-411c-93ac-2c020bd53d6f"); }
        }
    }
}