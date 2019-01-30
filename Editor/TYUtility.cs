#if UNITY_5
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TinyTools
{
    public class TYUtility
    {
        public static bool IsTextureWithAlphaChannel(string assetPath)
        {
            FileStream fs = new FileStream(Application.dataPath + assetPath.Replace("Assets", ""), FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(fs);
            byte[] bytes = reader.ReadBytes((int)fs.Length);
            Texture2D tempTex2D = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (tempTex2D == null)
                return false;
            Texture2D _tex = new Texture2D(tempTex2D.width, tempTex2D.height);
            _tex.LoadImage(bytes);
            Resources.UnloadAsset(tempTex2D);

            for (int i = 0; i < _tex.width; ++i)
                for (int j = 0; j < _tex.height; ++j)
                {
                    Color color = _tex.GetPixel(i, j);
                    float alpha = color.a;
                    if (alpha < 1.0f - 0.001f)
                    {
                        return true;
                    }
                }
            return false;
        }

        public static void ConvertNoAlphaTextureToECT1(TextureImporter textureImporter)
        {
            textureImporter.alphaSource = TextureImporterAlphaSource.None;
            //TextureImporterPlatformSettings tips = new TextureImporterPlatformSettings();
            //tips.name = "Android";
            ////tips.overridden = true;
            //tips.maxTextureSize = 2048;
            //tips.format = TextureImporterFormat.ETC_RGB4;
            //tips.compressionQuality = (int)TextureCompressionQuality.Normal;
            //textureImporter.SetPlatformTextureSettings(tips);
            textureImporter.SaveAndReimport();
        }
    }
}
#endif