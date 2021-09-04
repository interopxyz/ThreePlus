using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Model : MetaData
    {

        #region members

        protected Guid geoId = new Guid();

        protected Rg.Mesh mesh = null;
        protected Rg.NurbsCurve curve = null;

        public Material Material = new Material();
        public TangentDisplay Tangents = new TangentDisplay();
        public NormalDisplay Normals = new NormalDisplay();

        #endregion

        #region constructors

        public Model(Model model) : base()
        {
            this.geoId = new Guid(model.geoId.ToString());

            if (model.mesh != null) this.mesh = model.mesh.DuplicateMesh();
            if (model.curve != null) this.curve= model.curve.DuplicateCurve().ToNurbsCurve();

            this.Tangents = new TangentDisplay(model.Tangents);
            this.Normals = new NormalDisplay(model.Normals);

            this.Material = new Material(model.Material);
        }

        public Model(Rg.Mesh mesh):base()
        {
            geoId = Guid.NewGuid();

            this.type = "BufferGeometry";
            this.name = mesh.GetUserString("name");

            this.mesh = mesh.DuplicateMesh();
        }

        public Model(Rg.NurbsCurve curve) : base()
        {
            geoId = Guid.NewGuid();

            this.name = curve.GetUserString("name");

            this.curve = curve.DuplicateCurve().ToNurbsCurve();
        }

        #endregion

        #region properties

        public virtual bool IsMesh
        {
            get { return (this.mesh != null); }
        }

        public virtual bool IsCurve
        {
            get { return (this.curve != null); }
        }

        private string GeometryType
        {
            get { if (this.IsMesh) {
                    return "Mesh";
                        }
                return "Curve";
            }
        }

        public virtual Rg.Mesh Mesh 
        {
            get { return mesh; }
        }

        public virtual Rg.NurbsCurve Curve
        {
            get { return curve; }
        }


        public virtual Guid GeoId
        {
            get { return geoId; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Model | "+ GeometryType;
        }

        #endregion

    }
}
