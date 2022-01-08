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
        public enum EnvironmentModes { Color,Environment,CubeMap};
        protected EnvironmentModes environmentMode = EnvironmentModes.Color;

        protected Sd.Bitmap envMap = null;
        protected CubeMap cubeMap = null;
        protected Sd.Color background = Sd.Color.White;

        public bool IsBackground = true;
        public bool IsEnvironment = true;
        public bool IsIllumination = true;

        #endregion

        #region constructors

        public Environment() : base()
        {

        }

        public Environment(Environment environment) : base()
        {
            this.environmentMode = environment.environmentMode;

            this.background = environment.background;
            this.envMap = environment.EnvMap;
            this.cubeMap = environment.cubeMap;

            this.IsBackground = environment.IsBackground;
            this.IsEnvironment = environment.IsEnvironment;
            this.IsIllumination = environment.IsIllumination;
        }

        public Environment(Sd.Color background) : base()
        {
            this.environmentMode = EnvironmentModes.Color;
            this.background = background;
        }

        public Environment(Sd.Bitmap envMap) : base()
        {
            this.environmentMode = EnvironmentModes.Environment;
            this.envMap = new Sd.Bitmap(envMap);
        }

        public Environment(CubeMap cubeMap) : base()
        {
            this.environmentMode = EnvironmentModes.CubeMap;
            this.cubeMap = cubeMap;
        }

        public virtual EnvironmentModes EnvironmentMode
        {
            get { return environmentMode; }
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

        public virtual CubeMap CubeMap
        {
            get { return new CubeMap(cubeMap); }
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
            return "Environment | "+this.environmentMode.ToString();
        }

        #endregion
    }
}
