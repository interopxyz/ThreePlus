using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public static class ObjToJavascript
    {
        public static string ToHtml(this Scene input, bool local=false)
        {
            // https://cdn.jsdelivr.net/npm/three@0.131.3/examples/js/
            StringBuilder output = new StringBuilder();

            output.AppendLine("<!DOCTYPE html>");
            output.AppendLine("<html>");
            output.AppendLine("<head>");
            output.AppendLine("<meta charset=\"utf - 8\">");
            output.AppendLine("<title>"+input.Name+"</title>");
            output.AppendLine("<style>");
            output.AppendLine("body { margin: 0; }");
            output.AppendLine("</style>");
            output.AppendLine("</head>");
            output.AppendLine("<body>");
            if (local)
            {
                output.AppendLine("<script src=\"js/three.js\"></script>");
                output.AppendLine("<script src=\"js/OrbitControls.js\"></script>");
                output.AppendLine("<script src=\"js/VertexNormalsHelper.js\"></script>");
                output.AppendLine("<script src=\"js/VertexTangentsHelper.js\"></script>");

                if (input.AmbientOcclusion.HasAO)
                {
                    output.AppendLine("<script src=\"js/EffectComposer.js\"></script>");
                    output.AppendLine("<script src=\"js/CopyShader.js\"></script>");
                    output.AppendLine("<script src=\"js/ShaderPass.js\"></script>");

                    output.AppendLine("<script src=\"js/SSAOPass.js\"></script>");
                    output.AppendLine("<script src=\"js/SimplexNoise.js\"></script>");
                    output.AppendLine("<script src=\"js/SSAOShader.js\"></script>");
                }


            }
            else
            {
                output.AppendLine("<script src=\"http://threejs.org/build/three.min.js\"></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.101.1/examples/js/controls/OrbitControls.js\" ></script>");
            }
            output.AppendLine("<script type=\"text/javascript\" src=\"app.js\"></script>");
            output.AppendLine("</body>");
            output.AppendLine("</html>");

            return output.ToString();
        }

        public static string ToJavascript(this Scene input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const scene = new THREE.Scene();");

            output.AppendLine(input.Environment.ToJavascript());
            if (input.Atmosphere.HasFog) output.AppendLine(input.Atmosphere.ToJavascript());
            if (input.Ground.HasGround) output.AppendLine(input.Ground.ToJavascript());

            output.AppendLine("const renderer = new THREE.WebGLRenderer({ antialias: true });");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("document.body.appendChild(renderer.domElement);");

            output.Append(input.Camera.ToJavascript());
            output.Append(input.Grid.ToJavascript());
            output.Append(input.Axes.ToJavascript());

            int i = 0;
            foreach(Model model in input.Models)
            {
                string index = i.ToString();
                if (model.IsMesh) output.Append(model.Mesh.ToJavascript(index));
                output.AppendLine(model.Material.ToJavascript(index));
                output.AppendLine("const model"+ index + " = new THREE.Mesh( mesh"+ index + ", material" + index + " );");
                output.AppendLine("scene.add(model"+ index + ");");
                if (model.IsCurve) output.Append(model.Tangents.ToJavascript(index));
                if (model.IsMesh) output.Append(model.Normals.ToJavascript(index));
                i++;
            }

            i = 0;
            foreach (Light light in input.Lights)
            {
                string index = i.ToString();
                output.AppendLine(light.ToJavascript(index));
                output.AppendLine("scene.add(light" + index + ");");
                i++;
            }

            output.Append(input.AmbientOcclusion.ToJavascript());

            output.AppendLine("controls = new THREE.OrbitControls (camera, renderer.domElement);");
            output.AppendLine("const animate = function () {");
            output.AppendLine("controls.update();");
            output.AppendLine("requestAnimationFrame ( animate ); ");
            output.AppendLine("renderer.render (scene, camera);");

            if (input.AmbientOcclusion.HasAO) output.AppendLine("composer.render();");

            output.AppendLine("};");
            output.AppendLine("animate();");

            return output.ToString();
        }

        public static string ToJavascript(this AmbientOcclusion input)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasAO)
            {
                output.AppendLine("let composer = new THREE.EffectComposer( renderer );");
                output.AppendLine("const ssaoPass = new THREE.SSAOPass( scene, camera, window.innerWidth, window.innerHeight );");
                output.AppendLine("ssaoPass.kernelRadius = " + input.Radius + ";");
                output.AppendLine("ssaoPass.minDistance = " + input.MinDistance + ";");
                output.AppendLine("ssaoPass.maxDistance = " + input.MaxDistance+ ";");
                output.AppendLine("composer.addPass( ssaoPass );");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Ground input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("var groundMaterial = new THREE.MeshBasicMaterial();");
            output.AppendLine("var groundPlane = new THREE.Mesh(new THREE.PlaneBufferGeometry("+input.Size+","+ input.Size + "), groundMaterial);");
            output.AppendLine("groundPlane.position.y = "+input.Height+";");
            output.AppendLine("groundPlane.rotation.x = - Math.PI / 2;");
            output.AppendLine("groundPlane.receiveShadow = true;");
            output.AppendLine("scene.add(groundPlane);");

            return output.ToString();
        }

        public static string ToJavascript(this Atmosphere input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("scene.fog = new THREE.FogExp2( " + input.Color.ToJs() + " , "+input.Density+ " );");

            return output.ToString();
        }

        public static string ToJavascript(this Environment input)
        {
            StringBuilder output = new StringBuilder();

            string background = "scene.background = new THREE.Color(" + input.Background.ToJs() + ");";

            if (input.HasEnvMap)
            {
                output.AppendLine("var envMap = new THREE.TextureLoader().load(\"data:image/png;base64," + input.EnvMap.ToJs() + "\");");

                output.AppendLine("envMap.mapping = THREE.EquirectangularReflectionMapping;");
                if (input.IsBackground)
                {
                    output.AppendLine("scene.background = envMap;");
                }
                else
                {
                    output.AppendLine(background);
                }
                if (input.IsEnvironment) output.AppendLine("scene.environment = envMap;");
            }
            else
            {
                output.AppendLine(background);
            }
            return output.ToString();
        }

        public static string ToJavascript(this TangentDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const tanHelper" + index + " = new THREE.VertexTangentsHelper(model" + index + "," + input.Width + "," + input.Color.ToJs() + "," + input.Width + ");");
                output.AppendLine("scene.add(tanHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this NormalDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const nrmHelper" + index + " = new THREE.VertexNormalsHelper(model" + index + "," + input.Width + "," + input.Color.ToJs() + "," + input.Width + ");");
                output.AppendLine("scene.add(nrmHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Axes input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const axes = new THREE.AxesHelper(" + input.Scale + ");");
                output.AppendLine("axes.setColors (" + input.XAxis.ToJs() + ");");
                output.AppendLine("scene.add(axes);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Grid input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show) {
            output.AppendLine("var grid = new THREE.GridHelper(" + input.Size + "," + input.Divisions + ","+ input.AxisColor.ToJs() + ","+ input.GridColor.ToJs() + ");");
            output.AppendLine("scene.add(grid);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const camera = new THREE.PerspectiveCamera(" +input.FOV+", window.innerWidth / window.innerHeight, "+input.Near+"," + input.Far + ");");
            output.AppendLine("camera.position.set("+input.Position.ToJs()+");");
            output.AppendLine("camera.lookAt (new THREE.Vector3("+ input.Target.X + ","+ input.Target.Z + ","+ input.Target.Y + "));");

            return output.ToString();
        }

        public static string ToJavascript(this Material input, string name)
        {
            StringBuilder output = new StringBuilder();

            string starter = "const material" + name + " = new THREE.";
            switch (input.MaterialType)
            {
                default:
                    output.AppendLine(starter+"MeshBasicMaterial( { color: " + input.Color.ToJs() + ", transparent: " + input.Color.IsTransparent() + ", opacity: "+ input.Color.ToJsOpacity() + " } );");
                    break;
                case Material.Types.Lambert:
                    output.AppendLine(starter + "MeshLambertMaterial( { color: " + input.Color.ToJs() + ", transparent: " + input.Color.IsTransparent() + ", opacity: " + input.Color.ToJsOpacity() + " } );");
                    break;
                case Material.Types.Phong:
                    output.AppendLine(starter + "MeshPhongMaterial( { color: " + input.Color.ToJs() + ", transparent: " + input.Color.IsTransparent() + ", opacity: " + input.Color.ToJsOpacity() + ", shininess: " + (input.Shininess*100.0) + " } );");
                    break;
                case Material.Types.Standard:
                    output.AppendLine(starter + "MeshStandardMaterial( { color: " + input.Color.ToJs() + ", transparent: " + input.Color.IsTransparent() + ", opacity: " + input.Color.ToJsOpacity() + ", roughness: " + (input.Roughness) + ", metalness: " + (input.Metalness) + " } );");
                    break;
                case Material.Types.Physical:
                    output.AppendLine(starter + "MeshPhysicalMaterial( { color: " + input.Color.ToJs() + ", transparent: " + input.Color.IsTransparent() + ", opacity: " + input.Color.ToJsOpacity() + ", roughness: " + (input.Roughness * 100.0) + ", metalness: " + (input.Metalness * 100.0) + ", reflectivity: " + (input.Reflectivity * 100.0) + ", clearcoat: " + (input.Clearcoat * 100.0) + ", clearcoatRoughness: " + (input.ClearcoatRoughness * 100.0) + " } );");
                    break;
                case Material.Types.Normal:
                    output.AppendLine(starter + "MeshNormalMaterial();");
                    break;
                case Material.Types.Depth:
                    output.AppendLine(starter + "MeshDepthMaterial();");
                    break;
            }

            return output.ToString();
        }

        public static string ToJavascript(this Light input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            string starter = "const " + name + " = new THREE.";
            switch (input.LightType)
            {
                default:
                    output.AppendLine(starter + "AmbientLight(" + input.Color.ToJs() + ", " + input.Intensity + " );");
                    break;
                case Light.Types.Point:
                    output.AppendLine(starter + "PointLight(" + input.Color.ToJs() + ", " + input.Intensity + ", " + input.Distance + ", " + input.Decay + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    break;
                case Light.Types.Directional:
                    output.AppendLine(starter + "DirectionalLight(" + input.Color.ToJs() + ", " + input.Intensity + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    break;
                case Light.Types.Hemisphere:
                    output.AppendLine(starter + "HemisphereLight(" + input.Color.ToJs() + ", " + input.ColorB.ToJs() + ", " + input.Intensity + " );");
                    break;
            }

            return output.ToString();
        }

        public static string ToJavascript(this Rg.Mesh input,string name)
        {
            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();
            if (input.Normals.Count < 1) input.RebuildNormals();

            bool hasUV = (input.TextureCoordinates.Count > 0);

                StringBuilder output = new StringBuilder();

            output.AppendLine("const mesh" + name + " = new THREE.BufferGeometry();");

            int count = input.Faces.Count;
            var vertices = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count*3);
            var normals = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            var uvs = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);


            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                vertices[k*3] = input.Vertices[face.A].ToJs();
                normals[k * 3] = input.Normals[face.A].ToJs();

                vertices[k * 3 + 1] = input.Vertices[face.C].ToJs();
                normals[k * 3 + 1] = input.Normals[face.C].ToJs();

                vertices[k * 3 + 2] = input.Vertices[face.B].ToJs();
                normals[k * 3 + 2] = input.Normals[face.B].ToJs();
            }
            );

            if (hasUV) {
                Parallel.For(0, count, k =>
                {
                    Rg.MeshFace face = input.Faces[k];
                    uvs[k * 3] = input.TextureCoordinates[face.A].ToJs();
                    uvs[k * 3 + 1] = input.TextureCoordinates[face.C].ToJs();
                    uvs[k * 3 + 2] = input.TextureCoordinates[face.B].ToJs();
                }
                );
            }
            
            output.AppendLine("const vertices" + name + " = new Float32Array( [" + string.Join(",", vertices.Values) + "] );");
            output.AppendLine("const normals" + name + " = new Float32Array( [" + string.Join(",", normals.Values) + "] );");
            if (hasUV) output.AppendLine("const uv" + name + " = new Float32Array( [" + string.Join(",", uvs.Values) + "] );");

            output.AppendLine("mesh"+name + ".setAttribute( 'position', new THREE.BufferAttribute( vertices" + name + ", 3 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'normal', new THREE.BufferAttribute( normals" + name + ", 3 ) );");
            if (hasUV) output.AppendLine("mesh"+name + ".setAttribute( 'uv', new THREE.BufferAttribute( uv" + name + ", 2 ) );");

            return output.ToString();
        }

        #region conversions

        public static string ToJsTarget(this Rg.Point3d input, string name)
        {
            StringBuilder output = new StringBuilder();

            string targetObject = name + "Target";
            output.AppendLine("const "+ targetObject + "= new THREE.Object3D();");
            output.AppendLine(targetObject + ".position.set( "+input.ToJs()+" );");
            output.AppendLine("scene.add("+targetObject+");");
            output.AppendLine(name + ".target = " + targetObject + ";");

            return output.ToString();
        }

        public static string IsTransparent(this Sd.Color value)
        {
            string output = "false";
            if (value.A < 255) output = "true";

            return output;
        }

        public static string ToJsOpacity(this Sd.Color value, int digits = 5)
        {
            return "'" + Math.Round(((double)value.A)/255.0,digits)+ "'";
        }

        public static string ToJs(this Sd.Color value)
        {
            return "'"+Sd.ColorTranslator.ToHtml(value).ToString() +"'";
        }

        public static string ToJs(this Rg.Point3d value, int digits = 5)
        {
            return Math.Round(value.X, digits)+ ","+ Math.Round(value.Z, digits) + ","+ Math.Round(value.Y, digits);
        }

        public static string ToJs(this Rg.Point3f value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.Y, digits);
        }

        public static string ToJs(this Rg.Vector3d value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.Y, digits);
        }

        public static string ToJs(this Rg.Vector3f value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.Y, digits);
        }

        public static string ToJs(this Rg.Point2f value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Y, digits);
        }

        public static string ToJs(this Sd.Bitmap input)
        {
            var ms = new System.IO.MemoryStream();
            input.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] data = ms.ToArray();

            return Convert.ToBase64String(data);
        }

        #endregion

    }
}
