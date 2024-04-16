using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGamePlay : MonoBehaviour
{
    [Header("�I������")]
    public AudioClip backGroundMusic;

    [Header("��v�����ʽd��")]
    public float xMin;
    public float xMax, zMin, zMax;
    [Header("���H�t��")]
    public float followSpeed = 3f;

    /// <summary>
    /// �ɯ�ت��a(�y��)
    /// </summary>
    private Vector3 targetPos;
    /// <summary>
    /// �ƹ����������m
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
        //�I�s���ĺ޲z��
        SoundManager.instance.PlayBGM(backGroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }


    /// <summary>
    /// ��v�����H�J�I(�޿�B��)
    /// </summary>
    void Follow()
    {
        //�u�ʮt�� : �Z���Y�u(A�I, B�I, FPS�t�v)
        transform.position =
            Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        //�ާ@����(����ƹ�����)
        if (Input.GetMouseButtonDown(0))
        {//�I������l��m : ���߭��I
            mouseDownL = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {//�즲����m: ���V���
            mouseDargL = (Input.mousePosition - mouseDownL).normalized * Time.deltaTime * 10;
            targetPos.x -= mouseDargL.x;
            targetPos.z -= mouseDargL.y;

            targetPos.x = Mathf.Clamp(targetPos.x, xMin, xMax);
            targetPos.z = Mathf.Clamp(targetPos.z, zMin, zMax);

        }
    }
}
