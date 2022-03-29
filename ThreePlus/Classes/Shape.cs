using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public class Shape : MetaData
    {

        #region members
        public enum ShapeTypes { Circle, Ring, Plane, Box, Sphere, Cylinder, Capsule, Cone, Dodecahedron, Icosahedron, Octahedron, Tetrahedron, Torus, TorusKnot}

        protected ShapeTypes shapeType = ShapeTypes.Box;

        protected Rg.Plane plane = Rg.Plane.WorldXY;

        protected double sizeX = 10;
        protected double sizeY = 10;
        protected double sizeZ = 10;

        protected int divisionsU = 1;
        protected int divisionsV = 1;
        protected int divisionsW = 1;

        protected int turnsA = 2;
        protected int turnsB = 3;

        protected Rg.Mesh previewMesh = Rg.Mesh.CreateFromBox(new Rg.Box(Rg.Plane.WorldXY, new Rg.Interval(-5, 5), new Rg.Interval(-5, 5), new Rg.Interval(-5, 5)),1,1,1);

        #endregion

        #region constructors

        public Shape()
        {

        }

        public Shape(Shape shape):base(shape)
        {
            this.shapeType = shape.shapeType;

            this.plane = new Rg.Plane(shape.plane);

            this.sizeX = shape.sizeX;
            this.sizeY = shape.sizeY;
            this.sizeZ = shape.sizeZ;

            this.divisionsU = shape.divisionsU;
            this.divisionsV = shape.divisionsV;
            this.divisionsW = shape.divisionsW;

            this.turnsA = shape.turnsA;
            this.turnsB = shape.turnsB;

            this.previewMesh = shape.previewMesh;
        }

        public static Shape BoxShape(Rg.Box box,int divisionsX, int divisionsY, int divisionsZ)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Box;

            output.plane = new Rg.Plane(box.Plane);

            output.sizeX = box.X.Length;
            output.sizeY = box.Y.Length;
            output.sizeZ = box.Z.Length;

            output.divisionsU = divisionsX;
            output.divisionsV = divisionsY;
            output.divisionsW = divisionsZ;

            output.previewMesh = Rg.Mesh.CreateFromBox(new Rg.Box(box.Plane, new Rg.Interval(-output.sizeX/2.0, output.sizeX / 2.0), new Rg.Interval(-output.sizeY / 2.0, output.sizeY / 2.0), new Rg.Interval(-output.sizeZ / 2.0, output.sizeZ / 2.0)), divisionsX, divisionsY, divisionsZ);

            return output;
        }

        public static Shape SphereShape(Rg.Plane plane, double radius, int divisionsU, int divisionsV)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Sphere;

            output.plane = plane;

            output.sizeX = radius;

            output.divisionsU = divisionsU;
            output.divisionsV = divisionsV;

            output.previewMesh = Rg.Mesh.CreateFromSphere(new Rg.Sphere(plane, radius), divisionsU, divisionsV);

            return output;
        }

        public static Shape TorusShape(Rg.Plane plane, double radius,double thickness, int divisionsU, int divisionsV)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Torus;

            output.plane = plane;

            output.sizeX = radius;
            output.sizeY = thickness;

            output.divisionsU = divisionsU;
            output.divisionsV = divisionsV;

            output.previewMesh = Rg.Mesh.CreateFromTorus(new Rg.Torus(plane, radius, thickness), divisionsU, divisionsV);

            return output;
        }

        public static Shape TorusKnotShape(Rg.Plane plane, double radius, double thickness, int p, int q, int divisionsU, int divisionsV)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.TorusKnot;

            output.plane = plane;

            output.sizeX = radius;
            output.sizeY = thickness;

            output.turnsA = p;
            output.turnsB = q;

            output.divisionsU = divisionsU;
            output.divisionsV = divisionsV;

            output.previewMesh = Rg.Mesh.CreateFromSphere(new Rg.Sphere(plane, radius+thickness), divisionsU, divisionsV);

            return output;
        }

        public static Shape PlaneShape(Rg.Plane plane, double width, double height)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Plane;

            output.plane = plane;

            output.sizeX = width;
            output.sizeY = height;

            output.previewMesh = Rg.Mesh.CreateFromPlane(plane,new Rg.Interval(-width/2.0,width/2.0),new Rg.Interval(-height/2.0,height/2.0),1,1);

            return output;
        }

        public static Shape CylinderShape(Rg.Plane plane, double radius, double height, int divisions)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Cylinder;

            output.plane = plane;

            output.sizeX = radius;
            output.sizeY = height;

            output.divisionsU = divisions;

            Rg.Plane prevPlane = new Rg.Plane(plane);
            prevPlane.Origin = prevPlane.Origin - prevPlane.ZAxis * height / 2.0;

            output.previewMesh = Rg.Mesh.CreateFromCylinder(new Rg.Cylinder(new Rg.Circle(prevPlane, radius),height),1,divisions);

            return output;
        }

        public static Shape CapsuleShape(Rg.Plane plane, double radius, double height, int divisions)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Capsule;

            output.plane = plane;

            output.sizeX = radius;
            output.sizeY = height;

            output.divisionsU = divisions;

            Rg.Plane prevPlane = new Rg.Plane(plane);
            prevPlane.Origin = prevPlane.Origin - prevPlane.ZAxis*height /2.0;

            output.previewMesh = Rg.Mesh.CreateFromCylinder(new Rg.Cylinder(new Rg.Circle(prevPlane, radius), height), 1, divisions);

            return output;
        }

        public static Shape ConeShape(Rg.Plane plane, double radius, double height, int divisions)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Cone;

            output.plane = plane;

            output.sizeX = radius;
            output.sizeY = height;

            output.divisionsU = divisions;

            Rg.Plane prevPlane = new Rg.Plane(plane);
            prevPlane.Origin = prevPlane.Origin + prevPlane.ZAxis * height*0.5;
            prevPlane.Flip();
            
            output.previewMesh = Rg.Mesh.CreateFromCone(new Rg.Cone(prevPlane, height, radius), 1, divisions);

            return output;
        }

        public static Shape CircleShape(Rg.Plane plane, double radius, int divisions)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Circle;

            output.plane = plane;

            output.sizeX = radius;

            output.divisionsU = divisions;

            output.previewMesh = new Rg.Mesh().CreateDisk(plane, radius, divisions);

            return output;
        }

        public static Shape RingShape(Rg.Plane plane, double outerRadius, double innerRadius, int divisions)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Ring;

            double r0 = Math.Min(outerRadius, innerRadius);
            double r1 = Math.Max(outerRadius, innerRadius);

            output.plane = plane;

            output.sizeX = r0;
            output.sizeY = r1;

            output.divisionsU = divisions;

            output.previewMesh = new Rg.Mesh().CreateRing(plane, r0, r1, divisions);

            return output;
        }

        public static Shape IcosahedronShape(Rg.Plane plane, double radius)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Icosahedron;

            output.plane = plane;

            output.sizeX = radius;

            Rg.Mesh mesh = Rg.Mesh.CreateIcoSphere(new Rg.Sphere(plane, radius), 0);
            mesh.Unweld(Math.PI / 6.0, true);

            output.previewMesh = mesh;

            return output;
        }

        public static Shape DodecahedronShape(Rg.Plane plane, double radius)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Dodecahedron;

            output.plane = plane;

            output.sizeX = radius;

            output.previewMesh = new Rg.Mesh().CreateDodecahedron(plane, radius);

            return output;
        }

        public static Shape OctahedronShape(Rg.Plane plane, double radius)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Octahedron;

            output.plane = plane;

            output.sizeX = radius;

            output.previewMesh = new Rg.Mesh().CreateOctahedron(plane,radius);

            return output;
        }

        public static Shape TetrahedronShape(Rg.Plane plane, double radius)
        {
            Shape output = new Shape();
            output.shapeType = ShapeTypes.Tetrahedron;

            output.plane = plane;

            output.sizeX = radius;

            output.previewMesh = new Rg.Mesh().CreateTetrahedron(plane, radius);

            return output;
        }



        #endregion

        #region properties

        public virtual Rg.Mesh PreviewMesh
        {
            get { return previewMesh; }
        }

        public virtual ShapeTypes ShapeType
        {
            get { return this.shapeType; }
        }

        public virtual Rg.Plane Plane
        {
            get { return this.plane; }
        }

        public virtual double SizeX
        {
            get { return this.sizeX; }
        }

        public virtual double SizeY
        {
            get { return this.sizeY; }
        }

        public virtual double SizeZ
        {
            get { return this.sizeZ; }
        }

        public virtual double DivisionsU
        {
            get { return this.divisionsU; }
        }

        public virtual double DivisionsV
        {
            get { return this.divisionsV; }
        }

        public virtual double DivisionsW
        {
            get { return this.divisionsW; }
        }

        public virtual int TurnsA
        {
            get { return this.turnsA; }
        }

        public virtual int TurnsB
        {
            get { return this.turnsB; }
        }

        #endregion

        #region methods



        #endregion

    }
}
