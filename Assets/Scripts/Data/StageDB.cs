using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���d���(���c�զX) [�t��.�ǦC��]
/// </summary>
[System.Serializable]
public struct LevelData
{
    /// <summary>
    /// ���d�W��
    /// </summary>
    public string lvName;
    /// <summary>
    /// ���d�y�z
    /// </summary>
    public string lvDes;
    /// <summary>
    /// ���d����
    /// </summary>
    public int lv;
    /// <summary>
    /// ����J��������
    /// </summary>
    public int lvFactor;
    /// <summary>
    /// �q�����y(��)
    /// </summary>
    public int reward;
    /// <summary>
    /// �ϥ�
    /// </summary>
    public Sprite icon;
}

/// <summary>
/// ��ƫ��}���ɮ� [�ۭq�Ыؿ��]
/// </summary>
[CreateAssetMenu(fileName = "StageDB", menuName = "DB/StageDB", order = 0)]
public class StageDB : ScriptableObject
{
    //���d�M��
    public List<LevelData> levels = new List<LevelData>(5);
}
