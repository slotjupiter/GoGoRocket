using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    public void Start()
    {
        rcRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float yAxis = Input.GetAxisRaw("Vertical");
        RocketForward(yAxis);
    }

    private void RocketForward(float amount)
    {
        Vector2 force = (transform.up * amount) * 400;
        if(rcRigid)
        {
             rcRigid.AddForce(force);
        }
       
    }
}
