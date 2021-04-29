using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{   
    SoundManager soundmanage;
    Controller controller;
    RocketCombine rocketCombine;
    Rigidbody2D rcRigid;
    Vector3 force;
    float sumSpeed;
    float magnitude = 0.5f;
    float skyValue;
    float durationSkyChange = 1f;
    public float damage;
    public bool takeDamage;
    

    public void Start()
    {   
        skyValue = 0;
        takeDamage = false;

        rcRigid = gameObject.GetComponent<Rigidbody2D>();
        controller = GameObject.Find("ControlManager").GetComponent<Controller>();
        soundmanage = FindObjectOfType<SoundManager>();

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

    public void RocketGravity(float gravity)
    {
        rcRigid.gravityScale = gravity;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(gameObject.tag == "Rocket")
        {
            switch(other.tag)
            {
                case "SpaceZone":
                rcRigid.gravityScale = 0;
                skyValue += Time.deltaTime / durationSkyChange;
                controller.mainCamera.backgroundColor = Color.Lerp(Color.white, Color.black, skyValue);
                if(skyValue >= 1)
                {
                    skyValue = 1;
                }
                break;  

                case "Earth":
                rcRigid.gravityScale = 1;
                skyValue -= Time.deltaTime / durationSkyChange;
                controller.mainCamera.backgroundColor = Color.Lerp(Color.white, Color.black, skyValue);
                if(skyValue <= 0)
                {
                    skyValue = 0;
                }
                break;

                case "Finish":
                controller.gameUiController.EndGame();
                rcRigid.constraints = RigidbodyConstraints2D.FreezeAll;
                controller.canControl = false;
                break;
            }
        }  
    }

    public void OnCollisionEnter2D(Collision2D other) {

                switch(other.collider.tag)
                {
                case "Obstacle":
                soundmanage.Play("Hit",0f);
                takeDamage = true;
                rcRigid.velocity = -transform.position * magnitude;
                force.Normalize();
                rcRigid.AddForce(-force * magnitude);
                controller.ImpactEffect(other);
                break;
                }
    }

}
