using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenesChanger : MonoBehaviour
{
    public void GoTo(string sceneName)
    {
        //場景管理人.讀取場景(場景名稱);
       GameSystem.ToSceneByName(sceneName);
    }
    

    // 按下開始會執行一次:遊戲初始化
    void Start()
    {
        
    }

    // 遊戲執行中每一幀(1FPS)執行一次:更新遊戲狀態
    void Update()
    {
        
    }
}
