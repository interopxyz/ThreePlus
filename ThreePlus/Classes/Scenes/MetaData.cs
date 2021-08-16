using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class MetaData : SceneObject
    {

        #region members

        protected double version = 4.5;
        protected string name = string.Empty;
        protected string generator = "Object3D.toJSON";

        protected int layers = 1;
        protected double[] matrix = new double[] { 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0 };

        #endregion

        #region constructors

        public MetaData() : base()
        {
            this.title = "metadata";
        }

        public MetaData(MetaData metaData) : base(metaData)
        {
            this.version = metaData.version;
            this.name = metaData.name;
            this.generator = metaData.generator;
            this.layers = metaData.layers;
            this.matrix = metaData.matrix;
        }

        #endregion

        #region properties

        public virtual string Name
        {
            get { return name; }
        }

        public virtual double Version
        {
            get { return version; }
        }

        public virtual string Generator
        {
            get { return generator; }
        }

        public virtual int Layers
        {
            get { return layers; }
        }
        public virtual double[] Matrix
        {
            get { return matrix; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Camera | ";
        }

        #endregion

    }
}
