using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageCtrl : MonoBehaviour
{
    [Header("Stage編號")]
    public int stageIndex;

    //變數宣告 復數某物件[] = 新建 某物件[?個]
    [Header("關卡清單")]
    public LevelCtrl[] levelCtrls = new LevelCtrl[5];   

    private void OnMouseDown()
    {
        //判斷是否位於UI上
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //Debug.Log($"點擊Stage:{stageIndex} / 座標{transform.position}");
        GameSystem.UpdateStageUI(stageIndex);//目的調用資料
        CameraManager.ctrl.SetFocusPos(transform.position);//目的移動焦點
    }
}
