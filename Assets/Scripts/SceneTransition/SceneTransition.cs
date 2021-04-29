using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image imageTransition;
    [SerializeField] private Animator transition;
    Color colorImageTransition;
    bool resetVal;
    float _time = 0;

    public void FadeInFadeOut()
    {
        transition.SetTrigger("Change");
    }
}
