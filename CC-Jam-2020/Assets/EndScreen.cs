using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Image teddyImage, backgroundImage; 
    public TextMeshProUGUI my, teddy;
    public float speed, fade;
    public string myText, teddyText;

    public bool doFade;
    public GameObject[] buttons;
    
    private void Awake()
    {
        my.text = "";
        teddy.text = "";
        StartCoroutine(PlayText());
        teddyImage.DOFade(1, fade).SetUpdate(true);
        backgroundImage.DOFade(1, fade).SetUpdate(true);
    }
    
    IEnumerator PlayText()
    {
        foreach (char c in myText) 
        {
            my.text += c;
            yield return new WaitForSecondsRealtime (speed);
        }

        if(doFade)
            my.DOFade(0, fade).SetUpdate(true);
        
        foreach (char c in teddyText) 
        {
            teddy.text += c;
            yield return new WaitForSecondsRealtime (speed);
        }

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }
}
