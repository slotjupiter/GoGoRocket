using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollide : MonoBehaviour
{
   Rigidbody2D rigidBody;
   Vector3 scaleChange,randomVT;
   Vector2 position2D;
   float scaleRandom,dirRandom;
   float force = 10f;
   
   private void Start() {
       rigidBody = gameObject.GetComponent<Rigidbody2D>();
       position2D = transform.position;
       RandomScale();
   }

   private void RandomScale()
   {    
       scaleRandom = Random.Range(1f,4f);
       transform.localScale = new Vector3(scaleRandom,scaleRandom,scaleRandom); 
   }

   private void OnCollisionEnter2D(Collision2D c) {
       if(c.collider.tag == "Obstacle")
       {
        Vector2 dir = c.contacts[0].point - position2D;
        dir = -dir.normalized;
        rigidBody.velocity = dir * 1f;
        // rigidBody.AddForce(dir * force);
       }
   }
}
