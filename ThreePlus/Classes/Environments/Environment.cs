using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Environment : SceneObject
    {

        #region members

        protected Sd.Bitmap envMap = null;
        protected Sd.Color background = Sd.Color.White;
        public bool IsBackground = true;
        public bool IsEnvironment = true;

        #endregion

        #region constructors

        public Environment() : base()
        {

        }

        public Environment(Environment environment) : base()
        {
            this.envMap = environment.EnvMap;
            this.background = environment.background;
            this.IsBackground = environment.IsBackground;
            this.IsEnvironment = environment.IsEnvironment;
        }

        public Environment(Sd.Bitmap envMap) : base()
        {
            this.envMap = new Sd.Bitmap(envMap);
        }


        public Environment(Sd.Color background) : base()
        {
            this.background = background;
        }


        #endregion

        #region properties

        public virtual Sd.Color Background
        {
            get { return background; }
            set { background = value; }
        }

        public virtual Sd.Bitmap EnvMap
        {
            get { if (envMap!=null)
                {
                    return new Sd.Bitmap(envMap); 
                }
            else
                {
                    return null;
                }
            }
        }

        public virtual bool HasEnvMap
        {
            get { return (envMap!=null); }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Environment | ";
        }

        #endregion
    }
}
