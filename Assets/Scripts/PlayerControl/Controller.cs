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
    public float craneUpSpeed;
    [HideInInspector]
    public CraneMovement craneMovement;

    GameObject RocketParent;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
  
    bool createRK,dropRK = false;
    int modeControl;
    int index = 0;

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
            craneMovement.goUp(1.0f);
            craneMovement.speed += craneUpSpeed;
        }
        _rocketJoint = rocketBlock.AddComponent<FixedJoint2D>();
        _rocketJoint.connectedBody = _spawnrigidbody;
        
         
    } 

    void DropRocket()
    {   
        dropRK = true;
        Destroy(_rocketJoint);
        StartCoroutine(CreateRocket(1f));
         if(index == rocketParts.Length - 1)
            {
                buildCrane.SetActive(false);
            }
    }

    void RocketFlyControl()
    {

    }

}
