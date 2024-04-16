using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIRumbleInfoCtrl : MonoBehaviour
{
    [Header("戰鬥單位資訊UI元件")]
    public Image iconRumbleImg;
    public TextMeshProUGUI eReqText;

    /// <summary>
    /// 是否有戰鬥單位資料檢查接口
    /// </summary>
    public bool noData
    {
        get 
        {
            return data.NoData();
        }
    }
    public bool NoData()
    {
        return data.NoData();
    }


    private Toggle _toggle;
    private Toggle toggle
    {
        get 
        {
            if(_toggle == null) _toggle = GetComponent<Toggle>();
            return _toggle;
        }
    }
    /// <summary>
    /// 戰鬥單位資料
    /// </summary>
    public RumbleData data { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// 使用戰鬥單位資料(使用後即刻清除)
    /// </summary>
    public void UseData()
    {
        //臨時宣告(取出結構內的參數)
        RumbleData data = this.data;
        //操作結構參數(清除)
        data.ClearData();
        //操作結果放回去
        this.data = data;
        //關閉
        toggle.isOn = false;
    }


    /// <summary>
    /// 更新單位資料到UI控制器
    /// </summary>
    /// <param name="rumbleData">戰鬥單位資料</param>
    public void UpdateUI(RumbleData rumbleData)
    {
        data = rumbleData;//資料紀錄
        toggle.isOn = false;//預設不選
        //背景底圖(BackGround:設為灰)
        toggle.image.sprite = data.rumbleIcon;
        //選取圖(CheckMark:設為白)
        iconRumbleImg.sprite = data.rumbleIcon;
        //更新能量需求資訊
        eReqText.text = data.rumbleReq.ToString();
    }

    /// <summary>
    /// 能量是否足夠派遣(解鎖UI)
    /// </summary>
    public void LockUI()
    {
        //介面鎖定(外部設定變紅:能量不足)
        toggle.interactable = GameSystem.energy >= data.rumbleReq;
        //Icon鎖定(變暗)
        /*toggle.graphic.color = GameSystem.energy >= data.rumbleReq ?
            Color.white : Color.gray;*/
    }

    public void SelectRumble(bool B)
    {
        if (B)
        {//UI被點亮:紀錄選取的戰鬥單位資料UI
            GameSystem.SelectRumble(this);
        }
        else
        {//UI被熄滅:取消紀錄選取的戰鬥單位資料UI
            GameSystem.UnSelectRumble();
        }
    }

}
