using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components
{
    public class GH_Preview : GH_Component
    {
        private BoundingBox prevBox = new BoundingBox();
        protected double radius = 2;
        protected List<Light> prevLights = new List<Light>();
        protected List<Camera> prevCameras = new List<Camera>();
        protected List<Model> prevModels = new List<Model>();


        /// <summary>
        /// Initializes a new instance of the GH_Preview class.
        /// </summary>
        public GH_Preview()
          : base("GH_Preview", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_Preview(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
        }

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            prevBox = BoundingBox.Unset;
            prevLights = new List<Light>();
            prevCameras = new List<Camera>();
            prevModels = new List<Model>();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override BoundingBox ClippingBox
        {
            get
            {
                return prevBox;
            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            double mTol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            double aTol = Rhino.RhinoDoc.ActiveDoc.ModelAngleToleranceRadians;
            //args.Display.ZBiasMode = Rhino.Display.ZBiasMode.TowardsCamera;

            foreach(Camera camera in prevCameras)
            {
                Vector3d far = new Vector3d(camera.Frame.ZAxis);
                far.Unitize();
                far = far* camera.Far;
                Plane plnF = new Plane(camera.Frame);
                plnF.Origin = camera.Position + far;

                Vector3d near = new Vector3d(camera.Frame.ZAxis);
                near.Unitize();
                near = near * camera.Near;
                Plane plnN = new Plane(camera.Frame);
                plnN.Origin = camera.Position + near;

                double r = camera.Near / camera.Far;

                List<Point3d> ptsO = new List<Point3d> { camera.Frame.PointAt(-1, -1), camera.Frame.PointAt(-1, 1), camera.Frame.PointAt(1, 1), camera.Frame.PointAt(1, -1), camera.Frame.PointAt(-1, -1) };
                List<Point3d> ptsF = new List<Point3d> { plnF.PointAt(-1, -1), plnF.PointAt(-1, 1), plnF.PointAt(1, 1), plnF.PointAt(1, -1), plnF.PointAt(-1, -1) };
                List<Point3d> ptsN = new List<Point3d> { plnN.PointAt(-1, -1), plnN.PointAt(-1, 1), plnN.PointAt(1, 1), plnN.PointAt(1, -1), plnN.PointAt(-1, -1) };

                prevBox.Union(new Point3d(camera.Position));
                prevBox.Union(new BoundingBox(ptsO));
                prevBox.Union(new BoundingBox(ptsF));

                if (camera.IsOrthographic)
                {
                    args.Display.DrawArrow(new Line(camera.Position, camera.Target), Sd.Color.DarkGray);
                    args.Display.DrawLine(new Line(ptsN[0], ptsF[0]), Sd.Color.DarkGray);
                    args.Display.DrawLine(new Line(ptsN[1], ptsF[1]), Sd.Color.DarkGray);
                    args.Display.DrawLine(new Line(ptsN[2], ptsF[2]), Sd.Color.DarkGray);
                    args.Display.DrawLine(new Line(ptsN[3], ptsF[3]), Sd.Color.DarkGray);
                    args.Display.DrawPolyline( ptsO, Sd.Color.DarkGray);
                    args.Display.DrawPolyline(ptsF, Sd.Color.DarkGray);
                    args.Display.DrawPolyline(ptsN, Sd.Color.DarkGray);
                }
                else
                {
                    ptsN = new List<Point3d> { plnN.PointAt(-r, -r), plnN.PointAt(-r, r), plnN.PointAt(r, r), plnN.PointAt(r, -r), plnN.PointAt(-r, -r) };

                    args.Display.DrawArrow(new Line(camera.Position, camera.Target), Sd.Color.Black);
                    args.Display.DrawLine(new Line(camera.Position, ptsF[1]), Sd.Color.Black);
                    args.Display.DrawLine(new Line(camera.Position, ptsF[2]), Sd.Color.Black);
                    args.Display.DrawLine(new Line(camera.Position, ptsF[3]), Sd.Color.Black);
                    args.Display.DrawPolyline(ptsF, Sd.Color.Black);
                    args.Display.DrawPolyline(ptsN, Sd.Color.Black);
                }
            }

            // Surfaces
            foreach (Light light in prevLights)
            {
                double r = radius * light.Intensity;
                switch (light.LightType)
                {
                    case Light.Types.Ambient:
                        r = radius * 2 + r * 2;

                        prevBox.Union(new Point3d(r, r, r));
                        prevBox.Union(new Point3d(-r, -r, -r));

                        args.Display.DrawCircle(new Circle(Plane.WorldXY, new Point3d(0, 0, 0), r), light.Color);
                        args.Display.DrawCircle(new Circle(Plane.WorldYZ, new Point3d(0, 0, 0), r), light.Color);
                        args.Display.DrawCircle(new Circle(Plane.WorldZX, new Point3d(0, 0, 0), r), light.Color);
                        break;
                    case Light.Types.Hemisphere:
                        r = radius + r;

                        prevBox.Union(new Point3d(r , r, r));
                        prevBox.Union(new Point3d(-r , -r, -r));

                        args.Display.DrawCircle(new Circle(Plane.WorldXY, new Point3d(0, 0, 0), r), light.ColorB);
                        args.Display.DrawArc(new Arc(Plane.WorldYZ, r, Math.PI), light.Color);
                        args.Display.DrawArc(new Arc(new Circle(Plane.WorldZX, new Point3d(0, 0, 0), r), new Interval(-Math.PI/2.0,Math.PI/2.0)), light.Color);
                        break;
                    case Light.Types.Point:
                        r = radius/2.0 + r/2.0;

                        prevBox.Union(light.Position+new Point3d(r, r, r));
                        prevBox.Union(light.Position+new Point3d(-r, -r, -r));

                        args.Display.DrawCircle(new Circle(Plane.WorldXY, light.Position, r), light.Color);
                        args.Display.DrawCircle(new Circle(Plane.WorldYZ, light.Position, r), light.Color);
                        args.Display.DrawCircle(new Circle(Plane.WorldZX, light.Position, r), light.Color);
                        break;
                    case Light.Types.Directional:
                        r = radius/2.0 + r/2.0;
                        Vector3d normal = new Vector3d(light.Position - light.Target);
                        Plane plane = new Plane(light.Position, normal);
                        Circle circle = new Circle(plane, light.Position, r);
                        Line line = new Line(light.Position, light.Target);

                        prevBox.Union(circle.BoundingBox);
                        prevBox.Union(line.BoundingBox);

                        args.Display.DrawCircle(circle, light.Color);
                        args.Display.DrawArrow(line, light.Color);
                        break;
                    case Light.Types.Spot:
                        Vector3d norm = new Vector3d(light.Position - light.Target);
                        Plane frame = new Plane(light.Target, norm);
                        r = norm.Length * Math.Tan(light.Angle);
                        Circle circ = new Circle(frame, r);
                        Line axis = new Line(light.Position, light.Target);

                        prevBox.Union(circ.BoundingBox);
                        prevBox.Union(axis.BoundingBox);

                        args.Display.DrawCircle(circ, light.Color);
                        args.Display.DrawArrow(axis, light.Color);
                        args.Display.DrawLine(new Line(light.Position, light.Target + frame.XAxis * r), light.Color);
                        args.Display.DrawLine(new Line(light.Position, light.Target + frame.XAxis * -r), light.Color);
                        args.Display.DrawLine(new Line(light.Position, light.Target + frame.YAxis * r), light.Color);
                        args.Display.DrawLine(new Line(light.Position, light.Target + frame.YAxis * -r), light.Color);

                        break;
                }
            }
            
            // Set Display Override
            base.DrawViewportWires(args);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6f4e4a9c-c7eb-42d9-bfa7-e93b1e40339c"); }
        }
    }
}