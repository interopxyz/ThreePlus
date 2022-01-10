using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg = Rhino.Geometry;
using Sd = System.Drawing;

namespace ThreePlus
{
    public static class GhToThree
    {

        public static bool TryGetBitmap(this IGH_Goo goo, out Sd.Bitmap bitmap, out string message)
        {
            message = string.Empty;
            bitmap = null;

            string filePath = string.Empty;

            goo.CastTo<string>(out filePath);
            if (!goo.CastTo<Sd.Bitmap>(out bitmap))
            {
            if (File.Exists(filePath))
            {
                if (!filePath.GetBitmapFromFile(out bitmap))
                {
                    if (!Path.HasExtension(filePath))
                    {
                            message = "This is not a valid file path. This file does not have a valid bitmap extension";
                            return false;
                        }
                        else
                    {
                            message = "This is not a valid bitmap file type. The extension " + Path.GetExtension(filePath) + " is not a supported bitmap format";
                            return false;
                        }
                    }
            }
            else
            {
                message = "This is not a valid System Drawing Bitmap, or File Path to a valid Image file";
                    return false;
                }
            }

            return true;
        }

        public static Model ToModel(this IGH_Goo goo)
        {
            Rg.Mesh mesh = null;
            Rg.NurbsCurve curve = null;
            Rg.Point3d point3d = Rg.Point3d.Unset;
            Model model = null;
            if (goo.CastTo<Model>(out model))
            {
                model = new Model(model);
            }
            else
            {
                switch (goo.TypeName)
                {
                    case "NurbsCurve":
                        Rg.NurbsCurve nCurve = null;
                        if (goo.CastTo<Rg.NurbsCurve>(out nCurve))
                        {
                            curve = nCurve.ToNurbsCurve();
                            curve.SetUserString("name", "curve");
                            model = new Model(curve);
                        }
                        break;
                    case "Curve":
                        Rg.Curve crv = null;
                        if (goo.CastTo<Rg.Curve>(out crv))
                        {
                            curve = crv.DuplicateCurve().ToNurbsCurve();
                            curve.SetUserString("name", "curve");
                            model = new Model(curve);
                        }
                        break;
                    case "Arc":
                        Rg.Arc arc = new Rg.Arc();
                        if (goo.CastTo<Rg.Arc>(out arc))
                        {
                            curve = arc.ToNurbsCurve();
                            curve.SetUserString("name", "arc");
                            model = new Model(curve);
                        }
                        break;
                    case "Circle":
                        Rg.Circle circle = new Rg.Circle();
                        if (goo.CastTo<Rg.Circle>(out circle))
                        {
                            curve = circle.ToNurbsCurve();
                            curve.SetUserString("name", "circle");
                            model = new Model(curve);
                        }
                        break;
                    case "Ellipse":
                        Rg.Ellipse ellipse = new Rg.Ellipse();
                        if (goo.CastTo<Rg.Ellipse>(out ellipse))
                        {
                            curve = ellipse.ToNurbsCurve();
                            curve.SetUserString("name", "ellipse");
                            model = new Model(model);
                        }
                        break;
                    case "Line":
                        Rg.Line line = new Rg.Line();
                        if (goo.CastTo<Rg.Line>(out line))
                        {
                            curve = line.ToNurbsCurve();
                            curve.SetUserString("name", "line");
                            model = new Model(curve);
                        }
                        break;
                    case "Rectangle":
                        Rg.Rectangle3d rect = new Rg.Rectangle3d();
                        if (goo.CastTo<Rg.Rectangle3d>(out rect))
                        {
                            curve = rect.ToNurbsCurve();
                            curve.SetUserString("name", "rect");
                            model = new Model(curve);
                        }
                        break;
                    case "NurbsSurface":
                        Rg.NurbsSurface nSurface = null;
                        if (goo.CastTo<Rg.NurbsSurface>(out nSurface))
                        {
                            mesh = Rg.Mesh.CreateFromSurface(nSurface);
                            mesh.SetUserString("name", "nurbsSurface");
                            model = new Model(mesh);
                        }
                        break;
                    case "Surface":
                        Rg.Surface surface = null;
                        if (goo.CastTo<Rg.Surface>(out surface))
                        {
                            mesh = Rg.Mesh.CreateFromSurface(surface);
                            mesh.SetUserString("name", "surface");
                            model = new Model(mesh);
                        }
                        break;
                    case "Brep":
                        Rg.Brep brep = new Rg.Brep();
                        if (goo.CastTo<Rg.Brep>(out brep))
                        {
                            mesh = new Rg.Mesh();
                            mesh.Append(Rg.Mesh.CreateFromBrep(brep, Rg.MeshingParameters.Default));
                            mesh.SetUserString("name", "brep");
                            model = new Model(mesh);
                        }
                        break;
                    case "Mesh":
                        if (goo.CastTo<Rg.Mesh>(out mesh)) {
                            mesh = mesh.DuplicateMesh();
                            mesh.SetUserString("name", "mesh");
                            model = new Model(mesh);
                        }
                        break;
                    case "Point3d":
                        if(goo.CastTo<Rg.Point3d>(out point3d))
                        {
                            PointCloud cloud = new PointCloud(point3d, Sd.Color.Black);
                            model = new Model(cloud);
                        }
                        break;
                }
            }

            return model;
        }

    }
}
