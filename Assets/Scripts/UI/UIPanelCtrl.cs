using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//綁定同時需求的元件
[RequireComponent(typeof(CanvasGroup))]
public class UIPanelCtrl : MonoBehaviour
{

    private CanvasGroup CG;

    public bool isOn { get; private set; }

    //腳本啟動
    private void OnEnable()
    {
        //抓取 CanvasGroup 控制
        CG = GetComponent<CanvasGroup>();
        //預設介面關閉
        Switch(false);
    }

    /// <summary>
    /// 面板開關控制
    /// </summary>
    /// <param name="B">開:true / 關:false</param>
    public void Switch(bool B)
    {
        isOn = B;
        //如果(開)
        if (isOn)
        {
            CG.alpha = 1;
            CG.blocksRaycasts = true;
        }
        else //否則
        {
            CG.alpha = 0;
            CG.blocksRaycasts = false;
        }
    }
   
}
