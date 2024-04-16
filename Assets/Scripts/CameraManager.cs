using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//[ExecuteInEditMode] //�w����s
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// �R�A�����鱱��Ѽ�(�ߤ@)
    /// </summary>
    public static CameraManager ctrl;

    /// <summary>
    /// ������v�������z��m
    /// </summary>
    [Header("��v��")]
    public Transform camTrans;
    /// <summary>
    /// ����Z��
    /// </summary>
    [Header("����Z��")]
    [Range(minDis,maxDis)]
    public float distance = 5f;
    [Header("����X����")]
    [Range(minAngX, maxAngX )]
    public float angleX = 60f;
    [Header("����Y����")]
    [Range(0, 360)]
    public float angleY = 0f;
    [Header("���t�ץ�")]
    public Vector3 rotOffset;
    public Vector3 posOffset;
    [Header("���H�t��")]
    public float followSpeed = 3f;
    [Header("�즲�t��")]
    public float dragSpeed = 3f;
    [Header("����t��")]
    public float rotaSpeed = 3f;

    /// <summary>
    /// �Z���̤p��(�`��)
    /// </summary>
    private const float minDis = 15f;
    /// <summary>
    /// �Z���̤j��
    /// </summary>
    private const float maxDis = 20f;
    /// <summary>
    /// X���̤p��(�`��)
    /// </summary>
    private const float minAngX = 30f;
    /// <summary>
    /// X���̤j��(�`��)
    /// </summary>
    private const float maxAngX = 80f;
    /// <summary>
    /// Y���̤p��(�`��)
    /// </summary>
    private const float minAngY = -30f;
    /// <summary>
    /// Y���̤j��(�`��)
    /// </summary>
    private const float maxAngY = 30f;

    /// <summary>
    /// �ɯ�ت��a(�y��)
    /// </summary>
    private Vector3 targetPos;
    /// <summary>
    /// �ƹ��k�������m
    /// </summary>
    private Vector3 mouseDownR;
    private Vector3 mouseDargR; 
    /// <summary>
    /// �ƹ����������m
    /// </summary>
    private Vector3 mouseDownL;
    private Vector3 mouseDargL;
    /// <summary>
    /// �O�_�a��ɯ��I(��F�ت��a < 1M)
    /// </summary>
    private bool goal = true;

    /// <summary>
    /// �}���B��ɰ���@��
    /// </summary>
    private void OnEnable()
    {
        //�@�i�ѤU�ڬO�ߤ@�� CameraManager
        ctrl = this;
    }

    // Start is called before the first frame update
    void Update()
    { //�P�_�O�_���UI�W
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Follow();
    }

    /// <summary>
    /// �]�w�J�I��m(�ت��a)
    /// </summary>
    /// <param name="pos">�]�w���y��</param>
    public void SetFocusPos(Vector3 pos)
    {
        //�s��m
        targetPos = pos;
        //�}�l�ɯ� : �٥���F
        goal = false;
    }
    /// <summary>
    /// ��v�����H�J�I(�޿�B��)
    /// </summary>
    void Follow()
    {

        //��v���s��m =(��m�����ץ� +�J�I��m )  +  �V�q�� *  �Z�� 
        camTrans.position = (transform.position + posOffset) + Angle() * Distance();
        //�ݵۥؼ�(��v����۵J�I + ����ץ�)
        camTrans.LookAt(transform.position +  rotOffset);
        //�u�ʮt�� : �Z���Y�u(A�I, B�I, FPS�t�v)
        transform.position = 
            Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        if (!goal)
        {//�٥���F:�����ˬd
            if (Vector3.Distance(transform.position, targetPos) < 1f)
            {//���ت��a 1M ��:��F
                goal = true;
                //�q�� StageManager ���}���dUI
                //Debug.Log("��F �}UI");
                GameSystem.stageManager.OpenMapUI(goal);
            }
        }



        //�ާ@����(����ƹ�����)
        if (Input.GetMouseButtonDown(0))
        {//�I������l��m : ���߭��I
            mouseDownL = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {//�즲����m: ���V���
            mouseDargL = (Input.mousePosition - mouseDownL).normalized * Time.deltaTime* dragSpeed;
            targetPos.x -= mouseDargL.x;
            targetPos.z -= mouseDargL.y;

        }
    }

    /// <summary>
    /// �զX angleX, angleY ���ͪ��V�q��
    /// </summary>
    /// <returns>�V�q��</returns>
    Quaternion Angle()
    {
        //�ާ@����(����ƹ��k��)
        if (Input.GetMouseButtonDown(1))
        {//�I������l��m : ���߭��I
            mouseDownR = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {//�즲����m: ���V���
            mouseDargR = (Input.mousePosition - mouseDownR).normalized * Time.deltaTime *rotaSpeed;
            angleX = Mathf.Clamp(angleX - mouseDargR.y, minAngX, maxAngX);
            angleY = Mathf.Clamp(angleY + mouseDargR.x, minAngY, maxAngY);
        }

        return Quaternion.Euler(angleX, angleY, 0);
    }
    /// <summary>
    /// �h��@�ӳ�쪺��V�V�q * distance��
    /// </summary>
    /// <returns>��V�V�q(��h)</returns>
    Vector3 Distance()
    {
        //�ާ@�Z�� : �ƾ�.����(��,�̤p,�̤j)
        distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, minDis, maxDis);
        return Vector3.back * distance;
    }
}
