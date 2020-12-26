using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{   
    Controller controller;
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    Vector3 force;
    float sumSpeed;
    float magnitude = 100f;
    
    public float damage;
    public bool takeDamage;

    public void Start()
    {   
        takeDamage = false;
        rcRigid = gameObject.GetComponent<Rigidbody2D>();
        controller = GameObject.Find("ControlManager").GetComponent<Controller>();
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
            switch(other.tag)
            {
                case "SpaceZone":
                rcRigid.gravityScale = 0;
                break;  
                // case "Obstacle":
                // takeDamage = true;
                // rcRigid.velocity = -transform.position * 100;
                // Debug.Log("Trigger");
                // break;   
            }
        }  
    }

    public void OnCollisionEnter2D(Collision2D other) {

                switch(other.collider.tag)
                {
                case "Obstacle":
                takeDamage = true;
                rcRigid.velocity = -transform.position * magnitude;
                force.Normalize();
                rcRigid.AddForce(-force * magnitude);
     
                break;
                //  case "EmptyFuel":
                // // Vector2 forcez = transform.position - other.transform.position;
                // // forcez.Normalize();
                //  hit = true;
                // rcRigid.AddForce(-force * magnitude);
                // Debug.Log("Empty");
                // break;
                }
    }

    public void OnCollisionExit2D(Collision2D other) {
        switch(other.collider.tag)
                {
                case "Obstacle":
                // Vector2 forcez = transform.position - other.transform.position;
                // forcez.Normalize();
                // controller.FuelDecrease(damage);
                // damage = 0f;
                 Debug.Log("Exit");
                break;
                //  case "EmptyFuel":
                // // Vector2 forcez = transform.position - other.transform.position;
                // // forcez.Normalize();
                //  hit = true;
                // rcRigid.AddForce(-force * magnitude);
                // Debug.Log("Empty");
                // break;
                }
    }



}
