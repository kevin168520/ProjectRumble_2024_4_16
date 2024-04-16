using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 關卡資料(結構組合) [系統.序列化]
/// </summary>
[System.Serializable]
public struct LevelData
{
    /// <summary>
    /// 關卡名稱
    /// </summary>
    public string lvName;
    /// <summary>
    /// 關卡描述
    /// </summary>
    public string lvDes;
    /// <summary>
    /// 關卡等級
    /// </summary>
    public int lv;
    /// <summary>
    /// 限制入場的等級
    /// </summary>
    public int lvFactor;
    /// <summary>
    /// 通關獎勵(錢)
    /// </summary>
    public int reward;
    /// <summary>
    /// 圖示
    /// </summary>
    public Sprite icon;
}

/// <summary>
/// 資料型腳本檔案 [自訂創建選單]
/// </summary>
[CreateAssetMenu(fileName = "StageDB", menuName = "DB/StageDB", order = 0)]
public class StageDB : ScriptableObject
{
    //關卡清單
    public List<LevelData> levels = new List<LevelData>(5);
}
