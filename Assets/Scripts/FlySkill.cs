using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySkill : MonoBehaviour
{
    public CharacterCtrl target;
    public float flySpeed = 1.0f;
    public float attackPower;


   public void SetTarget(CharacterCtrl ctrl, float val)
   {
        target = ctrl;
        attackPower = val;
   }

    // Update is called once per frame
    void Update()
    {
        if(!target) Destroy(gameObject);
        //�ݵ۹�H
        transform.LookAt(target.transform);
        //���e��
        transform.Translate(Vector3.forward * Time.deltaTime * flySpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {//�ޯ౵Ĳ�ؼ�:�y���l�`
            target.HpCtrl(-attackPower);
            //�۷�
            Destroy(gameObject);
        }
    }
}
