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
        public static Rg.BoundingBox GetBoundary(this List<Model> input)
        {
            Rg.BoundingBox bbox = Rg.BoundingBox.Unset;

            foreach (Model model in input)
            {
                if (model.IsMesh) bbox.Union(model.Mesh.GetBoundingBox(false));
                if (model.IsCurve) bbox.Union(model.Curve.GetBoundingBox(false));
                if (model.IsCloud) bbox.Union(new Rg.BoundingBox(model.Cloud.Points));
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
            return "'" + Math.Round(((double)value.A) / 255.0, digits) + "'";
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

    }
}
