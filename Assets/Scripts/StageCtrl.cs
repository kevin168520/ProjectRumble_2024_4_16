using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageCtrl : MonoBehaviour
{
    [Header("Stage�s��")]
    public int stageIndex;

    //�ܼƫŧi �_�ƬY����[] = �s�� �Y����[?��]
    [Header("���d�M��")]
    public LevelCtrl[] levelCtrls = new LevelCtrl[5];   

    private void OnMouseDown()
    {
        //�P�_�O�_���UI�W
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //Debug.Log($"�I��Stage:{stageIndex} / �y��{transform.position}");
        GameSystem.UpdateStageUI(stageIndex);//�ت��եθ��
        CameraManager.ctrl.SetFocusPos(transform.position);//�ت����ʵJ�I
    }
}
