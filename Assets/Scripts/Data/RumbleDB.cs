using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戰鬥單位資料(結構組合) [系統.序列化]
/// </summary>
[System.Serializable]
public struct RumbleData
{
    /// <summary>
    /// 單位名稱
    /// </summary>
    public string rumbleName;
    /// <summary>
    /// 派遣需求能量
    /// </summary>
    public float rumbleReq;
    /// <summary>
    /// 單位圖示
    /// </summary>
    public Sprite rumbleIcon;
    /// <summary>
    /// 單位實體物件
    /// </summary>
    public CharacterCtrl characterCtrl;

    /// <summary>
    /// (謹慎使用)清除資料
    /// </summary>
    public void ClearData()
    {
        rumbleName = "";
        rumbleReq = 0;
        rumbleIcon = null;
        characterCtrl = null;
    }
    /// <summary>
    /// 資料遺失(不存在)檢查
    /// </summary>
    /// <returns><資料是否符合使用需求()/returns>
    public bool NoData()
    {
        return characterCtrl == null || rumbleIcon == null || 
            rumbleReq == 0 || rumbleName == "";
    }
}

/// <summary>
/// 資料型腳本檔案 [自訂創建選單]
/// </summary>
[CreateAssetMenu(fileName = "RumbleDB", menuName = "DB/RumbleDB", order = 1)]
public class RumbleDB : ScriptableObject
{
   public List<RumbleData> rumbles = new List<RumbleData>();

    /// <summary>
    /// 隨機抽取清單中的戰鬥單位
    /// </summary>
    /// <returns>戰鬥單位</returns>
    public RumbleData RandomRumble()
    {
        return rumbles[Random.Range(0, rumbles.Count)];
    }

}
