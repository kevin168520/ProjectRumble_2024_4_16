using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGamePlay : MonoBehaviour
{
    [Header("背景音樂")]
    public AudioClip backGroundMusic;

    [Header("攝影機活動範圍")]
    public float xMin;
    public float xMax, zMin, zMax;
    [Header("跟隨速度")]
    public float followSpeed = 3f;

    /// <summary>
    /// 導航目的地(座標)
    /// </summary>
    private Vector3 targetPos;
    /// <summary>
    /// 滑鼠左鍵按壓位置
    /// </summary>
    private Vector3 mouseDownL;
    private Vector3 mouseDargL;

    private void OnEnable()
    {
        GameSystem.AddGameUI();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        //呼叫音效管理員
        SoundManager.instance.PlayBGM(backGroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }


    /// <summary>
    /// 攝影機跟隨焦點(邏輯運算)
    /// </summary>
    void Follow()
    {
        //線性差值 : 距離縮短(A點, B點, FPS速率)
        transform.position =
            Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        //操作角度(按住滑鼠左鍵)
        if (Input.GetMouseButtonDown(0))
        {//點擊的原始位置 : 中心原點
            mouseDownL = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {//拖曳的位置: 指向方位
            mouseDargL = (Input.mousePosition - mouseDownL).normalized * Time.deltaTime * 10;
            targetPos.x -= mouseDargL.x;
            targetPos.z -= mouseDargL.y;

            targetPos.x = Mathf.Clamp(targetPos.x, xMin, xMax);
            targetPos.z = Mathf.Clamp(targetPos.z, zMin, zMax);

        }
    }
}
