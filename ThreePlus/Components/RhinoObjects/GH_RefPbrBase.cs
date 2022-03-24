using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.DocObjects;
using System.Linq;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefPbrBase : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefPbrBase class.
        /// </summary>
        public GH_RefPbrBase()
          : base("Reference PBR Base", "RefPbrBase",
              "References a PBR material from Rhino and deconstructs its base properties",
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
            pManager.AddTextParameter("Material Name", "N", "The name of the Rhino Material to reference", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("Diffuse Color", "DC", "The material base color", GH_ParamAccess.item);//0
            pManager.AddGenericParameter("Diffuse Map", "DM", "The Bitmap used to set the diffuse colors of the material", GH_ParamAccess.item);//1
            pManager.AddColourParameter("Ambient Color", "AC", "Adds a color to the unlit part of the object", GH_ParamAccess.item);//2
            pManager.AddColourParameter("Base Color", "BC", "The surface color, or albedo. Base color also affects the various tint amounts.", GH_ParamAccess.item);//3
            pManager.AddNumberParameter("Base Color Intensity", "BI", "A multiplier on the base color", GH_ParamAccess.item);//4
            pManager.AddGenericParameter("Base Color Map", "BM", "The Bitmap used to set the base color of the material", GH_ParamAccess.item);//5
            pManager.AddNumberParameter("Shine", "SH", "The Shine Factor of the material", GH_ParamAccess.item);//6
            pManager.AddNumberParameter("Reflectivity", "RF", "How Reflective value of the material", GH_ParamAccess.item);//7
            pManager.AddColourParameter("Reflection Color", "RC", "The reflection color tint", GH_ParamAccess.item);//8
            pManager.AddNumberParameter("Reflection Glossiness", "RG", "The reflection glossiness", GH_ParamAccess.item);//9
            pManager.AddNumberParameter("Reflective IOR", "RI", "Index of refraction", GH_ParamAccess.item);//10
            pManager.AddNumberParameter("Metallic", "MT", "The metallic-ness (0 = dielectric, 1 = metallic).  The only two physically correct values are 0.0 and 1.0 which specify the different models.  The metallic model has no diffuse component, but has tinted specular equal to the base color.", GH_ParamAccess.item);//11
            pManager.AddGenericParameter("Metallic Map", "MM", "The Bitmap used to alter the metalness of the material ", GH_ParamAccess.item);//12
            pManager.AddNumberParameter("Roughness", "RH", "Surface roughness.  Controls both diffuse and specular response.", GH_ParamAccess.item);//13
            pManager.AddGenericParameter("Roughness Map", "RM", "The Bitmap used to alter the roughness of the material", GH_ParamAccess.item);//14
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
            //Rd.Material material = pbr.Material;

            Rhino.Display.Color4f b = pbr.BaseColor;
            DA.SetData(0, material.DiffuseColor);
            DA.SetData(1, material.GetBitmap(Rd.TextureType.Diffuse));
            DA.SetData(2, material.AmbientColor);
            DA.SetData(3, Sd.Color.FromArgb((int)(255.0 * b.A), (int)(255.0 * b.R), (int)(255.0 * b.G), (int)(255.0 * b.B)));
            DA.SetData(4, b.L);
            DA.SetData(5, material.GetBitmap(Rd.TextureType.PBR_BaseColor));
            DA.SetData(6, material.Shine);
            DA.SetData(7, material.Reflectivity);
            DA.SetData(8, material.ReflectionColor);
            DA.SetData(9, material.ReflectionGlossiness);
            DA.SetData(10, pbr.ReflectiveIOR);
            DA.SetData(11, pbr.Metallic);
            DA.SetData(12, material.GetBitmap(Rd.TextureType.PBR_Metallic));
            DA.SetData(13, pbr.Roughness);
            DA.SetData(14, material.GetBitmap(Rd.TextureType.PBR_Roughness));
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
                return Properties.Resources.Three_PBR_Base_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("15996a4b-3586-4b16-a9c1-94cc1615eee8"); }
        }
    }
}