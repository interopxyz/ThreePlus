using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Sd = System.Drawing;

using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public static class RhExtensions
    {

        public static Sd.Color ToDrawingColor(this Rhino.Display.Color4f input)
        {
            return Sd.Color.FromArgb((int)(255.0*input.A), (int)(255.0 * input.R), (int)(255.0 * input.G), (int)(255.0 * input.B));
        }

        public static Rg.BoundingBox GetBoundary(this List<Model> input)
        {
            Rg.BoundingBox bbox = Rg.BoundingBox.Unset;

            foreach (Model model in input)
            {
                bbox.Union(model.BoundingBox);
            }

            return bbox;
        }

        public static string ToHex(this Sd.Color input)
        {
            return "#" + input.R.ToString("X2") + input.G.ToString("X2") + input.B.ToString("X2");
        }

        public static bool GetBitmapFromFile(this string FilePath, out Sd.Bitmap bitmap)
        {
            bitmap = null;
            if (Path.HasExtension(FilePath))
            {
                string extension = Path.GetExtension(FilePath);
                extension = extension.ToLower();
                switch (extension)
                {
                    default:
                        return (false);
                    case ".bmp":
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                        bitmap = (Sd.Bitmap)Sd.Bitmap.FromFile(FilePath);
                        return (bitmap != null);
                }

            }

            return (false);
        }

        public static string GetHash(this Sd.Bitmap input, SHA256 shaHash)
        {
            var ms = new System.IO.MemoryStream();
            input.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.ToArray();

            byte[] data = shaHash.ComputeHash(bytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string ToVertexString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);

            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                values[k * 3] = input.Vertices[face.A].ToStr();
                values[k * 3 + 1] = input.Vertices[face.B].ToStr();
                values[k * 3 + 2] = input.Vertices[face.C].ToStr();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToNormalString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);

            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                values[k * 3] = input.Normals[face.A].ToJs();
                values[k * 3 + 1] = input.Normals[face.B].ToJs();
                values[k * 3 + 2] = input.Normals[face.C].ToJs();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToUvString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            if(!(input.TextureCoordinates.Count > 0))return string.Empty;
            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                values[k * 3] = input.TextureCoordinates[face.A].ToStr();
                values[k * 3 + 1] = input.TextureCoordinates[face.B].ToStr();
                values[k * 3 + 2] = input.TextureCoordinates[face.C].ToStr();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToColorString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            if (input.VertexColors.Count < input.Vertices.Count) return string.Empty;
            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];
                    values[k * 3] = input.VertexColors[face.A].ToStrSet();
                    values[k * 3 + 1] = input.VertexColors[face.B].ToStrSet();
                    values[k * 3 + 2] = input.VertexColors[face.C].ToStrSet();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToColorFloatString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);

            if (input.VertexColors.Count < input.Vertices.Count) return string.Empty;
            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                    values[k * 3] = input.VertexColors[face.A].ToStrFloatSet();
                    values[k * 3 + 1] = input.VertexColors[face.B].ToStrFloatSet();
                    values[k * 3 + 2] = input.VertexColors[face.C].ToStrFloatSet();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToFaceString(this Rg.Mesh input)
        {
            int count = input.Faces.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);

            if (input.VertexColors.Count < input.Vertices.Count) return string.Empty;
            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                values[k * 3] = face.A.ToString();
                values[k * 3 + 1] = face.B.ToString();
                values[k * 3 + 2] = face.C.ToString();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToColorFloatString(this List<Sd.Color> input)
        {
            int count = input.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);

            Parallel.For(0, count, k =>
            {
                values[k] = input[k].ToStrFloatSet();
            }
);
            return string.Join(",", values.Values);
        }

        public static string ToVertexString(this List<Rg.Point3d> input)
        {
            int count = input.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);

            Parallel.For(0, count, k =>
            {
                values[k] = input[k].ToStrArray();
            });
            return string.Join(",", values.Values);
        }

        public static string ToColorsString(this Graphic input)
        {
            int count = input.Colors.Count;
            var values = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);


            if (input.HasColors)
            {
                Parallel.For(0, count, k =>
                {
                    values[k] = input.Colors[k].ToStrFloatSet();
                });
            }
                return string.Join(",", values.Values);
        }

        public static string ToStrOpacity(this Sd.Color value, int digits = 5)
        {
            return Math.Round(((double)value.A) / 255.0, digits).ToString();
        }

        public static string ToStrArray(this Sd.Color value, int digits = 5)
        {
            return Math.Round(value.R / 255.0, digits) + ", " + Math.Round(value.G / 255.0, digits) + ", " + Math.Round(value.B / 255.0, digits);
        }

        public static string ToStr(this Sd.Color value)
        {
            return "'" + value.ToHex() + "'";
        }

        public static string ToStr(this Rg.Point3d value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToStrArray(this Rg.Point3d value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToStr(this Rg.Point3f value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToJsArray(this Rg.Point3f value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToJs(this Rg.Vector3d value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToJs(this Rg.Vector3f value, int digits = 5)
        {
            return Math.Round(value.Y, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.X, digits);
        }

        public static string ToStrSet(this Sd.Color value)
        {
            return value.R + "," + value.G + "," + value.B;
        }

        public static string ToStrFloatSet(this Sd.Color value, int digits = 5)
        {
            return Math.Round(value.R / 255.0, digits) + "," + Math.Round(value.G / 255.0, digits) + "," + Math.Round(value.B / 255.0, digits);
        }

        public static string ToStr(this Rg.Point2f value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Y, digits);
        }

        public static string ToStr(this Sd.Bitmap input)
        {
            var ms = new System.IO.MemoryStream();
            input.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] data = ms.ToArray();

            return Convert.ToBase64String(data);
        }

        public static string SavePng(this Sd.Bitmap input, string path, string name)
        {
            string filename= name+".png";

            Sd.Bitmap bitmap = new Sd.Bitmap(input);

            bitmap.Save(path + filename);

            return filename;
        }

        #region meshes

        public static Rg.Mesh CreateDisk(this Rg.Mesh input, Rg.Plane plane, double radius, int divisions)
        {
            Rg.Mesh mesh = new Rg.Mesh();

            double s = Math.PI * 2.0 * (1.0 / divisions);
            mesh.Vertices.Add(plane.Origin);
            for (int i = 0; i < divisions; i++)
            {
                mesh.Vertices.Add(plane.PointAt(radius * Math.Sin(s * i), radius * Math.Cos(s * i)));
            }

            for (int i = 0; i < divisions - 1; i++)
            {
                mesh.Faces.AddFace(new Rg.MeshFace(0, i + 1, i + 2));
            }
            mesh.Faces.AddFace(new Rg.MeshFace(0, mesh.Vertices.Count - 1, 1));
            mesh.RebuildNormals();

            return mesh;
        }

        public static Rg.Mesh CreateRing(this Rg.Mesh input, Rg.Plane plane, double radiusA, double radiusB, int divisions)
        {
            Rg.Mesh mesh = new Rg.Mesh();

            double s = Math.PI * 2.0 * (1.0 / divisions);

            for (int i = 0; i < divisions; i++)
            {
                mesh.Vertices.Add(plane.PointAt(radiusA * Math.Sin(s * i), radiusA * Math.Cos(s * i)));
                mesh.Vertices.Add(plane.PointAt(radiusB * Math.Sin(s * i), radiusB * Math.Cos(s * i)));
            }

            for (int i = 0; i < divisions - 1; i++)
            {
                mesh.Faces.AddFace(new Rg.MeshFace(i * 2, i*2 + 1, i*2 + 3, i * 2 + 2));
            }
            mesh.Faces.AddFace(new Rg.MeshFace(mesh.Vertices.Count - 2, mesh.Vertices.Count - 1, 1,0));
            mesh.RebuildNormals();

            return mesh;
        }

        public static Rg.Mesh CreateOctahedron(this Rg.Mesh input, Rg.Plane plane, double radius)
        {
            Rg.Mesh mesh = new Rg.Mesh();

            mesh.Vertices.Add(plane.PointAt(0, 0, radius));
            mesh.Vertices.Add(plane.PointAt(-radius, 0, 0));
            mesh.Vertices.Add(plane.PointAt(0, -radius, 0));
            mesh.Vertices.Add(plane.PointAt(radius, 0, 0));
            mesh.Vertices.Add(plane.PointAt(0, radius, 0));
            mesh.Vertices.Add(plane.PointAt(0, 0, -radius));

            mesh.Faces.AddFace(new Rg.MeshFace(0, 1, 2));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 2, 3));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 3, 4));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 4, 1));

            mesh.Faces.AddFace(new Rg.MeshFace(5, 2, 1));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 3, 2));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 4, 3));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 1, 4));

            mesh.Faces.AddFace(new Rg.MeshFace(mesh.Vertices.Count - 2, mesh.Vertices.Count - 1, 1, 0));

            mesh.Unweld(Math.PI / 3.0, true);
            mesh.RebuildNormals();

            return mesh;
        }

        public static Rg.Mesh CreateTetrahedron(this Rg.Mesh input, Rg.Plane plane, double radius)
        {
            Rg.Mesh mesh = new Rg.Mesh();
            double a = radius / Math.Sqrt(3.0);
            mesh.Vertices.Add(plane.PointAt(a, a, a));
            mesh.Vertices.Add(plane.PointAt(-a, -a, a));
            mesh.Vertices.Add(plane.PointAt(-a, a, -a));
            mesh.Vertices.Add(plane.PointAt(a, -a, -a));

            mesh.Faces.AddFace(new Rg.MeshFace(1, 0, 2));
            mesh.Faces.AddFace(new Rg.MeshFace(2, 3, 1));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 1, 3));
            mesh.Faces.AddFace(new Rg.MeshFace(3, 2, 0));

            mesh.Unweld(Math.PI / 3.0, true);
            mesh.RebuildNormals();

            return mesh;
        }

        public static Rg.Mesh CreateDodecahedron(this Rg.Mesh input, Rg.Plane plane, double radius)
        {
            Rg.Mesh mesh = new Rg.Mesh();

            double phi = (Math.Sqrt(5.0) - 1.0) / 2.0;

            double a = 1 / Math.Sqrt(3.0);
            double b = a / phi;
            double c = a * phi;

            foreach (var i in new[] { -1, 1 })
            {
                foreach (var j in new[] { -1, 1 })
                {
                    mesh.Vertices.Add(new Rg.Point3d(0, i * c * radius, j * b * radius));
                    mesh.Vertices.Add(new Rg.Point3d(i * c * radius, j * b * radius, 0));
                    mesh.Vertices.Add(new Rg.Point3d(i * b * radius, 0, j * c * radius));

                    foreach (var k in new[] { -1, 1 })
                    {
                        mesh.Vertices.Add(new Rg.Point3d(i * a * radius, j * a * radius, k * a * radius));
                    }
                }
            }

            mesh.Faces.AddFace(new Rg.MeshFace(0, 3, 1));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 1, 11));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 11, 13));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(10, 0, 13));
            mesh.Faces.AddFace(new Rg.MeshFace(10, 13, 12));
            mesh.Faces.AddFace(new Rg.MeshFace(10, 12, 18));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(10, 18, 16));
            mesh.Faces.AddFace(new Rg.MeshFace(10, 16, 6));
            mesh.Faces.AddFace(new Rg.MeshFace(10, 6, 8));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(0, 10, 8));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 8, 2));
            mesh.Faces.AddFace(new Rg.MeshFace(0, 2, 3));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(5, 14, 11));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 11, 1));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 1, 4));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(15, 19, 17));
            mesh.Faces.AddFace(new Rg.MeshFace(15, 17, 14));
            mesh.Faces.AddFace(new Rg.MeshFace(15, 14, 5));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(15, 9, 6));
            mesh.Faces.AddFace(new Rg.MeshFace(15, 6, 16));
            mesh.Faces.AddFace(new Rg.MeshFace(15, 16, 19));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(5, 4, 7));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 7, 9));
            mesh.Faces.AddFace(new Rg.MeshFace(5, 9, 15));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(8, 6, 9));
            mesh.Faces.AddFace(new Rg.MeshFace(8, 9, 7));
            mesh.Faces.AddFace(new Rg.MeshFace(8, 7, 2));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(4, 1, 3));
            mesh.Faces.AddFace(new Rg.MeshFace(4, 3, 2));
            mesh.Faces.AddFace(new Rg.MeshFace(4, 2, 7));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(13, 11, 14));
            mesh.Faces.AddFace(new Rg.MeshFace(13, 14, 17));
            mesh.Faces.AddFace(new Rg.MeshFace(13, 17, 12));
                                   
            mesh.Faces.AddFace(new Rg.MeshFace(19, 16, 18));
            mesh.Faces.AddFace(new Rg.MeshFace(19, 18, 12));
            mesh.Faces.AddFace(new Rg.MeshFace(19, 12, 17));

            mesh.Unweld(Math.PI / 3.0, true);
            mesh.RebuildNormals();

            return mesh;

        }

        #endregion

    }
}
