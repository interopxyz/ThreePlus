using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.DocObjects;
using System.Linq;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefPbrSurface : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefPbrSurface class.
        /// </summary>
        public GH_RefPbrSurface()
          : base("Reference PBR Surface", "RefPbrSurface",
              "References a PBR material from Rhino and deconstructs its Surface effect properties",
              Constants.ShortName, "Doc")
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
            pManager.AddTextParameter("Material Name", "N", "The name of the material to reference", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("Emission Color", "EC", "", GH_ParamAccess.item);//0
            pManager.AddColourParameter("PBR Emission Color", "PC", "", GH_ParamAccess.item);//1
            pManager.AddNumberParameter("PBR Emission Value", "PV", "", GH_ParamAccess.item);//2
            pManager.AddGenericParameter("Emission Map", "EM", "", GH_ParamAccess.item);//3
            pManager.AddGenericParameter("Bump Map", "BM", "", GH_ParamAccess.item);//4
            pManager.AddGenericParameter("Displacement Map", "DM", "", GH_ParamAccess.item);//5
            pManager.AddGenericParameter("AO Map", "AM", "", GH_ParamAccess.item);//6
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = string.Empty;
            if (!DA.GetData(0, ref name)) return;

            var mats = (from m in Rhino.RhinoDoc.ActiveDoc.RenderMaterials select m).ToList();
            int index = mats.FindIndex(mat => mat.Name.Equals(name));

            if (index < 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Material " + name + " is not found.");
                return;
            }

            Rd.Material material = Rhino.RhinoDoc.ActiveDoc.RenderMaterials[index].SimulatedMaterial(Rhino.Render.RenderTexture.TextureGeneration.Skip);
            material.ToPhysicallyBased();
            Rd.PhysicallyBasedMaterial pbr = material.PhysicallyBased;

            Rhino.Display.Color4f e = pbr.Emission;
            DA.SetData(0, material.EmissionColor);
            DA.SetData(1, Sd.Color.FromArgb((int)(255.0 * e.A), (int)(255.0 * e.R), (int)(255.0 * e.G), (int)(255.0 * e.B)));
            DA.SetData(2, e.L);
            DA.SetData(3, material.GetBitmap(Rd.TextureType.PBR_Emission));
            DA.SetData(4, material.GetBitmap(Rd.TextureType.Bump));
            DA.SetData(5, material.GetBitmap(Rd.TextureType.PBR_Displacement));
            DA.SetData(6, material.GetBitmap(Rd.TextureType.PBR_AmbientOcclusion));


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
                return Properties.Resources.Three_PBR_Surface_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("bf71d5a5-8320-42ba-a07f-67ab2d0b695c"); }
        }
    }
}