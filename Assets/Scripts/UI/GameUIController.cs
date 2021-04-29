using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{   
    [Header("GAMEOVER")]
    // GAMEOVER
    [SerializeField] private GameObject[] gameoverImage;
    [SerializeField] private Image gameoverBGImage;
    [SerializeField] SceneTransition sceneTransition;
    private Image chooseImage;
    [SerializeField] private Text gameoverText; 
    [SerializeField] private float delayTimeLogo = 1.5f,delayTimeText = 2f;
    [Header("ENDGAME")]
    // GOAL
    [SerializeField] private Image gameendImage;
    [SerializeField] private Image gameendBGImage;
    [SerializeField] private Text[] gameendText; 
    [SerializeField] private float delayTimeImage = 2f,delayTimeEndText = 3f;

    [SerializeField] private SoundManager Soundmanager;
    float _timer = 0f;
    float _timer2 = 0f;
    float _timer3 = 0f;
    float _timer4 = 0f;
    int indexImage;
    bool workOnce = false;
    bool canReset = false;
    bool gameEnd = false;
    
    public void GameOver()
    {   
        if(!workOnce)
        {
            StartCoroutine(ShowGameOver(delayTimeLogo,delayTimeText));
        }    
    }

    public void ResetGame()
    {   
        if(canReset)
        {   
           StartCoroutine(LoadingAsyncScene()); 
        } 
    }

    public void EndGame()
    {
        StartCoroutine(ShowGameEnd(delayTimeImage,delayTimeEndText));    
    }

    private IEnumerator ShowGameOver(float delayTimeLogo,float delayTimeText)
    {   Soundmanager.StopAndPlayWithVolume("Die",0.2f,0.3f);
        gameoverBGImage.enabled = true;
        Color BGimageColor = gameoverBGImage.color;
        _timer += Time.deltaTime / 1f;
        BGimageColor.a = _timer;
        gameoverBGImage.color = BGimageColor;

        yield return new WaitForSeconds(delayTimeLogo);
        
        if(!workOnce)
        {
        indexImage = UnityEngine.Random.Range(0,gameoverImage.Length);
        gameoverImage[indexImage].SetActive(true); 
        workOnce = true;
        }

        chooseImage = gameoverImage[indexImage].GetComponent<Image>();
        Color imageColor = chooseImage.color;
        _timer2 += Time.deltaTime /1f;
        imageColor.a = _timer2;
        chooseImage.color = imageColor;
        
        yield return new WaitForSeconds(delayTimeText);
        canReset = true;
        gameoverText.enabled = true;
        
    }

    private IEnumerator LoadingAsyncScene()
    {   
        sceneTransition.FadeInFadeOut();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator ShowGameEnd(float delayTimeImage,float delayTimeEndText)
    {   
        gameendBGImage.enabled = true;
        Color EndBGColor = gameendBGImage.color;
        _timer3 += Time.deltaTime / 1f;
        EndBGColor.a = _timer3;
        gameendBGImage.color = EndBGColor;
        
        if(!gameEnd)
        {
        Soundmanager.StopAndPlayWithVolume("Die",0.2f,0.3f);
        gameEnd = true;
        }

        yield return new WaitForSeconds(delayTimeImage);

        gameendImage.enabled = true;
        Color EndimageColor = gameendImage.color;
        _timer4 += Time.deltaTime /1f;
        EndimageColor.a = _timer4;
        gameendImage.color = EndimageColor;

        yield return new WaitForSeconds(delayTimeEndText);

        foreach (Text t in gameendText){
        yield return new WaitForSeconds(2f);
        t.enabled = true;
        }
        
    }
}
