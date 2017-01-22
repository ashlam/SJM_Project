using UnityEngine;
using System.Collections;

namespace TeaSoft
{
    /// <summary>
    /// me在游戏编写过程中可能会经常用到的一些函数
    /// 
    /// 2014-11-20
    /// design by 顾文光
    /// </summary>
    public class GameHelper
    {
        /// <summary>
        /// 设置某个GameObject的开关
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isActive"></param>
        internal static void SwtichGameObjectActive(GameObject go, bool isActive)
        {
            if (go != null)
            {
                go.SetActive(isActive);
            }
        }

        /// <summary>
        /// 设置某个组件所在的GameObject的开关
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="isActive"></param>
        internal static void SwtichGameObjectActiveByPart(Component comp, bool isActive)
        {
            if (comp != null && comp.gameObject != null)
            {
                SwtichGameObjectActive(comp.gameObject, isActive);
            }
        }

        /// <summary>
        /// 去掉图片后缀名（一般用于UISprite）
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        internal static string ConvertImageName(string fullname)
        {
            //throw new System.NotImplementedException();
            return System.IO.Path.GetFileNameWithoutExtension(fullname);
        }


        /// <summary>
        /// 删除某节点下的所有子物体
        /// </summary>
        /// <param name="transform"></param>
        internal static void DestoryChildUnderTrans(Transform transform)
        {
            //throw new System.NotImplementedException();
            if (null != transform)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}