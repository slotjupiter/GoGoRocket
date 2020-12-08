 using UnityEngine;
 using System.Collections;
 
 public class CraneMovement : MonoBehaviour {
 
     public float distanceMove = 1.5f;  // Amount to move left and right from the start point
     public float speed = 2.0f; 
    Vector3 cranePos;
    Vector3 _cp;
 
     public void Start () {
         cranePos = transform.position;  
     }
     
     public void goLeftRight()
     {  Vector3  _cp = cranePos;
        _cp.x += distanceMove * Mathf.Sin (Time.time * speed);
        transform.position = _cp;
     }

     public void goUp(float upValue)
     {      
         cranePos = new Vector3(cranePos.x,cranePos.y + upValue,cranePos.z);
        //  transform.position = cranePos;
     }
     void Update () {
         
     }


 }