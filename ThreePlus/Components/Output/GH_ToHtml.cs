﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using System.Linq;

namespace ThreePlus.Components.Output
{
    public class GH_ToHtml : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ToHtml class.
        /// </summary>
        public GH_ToHtml()
          : base("To Html", "ToHtml",
              "Create text outputs for Html and JavaScript App representation of the scene",
              Constants.ShortName, "Output")
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
            pManager.AddGenericParameter("Scene", "S", "A Three Plus Scene", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "H", "The HTML text", GH_ParamAccess.list);
            pManager.AddTextParameter("Javascript", "J", "The JavaScript text", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Caution: Only use for smaller text files. Visualizing larger text output can cause Grasshopper to crash.");

            Scene scene = new Scene();
            if (!DA.GetData(0, ref scene)) return;

            string html = scene.ToHtml(false);
            List<string> htmls = html.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None).ToList();

            string js = scene.ToJavascript("",false);
            List<string> jss = js.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None).ToList();


            DA.SetDataList(0, htmls);
            DA.SetDataList(1, jss);
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
                return Properties.Resources.Three_Output_Html_Text_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9655eb45-8d33-4633-9988-e012c6ff9eab"); }
        }
    }
}