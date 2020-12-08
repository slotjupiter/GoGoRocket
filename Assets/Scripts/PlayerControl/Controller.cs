using System.Collections;
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
    public RocketController rocketController;

    GameObject RocketParent;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
  
    bool createRK,dropRK = false;
    int modeControl;
    int index = 0;
    int childIndex;

    private void Start() {
       _spawnrigidbody = spawnObject.GetComponent<Rigidbody2D>();
       RocketParent = GameObject.Find("RocketMain");
       modeControl = 1;
     
       starterRocket();
    }

    public void Update() {
        // Debug.Log("RPlength = " + rocketParts.Length + " "+ index);
        craneMovement.goLeftRight();
        if(modeControl == 1)
        {  
           BuildRocketControl();
        }
        else if (modeControl == 2)
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
            craneMovement.goUp(1f);
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
        StartCoroutine(CreateRocket(0.8f));
         if(index == rocketParts.Length - 1)
            {
                buildCrane.SetActive(false);
                modeControl = 2;
            }
    }

    void RocketFlyControl()
    {
         if(Input.GetKeyDown(KeyCode.Space))
        {  
           RocketParent.transform.GetChild(0).gameObject.AddComponent<RocketController>();
        }
    }

    private void CheckChildIndex(GameObject parent)
    {
        parent.transform.GetChild(0);
        // Debug.Log("Child = " + parent.transform.GetChild(0));
    }

}
