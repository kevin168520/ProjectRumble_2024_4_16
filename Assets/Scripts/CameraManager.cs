using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//[ExecuteInEditMode] //預覽更新
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// 靜態的本體控制參數(唯一)
    /// </summary>
    public static CameraManager ctrl;

    /// <summary>
    /// 控制攝影機的物理位置
    /// </summary>
    [Header("攝影機")]
    public Transform camTrans;
    /// <summary>
    /// 拍攝距離
    /// </summary>
    [Header("拍攝距離")]
    [Range(minDis,maxDis)]
    public float distance = 5f;
    [Header("拍攝X角度")]
    [Range(minAngX, maxAngX )]
    public float angleX = 60f;
    [Header("拍攝Y角度")]
    [Range(0, 360)]
    public float angleY = 0f;
    [Header("偏差修正")]
    public Vector3 rotOffset;
    public Vector3 posOffset;
    [Header("跟隨速度")]
    public float followSpeed = 3f;
    [Header("拖曳速度")]
    public float dragSpeed = 3f;
    [Header("旋轉速度")]
    public float rotaSpeed = 3f;

    /// <summary>
    /// 距離最小值(常數)
    /// </summary>
    private const float minDis = 15f;
    /// <summary>
    /// 距離最大值
    /// </summary>
    private const float maxDis = 20f;
    /// <summary>
    /// X角最小值(常數)
    /// </summary>
    private const float minAngX = 30f;
    /// <summary>
    /// X角最大值(常數)
    /// </summary>
    private const float maxAngX = 80f;
    /// <summary>
    /// Y角最小值(常數)
    /// </summary>
    private const float minAngY = -30f;
    /// <summary>
    /// Y角最大值(常數)
    /// </summary>
    private const float maxAngY = 30f;

    /// <summary>
    /// 導航目的地(座標)
    /// </summary>
    private Vector3 targetPos;
    /// <summary>
    /// 滑鼠右鍵按壓位置
    /// </summary>
    private Vector3 mouseDownR;
    private Vector3 mouseDargR; 
    /// <summary>
    /// 滑鼠左鍵按壓位置
    /// </summary>
    private Vector3 mouseDownL;
    private Vector3 mouseDargL;
    /// <summary>
    /// 是否靠近導航點(到達目的地 < 1M)
    /// </summary>
    private bool goal = true;

    /// <summary>
    /// 腳本運行時執行一次
    /// </summary>
    private void OnEnable()
    {
        //詔告天下我是唯一的 CameraManager
        ctrl = this;
    }

    // Start is called before the first frame update
    void Update()
    { //判斷是否位於UI上
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Follow();
    }

    /// <summary>
    /// 設定焦點位置(目的地)
    /// </summary>
    /// <param name="pos">設定的座標</param>
    public void SetFocusPos(Vector3 pos)
    {
        //新位置
        targetPos = pos;
        //開始導航 : 還未到達
        goal = false;
    }
    /// <summary>
    /// 攝影機跟隨焦點(邏輯運算)
    /// </summary>
    void Follow()
    {

        //攝影機新位置 =(位置平移修正 +焦點位置 )  +  向量角 *  距離 
        camTrans.position = (transform.position + posOffset) + Angle() * Distance();
        //看著目標(攝影機對著焦點 + 旋轉修正)
        camTrans.LookAt(transform.position +  rotOffset);
        //線性差值 : 距離縮短(A點, B點, FPS速率)
        transform.position = 
            Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        if (!goal)
        {//還未到達:持續檢查
            if (Vector3.Distance(transform.position, targetPos) < 1f)
            {//離目的地 1M 內:到達
                goal = true;
                //通知 StageManager 打開關卡UI
                //Debug.Log("到達 開UI");
                GameSystem.stageManager.OpenMapUI(goal);
            }
        }



        //操作角度(按住滑鼠左鍵)
        if (Input.GetMouseButtonDown(0))
        {//點擊的原始位置 : 中心原點
            mouseDownL = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {//拖曳的位置: 指向方位
            mouseDargL = (Input.mousePosition - mouseDownL).normalized * Time.deltaTime* dragSpeed;
            targetPos.x -= mouseDargL.x;
            targetPos.z -= mouseDargL.y;

        }
    }

    /// <summary>
    /// 組合 angleX, angleY 產生的向量角
    /// </summary>
    /// <returns>向量角</returns>
    Quaternion Angle()
    {
        //操作角度(按住滑鼠右鍵)
        if (Input.GetMouseButtonDown(1))
        {//點擊的原始位置 : 中心原點
            mouseDownR = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {//拖曳的位置: 指向方位
            mouseDargR = (Input.mousePosition - mouseDownR).normalized * Time.deltaTime *rotaSpeed;
            angleX = Mathf.Clamp(angleX - mouseDargR.y, minAngX, maxAngX);
            angleY = Mathf.Clamp(angleY + mouseDargR.x, minAngY, maxAngY);
        }

        return Quaternion.Euler(angleX, angleY, 0);
    }
    /// <summary>
    /// 退後一個單位的方向向量 * distance倍
    /// </summary>
    /// <returns>方向向量(後退)</returns>
    Vector3 Distance()
    {
        //操作距離 : 數學.限制(值,最小,最大)
        distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, minDis, maxDis);
        return Vector3.back * distance;
    }
}
