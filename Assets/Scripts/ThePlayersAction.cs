using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlayersAction : MonoBehaviour
{
    private bool _bIsDash;
    private float dashTime;
    private Vector3 directionXOZ;
    public float dashDuration;// 控制冲刺时间
    public float dashSpeed;// 冲刺速度
    // Start is called before the first frame update
    void Start()
    {
        _bIsDash = false;
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void Dash(CharacterController rb)
    {

        ////Vector3 i = new Vector3(10f, 0f, 0f);
        ////rb.Move(i);
        ////Debug.Log("dash");
        ////if (!_bIsDash)
        ////{

        //rb = GetComponent<CharacterController>();
        //        _bIsDash = true;
        //        directionXOZ = rb.transform.forward;// forward 指向物体当前的前方
        //        directionXOZ.y = 0f;// 只做平面的上下移动和水平移动，不做高度上的上下移动

        //}
        //else
        //{
        //    if (dashTime <= 0)// reset
        //    {
        //        _bIsDash = false;

        //        dashTime = dashDuration;
        //    }
        //    else
        //    {
        //        dashTime -= Time.deltaTime;
        //       rb.Move(directionXOZ * dashTime * dashSpeed);// rigidbody = GetComponent<Rigidbody>(); 加在 Start() 函数中
        //    }
        //}

    }
    public void clickDash()
    {
        _bIsDash = true;
    }
}
