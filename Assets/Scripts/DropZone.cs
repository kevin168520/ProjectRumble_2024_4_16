using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshRenderer meshRenderer
    {
        get 
        {
            if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
    }
    [SerializeField]
    private Color showColor;

    public PathCtrl pathCtrl;

    private void OnEnable()
    {
        ShowUp(false);
        GameSystem.SetShowUp(ShowUp);
    }

    private void OnDisable()
    {
        GameSystem.RemoveShowUp(ShowUp);
    }

    public void ShowUp(bool B)
    {
        meshRenderer.material.color = B ? showColor : Color.clear;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{name}:{other.name} drop in.");
        if (other.tag == "Player")
        {
            CharacterCtrl charCtrl = other.GetComponent<CharacterCtrl>(); 
            charCtrl.SetPath(pathCtrl);
        }
    }
    */

    private void OnMouseDown()
    {
        //判斷是否位於UI上
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (GameSystem.isSelectRumble)
        {//是否有戰鬥兵可放
            Instantiate(GameSystem.selectRumble,
                transform.position, transform.rotation)
                .SetGroup(PlayerGroup.P1).SetPath(pathCtrl);//方法鍊
            GameSystem.DropRumble();
            GameSystem.UnSelectRumble();

        }
       
    }
}
