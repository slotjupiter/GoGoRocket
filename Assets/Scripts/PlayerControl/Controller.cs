using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector]
    public FuelBar fuelBar;
    [Header("--- Rocket ---")]
    public float speed = 50f;
    public float rotationSpeed = 3f;
    public float rcVelocity = 4f;
    public  ParticleSystem rocketFlameParticle;
    [Header("--- Rocket Fuel UI ---")]
    public float fuel = 500f;
    public Text rocketPartsText;
    public Text rocketFuelNum;
    
    RocketController rocketController;

    GameObject RocketParent,rocketFirst;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
    Transform RocketParentTransform;
    Vector2 lastPosition;
  
    bool createRK,dropRK,startControlRocket,ejectRK = false;
    int _modeControl;
    int index = 0;
    int childIndex;
    float rocketPartsAmount,rocketFuelLeft,distanceTravelled;
    
    private void Start() {
       _spawnrigidbody = spawnObject.GetComponent<Rigidbody2D>();
       RocketParent = GameObject.Find("RocketMain");
       RocketParentTransform = RocketParent.transform;
       _modeControl = 1;
       rocketFlameParticle.startLifetime = 0.1f;
       starterRocket();

    }

    public void Update() {

        rocketPartsAmount = RocketParent.transform.childCount;

        craneMovement.goLeftRight();

        Transformfunc.CenterOnChild(RocketParentTransform);
        CheckChildIndex(RocketParent);
        
        switch(_modeControl)
        {
        case 1:
        BuildRocketControl();
        break;

        case 2:
        FuelController();
        FuelBarUI();
        fuelBar.setMaxFuel(fuel);
        fuelBar.setFuel(rocketFuelLeft);
        lastPosition = RocketParent.transform.position;

            if(rocketFuelLeft < 0 && !ejectRK)
            {    
                distanceTravelled = 0;
                RocketEjection();
                ejectRK = true;
                if(ejectRK)
                {
                    ejectRK = false;
                }
            }

        break;
        }
    }

    private void FixedUpdate() {
          if (_modeControl == 2)
        {
            RocketFlyControl();
        }
    }

    private void BuildRocketControl()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {  
           DropRocket();
        }
    
    }

    private void starterRocket()
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

    IEnumerator ChangeTagChild(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Transformfunc.ChangeChildTag(RocketParentTransform);
    }

    void DropRocket()
    {   
        dropRK = true;

        Destroy(_rocketJoint);

        StartCoroutine(CreateRocket(0.5f));
         if(index == rocketParts.Length - 1)
            {   
                StartCoroutine(ChangeTagChild(0.5f));
                buildCrane.SetActive(false);
                _modeControl = 2;
            }
    }

    void RocketFlyControl()
    {      
        if(!startControlRocket)
        {
        Rigidbody2D RPrigid = RocketParent.AddComponent<Rigidbody2D>();
        RPrigid.gravityScale = 1;
        RPrigid.useAutoMass = true;
        //  ParticleSystem rocketParticle = Instantiate(rocketFlameParticle,RocketParentTransform.GetChild(0).position, transform.rotation);
        //  rocketParticle.SetParent(RocketParent.transform);
        RocketParent.AddComponent<RocketController>();
        rocketController = RocketParent.GetComponent<RocketController>();
        startControlRocket = true;

        }
        
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        rocketController.RocketForward(yAxis,rocketPartsAmount,speed);
        rocketController.RocketRotate(RocketParent,xAxis,rotationSpeed);
        rocketController.RocketVelocity(rcVelocity);
        rocketFlameParticle.startLifetime = yAxis +1;

    }

    private void FuelController()
    {
        distanceTravelled += Vector2.Distance(RocketParent.transform.position, lastPosition);
        lastPosition = RocketParent.transform.position;
        rocketFuelLeft = fuel - (Mathf.Abs(distanceTravelled));
    }

    private void RocketEjection()
    {
        RocketParentTransform.GetChild(0).gameObject.AddComponent<Rigidbody2D>();
        RocketParentTransform.GetChild(0).gameObject.tag = "EmptyFuel";
        RocketParentTransform.GetChild(0).position += Vector3.down * Time.deltaTime;
        RocketParentTransform.GetChild(0).parent = null;
    }

    private void FuelBarUI()
    {
        string nameofRocketComponents = RocketParentTransform.GetChild(0).gameObject.name.Replace("(Clone)", "");
        rocketPartsText.text = nameofRocketComponents;
        rocketFuelNum.text = "" + Mathf.RoundToInt(rocketFuelLeft/5);
    }

    private void CheckChildIndex(GameObject parent)
    {   
        // rocketPartsAmount = parent.transform.childCount;
        parent.transform.GetChild(0);
        // Debug.Log("Child = " + parent.transform.GetChild(0));
    }

}
