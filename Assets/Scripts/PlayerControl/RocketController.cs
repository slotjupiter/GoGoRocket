using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{   
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    Vector3 force;
    float sumSpeed;
    public Vector2 lastPosition;

    public void Start()
    {
        rcRigid = gameObject.GetComponent<Rigidbody2D>();
        // lastPosition = gameObject.transform.position;
    }

    public void RocketVelocity(float rcVelocity)
    {   
        float x = Mathf.Clamp(rcRigid.velocity.x, -rcVelocity,rcVelocity);
        float y = Mathf.Clamp(rcRigid.velocity.y, -rcVelocity,rcVelocity);

        rcRigid.velocity = new Vector2(x,y);
    }
    public void RocketForward(float amount,float blockAmount,float speed)
    {   
        sumSpeed = blockAmount * speed;
        force = (transform.up * amount) * sumSpeed * Time.deltaTime;

        if(rcRigid)
        {
            rcRigid.AddForce(force);
        }
       
    }

    public void RocketRotate(GameObject rkobject,float amount,float rotationSpeed)
    {
        rkobject.transform.Rotate(0,0,amount * -rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(gameObject.tag == "Rocket")
        {
            if(other.tag == "SpaceZone")
            {
                rcRigid.gravityScale = 0;
            }
        }
    }

}
