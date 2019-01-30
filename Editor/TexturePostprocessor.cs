#if UNITY_5
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TinyTools
{
    /// <summary>
    /// 贴图资源后处理程序
    /// </summary>
    public class TexturePostprocessor : AssetPostprocessor
    {
        private int m_Order = TinyToolsSettings.TexturePostprocessOrder;
        public override int GetPostprocessOrder()
        {
            return m_Order;
        }

        void OnPreprocessTexture()
        {
            return;
            TextureImporter textureImporter = assetImporter as TextureImporter;
            //　跳过精灵图集贴图
            if (textureImporter.textureType == TextureImporterType.Sprite)
                return;
            // 对除了精灵图集其他贴图进行处理
            if (textureImporter.alphaSource==TextureImporterAlphaSource.FromInput&&!TYUtility.IsTextureWithAlphaChannel(assetPath))
            {
                if (Path.GetExtension(assetPath).Equals(".tga", System.StringComparison.CurrentCultureIgnoreCase))
                    return;
                Debug.Log("[TinyTools]>[TexturePostprocessor]>[Convert ECT1]-->" + assetPath);
                TYUtility.ConvertNoAlphaTextureToECT1(textureImporter);
            }
        }
    }
}
#endif