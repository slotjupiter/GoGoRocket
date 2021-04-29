using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{   [Header("-- Dialog All Objects --")]
    [SerializeField] private GameObject _dialogTextObject;
    [SerializeField] private GameObject _dialogTextBox;
    [SerializeField] private GameObject _dialogName;
    [SerializeField] private GameObject _dialogNext;
    [SerializeField] private GameObject _dialogSkip;
    [SerializeField] SceneTransition sceneTransition;
    private Text _dialogText;
    private Text _dialogSpeakerName;
    [SerializeField] private PlayableDirector cutscene;
    [Header("-- Typing Speed and Name --")]
    [SerializeField] private float typingSpeed;
    [SerializeField] private string[] namesEN = new string[] {"Jonathan", "Uncle Gilbert", "Mipme Jr. (Pet)"};
    [SerializeField] private string[] namesTH = new string[] {"โจนาธาน", "ลุงกิลเบิร์ต", "มิปมี่ จูเนียร์ (สัตว์เลี้ยง)"};

    [System.Serializable]
    public struct Conversation
    {   
        [SerializeField] private string Description;
        [Header("-- 0 = Jonathan, 1 = Gilbert, 2 = Mipme --")]
        [SerializeField] public int indexname;
        [SerializeField] public string sentencesEn;
        [SerializeField] public string sentencesTh;    
    }
    [SerializeField] public Conversation[] conversation;

    int index;
    string _selectedLocale,en,th;
    bool selected = false;
    bool startDialog = false;
    bool stop = false;
    
    private void Awake() {
         if(PlayerPrefs.HasKey("selected-locale"))
        {   _selectedLocale = PlayerPrefs.GetString("selected-locale");}
    }
    
    private void Start() {
        _dialogTextObject.SetActive(false);
        _dialogText = _dialogTextObject.GetComponent<Text>();
        _dialogSpeakerName = _dialogName.GetComponent<Text>();
    }

    public void DialogStart() 
     {  
        if(_dialogTextBox.activeInHierarchy)
        { 
        startDialog = true;
        _dialogTextObject.SetActive(true);
        _dialogName.SetActive(true);
        // cutscene.Pause();
        cutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
        if(_dialogTextObject.activeInHierarchy)
        {   
             switch(_selectedLocale)
            {
                case "en":
                _dialogSpeakerName.text = ""; 
                _dialogText.text ="";
                StartCoroutine(TypingEn());
                break;
                case "th":
                _dialogSpeakerName.text = ""; 
                _dialogText.text ="";
                StartCoroutine(TypingTh());
                break;
            }   
        }
        }
     }

    public void DialogStop()
    {   
        stop = true;
        if(stop)
        {
        index++;
        _dialogTextObject.SetActive(false);
        _dialogName.SetActive(false);
        _dialogTextBox.SetActive(false);
        _dialogNext.SetActive(false);
        // cutscene.Resume();  
         cutscene.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
            

    IEnumerator TypingEn()
    {   _dialogSpeakerName.text = "[ " + namesEN[conversation[index].indexname] + " ]";  
        foreach(char letter in conversation[index].sentencesEn.ToCharArray())
        {  
            _dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        if(_dialogText.text == conversation[index].sentencesEn)
        {
            _dialogNext.SetActive(true);
        }
        else
        {
            _dialogNext.SetActive(false);
        }
    }

     IEnumerator TypingTh()
    {   _dialogSpeakerName.text = "[ " + namesTH[conversation[index].indexname] + " ]";   
        foreach(char letter in conversation[index].sentencesTh.ToCharArray())
        {    
            _dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if(_dialogText.text == conversation[index].sentencesTh)
        {
            _dialogNext.SetActive(true);
        }
        else
        {
            _dialogNext.SetActive(false);
        }
    }

    public void Update() {
        
        if(!startDialog && _dialogTextBox.activeInHierarchy)
        {
           DialogStart();  
        }
        else if(_dialogText.text == "" && _dialogTextBox.activeInHierarchy && !stop)
        {  
           DialogStop();
        }
        else if(!_dialogTextBox.activeInHierarchy)
        {   
            stop = false;
            startDialog = false; 
        }

       NextSentences();
       Skip();

    }

    private void NextSentences()
    {   
          if(Input.GetKeyDown(KeyCode.Space) && _dialogNext.activeInHierarchy)
        {
            switch(_selectedLocale)
            {
                case "en":
                if(index < conversation[index].sentencesEn.Length - 1)
                {    
                    index++;
                    _dialogText.text ="";
                    _dialogSpeakerName.text = "";
                    _dialogNext.SetActive(false);
                    StartCoroutine(TypingEn());  
                }
                else
                {   
                    _dialogSpeakerName.text = "";
                    _dialogText.text ="";
                    _dialogNext.SetActive(false);
                }
                break;
                case "th":
                if(index < conversation[index].sentencesTh.Length - 1)
                {     
                    index++;
                    _dialogText.text ="";
                    _dialogSpeakerName.text = "";
                    _dialogNext.SetActive(false);
                    StartCoroutine(TypingTh());  
                }
                else
                {   
                    _dialogSpeakerName.text = "";
                    _dialogText.text ="";
                    _dialogNext.SetActive(false);
                }
                break;
            }
            
        }
    }

    private void Skip()
    {   StartCoroutine(SkipButtonAppear(3f));
        if(Input.GetKeyDown(KeyCode.Escape))
        {   
            StartCoroutine(LoadingAsyncScene());       
        }
    }

    IEnumerator LoadingAsyncScene()
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

    IEnumerator SkipButtonAppear(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        _dialogSkip.SetActive(true);
    }
}
