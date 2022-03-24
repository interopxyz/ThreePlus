using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rg = Rhino.Geometry;

namespace ThreePlus.Components.RhinoObjects
{
    public class GH_RefGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_RefGeometry class.
        /// </summary>
        public GH_RefGeometry()
          : base("Reference Objects", "RefObj",
              "Reference geometry and lights from a Rhino Doc by Guid",
              Constants.ShortName, "Doc")
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
            pManager.AddTextParameter("GUID", "G", "The GUID of referenced Rhino objects", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "E", "Three Plus Model Elements", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string strGuid = string.Empty;
            DA.GetData(0, ref strGuid);

            Guid guid = new Guid(strGuid);

            Rhino.DocObjects.RhinoObject rhinoObject = Rhino.RhinoDoc.ActiveDoc.Objects.FindId(guid);
            if (rhinoObject != null)
            {
                string type = rhinoObject.Geometry.GetType().Name;

                Model model = new Model();
                Light light = new Light();
                bool isModel = false;
                bool isLight = false;

                switch (type)
                {
                    case "Point":
                         model = new Model(((Rg.Point)rhinoObject.Geometry).Location);
                        isModel = true;
                        break;
                    case "PointCloud":
                         model = new Model(new PointCloud((Rg.PointCloud)rhinoObject.Geometry));
                        isModel = true;
                        break;
                    case "PolylineCurve":
                        model = new Model(((Rg.PolylineCurve)rhinoObject.Geometry).ToNurbsCurve());
                        isModel = true;
                        break;
                    case "NurbsCurve":
                        model = new Model(((Rg.NurbsCurve)rhinoObject.Geometry));
                        isModel = true;
                        break;
                    case "ArcCurve":
                        model = new Model(((Rg.ArcCurve)rhinoObject.Geometry).ToNurbsCurve());
                        isModel = true;
                        break;
                    case "Surface":
                        model = new Model(Rg.Mesh.CreateFromSurface((Rg.Surface)rhinoObject.Geometry));
                        isModel = true;
                        break;
                    case "NurbsSurface":
                        model = new Model(Rg.Mesh.CreateFromSurface((Rg.NurbsSurface)rhinoObject.Geometry));
                        isModel = true;
                        break;
                    case "Brep":
                        Rg.Mesh mesh = new Mesh();
                        mesh.Append(Rg.Mesh.CreateFromBrep((Rg.Brep)rhinoObject.Geometry, MeshingParameters.Default));
                        model = new Model(mesh);
                        isModel = true;
                        break;
                    case "Extrusion":
                        Rg.Extrusion extrusion = (Rg.Extrusion)rhinoObject.Geometry;
                        Rg.Brep brep = extrusion.ToBrep();
                        Rg.Mesh brepMesh = new Mesh();
                        brepMesh.Append(Rg.Mesh.CreateFromBrep(brep, MeshingParameters.Default));
                        model = new Model(brepMesh);
                        isModel = true;
                        break;
                    case "Mesh":
                        model = new Model((Rg.Mesh)rhinoObject.Geometry);
                        isModel = true;
                        break;
                    case "Light":
                        Rg.Light rhLight = (Rg.Light)rhinoObject.Geometry;

                        Rg.Vector3d direction = new Rg.Vector3d(rhLight.Direction);

                        if (rhLight.IsDirectionalLight) light = Light.DirectionalLight(rhLight.Location, rhLight.Location+ direction,rhLight.Intensity, rhLight.Diffuse);
                        if (rhLight.IsPointLight) light = Light.PointLight(rhLight.Location, rhLight.Intensity, 10, 0.0, rhLight.Diffuse);

                        isLight = true;
                        break;
                    default:
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, type+" is not a supported type.");
                        break;
                }



                if (isModel)
                {
                    if (model.IsMesh) model.Material = new Material(rhinoObject.GetMaterial(true));
                    if (model.IsCurve) model.Graphic = new Graphic(rhinoObject.Attributes);
                    DA.SetData(0, model);
                }

                if (isLight) DA.SetData(0, light);
            }
            else
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No Object in the Rhino scene matches the GUID ("+strGuid+").");
                return;
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
                return Properties.Resources.Three_Reference_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("26ca0af9-4f2d-4301-9cb6-fb2718ea9ebe"); }
        }
    }
}