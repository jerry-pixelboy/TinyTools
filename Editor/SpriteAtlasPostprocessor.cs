#if UNITY_5
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TinyTools
{
    /// <summary>
    /// 精灵图集后处理程序
    /// </summary>
    class SpriteAtlasPostprocessor : AssetPostprocessor
    {
        private int m_Order = TinyToolsSettings.SpriteAtlasPostprocessOrder;
        public override int GetPostprocessOrder()
        {
            return m_Order;
        }

        void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter.textureType == TextureImporterType.Sprite && textureImporter.spriteImportMode == SpriteImportMode.Multiple)
            {
                textureImporter.spritePackingTag = "pt_" + Path.GetFileNameWithoutExtension(assetPath);
                textureImporter.alphaIsTransparency = false;
                textureImporter.mipmapEnabled = false;
                Debug.Log("[TinyTools]>[SpriteAtlasPostprocessor]-->" + assetPath);
            }
        }
    }
}
#endif