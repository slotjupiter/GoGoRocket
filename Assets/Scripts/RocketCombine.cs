using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCombine : MonoBehaviour
{    
    bool _blockConnected = false;
    bool _entered;
    GameObject rocketBlock;
    GameObject RocketParent;
    FixedJoint2D rocketJoint;
    public Rigidbody2D rocketRigid;

   private void Start() {
       rocketBlock = gameObject;
       rocketRigid = GetComponent<Rigidbody2D>();
       rocketRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
       RocketParent = GameObject.Find("RocketMain");
   }

    private void OnCollisionEnter2D(Collision2D rocketCol) {
        Collider2D collider = rocketCol.collider;
        Vector3 contactPoint = rocketCol.contacts[0].point;
        Vector3 centerPoint = collider.bounds.center;
        
        //BaseRocketComponent Collision Condition
        if(gameObject.tag == "BaseRocketComponent")
        {
            if(rocketCol.collider.tag == "RocketComponent") 
            {   
                _blockConnected = true; 
                rocketConnected();
                if(!_entered && contactPoint.y < centerPoint.y)
                {
                    rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
                    rocketJoint.connectedBody = rocketCol.collider.transform.GetComponentInParent<Rigidbody2D>();
                    rocketJoint.autoConfigureConnectedAnchor = false;
                    rocketJoint.enableCollision = false; 
                    _entered = true;
                }  
            }
        }
        
        //RocketComponent Collision Condition destroy when face the ground
         if(gameObject.tag == "RocketComponent")
        {
            if(rocketCol.collider.tag == "RocketComponent" || rocketCol.collider.tag == "BaseRocketComponent") 
            {   
                _blockConnected = true; 
                rocketConnected();
                if(!_entered && contactPoint.y < centerPoint.y)
                {
                    rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
                    rocketJoint.connectedBody = rocketCol.collider.transform.GetComponentInParent<Rigidbody2D>();
                    rocketJoint.autoConfigureConnectedAnchor = false;
                    rocketJoint.enableCollision = false; 
                    _entered = true;
                }  
            } 
            else 
            { 
                Destroy(gameObject); 
            }
        }
      
    }

    
 

    public void rocketConnected()
    {
        if(_blockConnected == true)
        {
            rocketBlock.transform.SetParent(RocketParent.transform);
            //  rocketBlock.transform.SetSiblingIndex(0);
            
        }
    }
    
    
   
}
