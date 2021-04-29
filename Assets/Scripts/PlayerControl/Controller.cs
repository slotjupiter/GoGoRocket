using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
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
    // [HideInInspector]
    public CraneMovement craneMovement;
    // [HideInInspector]
    public FuelBar fuelBar;
    [Header("--- Rocket ---")]
    public float speed = 50f;
    public float rotationSpeed = 3f;
    public float rcVelocity = 4f;
    public  ParticleSystem rocketFlameParticle;
    public GameObject fuelDecreaseParticle;
    [Header("--- Rocket Fuel UI ---")]
    public float fuel = 500f;
    public Text rocketPartsText;
    public Text rocketFuelNum;
    public float rocketFuelLeft;
    [Header("--- Camera ---")]
    [SerializeField] public Camera mainCamera;
    [SerializeField]GameObject buildCam;
    [SerializeField]GameObject rocketCam;
    [Header("--- Game UI ---")]
    [SerializeField] public GameUIController gameUiController;
    [SerializeField] private Text distanceText;
    [SerializeField] private Transform GoalPosition;
    [SerializeField] private GameObject impactParticle;
    [Header("--- Sound ---")]
    [SerializeField] private SoundManager Soundmanager;
    bool audioplay = false;
    RocketController rocketController;

    GameObject RocketParent,rocketFirst,gameUI;
    GameObject InstanceImpactParticle;
    Rigidbody2D _spawnrigidbody;
    FixedJoint2D _rocketJoint;
    Transform RocketParentTransform;
    Vector2 lastPosition;
    ParticleSystem  _rocketParticle;

    bool createRK,dropRK,startControlRocket,ejectRK = false;
    public bool canControl = true;
    bool GAMEOVER = false;
    bool canSpace = false;
    int _modeControl;
    int index = 0;
    int childIndex;
    float timer = 1f;
    
    float rocketPartsAmount,distanceTravelled;
    float lower;
  
    private void Start() {
        
        if(PlayerPrefs.HasKey("selected-locale"))
        {   
        string lang = PlayerPrefs.GetString("selected-locale");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(lang);
        PlayerPrefs.Save();
        }

        Soundmanager.Play("BGM",0.1f);
        buildCam.SetActive(true);

       GameUIController gameUiController = GetComponent<GameUIController>();
       _spawnrigidbody = spawnObject.GetComponent<Rigidbody2D>();
       RocketParent = GameObject.Find("RocketMain");
       rocketController = RocketParent.GetComponent<RocketController>();
       RocketParentTransform = RocketParent.transform;
       _modeControl = 1;
     
       starterRocket();
          if(fuelBar != null)
        {
            
            fuelBar.setMaxFuel(fuel);
            rocketFuelLeft = fuel;
        }
        else
        {
            return;
        }
    }

    public void Update() {
     
        rocketPartsAmount = RocketParent.transform.childCount;
      
        craneMovement.goLeftRight();

        Transformfunc.CenterOnChild(RocketParentTransform);
        CheckChild(RocketParent);
        
        switch(_modeControl)
        {
        case 1:
        BuildRocketControl();
        break;

        case 2:
        CameraController.CameraChange(buildCam,rocketCam);
        if(canControl)
        {
         float xAxis = Input.GetAxis("Horizontal");
         rocketController.RocketRotate(RocketParent,xAxis,rotationSpeed);
        }
        
        DistanceToMoon();
        FuelController();
        FuelBarUI();
       
            if(rocketFuelLeft <= 0 && !ejectRK)
            {   
                rocketFuelLeft = 0;
                distanceTravelled = 0;

                if(rocketPartsAmount != 1)
                {
                RocketEjection();
                ejectRK = true; 
                }
                
                if(ejectRK == true)
                {
                    fuelBar.setMaxFuel(fuel);
                    rocketFuelLeft = fuel;
                    if(rocketFuelLeft == fuel)
                    {
                        ejectRK = false;
                    }
                }
            }

        break;

        }
    }

    private void FixedUpdate() {
          if (_modeControl == 2 && canControl)
        {
            RocketFlyControl();
        }
    }

    private void BuildRocketControl()
    {
        timer += Time.deltaTime;
        if ( timer >= 0.5f )
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {  
            DropRocket();
            timer = 0;
            }
        }
    }
    
    private void starterRocket()
    {
        rocketBlockStart = Instantiate(rocketParts[0], spawnrocketPoint.transform.position, transform.rotation);
        rocketBlockStart.transform.SetParent(RocketParent.transform);
        _rocketJoint = rocketBlockStart.AddComponent<FixedJoint2D>();
        _rocketJoint.connectedBody = _spawnrigidbody;
        _rocketJoint.autoConfigureConnectedAnchor = false;
         
    }
    IEnumerator CreateRocket(float timeSpawn)
    {   
           yield return new WaitForSeconds(timeSpawn); 
            index++;
            rocketBlock = Instantiate(rocketParts[index], spawnrocketPoint.transform.position, transform.rotation);
            dropRK = false;
            craneMovement.goUp(2f);
            craneMovement.speed += craneUpSpeed;
            craneMovement.distanceMove += craneUpDistance;
        
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
        if(dropRK)
        {
           StartCoroutine(CreateRocket(1f)); 
        }
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

        _rocketParticle = Instantiate(rocketFlameParticle,RocketParentTransform.position,Quaternion.Euler(90, 0, 0));
        
        RocketParent.AddComponent<RocketController>();
        rocketController = RocketParent.GetComponent<RocketController>();
        startControlRocket = true;
        }
        
        float yAxis = Input.GetAxis("Vertical");
        
         if(yAxis >= 0.1 && !audioplay)
         {
            Soundmanager.Play("RocketFire",0.2f);
            audioplay = true;
         }
         else if(yAxis == 0 && audioplay)
         {
            audioplay = false;
            Soundmanager.Stop("RocketFire",0.2f);
         }
        
        rocketController.RocketForward(yAxis,rocketPartsAmount,speed);
   
        rocketController.RocketVelocity(rcVelocity);
       
        Transform _rocketParentChildzero = RocketParentTransform.GetChild(0).transform;
        _rocketParticle.transform.SetParent(_rocketParentChildzero);
        _rocketParticle.transform.position = _rocketParentChildzero.transform.position;
        _rocketParticle.startLifetime = yAxis + 0.5f;

    }

    void DistanceToMoon()
    {
        float distanceBetween = Vector2.Distance(RocketParent.transform.position,GoalPosition.transform.position);
        distanceText.text = (Mathf.RoundToInt(distanceBetween) * 100).ToString();
    }

    public void FuelController()
    {   
        distanceTravelled = Vector2.Distance(RocketParent.transform.position, lastPosition);
        lastPosition = RocketParent.transform.position;
         
        rocketFuelLeft = rocketFuelLeft - distanceTravelled;
        if(rocketController.takeDamage == true)
        {
            FuelDecrease(50f);
            rocketController.takeDamage = false;
            GameObject InstanceparticleFuel = Instantiate(fuelDecreaseParticle,fuelBar.transform.position, 
            fuelBar.transform.rotation);
            Destroy(InstanceparticleFuel,1f);
        }
         fuelBar.setFuel(rocketFuelLeft);
         
    }

    public void FuelDecrease(float damage)
    {   
        rocketFuelLeft -= damage;   
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

    
    private void CheckChild(GameObject parent)
    {   
        if(parent.transform.childCount == 1 && rocketFuelLeft <= 0)
        {   
            canControl = false;
            GAMEOVER = true;

            rocketController.RocketGravity(1f);
            rocketFuelLeft = 0;

            gameUiController.GameOver();

            if(GAMEOVER && Input.GetKeyDown(KeyCode.Space))
            {
                gameUiController.ResetGame();
           
            }
        } 
        
    }

    public void ImpactEffect(Collision2D col) 
    {   
        foreach (ContactPoint2D contact in col.contacts)
        {
        InstanceImpactParticle = Instantiate(impactParticle, contact.point, Quaternion.identity);
        }
        Destroy(InstanceImpactParticle,1f);
    }
    
}
