﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{   
    [Header("--- Rocket Blocks ---")]
    public GameObject[] rocketParts;
    public GameObject rocketBlock,rocketBlockStart;
    [Header("--- Spawn Point ---")]
    public GameObject spawnObject;
    public Transform spawnrocketPoint;
    [Header("--- Build Crane ---")]
    public GameObject buildCrane;
    public float craneUpSpeed,craneUpDistance;
    [HideInInspector]
    public CraneMovement craneMovement;
    
    RocketController rocketController;

    GameObject RocketParent;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
  
    bool createRK,dropRK,startControlRocket = false;
    bool ModeRocket = false;
    int _modeControl;
    int index = 0;
    int childIndex;
    public float rocketMultiple;

    private void Start() {
       _spawnrigidbody = spawnObject.GetComponent<Rigidbody2D>();
       RocketParent = GameObject.Find("RocketMain");
   
       _modeControl = 1;
     
       starterRocket();
    }

    public void Update() {
        // Debug.Log("RPlength = " + rocketParts.Length + " "+ index);
         rocketMultiple = RocketParent.transform.childCount;
        // Debug.Log(rocketMultiple);
        craneMovement.goLeftRight();
        if(_modeControl == 1)
        {  
           BuildRocketControl();
        }
        else if (_modeControl == 2)
        {
            RocketFlyControl();
        }
        CheckChildIndex(RocketParent);
    }

    void BuildRocketControl()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {  
           DropRocket();
        }
    
    }

    void starterRocket()
    {
        rocketBlockStart = Instantiate(rocketParts[0], spawnrocketPoint.transform.position, transform.rotation);
        rocketBlockStart.transform.SetParent(RocketParent.transform);
        _rocketJoint = rocketBlockStart.AddComponent<FixedJoint2D>();
        _rocketJoint.connectedBody = _spawnrigidbody;
         
    }
    IEnumerator CreateRocket(float timeSpawn)
    {   
        if(dropRK)
        {   yield return new WaitForSeconds(timeSpawn); 
            index++;
            rocketBlock = Instantiate(rocketParts[index], spawnrocketPoint.transform.position, transform.rotation);
            dropRK = false;
            craneMovement.goUp(1.2f);
            craneMovement.speed += craneUpSpeed;
            craneMovement.distanceMove += craneUpDistance;
        }
        _rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
        _rocketJoint.connectedBody = _spawnrigidbody;
        
         
    } 

    void DropRocket()
    {   
        dropRK = true;
        Destroy(_rocketJoint);
        //  craneMovement.goUp(0.5f);
        StartCoroutine(CreateRocket(0.5f));
         if(index == rocketParts.Length - 1)
            {
                buildCrane.SetActive(false);
                _modeControl = 2;
            }
    }

    void RocketFlyControl()
    {      
        if(!startControlRocket)
        {
        RocketParent.transform.GetChild(0).gameObject.AddComponent<RocketController>();
        rocketController = RocketParent.transform.GetChild(0).GetComponent<RocketController>();
        
        
        startControlRocket = true;
        }
        float yAxis = Input.GetAxisRaw("Vertical");
        rocketController.RocketForward(yAxis,rocketMultiple);
       

    }

    private void CheckChildIndex(GameObject parent)
    {
        parent.transform.GetChild(0);
        // Debug.Log("Child = " + parent.transform.GetChild(0));
    }

}
