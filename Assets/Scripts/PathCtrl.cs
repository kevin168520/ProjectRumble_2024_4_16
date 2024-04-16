using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCtrl : MonoBehaviour
{
    
    private List<Transform> points;
    /// <summary>
    /// 最後一個點的編號
    /// </summary>
    public int lastIndex
    {
        get 
        {
            return points.Count - 1;
        }
    }

    private void OnEnable()
    {
        //路徑點清單 = 建立清單(集合物件) 轉換
        points = new List<Transform>(GetComponentsInChildren<Transform>());
        //預設會將自身抓取 : 所以移除第一個物件
        points.RemoveAt(0);
    }

    /// <summary>
    /// 使用序列號取得路徑點物件
    /// </summary>
    /// <param name="index">序列號</param>
    /// <returns>路徑點物件</returns>
    public Transform GetPoint(int index)
    {
        //序列號0以上且小於清單總數 ? 回傳點物件 : 無
        return index >= 0 && index < points.Count 
            ? points[index] : null;
    }
}
