using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("定位器")]
    public Transform GPS;

    [Header("舞台清單")]
    public StageCtrl[] stageCtrls = new StageCtrl[2];

    [Header("舞台資料清單")]
    public List<StageDB> stageDB = new List<StageDB>();

    [Header("關卡UI")]
    public UIPanelCtrl mapPanelCtrl;

    /// <summary>
    /// 腳本運行時執行一次
    /// </summary>
    private void OnEnable()
    {
        //連結至遊戲系統：託管
        GameSystem.SetStageManager(this);
    }

    /// <summary>
    /// GPS至對應Level定位
    /// </summary>
    /// <param name="stageNum">Stage編號</param>
    /// <param name="levelNum">Level編號</param>
    public void SetGPS(int stageNum, int levelNum)
    {
        GPS.position = stageCtrls[stageNum - 1].levelCtrls[levelNum - 1].
            transform.position + Vector3.up;
    }

    /// <summary>
    /// 取得對應編號的 StageDB
    /// </summary>
    /// <param name="stageIndex">Stage編號</param>
    public StageDB GetStageDB(int stageIndex)
    {
        return stageDB[stageIndex - 1];
    }

    /// <summary>
    /// 控制 Level選單啟/閉
    /// </summary>
    /// <param name="B">啟：true / 閉：false</param>
    public void OpenMapUI(bool B)
    {
        mapPanelCtrl.Switch(B);
    }

    /// <summary>
    /// 對應關卡按鈕編號
    /// </summary>
    /// <param name="levelIndex"></param>
    public void GoToStage(int levelIndex)
    {
        GameSystem.SelectLevel(levelIndex);
    }
}