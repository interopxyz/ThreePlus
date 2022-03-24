using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Sd = System.Drawing;

namespace ThreePlus.Components.Assets
{
    public class GH_Assets_CubeMaps : GH_BaseImage
    {
        /// <summary>
        /// Initializes a new instance of the GH_Assets_CubeMaps class.
        /// </summary>
        public GH_Assets_CubeMaps()
          : base("Cube Maps", "CubeMaps",
              "A series of preselected public domain cube maps sourced from: http://www.humus.name/",
              Constants.ShortName, "Assets")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override void CreateAttributes()
        {
            img = new Sd.Bitmap(Properties.Resources.EarthPosx);
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Map Index", "M", "The index of the default Cube Map", GH_ParamAccess.item, 11);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Intensity", "I", "The intensity of a light probe", GH_ParamAccess.item, 1.0);
            pManager[1].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[0];
            paramA.AddNamedValue("Earth", 0);
            paramA.AddNamedValue("Forest", 1);
            paramA.AddNamedValue("Gamla Stan", 2);
            paramA.AddNamedValue("Heroes Square", 3);
            paramA.AddNamedValue("Lycksele", 4);
            paramA.AddNamedValue("Nissi Beach", 5);
            paramA.AddNamedValue("Park", 6);
            paramA.AddNamedValue("Perea Beach", 7);
            paramA.AddNamedValue("Pond", 8);
            paramA.AddNamedValue("Skansen", 9);
            paramA.AddNamedValue("Snow Park", 10);
            paramA.AddNamedValue("Swedish Royal Castle", 11);
            paramA.AddNamedValue("Tallinn", 12);
            paramA.AddNamedValue("Tantolunden", 13);
            paramA.AddNamedValue("Teide", 14);
            paramA.AddNamedValue("Vasa", 15);
            paramA.AddNamedValue("Yokohama Park", 16);
            paramA.AddNamedValue("Yokohama Pier", 17);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CubeMap", "Img", "The selected CubeMap", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int index = 11;
            DA.GetData(0, ref index);

            double intensity = 1.0;
            bool hasIntensity = DA.GetData(1, ref intensity);

            CubeMap cubeMap = new CubeMap();
            switch (index)
            {
                case 0:
                    cubeMap = CubeMap.Earth();
                    break;
                case 1:
                    cubeMap = CubeMap.Forest();
                    break;
                case 2:
                    cubeMap = CubeMap.GamlaStan();
                    break;
                case 3:
                    cubeMap = CubeMap.HeroesSquare();
                    break;
                case 4:
                    cubeMap = CubeMap.Lycksele();
                    break;
                case 5:
                    cubeMap = CubeMap.NissiBeach();
                    break;
                case 6:
                    cubeMap = CubeMap.Park();
                    break;
                case 7:
                    cubeMap = CubeMap.PereaBeach();
                    break;
                case 8:
                    cubeMap = CubeMap.Pond();
                    break;
                case 9:
                    cubeMap = CubeMap.Skansen();
                    break;
                case 11:
                    cubeMap = CubeMap.SwedishRoyalCastle();
                    break;
                case 12:
                    cubeMap = CubeMap.Tallinn();
                    break;
                case 13:
                    cubeMap = CubeMap.Tantolunden();
                    break;
                case 14:
                    cubeMap = CubeMap.Teide();
                    break;
                case 15:
                    cubeMap = CubeMap.Vasa();
                    break;
                case 16:
                    cubeMap = CubeMap.YokohamaPark();
                    break;
                case 17:
                    cubeMap = CubeMap.YokohamaPier();
                    break;
                default:
                    cubeMap = CubeMap.SnowPark();
                    break;
            }

            img = new Sd.Bitmap(cubeMap.PosX);

            if (hasIntensity) cubeMap.Intensity = intensity;

            DA.SetData(0, cubeMap);
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
                return Properties.Resources.Three_Assets_Cube_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c512d342-a29f-4082-ac64-fb77d1749d81"); }
        }
    }
}