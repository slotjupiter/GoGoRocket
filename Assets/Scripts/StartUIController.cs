using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
public class StartUIController : MonoBehaviour
{
    public Camera main;
    public Transform targetPosition;
    public GameObject startTextObject;
    public Text startText;
    [SerializeField] private AudioSource aud;
    [SerializeField] SceneTransition sceneTransition;
    Camera cameraHandler;
    Text pressanykeyText;
    Color blinkColor;

    public void Start() 
    {
    cameraHandler = main.GetComponent<Camera>();
    pressanykeyText = startText.GetComponent<Text>();
    blinkColor = pressanykeyText.color;
    }

    bool selected = false;
   
   public void LanguageSelected(string id)
   {   
    PlayerPrefs.SetString("selected-locale",id);
      // PlayerPrefs.Save();
      // SceneManager.LoadScene(1);
        if(PlayerPrefs.HasKey("selected-locale"))
        {   
        string lang = PlayerPrefs.GetString("selected-locale");
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(lang);
        PlayerPrefs.Save();
        }
    selected = true;
   }

    private void Update() {
        CameraMove();

        if(Input.anyKey && startTextObject.activeSelf)
        { 
          StartCoroutine(LoadingAsyncScene()); 
        }
    }
   void CameraMove()
   {    
    Vector3 cameraPos = cameraHandler.transform.position;
    Vector3 targetPos = new Vector3 (0,targetPosition.transform.position.y,0);
    float step = 12f * Time.deltaTime;
      if(selected)
      {
       cameraHandler.transform.position = Vector3.MoveTowards(cameraHandler.transform.position, targetPos, step);
       StartCoroutine(textToStart(0.3f)); 
      }
   }

   IEnumerator textToStart(float delayTime)
   {
    yield return new WaitForSeconds(delayTime); 
    startTextObject.SetActive(true);
    textBlink();
   }

   void textBlink()
    {
    if(startTextObject.activeSelf)
      {
        blinkColor.a = Mathf.Round(Mathf.PingPong(Time.unscaledTime * 2.0f, 1.5f));

        pressanykeyText.color = blinkColor;
      }
    }

      IEnumerator LoadingAsyncScene()
    {   GetComponent<AudioSource>().Play();
        sceneTransition.FadeInFadeOut();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CutScene");
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}