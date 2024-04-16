using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpPanelCtrl : MonoBehaviour
{
    [Header("下拉選單動畫控制器")]
    public Animator aniDropDown;
    [Header("計時器文字")]
    public TextMeshProUGUI textTimer;
    [Header("敵方HP BAR")]
    public Image barP2;
    [Header("我方HP BAR")]
    public Image barP1;

    private float interval = 0;
    private int timer
    {

        get
        {
            return GameSystem.timer;
        }
        set
        {
            GameSystem.timer = value;
        }
    }
    private string timeStr
    {
        get
        {
            //回傳 時間顯示 timer除60  = > 的商:餘數
            return $"{(timer / 60).ToString("F0")}:{(timer % 60).ToString("00")}";
        }
    }

    private bool lose
    {
        get
        {
            return timer <= 0;
        }
    }

    private void OnEnable()
    {
        GameSystem.SetUpPanelCtrl(this);
        //更新計時文字
        textTimer.text = timeStr;
    }

    private void OnDisable()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSystem.inBattle) return;
        UPdateTimer();
    }

    /// <summary>
    /// 玩家主堡HP HUD(抬頭顯示UI)
    /// </summary>
    /// <param name="val">百分點</param>
    public void HpBarCtrlP1(float val)
    {
        barP1.fillAmount = val;
        //PLAYER HP 歸零
        if (barP1.fillAmount <= 0) GameSystem.GameOverInfo();
    }

    /// <summary>
    /// 敵方主堡HP HUD(抬頭顯示UI)
    /// </summary>
    /// <param name="val">百分點</param>
    public void HpBarCtrlP2(float val)
    {
        barP2.fillAmount = val;
        //Boss HP 歸零(獲勝)
        if (barP2.fillAmount <= 0)
        {
            GameSystem.GameOverInfo();                   
        }
    }

    /// <summary>
    /// 更新計時器
    /// </summary>
    void UPdateTimer()
    {
        if (!lose && interval >= 1)
        {//時間未結束 && 間隔1秒
            timer--;//計時倒數
            interval = 0;//重置間隔
            //計時器歸零(時間到)
            if (timer <= 0) GameSystem.GameOverInfo();
        }
        else
        {//計算間隔時間
            interval += Time.deltaTime;
        }
        //更新計時器文字
        textTimer.text = timeStr;
    }

    public void DropDownSwitch(bool B)
    {
        //遊戲全體時間總控制(開關 ? 暫停 : 1倍數)撥放
        Time.timeScale = B ? 0 : 1;
        //播放動畫時重 (開關 ? 頭 : 尾 ) 播放
        aniDropDown.Play("UpPanel", 0, B ? 0 : 1);
        //播放動畫時速度 (開關 ? 正 : 逆 ) 播放
        aniDropDown.SetFloat("Speed", B ? 2 : -5);       
    }

    /// <summary>
    /// 重載關卡
    /// </summary>
    public void ReloadStage()
    {
        Time.timeScale = 1;
        GameSystem.ReloadScene();
        GameSystem.ClearTargetSystem();
    }
    /// <summary>
    /// 離開關卡
    /// </summary>
    public void ExitStage()
    {
        Time.timeScale = 1;
        GameSystem.ToSceneByName("Menu");
        GameSystem.ClearTargetSystem();
    }
}
