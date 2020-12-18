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
    
    RocketController rocketController;

    GameObject RocketParent,rocketFirst;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
    Transform RocketParentTransform;
  
    bool createRK,dropRK,startControlRocket = false;
    int _modeControl;
    int index = 0;
    int childIndex;
    float rocketPartsAmount;

    private void Start() {
       _spawnrigidbody = spawnObject.GetComponent<Rigidbody2D>();
       RocketParent = GameObject.Find("RocketMain");
       RocketParentTransform = RocketParent.transform;
       _modeControl = 1;
     
       starterRocket();
    }

    public void Update() {
        // Debug.Log("RPlength = " + rocketParts.Length + " "+ index);
         rocketPartsAmount = RocketParent.transform.childCount;
        // Debug.Log(rocketMultiple);
        craneMovement.goLeftRight();
        if(_modeControl == 1)
        {  
           BuildRocketControl();
        }
        Transformfunc.CenterOnChild(RocketParentTransform);
        CheckChildIndex(RocketParent);
    }

    private void FixedUpdate() {
          if (_modeControl == 2)
        {
            RocketFlyControl();
        }
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
        // RocketParent.transform.GetChild(0).gameObject.AddComponent<RocketController>();
        // rocketController = RocketParent.transform.GetChild(0).GetComponent<RocketController>();
        // rocketFirst = RocketParent.transform.GetChild(0).gameObject;
        
        Rigidbody2D RPrigid = RocketParent.AddComponent<Rigidbody2D>();
        RocketParent.AddComponent<RocketController>();
        // RocketParent.transform.GetChild(0).GetComponent<Rigidbody2D>();
        rocketController = RocketParent.GetComponent<RocketController>();
       
        RPrigid.gravityScale = 1;
    
        RPrigid.useAutoMass = true;


        startControlRocket = true;
        }
        
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        rocketController.RocketForward(yAxis,rocketPartsAmount);
        rocketController.RocketRotate(RocketParent,xAxis * -3);
       
       

    }

    private void CheckChildIndex(GameObject parent)
    {   
        // rocketPartsAmount = parent.transform.childCount;
        parent.transform.GetChild(0);
        // Debug.Log("Child = " + parent.transform.GetChild(0));
    }

}
