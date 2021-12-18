using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;

namespace ThreePlus
{
    public class Ground : SceneObject
    {

        #region members

        protected double height = 0.0;
        protected double size = 10000.0;
        protected bool hasGround = false;

        #endregion

        #region constructors

        public Ground() : base()
        {

        }

        public Ground(Ground ground) : base()
        {
            this.height = ground.height;
            this.size = ground.size;
            this.hasGround = ground.hasGround;
        }

        public Ground(double size, double height) : base()
        {
            this.hasGround = true;
            this.height = height;
            this.size = size;
        }

        #endregion

        #region properties

        public virtual double Height
        {
            get { return height; }
        }

        public virtual double Size
        {
            get { return size; }
        }

        public virtual bool HasGround
        {
            get { return hasGround; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Ground | ";
        }

        #endregion


    }
}
