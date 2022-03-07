using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components
{
    public class GH_AddModelLink : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_AddModelLink class.
        /// </summary>
        public GH_AddModelLink()
          : base("Add Model Link", "Link",
              "Adds a hyperlink to an Element's user data with a URL key."+System.Environment.NewLine+"(Note: The Add Data component can also add / override a hyperlink with a URL key)",
              Constants.ShortName, "Helpers")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Model or Curve, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddTextParameter("Hyperlink", "H", "A valid hyperlink to attach to the model", GH_ParamAccess.item, "https://www.google.com/");
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "The updated Model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            Model model = null;

            if (goo.CastTo<Model>(out model))
            {
                model = new Model(model);
            }
            else
            {
                model = goo.ToModel();
            }

            string url = "https://www.google.com/";
            if(!DA.GetData(1, ref url))return;
            
            if(!model.Data.ContainsKey(url))
            {
                model.Data.Add("URL", url);
            }
            else
            {
                model.Data["URL"] = url;
            }

            DA.SetData(0, model);
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
                return Properties.Resources.Three_Helper_Link_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("98cce9ec-4c84-4594-ae59-eb9a3d0cb708"); }
        }
    }
}