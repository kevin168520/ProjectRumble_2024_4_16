using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("�w�쾹")]
    public Transform GPS;

    [Header("�R�x�M��")]
    public StageCtrl[] stageCtrls = new StageCtrl[2];

    [Header("�R�x��ƲM��")]
    public List<StageDB> stageDB = new List<StageDB>();

    [Header("���dUI")]
    public UIPanelCtrl mapPanelCtrl;

    /// <summary>
    /// �}���B��ɰ���@��
    /// </summary>
    private void OnEnable()
    {
        //�s���ܹC���t�ΡG�U��
        GameSystem.SetStageManager(this);
    }

    /// <summary>
    /// GPS�ܹ���Level�w��
    /// </summary>
    /// <param name="stageNum">Stage�s��</param>
    /// <param name="levelNum">Level�s��</param>
    public void SetGPS(int stageNum, int levelNum)
    {
        GPS.position = stageCtrls[stageNum - 1].levelCtrls[levelNum - 1].
            transform.position + Vector3.up;
    }

    /// <summary>
    /// ���o�����s���� StageDB
    /// </summary>
    /// <param name="stageIndex">Stage�s��</param>
    public StageDB GetStageDB(int stageIndex)
    {
        return stageDB[stageIndex - 1];
    }

    /// <summary>
    /// ���� Level����/��
    /// </summary>
    /// <param name="B">�ҡGtrue / ���Gfalse</param>
    public void OpenMapUI(bool B)
    {
        mapPanelCtrl.Switch(B);
    }

    /// <summary>
    /// �������d���s�s��
    /// </summary>
    /// <param name="levelIndex"></param>
    public void GoToStage(int levelIndex)
    {
        GameSystem.SelectLevel(levelIndex);
    }
}