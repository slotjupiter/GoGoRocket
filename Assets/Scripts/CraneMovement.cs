 using UnityEngine;
 using System.Collections;
 
 public class CraneMovement : MonoBehaviour {
 
    public float distanceMove = 1.5f;  
    public float speed = 2.0f; 
    public Transform craneBase,magnet;
    Vector2 craneBasePos;
    Vector3 cranePos;
    // float _cp;
 
    public void Start () 
    {
        cranePos = magnet.transform.position;  
    }
     
    public void goLeftRight()
     {  
        Vector3  _cp = cranePos;
        _cp.x += distanceMove * Mathf.Sin (Time.time * speed);
        magnet.transform.position = new Vector3(_cp.x,transform.position.y-0.5f,0);
     }

    public void goUp(float UpValue)
    {   Vector3  _cb = transform.position;
        _cb.y = transform.position.y + UpValue;
        transform.position = new Vector3(0,_cb.y,0);
    }
   
 }