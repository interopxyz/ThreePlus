using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class Script: SceneObject
    {

        #region members

        protected string contents = string.Empty;

        #endregion

        #region constructors

        public Script() : base()
        {
            this.type = "Script";
        }

        public Script(Script script) : base(script)
        {
            this.contents = script.contents;
        }

        public Script(string name, string script) : base()
        {
            this.type = "Script";
            this.title = name;

            this.contents = contents;
        }

        #endregion

        #region properties

        public virtual string Contents
        {
            get { return contents; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Script";
        }

        #endregion



    }
}
