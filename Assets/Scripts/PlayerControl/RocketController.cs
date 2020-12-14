using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    [HideInInspector]
   
    float rocketPower;
    public void Start()
    {
        rcRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    public void RocketForward(float amount,float blockAmount)
    {   
        rocketPower = blockAmount * 50;
        Vector2 force = (transform.up * amount) * rocketPower;
        if(rcRigid)
        {
             rcRigid.AddForce(force);
        }
       
    }

}
