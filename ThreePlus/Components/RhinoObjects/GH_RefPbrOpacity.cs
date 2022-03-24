using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.DocObjects;
using System.Linq;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefPbrOpacity : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefPbrOpacity class.
        /// </summary>
        public GH_RefPbrOpacity()
          : base("Reference PBR Opacity", "RefPbrOpacity",
              "References a PBR material from Rhino and deconstructs its Opacity properties",
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
            pManager.AddNumberParameter("Opacity", "OP", "The opacity of the material (0.0 = transparent to 1.0 = opaque)", GH_ParamAccess.item);//0
            pManager.AddNumberParameter("Transparency", "TR", "The transparency of the material (0.0 = opaque to 1.0 = transparent)", GH_ParamAccess.item);//1
            pManager.AddColourParameter("Transparency Color", "TC", "The transparency color", GH_ParamAccess.item);//2
            pManager.AddGenericParameter("Opacity Map", "OM", "The Bitmap used to alter the opacity value of the material.", GH_ParamAccess.item);//3
            pManager.AddNumberParameter("Opacity IOR", "OI", "Index of refraction for transmitted light.", GH_ParamAccess.item);//4
            pManager.AddGenericParameter("Opacity IOR Map", "IM", "The Bitmap used to alter the opacity IOR value of the material.", GH_ParamAccess.item);//5
            pManager.AddNumberParameter("Opacity Roughness", "OR", "Controls roughness used for transmitted light", GH_ParamAccess.item);//6
            pManager.AddGenericParameter("Opacity Roughness Map", "RM", "The Bitmap used to alter the opacity roughness value of the material.", GH_ParamAccess.item);//7
            pManager.AddNumberParameter("Alpha", "AL", "The alpha transparency value.", GH_ParamAccess.item);//8
            pManager.AddGenericParameter("Alpha Map", "AM", "The Bitmap used to alter the alpha value of the material.", GH_ParamAccess.item);//9
            pManager.AddNumberParameter("Fresnel IOR", "FI", "Fresnel Index of Refraction", GH_ParamAccess.item);//10
            pManager.AddNumberParameter("Refraction Glossiness", "RG", "Simulates a rougher surface, by diffusing the light rays in different directions.", GH_ParamAccess.item);//11
            pManager.AddColourParameter("Specular Color", "SC", "The reflected color of the object's highlights.", GH_ParamAccess.item);//12
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

            DA.SetData(0, pbr.Opacity);
            DA.SetData(1, material.Transparency);
            DA.SetData(2, material.TransparentColor);
            DA.SetData(3, material.GetBitmap(Rd.TextureType.Opacity));
            DA.SetData(4, pbr.OpacityIOR);
            DA.SetData(5, material.GetBitmap(Rd.TextureType.PBR_OpacityIor));
            DA.SetData(6, pbr.OpacityRoughness);
            DA.SetData(7, material.GetBitmap(Rd.TextureType.PBR_OpacityRoughness));
            DA.SetData(8, pbr.Alpha);
            DA.SetData(9, material.GetBitmap(Rd.TextureType.PBR_Alpha));
            DA.SetData(10, material.FresnelIndexOfRefraction);
            DA.SetData(11, material.RefractionGlossiness);
            DA.SetData(12, material.SpecularColor);


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
                return Properties.Resources.Three_PBR_Opacity_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("14f10f73-401b-4a03-88f4-21a35c6366d2"); }
        }
    }
}