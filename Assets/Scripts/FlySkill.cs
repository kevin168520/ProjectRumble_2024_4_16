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
        //看著對象
        transform.LookAt(target.transform);
        //往前飛
        transform.Translate(Vector3.forward * Time.deltaTime * flySpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {//技能接觸目標:造成損害
            target.HpCtrl(-attackPower);
            //自毀
            Destroy(gameObject);
        }
    }
}
