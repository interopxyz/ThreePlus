using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;
using Rd = Rhino.DocObjects;
using System.Linq;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefPbrVolumetrics : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefPbr class.
        /// </summary>
        public GH_RefPbrVolumetrics()
          : base("Reference PBR Volumetrics", "RefPbrVolumetrics",
              "References a PBR material from Rhino and deconstructs its Volumetric properties",
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
            pManager.AddNumberParameter("Subsurface", "SS", "Subsurface scattering (0.0 = pure diffuse to 1.0 = subsurface scattering)", GH_ParamAccess.item);//0
            pManager.AddGenericParameter("Subsurface Map", "SM", "The Bitmap used to alter the subsurface value of the material.", GH_ParamAccess.item);//1
            pManager.AddColourParameter("Subsurface Scattering Color", "SC", "Subsurface scattering base color.", GH_ParamAccess.item);//2
            pManager.AddNumberParameter("Subsurface Scattering Value", "SV", "Subsurface scattering multiplier value.", GH_ParamAccess.item);//3
            pManager.AddGenericParameter("Subsurface Scattering Map", "CM", "The Bitmap used to alter the subsurface scattering value of the material.", GH_ParamAccess.item);//4
            pManager.AddNumberParameter("Subsurface Scattering Radius", "SR", "Average distance that light scatters below the surface. Higher radius gives a softer appearance, as light bleeds into shadows and through the object.", GH_ParamAccess.item);//5
            pManager.AddGenericParameter("Subsurface Scattering Radius Map", "RM", "The Bitmap used to alter the subsurface scattering radius value of the material.", GH_ParamAccess.item);//6
            pManager.AddNumberParameter("Specular", "SP", "The reflectivity at the incident vector.", GH_ParamAccess.item);//7
            pManager.AddGenericParameter("Specular Map", "PM", "The Bitmap used to alter the specular value of the material.", GH_ParamAccess.item);//8
            pManager.AddNumberParameter("Specular Tint", "ST", "The reflectivity color at the incident vector.", GH_ParamAccess.item);//9
            pManager.AddGenericParameter("Specular Tint Map", "TM", "The Bitmap used to alter the specular tint value of the material.", GH_ParamAccess.item);//10
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

            Rhino.Display.Color4f s = pbr.SubsurfaceScatteringColor;
            DA.SetData(0, pbr.Subsurface);
            DA.SetData(1, material.GetBitmap(Rd.TextureType.PBR_Subsurface));
            DA.SetData(2, Sd.Color.FromArgb((int)(255.0*s.A), (int)(255.0 * s.R), (int)(255.0 * s.G), (int)(255.0 * s.B)));
            DA.SetData(3, s.L);
            DA.SetData(4, material.GetBitmap(Rd.TextureType.PBR_SubsurfaceScattering));
            DA.SetData(5, pbr.SubsurfaceScatteringRadius);
            DA.SetData(6, material.GetBitmap(Rd.TextureType.PBR_SubsurfaceScatteringRadius));
            DA.SetData(7, pbr.Specular);
            DA.SetData(8, material.GetBitmap(Rd.TextureType.PBR_Specular));
            DA.SetData(9, pbr.SpecularTint);
            DA.SetData(10, material.GetBitmap(Rd.TextureType.PBR_SpecularTint));

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
                return Properties.Resources.Three_PBR_Volume_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("66450dc7-e522-43e6-b652-486a6ad6b847"); }
        }
    }
}