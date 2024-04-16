using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �԰������(���c�զX) [�t��.�ǦC��]
/// </summary>
[System.Serializable]
public struct RumbleData
{
    /// <summary>
    /// ���W��
    /// </summary>
    public string rumbleName;
    /// <summary>
    /// �����ݨD��q
    /// </summary>
    public float rumbleReq;
    /// <summary>
    /// ���ϥ�
    /// </summary>
    public Sprite rumbleIcon;
    /// <summary>
    /// �����骫��
    /// </summary>
    public CharacterCtrl characterCtrl;

    /// <summary>
    /// (�ԷV�ϥ�)�M�����
    /// </summary>
    public void ClearData()
    {
        rumbleName = "";
        rumbleReq = 0;
        rumbleIcon = null;
        characterCtrl = null;
    }
    /// <summary>
    /// ��ƿ�(���s�b)�ˬd
    /// </summary>
    /// <returns><��ƬO�_�ŦX�ϥλݨD()/returns>
    public bool NoData()
    {
        return characterCtrl == null || rumbleIcon == null || 
            rumbleReq == 0 || rumbleName == "";
    }
}

/// <summary>
/// ��ƫ��}���ɮ� [�ۭq�Ыؿ��]
/// </summary>
[CreateAssetMenu(fileName = "RumbleDB", menuName = "DB/RumbleDB", order = 1)]
public class RumbleDB : ScriptableObject
{
   public List<RumbleData> rumbles = new List<RumbleData>();

    /// <summary>
    /// �H������M�椤���԰����
    /// </summary>
    /// <returns>�԰����</returns>
    public RumbleData RandomRumble()
    {
        return rumbles[Random.Range(0, rumbles.Count)];
    }

}
