using UnityEngine;

namespace UnityScriptExtension
{
    /// <summary>
    /// GameObject原始类型扩展
    /// </summary>
    public static class GameObjectExtension
    {
        /// <summary>
        /// 设置层（包括激活或未激活子节点）
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="layerName">层名</param>
        public static void SetLayerIncludeChildren(this GameObject target, string layerName)
        {
            target.layer = LayerMask.NameToLayer(layerName);
            Transform[] childs = target.GetComponentsInChildren<Transform>(true);
            int length = childs.Length;
            for (int i = 0; i < length; i++)
            {
                Transform child = childs[i];
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }

        /// <summary>
        /// 保证游戏在未激活的情况下被激活,防止在激活状态下反复被激活
        /// </summary>
        /// <param name="target">游戏对象</param>
        /// <param name="value">激活对象或者使对象无效</param>
        public static void SetActiveOptimized(this GameObject target, bool value)
        {
            if (!target.activeInHierarchy & value)
                target.SetActive(value);
            else if (target.activeInHierarchy & !value)
                target.SetActive(value);
        }
    }
}