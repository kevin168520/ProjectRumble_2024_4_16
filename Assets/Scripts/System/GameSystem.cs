using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 背景運行：靜態資料管理中心
/// </summary>
public static class GameSystem
{
    #region PlayerInfo
    /// <summary>
    /// 當前等級
    /// </summary>
    private static int _level = 1;
    public static int level 
    {
        get
        {
            return _level;
        }     
    }
    /// <summary>
    /// 取得的總經驗
    /// </summary>
    public static int expTotal { get; private set; }
    /// <summary>
    /// 顯示用的經驗分子
    /// </summary>
    public static int exp 
    { 
        get
        {
            int expBefore = 0;
            for (int i = 0; i < _level; i++)
            {
                expBefore += i * 10;
            }

            return expTotal  - expBefore;
        }
    }
    /// <summary>
    /// 顯示用的經驗分母
    /// </summary>
    public static int expMax 
    {
        get
        {
            return _level * 10;
        }
    }
    /// <summary>
    /// UI用經驗百分點
    /// </summary>
    public static float expFillAmount
    {
        get
        {
            return (float)exp / (float)expMax; 
        }
    }
    /// <summary>
    /// 控制經驗值
    /// </summary>
    /// <param name="val">經驗數據</param>
    public static void ExpCtrl(int val)
    {
        expTotal += val;
        if (exp >= expMax)
        {//經驗分子大於等於分母 : 升級
            _level++;
        }
    }

    /// <summary>
    /// 關卡里程轉字串
    /// </summary>
    public static string lvMileageStr
    {
        get
        {//顯示時減 1 顯示 : 已達成的部分
            return (lvMileage - 1).ToString();
        }
    }

    /// <summary>
    /// 持有金錢
    /// </summary>
    public static int money { get; private set; }
    /// <summary>
    /// 金錢控制
    /// </summary>
    /// <param name="val">控制數據</param>
    public static void MoneyCtrl(int val)
    {
        //錢不夠(負val觸發)
        if (money + val < 0) return;
        money += val;
    }
    #endregion PlayerInfo

    #region StageManager
    /// <summary>
    /// 關卡里程(進度 : 當前數字是正在攻略的關卡)
    /// </summary>
    public static int lvMileage = 1;
    /// <summary>
    /// 當前舞台索引
    /// </summary>
    private static int currentStageIndex
    {
        get
        {//關卡里程 除 5 + 1
            return lvMileage / 5 + 1;
        }
    }
    /// <summary>
    /// 當前關卡索引
    /// </summary>
    private static int currentLevelIndex
    {
        get
        {//關卡里程除 5 取餘數
            return lvMileage % 5;
        }
    }
    /// <summary>
    /// 獲勝時呼叫檢查
    /// </summary>
    public static void CheckLvMileage()
    {//選取的里程是否為進程
        if (selectLvMileage >= lvMileage) lvMileage++;
    }
    /// <summary>
    /// 隱藏的實體位置
    /// </summary>
    private static StageManager _stageManager;
    /// <summary>
    /// 對外公開接口(唯讀)
    /// </summary>
    public static StageManager stageManager
    {
        get { return _stageManager; }
    }
    public static void SetStageManager(StageManager manager)
    {
        _stageManager = manager;
        _stageManager.SetGPS(currentStageIndex, currentLevelIndex);
    }
    #endregion StageManager

    #region UIStageMenu
    /// <summary>
    /// 選單開啟中的 Stage 編號
    /// </summary>
    private static int selectStageIndex;
    /// <summary>
    /// 點選開啟中 Stage 的 Level
    /// </summary>
    private static int selectLevelIndex;
    public static int selectLvMileage
    {
        get
        {
            return (selectStageIndex - 1) * 5 + selectLevelIndex + 1;
        }
    }

    /// <summary>
    /// stageMenuCtrl 實體控制腳本
    /// </summary>
    private static UIStageMenuCtrl stageMenuCtrl;
    /// <summary>
    /// stageMenuCtrl 設定功能
    /// </summary>
    /// <param name="ctrl">控制器</param>
    public static void SetStageMenuCtrl(UIStageMenuCtrl ctrl)
    {
        stageMenuCtrl = ctrl;
    }
    /// <summary>
    /// levelInfoCtrl 實體控制腳本
    /// </summary>
    private static UILevelInfoCtrl levelInfoCtrl;
    /// <summary>
    /// levelInfoCtrl 設定功能
    /// </summary>
    /// <param name="ctrl">控制器</param>
    public static void SetLevelInfoCtrl(UILevelInfoCtrl ctrl)
    {
        levelInfoCtrl = ctrl;
    }

    /// <summary>
    /// 更新 StageUI 內容，並且記錄開啟的 StageIndex
    /// </summary>
    /// <param name="stageIndex">關卡編號</param>
    public static void UpdateStageUI(int stageIndex)
    {
        selectStageIndex = stageIndex;
        stageMenuCtrl.UpdateMenu(stageManager.GetStageDB(selectStageIndex).levels);
        stageMenuCtrl.UpdateHeader($"地圖 {selectStageIndex}");
    }

    /// <summary>
    /// 選定 Level 
    /// </summary>
    /// <param name="levelIndex">Level 號碼</param>
    public static void SelectLevel(int levelIndex)
    {
        selectLevelIndex = levelIndex;
        //開啟 Level 面板(顯示 Level Info)
        levelInfoCtrl.UpdateInfo(stageManager.
            GetStageDB(selectStageIndex).levels[selectLevelIndex]);
    }

    public static void LevelStart()
    {
        ToSceneByName($"Stage{selectStageIndex}-{selectLevelIndex}");
    }
    #endregion UIStageMenu

    #region 場景切換管理
    /// <summary>
    /// 當前所在場景名稱(關卡名)
    /// </summary>
    public static string currentSceneName { get; private set; }
    /// <summary>
    /// 以場景名稱切換(關卡名)
    /// </summary>
    /// <param name="sceneName">場景名稱</param>
    public static void ToSceneByName(string sceneName)
    {
        currentSceneName = sceneName;
        //場景管理人.讀取場景(場景名稱);
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// 重載當前使用場景
    /// </summary>
    public static void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneName);
    }
    /// <summary>
    /// 和Stage綁定疊加
    /// </summary>
    public static void AddGameUI()
    {
        SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
    }
    #endregion

    #region 目標系統
    /// <summary>
    /// P1在場上的角色清單
    /// </summary>
    private static List<CharacterCtrl> listP1 = new List<CharacterCtrl>();
    /// <summary>
    /// P2在場上的角色清單
    /// </summary>
    private static List<CharacterCtrl> listP2 = new List<CharacterCtrl>();
    /// <summary>
    /// 將目標加入管理系統
    /// </summary>
    /// <param name="group">陣營分組</param>
    /// <param name="character">目標角色</param>
    public static void AddTarget(this PlayerGroup group, CharacterCtrl character)
    {
        switch (group)
        {//依照分組決定加入的清單
            case PlayerGroup.P1:
                listP1.Add(character);
                break;

            case PlayerGroup.P2:
                listP2.Add(character);
                break;
        }
    }
    /// <summary>
    /// 將目標移除管理系統
    /// </summary>
    /// <param name="group">陣營分組</param>
    /// <param name="character">目標角色</param>
    public static void RemoveTarget(this PlayerGroup group, CharacterCtrl character)
    {
        switch (group)
        {//依照分組決定加入的清單
            case PlayerGroup.P1:
                listP1.Remove(character);
                break;

            case PlayerGroup.P2:
                listP2.Remove(character);
                break;
        }
    }

    /// <summary>
    /// 取得敵方目標清單
    /// </summary>
    /// <param name="main">請求方</param>
    /// <returns>敵方目標清單</returns>
    public static List<CharacterCtrl> GetEnemyList(this CharacterCtrl main)
    {
        return main.getGroup == PlayerGroup.P1 ? listP2 : listP1;
    }
    /// <summary>
    /// 取得我方目標清單
    /// </summary>
    /// <param name="main">請求方</param>
    /// <returns>我方目標清單</returns>
    public static List<CharacterCtrl> GetTeamList(this CharacterCtrl main)
    {
        return main.getGroup == PlayerGroup.P2 ? listP2 : listP1;
    }
    /// <summary>
    /// 清除目標系統內的舊物件
    /// </summary>
    public static void ClearTargetSystem()
    {
        listP1.Clear(); listP2.Clear();
    }
    /// <summary>
    /// 取得敵方目標(距離最近)
    /// </summary>
    /// <param name="main">請求方</param>
    /// <param name="range">範圍</param>
    /// <returns>敵方目標</returns>
    public static CharacterCtrl SearchTarget(this CharacterCtrl main, float range)
    {
        float minRange = 999f;//預設距離
        CharacterCtrl target = null;
        CharacterCtrl closestTarget = null;
        for (int i = 0; i < main.GetEnemyList().Count; i++)
        {//清查清單內的對象：是否在範圍內
            target = main.GetEnemyList()[i];
            //在搜索範圍內 && 比最後查詢最近目標距離還近
            if (main.InRange(target, range) && main.GetDistance(target) < minRange)
            {
                //更新最近目標
                closestTarget = target;
                //更新最近距離
                minRange = main.GetDistance(target);
            }
        }
        return closestTarget;
    }
    /// <summary>
    /// 推擠我方單位
    /// </summary>
    /// <param name="main">請求方</param>
    /// <param name="hitObj"></param>
    /// <returns></returns>
    public static CharacterCtrl PushTarget(this CharacterCtrl main, GameObject hitObj)
    {
       
        CharacterCtrl target = null;
        CharacterCtrl pushTarget = null;
        for (int i = 0; i < main.GetTeamList().Count; i++)
        {//清查清單內的對象：是否在範圍內
            target = main.GetTeamList()[i];
            //在搜索範圍內 && 比最後查詢最近目標距離還近
            if (hitObj == target.gameObject)
            {
                //推擠目標
                pushTarget = target;
                break;
            }
        }
        return pushTarget;
    }
    #endregion 目標系統

    #region 擴充(工具)功能

    public static float GetDistance(this CharacterCtrl center, CharacterCtrl target)
    {
        return Vector3.Distance(center.transform.position, target.transform.position);
    }
    /// <summary>
    /// 檢查目標物件是否在中心物指定範圍內
    /// </summary>
    /// <param name="center">中心物件(擴充對象)</param>
    /// <param name="target">目標物件(檢查對象)</param>
    /// <param name="range">檢查範圍</param>
    /// <returns>是/否在範圍內</returns>
    public static bool InRange(this Transform center, Transform target, float range)
    {
        return Vector3.Distance(center.position, target.position) <= range;
    }
    /// <summary>
    /// 檢查目標物件是否在中心物指定範圍內
    /// </summary>
    /// <param name="center">中心物件(擴充對象)</param>
    /// <param name="target">目標物件(檢查對象)</param>
    /// <param name="range">檢查範圍</param>
    /// <returns>是/否在範圍內</returns>
    public static bool InRange(this CharacterCtrl center, CharacterCtrl target, float range)
    {
        return Vector3.Distance(center.transform.position, target.transform.position) <= range;
    }
    #endregion 擴充(工具)功能

    #region 戰鬥資訊系統
    
    public static bool inBattle { get; private set; }
    public static void StartBattle()
    {
        //開戰
        inBattle = true;
        //開始生怪
        spawnerManager.StartSpawn();
    }
    public static SpawnerManager spawnerManager { get; private set; }
    public static void SetSpawnerManager(SpawnerManager spawner)
    {
        //Spawner託管
        spawnerManager = spawner;
       
        //初始能量
        _energy = 7f;
    }


    /// <summary>
    /// 戰鬥計時(實體資料存放)
    /// </summary>
    private static int _timer = 210;
    /// <summary>
    /// 戰鬥計時(操作取用接口)
    /// </summary>
    public static int timer
    {
        get 
        {
            return _timer;
        }
        set 
        {
            _timer = Mathf.Clamp(value, 0, 210);
        }
    }
    /// <summary>
    /// 勝利判定:計時器大於0 & Boss 死亡
    /// </summary>
    public static bool win
    {
        get 
        {
            return timer > 0 && p1Win;
        }
    }

    /// <summary>
    /// 我方Base角色控制器
    /// </summary>
    public static CharacterCtrl baseCtrl { get; private set; }
    public static bool p1Win
    {
        get 
        {
            return bossCtrl.isDead;
        }
    }

    /// <summary>
    /// 敵方Boss角色控制器
    /// </summary>
    public static CharacterCtrl bossCtrl { get; private set; }
    public static bool p2Win
    {
        get
        {
            return baseCtrl.isDead;
        }
    }



    /// <summary>
    /// 設定 Base 角色控制器
    /// </summary>
    /// <param name="ctrl"></param>
    public static void SetBaseCtrl(CharacterCtrl ctrl)
    {
        baseCtrl = ctrl;
        upPanelCtrl?.HpBarCtrlP1(baseCtrl.hpFillamount);
    }

    /// <summary>
    /// 設定 Boss 角色控制器
    /// </summary>
    /// <param name="ctrl"></param>
    public static void SetBossCtrl(CharacterCtrl ctrl)
    {
        bossCtrl = ctrl;
        upPanelCtrl?.HpBarCtrlP2(bossCtrl.hpFillamount);
    }
    /// <summary>
    /// 指定刷新的 HUD UI
    /// </summary>
    /// <param name="type">指定的單位類型</param>
    public static void UpdateHpBar(UnitType type)
    {
        if(type == UnitType.Base)
            upPanelCtrl?.HpBarCtrlP1(baseCtrl.hpFillamount);
        if (type == UnitType.Boss)
        {
            upPanelCtrl?.HpBarCtrlP2(bossCtrl.hpFillamount);
            if (bossCtrl.isDead)
            {
               CheckLvMileage();
               ExpCtrl(20);
               MoneyCtrl(30);
            }
        }
           
    }

    /// <summary>
    /// 上方面板UI元件
    /// </summary>
    public static UIUpPanelCtrl upPanelCtrl { get; private set; }

    /// <summary>
    /// 設定上方面板UI元件(託管)
    /// </summary>
    /// <param name="ctrl">上方面板UI元件</param>
    public static void SetUpPanelCtrl(UIUpPanelCtrl ctrl)
    {
        upPanelCtrl = ctrl;
        //先找到 Boss/Base 透過託管UI更新 HP 百分點
        if (baseCtrl) upPanelCtrl.HpBarCtrlP1(baseCtrl.hpFillamount);
        if (bossCtrl) upPanelCtrl.HpBarCtrlP2(bossCtrl.hpFillamount);
        //重製計時器
        _timer = 210;
        //戰鬥預備
        inBattle = false;
    }

    /// <summary>
    /// 能量值(實體資料存放)
    /// </summary>
    private static float _energy = 0f;
    /// <summary>
    /// 能量值(操作取用接口)
    /// </summary>
    public static float energy
    {
        get 
        {
            return _energy;
        }
        set 
        {
            _energy = Mathf.Clamp(value, 0f, 10f);
        }
    }
    /// <summary>
    /// 戰鬥資訊面板UI元件
    /// </summary>
    public static UIBattlePanelCtrl battlePanelUI { get; private set; }
    /// <summary>
    /// 戰鬥資訊面板UI元件(託管)
    /// </summary>
    /// <param name="ctrl">戰鬥資訊面板UI元件</param>
    public static void SetBattlePanelUI(UIBattlePanelCtrl ctrl)
    {
        battlePanelUI = ctrl;
    }
    /// <summary>
    /// 從介面挑選的戰鬥單位(暫存)
    /// </summary>
    public static UIRumbleInfoCtrl selectRumbleUI { get; private set; }
    
    public static CharacterCtrl selectRumble
    {
        get 
        {
            return selectRumbleUI.data.characterCtrl;
        }
    }
    /// <summary>
    /// 是否選取戰鬥單位
    /// </summary>
    public static bool isSelectRumble
    {
        get 
        {
           /* selectRumbleUI?.NoData();

            if (selectRumbleUI)
            {
                selectRumbleUI.NoData();
            }*/           
            return selectRumbleUI != null && 
                !selectRumbleUI.noData;
        }
    }
    public static Action<bool> dropZoneShowUp { get; private set; }
    public static void SetShowUp(Action<bool> action)
    {
        dropZoneShowUp += action;
    }
    public static void RemoveShowUp(Action<bool> action)
    {
        dropZoneShowUp -= action;
    }

    /// <summary>
    /// 紀錄設定選取的戰鬥單位
    /// </summary>
    /// <param name="data">戰鬥單位資料</param>
    public static void SelectRumble(UIRumbleInfoCtrl ctrl)
    {
        selectRumbleUI = ctrl;
        dropZoneShowUp(true);
    }
    /// <summary>
    /// 放置戰鬥單位(資料消耗)
    /// </summary>
    public static void DropRumble()
    {
        selectRumbleUI.UseData();//資料消耗
        battlePanelUI.UpdateRumbleList();//資料補充
    }
    /// <summary>
    /// 取消選取戰鬥單位UI
    /// </summary>
    public static void UnSelectRumble()
    {      
        selectRumbleUI = null;
        dropZoneShowUp(false);
    }
    #endregion 戰鬥資訊系統

    #region 遊戲結束資訊
    /// <summary>
    /// 遊戲結束資訊面板(託管)
    /// </summary>
    public static UIGameOverPanelCtrl gameOverPanelCtrl { get; private set; }
    /// <summary>
    /// 設定遊戲結束資訊面板(託管)
    /// </summary>
    /// <param name="ctrl">控制器</param>
    public static void SetGameOverPanel(UIGameOverPanelCtrl ctrl)
    {
        gameOverPanelCtrl = ctrl;
    }
    public static void GameOverInfo()
    {
        gameOverPanelCtrl.ShowInfo();
        //戰鬥結束
        inBattle = false;
        spawnerManager.StopSpawn();
    }

    #endregion 遊戲結束資訊

}