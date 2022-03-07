using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace ThreePlus.Components
{
    public class GH_AddModelData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_AddModelData class.
        /// </summary>
        public GH_AddModelData()
          : base("Add Model Data", "Data",
              "Add keys and values to a model element's user data." + System.Environment.NewLine + "(Note: A URL key can be used with a link click event)",
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
            pManager.AddTextParameter("Name", "N", "The optional name of the model", GH_ParamAccess.item,"");
            pManager[1].Optional = true;
            pManager.AddTextParameter("Keys", "K", "A list of titles to attach to the model", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddTextParameter("Values", "V", "The values coordinted with the titles to attach to the model", GH_ParamAccess.list);
            pManager[3].Optional = true;
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

            string name = string.Empty;
            bool hasName = DA.GetData(1, ref name);

            List<string> keys = new List<string>();
            DA.GetDataList(2, keys);

            List<string> vals = new List<string>();
            DA.GetDataList(3, vals);

            if (hasName) model.Name = name;

            int count = keys.Count;
            for (int i = vals.Count; i < count; i++) vals.Add(string.Empty);

            for (int i = 0; i < count; i++)
            {
                if (!model.Data.ContainsKey(keys[i]))
                {
                    model.Data.Add(keys[i], vals[i]);
                }
                else
                {
                    model.Data[keys[i]] = vals[i];
                }
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
                return Properties.Resources.Three_Helper_Data_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d923ea93-e1f1-4378-b117-6c16ce74ce45"); }
        }
    }
}