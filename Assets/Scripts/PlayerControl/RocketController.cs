using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{   
    public float speed = 20f;
    public float rkVelocity = 3;
    public float rotationSp = 3;
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    Vector3 force;
    float rocketPower;
    public void Start()
    {
        rcRigid = gameObject.GetComponent<Rigidbody2D>();
     
    }

    // private void FixedUpdate() {
    //         float yAxis = Input.GetAxis("Vertical");
    //    RocketForward(yAxis);
    // }
    private void RocketVelocity()
    {
        float x = Mathf.Clamp(rcRigid.velocity.x, -rkVelocity,rkVelocity);
        float y = Mathf.Clamp(rcRigid.velocity.y, -rkVelocity,rkVelocity);

        rcRigid.velocity = new Vector2(x,y);
    }
    public void RocketForward(float amount)
    {   
        // float speedz = speed * rocketChild;
        // rocketPower = blockAmount * 10;
        force = (transform.up * amount) * speed;
        if(rcRigid)
        {
             rcRigid.AddForce(force);
        }
       
    }

    public void RocketRotate(GameObject rkobject,float amount)
    {
        rkobject.transform.Rotate(0,0,amount);
    }

}
