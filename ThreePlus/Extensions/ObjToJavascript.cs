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
        public static string ToHtml(this Scene input, bool local = false)
        {
            // https://cdn.jsdelivr.net/npm/three@0.131.3/examples/js/
            StringBuilder output = new StringBuilder();

            output.AppendLine("<!DOCTYPE html>");
            output.AppendLine("<html>");
            output.AppendLine("<head>");
            output.AppendLine("<meta charset=\"utf - 8\">");
            output.AppendLine("<title>" + input.Name + "</title>");
            output.AppendLine("<style>");
            output.AppendLine("body { margin: 0; }");
            output.AppendLine("</style>");
            output.AppendLine("</head>");
            output.AppendLine("<body>");
            if (local)
            {
                output.AppendLine("<script src=\"js/three.min.js\"></script>");
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

                if (input.Outline.HasOutline) output.AppendLine("<script src=\"js/OutlineEffect.js\"></script>");//
                if (input.Environment.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
                {
                    output.AppendLine("<script src=\"js/LightProbeGenerator.js\"></script>");//
                    output.AppendLine("<script src=\"js/LightProbeHelper.js\"></script>");//
                }
            }
            else
            {
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/build/three.min.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/controls/OrbitControls.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/VertexNormalsHelper.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/VertexTangentsHelper.js\" ></script>");

                if (input.AmbientOcclusion.HasAO)
                {
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/EffectComposer.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/shaders/CopyShader.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/ShaderPass.js\" ></script>");

                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/SSAOPass.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/math/SimplexNoise.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/shaders/SAOShader.js\" ></script>");
                }

                if (input.Outline.HasOutline) output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/effects/OutlineEffect.js\" ></script>");
                if (input.Environment.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
                {
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lights/LightProbeGenerator.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/LightProbeHelper.js\" ></script>");
                }
            }
            output.AppendLine("<script type=\"text/javascript\" src=\"app.js\"></script>");
            output.AppendLine("</body>");
            output.AppendLine("</html>");

            return output.ToString();
        }

        public static string ToJavascript(this Scene input)
        {
            Dictionary<string, string> maps = new Dictionary<string, string>();
            StringBuilder output = new StringBuilder();
            bool hasMaterial = false;

            output.AppendLine("const scene = new THREE.Scene();");

            output.AppendLine(input.Environment.ToJavascript());
            if (input.Atmosphere.HasFog) output.AppendLine(input.Atmosphere.ToJavascript());

            output.AppendLine("const renderer = new THREE.WebGLRenderer({ antialias: true });");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("const gradFormat = ( renderer.capabilities.isWebGL2 ) ? THREE.RedFormat : THREE.LuminanceFormat;");
            if (input.HasShadows)
            {
                output.AppendLine("renderer.shadowMap.enabled = true;");
                output.AppendLine("renderer.shadowMap.type = THREE.PCFSoftShadowMap;");
            }
            output.AppendLine("document.body.appendChild(renderer.domElement);");

            output.AppendLine(input.Grid.ToJavascript());
            output.AppendLine(input.Axes.ToJavascript());

            System.Security.Cryptography.SHA256 crypt = System.Security.Cryptography.SHA256.Create();

            foreach (Model model in input.Models)
            {
                if (model.IsMesh)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        if (model.Material.Maps[j] != null)
                        {
                            string key = model.Material.Maps[j].GetHash(crypt);
                            if (!maps.ContainsKey(key))
                            {
                                string name = "map" + maps.Count;
                                maps.Add(key, name);
                                output.AppendLine(model.Material.Maps[j].ToJsObjMap(name));
                            }
                            model.Material.MapNames[j] = maps[key];
                        }
                    }
                }
            }

            int i = 0;
            Rg.BoundingBox bbox = Rg.BoundingBox.Unset;
            foreach (Model model in input.Models)
            {
                string index = i.ToString();
                if (model.IsMesh)
                {
                    if (model.Material.MaterialType == Material.Types.None)
                    {
                        if (model.Mesh.VertexColors.Count == model.Mesh.Vertices.Count)
                        {
                            output.AppendLine("const material" + index + " = new THREE.MeshBasicMaterial( {vertexColors: THREE.VertexColors, side: THREE.DoubleSide} );");
                            hasMaterial = true;
                        }
                    }
                }

                if (model.IsCurve)
                {
                    output.AppendLine(model.Curve.ToJavascript(index));
                    bbox.Union(model.Curve.GetBoundingBox(false));

                    output.Append(model.Tangents.ToJavascript(index));
                    output.AppendLine("scene.add(line" + index + ");");
                }
                else
                {

                    if (!hasMaterial) output.AppendLine(model.Material.ToJavascript(index));

                    if (model.IsPlane) output.Append(model.Plane.ToJavascript(model.Size, index));

                    if (model.IsMesh)
                    {
                        output.Append(model.Mesh.ToJavascript(index));
                        bbox.Union(model.Mesh.GetBoundingBox(false));
                    }

                    if (input.HasShadows)
                    {
                        output.AppendLine("model" + index + ".castShadow = true;");
                        if (model.Material.MaterialType != Material.Types.Toon) output.AppendLine("model" + index + ".receiveShadow = true;");
                    }

                    output.AppendLine("scene.add(model" + index + ");");
                    if (model.HasHelper)
                    {
                        output.AppendLine("const bbox" + index + " = new THREE.BoxHelper( model" + index + ", " + model.HelperColor.ToJs() + " );");
                        output.AppendLine("scene.add( bbox" + index + " );");
                    }

                    if (model.IsMesh) output.Append(model.Normals.ToJavascript(index));

                }
                    i++;
            }

            if (input.Camera.IsDefault)
            {
                input.Camera.Position = bbox.Max + bbox.Diagonal;
                input.Camera.Target = bbox.Center;
            }

            output.AppendLine(input.Camera.ToJavascript());

            i = 0;
            int lightCount = 16;
            if (input.Lights.Count < 16) lightCount = input.Lights.Count;
            foreach (Light light in input.Lights)
            {
                string index = i.ToString();
                output.AppendLine(light.ToJavascript(index));
                if (light.HasShadow) output.Append(light.ToJsShadow(index, bbox));
                output.AppendLine("scene.add(light" + index + ");");
                i++;
            }

            output.Append(input.AmbientOcclusion.ToJavascript());
            output.AppendLine(input.Outline.ToJavascript());

            output.AppendLine("window.addEventListener('resize', onWindowResize);");
            output.AppendLine("function onWindowResize()");
            output.AppendLine("{");
            output.AppendLine("camera.aspect = window.innerWidth / window.innerHeight;");
            output.AppendLine("camera.updateProjectionMatrix();");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("}");

            output.AppendLine("controls = new THREE.OrbitControls (camera, renderer.domElement);");
            output.AppendLine("controls.target.set(" + input.Camera.Target.ToJs() + ");");

            output.AppendLine("const animate = function () {");
            output.AppendLine("controls.update();");
            output.AppendLine("requestAnimationFrame ( animate ); ");
            output.AppendLine("renderer.render (scene, camera);");

            if (input.AmbientOcclusion.HasAO) output.AppendLine("composer.render();");
            if (input.Outline.HasOutline) output.AppendLine("outline.render(scene,camera);");

            output.AppendLine("};");
            output.AppendLine("animate();");

            return output.ToString();
        }

        //public static string ToJsVertexMaterial(this Rg.Mesh mesh, string name)
        //{
        //    StringBuilder output = new StringBuilder();

        //    string starter = "const material" + name + " = new THREE.MeshBasicMaterial({ vertexColors: THREE.VertexColors });";

        //    return output.ToString();
        //}

        public static string ToJsShadow(this Light input, string index, Rg.BoundingBox bbox)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            int radius = (int)(bbox.Diagonal.Length);
            switch (input.LightType)
            {
                case Light.Types.Directional:
                case Light.Types.Spot:
                case Light.Types.Point:
                    output.AppendLine(name + ".castShadow = true;");

                    output.AppendLine(name + ".shadow.camera.left = -" + radius + ";");
                    output.AppendLine(name + ".shadow.camera.right = " + radius + ";");
                    output.AppendLine(name + ".shadow.camera.top = " + radius + ";");
                    output.AppendLine(name + ".shadow.camera.bottom = " + -radius + ";");

                    output.AppendLine(name + ".shadow.camera.near = 0.01;");
                    output.AppendLine(name + ".shadow.camera.far = " + (int)Math.Ceiling(input.Position.DistanceTo(bbox.Center) * 1.5 + radius) * 10 + ";");

                    output.AppendLine(name + ".shadow.mapSize = new THREE.Vector2(" + radius * 20 + "," + radius * 20 + ");");
                    break;

            }
            return output.ToString();
        }

        public static string ToJavascript(this Outline input)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasOutline)
            {
                output.AppendLine("let outline = new THREE.OutlineEffect(renderer)");
            }

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
                output.AppendLine("ssaoPass.maxDistance = " + input.MaxDistance + ";");
                output.AppendLine("composer.addPass( ssaoPass );");
            }

            return output.ToString();
        }

        //public static string ToJavascript(this Ground input)
        //{
        //    StringBuilder output = new StringBuilder();

        //    output.AppendLine("var groundPlane = new THREE.Mesh(new THREE.PlaneBufferGeometry(" + input.Size + "," + input.Size + "), materialGround);");
        //    output.AppendLine("groundPlane.position.y = " + input.Height + ";");
        //    output.AppendLine("groundPlane.rotation.x = - Math.PI / 2;");
        //    output.AppendLine("groundPlane.receiveShadow = true;");
        //    output.AppendLine("scene.add(groundPlane);");

        //    return output.ToString();
        //}

        public static string ToJavascript(this Atmosphere input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("scene.fog = new THREE.FogExp2( " + input.Color.ToJs() + " , " + input.Density / 1000.00 + " );");

            return output.ToString();
        }

        public static string ToJsObjMap(this Sd.Bitmap input, string name)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("var " + name + " = new THREE.TextureLoader().load(\"data:image/png;base64," + input.ToJs() + "\");");
            output.AppendLine(name + ".mapping = THREE.UVMapping;");


            return output.ToString();

        }

        public static string ToJavascript(this CubeMap input)
        {
            StringBuilder output = new StringBuilder();

            StringBuilder images = new StringBuilder();
            images.Append("\"data:image/png;base64," + input.PosX.ToJs() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegX.ToJs() + "\", ");
            images.Append("\"data:image/png;base64," + input.PosY.ToJs() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegY.ToJs() + "\", ");
            images.Append("\"data:image/png;base64," + input.PosZ.ToJs() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegZ.ToJs() + "\"");

            output.AppendLine("let lightProbe = new THREE.LightProbe();");
            output.AppendLine("scene.add( lightProbe );");

            output.AppendLine("new THREE.CubeTextureLoader().load([" + images.ToString() + "], function ( cubeTexture ) {");
            output.AppendLine("cubeTexture.encoding = THREE.sRGBEncoding;");

            output.AppendLine("lightProbe.copy( THREE.LightProbeGenerator.fromCubeTexture( cubeTexture ) );");
            output.AppendLine("});");


            return output.ToString();
        }

        public static string ToJavascript(this Environment input)
        {
            StringBuilder output = new StringBuilder();

            switch (input.EnvironmentMode)
            {
                default:
                    output.AppendLine("scene.background = new THREE.Color(" + input.Background.ToJs() + ");");
                    break;
                case Environment.EnvironmentModes.Environment:
                    output.AppendLine("var envMap = new THREE.TextureLoader().load(\"data:image/png;base64," + input.EnvMap.ToJs() + "\");");
                    output.AppendLine("envMap.mapping = THREE.EquirectangularReflectionMapping;");
                    if (input.IsBackground) output.AppendLine("scene.background = envMap;");
                    if (input.IsEnvironment) output.AppendLine("scene.environment = envMap;");
                    break;
                case Environment.EnvironmentModes.CubeMap:
                    StringBuilder images = new StringBuilder();
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosX.ToJs() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegX.ToJs() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosY.ToJs() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegY.ToJs() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosZ.ToJs() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegZ.ToJs() + "\"");

                    output.AppendLine("let lightProbe = new THREE.LightProbe();");
                    output.AppendLine("scene.add( lightProbe );");

                    output.AppendLine("new THREE.CubeTextureLoader().load([" + images.ToString() + "], function ( envMap ) {");
                    output.AppendLine("envMap.encoding = THREE.sRGBEncoding;");

                    output.AppendLine("lightProbe.copy( THREE.LightProbeGenerator.fromCubeTexture( envMap ) );");
                    if (input.IsBackground) output.AppendLine("scene.background = envMap;");
                    if (input.IsEnvironment) output.AppendLine("scene.environment = envMap;");
                    output.AppendLine("});");
                    break;
            }
            if (input.EnvironmentMode != Environment.EnvironmentModes.Color)
            {
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

            if (input.Show)
            {
                output.AppendLine("var grid = new THREE.GridHelper(" + input.Size + "," + input.Divisions + "," + input.AxisColor.ToJs() + "," + input.GridColor.ToJs() + ");");
                output.AppendLine("scene.add(grid);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const camera = new THREE.PerspectiveCamera(" + input.FOV + ", window.innerWidth / window.innerHeight, " + input.Near + "," + input.Far + ");");
            output.AppendLine("camera.position.set(" + input.Position.ToJs() + ");");
            output.AppendLine("camera.lookAt (new THREE.Vector3(" + input.Target.X + "," + input.Target.Z + "," + input.Target.Y + "));");

            return output.ToString();
        }

        public static string ToJavascript(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();
            string starter = "const material" + index + " = new THREE.";
            switch (input.MaterialType)
            {
                default:
                    output.AppendLine(starter + "MeshBasicMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Lambert:
                    output.AppendLine(starter + "MeshLambertMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Phong:
                    output.AppendLine(starter + "MeshPhongMaterial( { "
                        + input.ToJsDiffuse(index)
                        + ", shininess: " + (input.Shininess * 100.0)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsNormal(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Standard:
                    output.AppendLine(starter + "MeshStandardMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsMetalness(index)
                        + input.ToJsNormal(index)
                        + input.ToJsRoughness(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Physical:
                    output.AppendLine(starter + "MeshPhysicalMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsClearcoat(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsMetalness(index)
                        + input.ToJsNormal(index)
                        + input.ToJsRoughness(index)
                        + input.ToJsSheen(index)
                        + input.ToJsOpacity(index)
                        + ", reflectivity: " + (input.Reflectivity)
                        + " } );");
                    break;
                case Material.Types.Toon:
                    output.AppendLine("const gradSer" + index + " = new Uint8Array( [" + String.Join(",", input.Steps) + "] );");
                    output.AppendLine("const gradMap" + index + " = new THREE.DataTexture( gradSer" + index + ", gradSer" + index + ".length, 1, gradFormat );");
                    output.AppendLine("gradMap" + index + ".needsUpdate = true;");
                    output.AppendLine(starter + "MeshToonMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsNormal(index)
                        + input.ToJsOpacity(index)
                        + ",  gradientMap:gradMap" + index
                        + " } );");
                    break;
                case Material.Types.Shadow:
                    output.AppendLine(starter + "ShadowMaterial( { transparent:true, color: " + input.DiffuseColor.ToJs() + " } );");
                    break;
                case Material.Types.Normal:
                    output.AppendLine(starter + "MeshNormalMaterial();");
                    break;
                case Material.Types.Depth:
                    output.AppendLine(starter + "MeshDepthMaterial();");
                    break;
            }

            if (input.IsWireframe) output.AppendLine("material" + index + ".wireframe = true;");
            if (input.IsFlatShaded) output.AppendLine("material" + index + ".flatShading = true;");

            return output.ToString();
        }

        public static string ToJsClearcoat(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasClearcoat) output.Append(", clearcoat : " + input.Clearcoat);
            if (input.HasClearcoatMap) output.Append(", clearcoatMap : " + input.ClearCoatMapName);
            if (input.HasClearcoatRoughness) output.Append(", clearcoatRoughness : " + input.ClearcoatRoughness);
            if (input.HasClearcoatRoughnessMap) output.Append(", clearcoatRoughnessMap : " + input.ClearCoatRoughnessMapName);
            if (input.HasClearcoatNormalMap) output.Append(", clearcoatNormalMap : " + input.ClearCoatNormalMapName);

            return output.ToString();
        }

        public static string ToJsBump(this Material input, string index)
        {
            if (input.HasBumpMap) return ", bumpMap : " + input.BumpMapName + ", bumpScale : " + input.BumpIntensity;
            return string.Empty;
        }

        public static string ToJsDiffuse(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            output.Append("color: " + input.DiffuseColor.ToJs());
            if (input.HasTextureMap) output.Append(", map : " + input.TextureMapName);

            return output.ToString();
        }

        public static string ToJsDisplacement(this Material input, string index)
        {
            if (input.HasDisplacement) return ", displacementMap : " + input.DisplacementMapName + ", displacementScale : " + input.DisplacementScale;
            return string.Empty;
        }

        public static string ToJsMetalness(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.IsMetalic) output.Append(", metalness : " + input.Metalness);
            if (input.HasMetalicMap) output.Append(", metalnessMap : " + input.MetalnessMapName);

            return output.ToString();
        }

        public static string ToJsNormal(this Material input, string index)
        {
            if (input.HasNormalMap) return ", normalMap : " + input.NormalMapName + ", normalScale : " + input.NormalScale + ", normalMapType : THREE.ObjectSpaceNormalMap";
            return string.Empty;
        }

        public static string ToJsRoughness(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasRoughness) output.Append(", roughness : " + input.Roughness);
            if (input.HasRoughnessMap) output.Append(", roughnessMap : " + input.RoughnessMapName);

            return output.ToString();
        }

        public static string ToJsSheen(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasSheen) output.Append(", sheen : " + input.Sheen + ", sheenColor : " + input.SheenColor.ToJs());
            if (input.HasSheenMap) output.Append(", sheenColorMap : " + input.SheenColorMapName);
            if (input.HasSheenRoughness) output.Append(", sheenRoughness : " + input.SheenRoughness);
            if (input.HasSheenRoughnessMap) output.Append(", sheenRoughnessMap : " + input.SheenRoughnessMapName);

            return output.ToString();
        }

        public static string ToJsEmissive(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.IsEmissive) output.Append(", emissive : " + input.EmissiveColor.ToJs() + ", emissiveIntensity : " + input.EmissiveIntensity);
            if (input.HasEmissiveMap) output.Append(", emissiveMap : " + input.EmissiveMapName);

            return output.ToString();
        }

        public static string ToJsOpacity(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            output.Append(", opacity : " + input.DiffuseColor.ToJsOpacity());
            output.Append(", transparent: " + input.IsTransparent.ToString().ToLower());
            if (input.HasOpacityMap) output.Append(", alphaMap : " + input.OpacityMapName);


            return output.ToString();
        }

        public static string ToJavascript(this Light input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            string starter = "const " + name + " = new THREE.";
            string hName = "h" + name;
            string helperStart = "const " + hName + " = new THREE.";
            switch (input.LightType)
            {
                default:
                    output.AppendLine(starter + "AmbientLight(" + input.Color.ToJs() + ", " + input.Intensity + " );");
                    break;
                case Light.Types.Point:
                    output.AppendLine(starter + "PointLight(" + input.Color.ToJs() + ", " + input.Intensity + ", " + input.Distance + ", " + input.Decay + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    if (input.HasHelper) output.AppendLine(helperStart + "PointLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToJs() + " );");
                    break;
                case Light.Types.Directional:
                    output.AppendLine(starter + "DirectionalLight(" + input.Color.ToJs() + ", " + input.Intensity + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    if (input.HasHelper) output.AppendLine(helperStart + "DirectionalLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToJs() + " );");
                    break;
                case Light.Types.Hemisphere:
                    output.AppendLine(starter + "HemisphereLight(" + input.Color.ToJs() + ", " + input.ColorB.ToJs() + ", " + input.Intensity + " );");
                    if (input.HasHelper) output.AppendLine(helperStart + "HemisphereLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToJs() + " );");
                    break;
                case Light.Types.Spot:
                    output.AppendLine(starter + "SpotLight(" + input.Color.ToJs() + ", " + input.Intensity + ", " + input.Distance + ", " + input.Angle + ", " + input.Penumbra + ", " + input.Decay + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    if (input.HasHelper) output.AppendLine(helperStart + "SpotLightHelper(" + name + ", " + input.HelperColor.ToJs() + " );");
                    break;
            }
            if (input.HasHelper) output.AppendLine("scene.add(" + hName + ");");
            return output.ToString();
        }

        public static string ToJavascript(this Rg.Plane input, double size, string index)
        {

            StringBuilder output = new StringBuilder();

            string name = "model" + index;
            output.AppendLine("var " + name + " = new THREE.Mesh(new THREE.PlaneBufferGeometry(" + size + "," + size + "), material" + index + ");");
            output.AppendLine(name + ".position.y = " + input.Origin.Z + ";");
            output.AppendLine(name + ".rotation.x = - Math.PI / 2;");
            output.AppendLine(name + ".receiveShadow = true;");
            output.AppendLine("scene.add(" + name + ");");

            return output.ToString();
        }

        public static string ToJavascript(this Rg.NurbsCurve input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "curve" + index;

            output.AppendLine("const " + name + " = new THREE.BufferGeometry();");
            output.AppendLine("const lineMat"+index+" = new THREE.LineBasicMaterial({ color: 0x0000ff });");

            int count = input.Points.Count;
            var points = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);
            Parallel.For(0, count, k =>
            {
                points[k] = input.Points[k].Location.ToJs();
            }
            );

            output.AppendLine("const positions" + index + " = new Float32Array( [" + string.Join(",", points.Values) + "] );");
            output.AppendLine(name + ".setAttribute( 'position', new THREE.BufferAttribute( positions" + index + ", 3 ) );");
            output.AppendLine("const line"+index+" = new THREE.Line( "+name+", lineMat" + index +" );");

            return output.ToString();
        }

        public static string ToJavascript(this Rg.Mesh input, string index)
        {
            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();
            if (input.Normals.Count < 1) input.RebuildNormals();

            bool hasUV = (input.TextureCoordinates.Count > 0);
            bool hasColor = (input.VertexColors.Count == input.Vertices.Count);

            StringBuilder output = new StringBuilder();
            string name = "mesh" + index;

            output.AppendLine("const " + name + " = new THREE.BufferGeometry();");

            int count = input.Faces.Count;
            var vertices = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            var normals = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            var uvs = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);
            var colors = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count * 3);


            Parallel.For(0, count, k =>
            {
                Rg.MeshFace face = input.Faces[k];

                vertices[k * 3] = input.Vertices[face.A].ToJs();
                normals[k * 3] = input.Normals[face.A].ToJs();

                vertices[k * 3 + 1] = input.Vertices[face.C].ToJs();
                normals[k * 3 + 1] = input.Normals[face.C].ToJs();

                vertices[k * 3 + 2] = input.Vertices[face.B].ToJs();
                normals[k * 3 + 2] = input.Normals[face.B].ToJs();
            }
            );

            if (hasUV)
            {
                Parallel.For(0, count, k =>
                {
                    Rg.MeshFace face = input.Faces[k];
                    uvs[k * 3] = input.TextureCoordinates[face.A].ToJs();
                    uvs[k * 3 + 1] = input.TextureCoordinates[face.C].ToJs();
                    uvs[k * 3 + 2] = input.TextureCoordinates[face.B].ToJs();
                }
                );
            }

            if (hasColor)
            {
                Parallel.For(0, count, k =>
                {
                    Rg.MeshFace face = input.Faces[k];
                    colors[k * 3] = input.VertexColors[face.A].ToJsSet();
                    colors[k * 3 + 1] = input.VertexColors[face.C].ToJsSet();
                    colors[k * 3 + 2] = input.VertexColors[face.B].ToJsSet();
                }
                );
            }

            output.AppendLine("const vertices" + index + " = new Float32Array( [" + string.Join(",", vertices.Values) + "] );");
            output.AppendLine("const normals" + index + " = new Float32Array( [" + string.Join(",", normals.Values) + "] );");
            if (hasUV) output.AppendLine("const uv" + index + " = new Float32Array( [" + string.Join(",", uvs.Values) + "] );");
            if (hasColor) output.AppendLine("const clr" + index + " = new Uint8Array( [" + string.Join(",", colors.Values) + "] );");

            output.AppendLine(name + ".setAttribute( 'position', new THREE.BufferAttribute( vertices" + index + ", 3 ) );");
            output.AppendLine(name + ".setAttribute( 'normal', new THREE.BufferAttribute( normals" + index + ", 3 ) );");
            if (hasUV) output.AppendLine(name + ".setAttribute( 'uv', new THREE.BufferAttribute( uv" + index + ", 2 ) );");
            if (hasColor) output.AppendLine(name + ".setAttribute( 'color', new THREE.BufferAttribute( clr" + index + ", 3, true ) );");
            output.AppendLine("const model" + index + " = new THREE.Mesh( mesh" + index + ", material" + index + " );");
            return output.ToString();
        }

        #region conversions

        public static string ToJsTarget(this Rg.Point3d input, string name)
        {
            StringBuilder output = new StringBuilder();

            string targetObject = name + "T";
            output.AppendLine("const " + targetObject + "= new THREE.Object3D();");
            output.AppendLine(targetObject + ".position.set( " + input.ToJs() + " );");
            output.AppendLine("scene.add(" + targetObject + ");");
            output.AppendLine(name + ".target = " + targetObject + ";");
            output.AppendLine(name + ".target.updateMatrixWorld();");
            return output.ToString();
        }

        public static string ToJsOpacity(this Sd.Color value, int digits = 5)
        {
            return "'" + Math.Round(((double)value.A) / 255.0, digits) + "'";
        }

        public static string ToJs(this Sd.Color value)
        {
            return "'" + Sd.ColorTranslator.ToHtml(value).ToString() + "'";
        }

        public static string ToJs(this Rg.Point3d value, int digits = 5)
        {
            return Math.Round(value.X, digits) + "," + Math.Round(value.Z, digits) + "," + Math.Round(value.Y, digits);
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

        public static string ToJsSet(this Sd.Color value)
        {
            return value.R + "," + value.G + "," + value.B;
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
