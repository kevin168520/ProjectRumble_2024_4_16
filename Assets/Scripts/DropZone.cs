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
        //�P�_�O�_���UI�W
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (GameSystem.isSelectRumble)
        {//�O�_���԰��L�i��
            Instantiate(GameSystem.selectRumble,
                transform.position, transform.rotation)
                .SetGroup(PlayerGroup.P1).SetPath(pathCtrl);//��k��
            GameSystem.DropRumble();
            GameSystem.UnSelectRumble();

        }
       
    }
}
