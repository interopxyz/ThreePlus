using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Model : MetaData
    {

        #region members
        public enum GeometryTypes { None,Cloud,Plane,Curve,Mesh}
        protected GeometryTypes geometryType = GeometryTypes.None;

        protected Guid geoId = new Guid();

        protected Rg.Plane plane = Rg.Plane.Unset;
        protected double size = double.NaN;
        protected Rg.Mesh mesh = null;
        protected Rg.NurbsCurve curve = null;

        public Material Material = new Material();
        public Graphic Graphic = new Graphic();
        public TangentDisplay Tangents = new TangentDisplay();
        public NormalDisplay Normals = new NormalDisplay();

        protected bool hasHelper = false;
        protected Color helperColor = Color.Gray;

        #endregion

        #region constructors

        public Model() : base()
        {

        }

        public Model(Model model) : base(model)
        {
            this.geoId = new Guid(model.geoId.ToString());

            if (model.mesh != null) this.mesh = model.mesh.DuplicateMesh();
            if (model.curve != null) this.curve = model.curve.DuplicateCurve().ToNurbsCurve();
            if (model.plane != Rg.Plane.Unset) this.plane= (Rg.Plane)model.plane.Clone();

            this.Tangents = new TangentDisplay(model.Tangents);
            this.Normals = new NormalDisplay(model.Normals);

            this.Material = new Material(model.Material);
            this.Graphic = new Graphic(model.Graphic);
            this.hasHelper = model.hasHelper;
            this.helperColor = model.helperColor;

            this.geometryType = model.geometryType;
            this.size = model.size;
        }

        public Model(Rg.Plane plane, double size) : base()
        {
            this.geoId = Guid.NewGuid();
            this.geometryType = GeometryTypes.Plane;

            this.name = plane.ToString();
            
            this.plane = (Rg.Plane)plane;
            this.size = size;
        }

        public Model(Rg.Mesh mesh):base()
        {
            this.geoId = Guid.NewGuid();
            this.geometryType = GeometryTypes.Mesh;

            this.type = "BufferGeometry";
            this.name = mesh.GetUserString("name");

            this.mesh = mesh.DuplicateMesh();
        }

        public Model(Rg.NurbsCurve curve) : base()
        {
            this.geoId = Guid.NewGuid();
            this.geometryType = GeometryTypes.Curve;

            this.name = curve.GetUserString("name");

            this.curve = curve.DuplicateCurve().ToNurbsCurve();
        }

        #endregion

        #region properties

        public virtual bool IsMesh
        {
            get { return this.geometryType == GeometryTypes.Mesh; }
        }

        public virtual bool IsCurve
        {
            get { return this.geometryType == GeometryTypes.Curve; }
        }

        public virtual bool IsPlane
        {
            get { return this.geometryType == GeometryTypes.Plane; }
        }

        private string GeometryType
        {
            get { return this.geometryType.ToString(); }
        }

        public virtual Rg.Mesh Mesh 
        {
            get { return mesh; }
        }

        public virtual Rg.NurbsCurve Curve
        {
            get { return curve; }
        }

        public virtual Rg.Plane Plane
        {
            get { return plane; }
        }

        public virtual double Size
        {
            get { return size; }
        }

        public virtual Guid GeoId
        {
            get { return geoId; }
        }

        public virtual bool HasHelper
        {
            get { return hasHelper; }
        }

        public virtual Color HelperColor
        {
            get { return helperColor; }
        }

        #endregion

        #region methods

        public void SetHelper(Color helperColor)
        {
            this.hasHelper = true;
            this.helperColor = helperColor;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Model | "+ GeometryType.ToString() + " | "+Material.MaterialType.ToString();
        }

        #endregion

    }
}
