using UnityEngine;
using System.Collections;
using UnityEditor;



namespace TeaSoft
{
    /// <summary>
    /// 重新载入关卡
    /// 
    /// 脚本创建日期
    /// design by 顾文光
    /// </summary>
    /// 
    public class MenuForRestartCurrentScene : Editor
    {
        //% (ctrl on Windows, cmd on OS X), # (shift), & (alt), _ (no key modifiers). For example to create a menu with hotkey shift-alt-g use "GameObject/Do Something #&g". 
        [MenuItem("DEBUG/Restart Current Scene &q")]
        static void RestartCurrentScene()
        {
            if (Application.isPlaying)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
            }
        }
    }
}