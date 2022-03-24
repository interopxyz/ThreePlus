using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.DocObjects;
using System.Linq;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefPbrFinishes : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefPbrFinishes class.
        /// </summary>
        public GH_RefPbrFinishes()
          : base("Reference PBR Finishes", "RefPbrFinishes",
              "References a PBR material from Rhino and deconstructs its Sheen, Clearcoat, and Anisotropic properties",
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
            pManager.AddNumberParameter("Sheen", "SH", "An additional grazing reflective component, primarily intended for cloth.", GH_ParamAccess.item);//0
            pManager.AddGenericParameter("Sheen Map", "SM", "The Bitmap used to alter the sheen value of the material.", GH_ParamAccess.item);//1
            pManager.AddNumberParameter("Sheen Tint", "ST", "Amount to tint sheen component towards the base color", GH_ParamAccess.item);//2
            pManager.AddGenericParameter("Sheen Tint Map", "TM", "The Bitmap used to alter the tint value of the material.", GH_ParamAccess.item);//3
            pManager.AddNumberParameter("Clearcoat", "CC", "Extra white specular layer on top of others. This is useful for materials like car paint, etc.", GH_ParamAccess.item);//4
            pManager.AddGenericParameter("Clearcoat Map", "CM", "The Bitmap used to alter the clearcoat value of the material.", GH_ParamAccess.item);//5
            pManager.AddNumberParameter("Clearcoat Roughness", "CR", "Roughness of clearcoat specular.", GH_ParamAccess.item);//6
            pManager.AddGenericParameter("Clearcoat Roughness Map", "RM", "The Bitmap used to alter the roughness value of the material.", GH_ParamAccess.item);//7
            pManager.AddGenericParameter("Clearcoat Bump Map", "BM", "The Bitmap used to alter the bump value of the material.", GH_ParamAccess.item);//8
            pManager.AddNumberParameter("Anisotropic", "AN", "A glossy reflection, with separate control over U and V direction roughness.", GH_ParamAccess.item);//9
            pManager.AddGenericParameter("Anisotropic Map", "AM", "The Bitmap used to alter the anisotropic value of the material.", GH_ParamAccess.item);//10
            pManager.AddNumberParameter("Anisotropic Rotation", "AR", "Rotation of the anisotropic tangent direction.", GH_ParamAccess.item);//11
            pManager.AddGenericParameter("Anisotropic Rotation Map", "RM", "The Bitmap used to alter the anisotropic rotation value of the material.", GH_ParamAccess.item);//12
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

            DA.SetData(0, pbr.Sheen);
            DA.SetData(1, material.GetBitmap(Rd.TextureType.PBR_Sheen));
            DA.SetData(2, pbr.SheenTint);
            DA.SetData(3, material.GetBitmap(Rd.TextureType.PBR_SheenTint));
            DA.SetData(4, pbr.Clearcoat);
            DA.SetData(5, material.GetBitmap(Rd.TextureType.PBR_Clearcoat));
            DA.SetData(6, pbr.ClearcoatRoughness);
            DA.SetData(7, material.GetBitmap(Rd.TextureType.PBR_ClearcoatRoughness));
            DA.SetData(8, material.GetBitmap(Rd.TextureType.PBR_ClearcoatBump));
            DA.SetData(9, pbr.Anisotropic);
            DA.SetData(10, material.GetBitmap(Rd.TextureType.PBR_Anisotropic));
            DA.SetData(11, pbr.AnisotropicRotation);
            DA.SetData(12, material.GetBitmap(Rd.TextureType.PBR_Anisotropic_Rotation));

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
                return Properties.Resources.Three_PBR_Finishes_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("891f4d32-b559-4b88-8f26-d05ae8e9ede1"); }
        }
    }
}