using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class Shader : SceneObject
    {

        #region members 
        protected string vertexShaderCode = string.Empty;
        protected string fragmentShaderCode = string.Empty;
        protected List<KeyValuePair<string, object>> uniforms = null;
        #endregion

        #region constructors

        public Shader() : base()
        {
            this.type = "Shader";
        }

        public Shader(Shader script) : base(script)
        {
            this.vertexShaderCode = script.vertexShaderCode;
            this.fragmentShaderCode = script.fragmentShaderCode;
            this.uniforms = new List<KeyValuePair<string, object>>(script.uniforms);
        }

        public Shader(string vertexShaderCode, string fragmentShaderCode) : base()
        {
            this.type = "Shader";

            this.vertexShaderCode = vertexShaderCode;
            this.fragmentShaderCode = fragmentShaderCode;
        }
        public Shader(string vertexShaderCode, string fragmentShaderCode, List<KeyValuePair<string, object>> uniforms) : base()
        {
            this.type = "Shader";

            this.vertexShaderCode = vertexShaderCode;
            this.fragmentShaderCode = fragmentShaderCode;
            this.uniforms = uniforms;
        }
        #endregion

        #region properties

        public virtual string VertexShaderCode
        {
            get { return vertexShaderCode; }
        }
        public virtual string FragmentShaderCode
        {
            get { return fragmentShaderCode; }
        }
        public bool HasUniforms
        {
            get
            {
                return uniforms != null && uniforms.Count > 0;
            }
        }
        public List<KeyValuePair<string, object>> Uniforms
        {
            get { return uniforms; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Shader";
        }

        #endregion



    }
}

