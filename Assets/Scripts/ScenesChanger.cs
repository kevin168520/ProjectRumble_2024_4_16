using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenesChanger : MonoBehaviour
{
    public void GoTo(string sceneName)
    {
        //�����޲z�H.Ū������(�����W��);
       GameSystem.ToSceneByName(sceneName);
    }
    

    // ���U�}�l�|����@��:�C����l��
    void Start()
    {
        
    }

    // �C�����椤�C�@�V(1FPS)����@��:��s�C�����A
    void Update()
    {
        
    }
}
