using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{   
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] private string scene;

    private void OnEnable() {
        StartCoroutine(LoadingAsyncScene(scene));    
    }

     IEnumerator LoadingAsyncScene(string scene)
    {   
        sceneTransition.FadeInFadeOut();
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
