using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreePlus.Components.Materials
{
    public class GH_MaterialShader : GH_Component, IGH_VariableParameterComponent
    {
        public GH_MaterialShader()
          : base("Shader Material", "Shader Material",
              "This material can create shaders written in GLSL that runs on the GPU.",
              Constants.ShortName, "Materials")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "A Model, Mesh, or Brep", GH_ParamAccess.item);
            pManager.AddTextParameter("Vertex Code", "V", "The vertex shader code in GLSL.", GH_ParamAccess.item);
            pManager.AddTextParameter("Fragment Code", "F", "The fragment shader code in GLSL.", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "The updated Model", GH_ParamAccess.item);
        }

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

            string vertexCode = string.Empty;
            DA.GetData(1, ref vertexCode);

            string fragmentCode = string.Empty;
            DA.GetData(2, ref fragmentCode);

            List<KeyValuePair<string, object>> uniforms = null;
            if (Params.Input.Count > 3)
            {
                uniforms = new List<KeyValuePair<string, object>>();
                for (var i = 3; i < Params.Input.Count; i++)
                {
                    object data = null;
                    if (DA.GetData(i, ref data) && data != null)
                    {
                        if (data is IGH_Goo gooData)
                            data = gooData.ScriptVariable();
                        uniforms.Add(new KeyValuePair<string, object>(Params.Input[i].NickName, data));
                    }
                }
            }

            Shader shader = new Shader(vertexCode, fragmentCode, uniforms);

            model.Material = Material.ShaderMaterial(shader);

            DA.SetData(0, model);
        }

        #region IGH_VariableParameterComponent
        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input && index > 2;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return side == GH_ParameterSide.Input && index > 2;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            index -= 2;
            return new Grasshopper.Kernel.Parameters.Param_GenericObject()
            {
                Name = "Uniform" + index,
                NickName = "u" + index,
                Description = "Uniform data to access from shader code. Change its nickname to change its variable name.",
                Access = GH_ParamAccess.item
            };
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
        }
        #endregion

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8e26e402-52a8-4391-ac24-495c93b071dd"); }
        }
    }
}
