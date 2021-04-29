using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCombine : MonoBehaviour
{    
    bool _blockConnected = false;
    bool _entered;
    GameObject rocketBlock;
    GameObject RocketParent;
    GameObject Instanceparticle,InstanceDestroyparticle;
    FixedJoint2D rocketJoint;
    public Rigidbody2D rocketRigid;
    public GameObject partsConnectedparticle;
    public GameObject destroyParticle;

   private void Start() {
       rocketBlock = gameObject;
       rocketRigid = GetComponent<Rigidbody2D>();
       rocketRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
       RocketParent = GameObject.Find("RocketMain");
   }

    private void OnCollisionEnter2D(Collision2D other) {
        Collider2D collider = other.collider;
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 centerPoint = collider.bounds.center;
        
        //BaseRocketComponent Collision Condition
        if(gameObject.tag == "BaseRocketComponent")
        {
            if(other.collider.tag == "RocketComponent") 
            {   
                _blockConnected = true; 
                rocketConnected();
                // if(!_entered && contactPoint.y < centerPoint.y)
                 if(!_entered)
                {
                    // rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
                    // rocketJoint.connectedBody = other.collider.transform.GetComponentInParent<Rigidbody2D>();
                    // rocketJoint.autoConfigureConnectedAnchor = false;
                    // rocketJoint.enableCollision = false; 
                    
                    //   rocketJoint.dampingRatio = 1;
                    // rocketRigid.simulated = false;
                    _entered = true;
                    // Vector3 test = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+1,gameObject.transform.position.z);
                    // Instanceparticle = Instantiate(partsConnectedparticle, test, gameObject.transform.rotation);
                    Destroy(rocketRigid);
                }  
            }
            else if(other.collider.tag == "Ground") 
            {
                  Destroy(rocketRigid);
            }
        }
        
        //RocketComponent Collision Condition destroy when face the ground
         if(gameObject.tag == "RocketComponent")
        {
            if(other.collider.tag == "RocketComponent" || other.collider.tag == "BaseRocketComponent") 
            {   
                _blockConnected = true; 
                rocketConnected();
                // if(!_entered && contactPoint.y < centerPoint.y)
                if(!_entered)
                {
                    // rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
                    // rocketJoint.connectedBody = other.collider.transform.GetComponentInParent<Rigidbody2D>();
                    // rocketJoint.autoConfigureConnectedAnchor = false;
                    // rocketJoint.enableCollision = false; 
                    // rocketRigid.bodyType = RigidbodyType2D.Kinematic;
                    // rocketJoint.dampingRatio = 1;
                    // rocketRigid.simulated = false;
                    _entered = true;
                        Vector3 particlePos = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-1,gameObject.transform.position.z);
                    Instanceparticle = Instantiate(partsConnectedparticle, particlePos, gameObject.transform.rotation);
                    Destroy(Instanceparticle,3f);
                      Destroy(rocketRigid);
                }  
            } 
            else if(other.collider.tag == "Ground")
            {   InstanceDestroyparticle = Instantiate(destroyParticle,gameObject.transform.position, gameObject.transform.rotation);
                Destroy(InstanceDestroyparticle,3f);
                Destroy(gameObject); 
            }
        }
        // rocketRigid.constraints = RigidbodyConstraints2D.None;
    }

    private void OnCollisionExit2D(Collision2D other) {
      
        if(gameObject.transform.parent = null)
        {  
        Destroy(gameObject,1f);
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
