#if UNITY_5
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CheckTextures : EditorWindow
{
    // 日志字符串
    private static string logStr = string.Empty;
    private static string[] convertTexGuids = null;
    private static int index = 0;

    [MenuItem("Assets/Tiny Tools/Check Textures")]
    public static void ConvertNoAlphaTextureToECT1Format()
    {
        if (Selection.assetGUIDs.Length == 0)
        {
            Debug.Log("Nothing textures to convert.");
            return;
        }

        index = 0;
        logStr = string.Empty;
        // 显示进度
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayProgressBar("Converting ECT1...", "", 0f);

        logStr += ("Work directory:" + AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]) + "\n");
        logStr += ("Converted Textures:\n");
        convertTexGuids = AssetDatabase.FindAssets("t:Texture", new string[] { AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]) });

        EditorApplication.update += InternalUpdate;
    }

    private static void InternalUpdate()
    {
        if (index == convertTexGuids.Length)
        {
            EditorApplication.update -= InternalUpdate;
            if (logStr.LastIndexOf("\n") > 0)
                logStr = logStr.Substring(0, logStr.LastIndexOf("\n"));
            // 显示日志
            Debug.Log("Worker:Convert No Alpha Textures To ECT1\n" + logStr);
            //
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Resources.UnloadUnusedAssets();
            EditorUtility.ClearProgressBar();
            return;
        }

        string guid = convertTexGuids[index];
        string path = AssetDatabase.GUIDToAssetPath(guid);
        string fullPath = string.Empty;
        fullPath = Application.dataPath + path.Substring(path.IndexOf("Assets") + ("Assets".Length));
        if (!File.Exists(fullPath))
        {
            index++;
            return;
        }

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (tex != null)
        {
            // 不带有透明通道
            // 暂时先跳过tga贴图
            if (!IsTextureWithAlphaChannel(fullPath) && !(Path.GetExtension(path).Equals(".tga", System.StringComparison.CurrentCultureIgnoreCase)))
            {
                EditorUtility.DisplayProgressBar("Converting ECT1...", path.Replace("Assets/", ""), index / (float)(convertTexGuids.Length - 1));
                ConvertNoAlphaTexToECT1(path);
                logStr += (path + "\n");
            }
            // 带有透明通道
            else
            {
                ConvertAlphaTexToXXX(path);
            }
        }
        // 
        index++;
    }

    private static bool IsTextureWithAlphaChannel(string fullPath)
    {
        FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        BinaryReader reader = new BinaryReader(fs);
        byte[] bytes = reader.ReadBytes((int)fs.Length);
        Texture2D tempTex2D = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath.Replace(Application.dataPath, "Assets"));
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

    private static void ConvertNoAlphaTexToECT1(string texPathPath)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(texPathPath) as TextureImporter;
        if (textureImporter == null) return;
        if (textureImporter.alphaSource == TextureImporterAlphaSource.None)
            return;
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

    private static void ConvertAlphaTexToXXX(string texPath)
    {

    }
}
#endif
