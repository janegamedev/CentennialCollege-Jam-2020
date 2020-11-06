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

    public GameObject[] buttons;
    
    private void Awake()
    {
        StartCoroutine(PlayText());
        teddyImage.DOFade(1, fade);
        backgroundImage.DOFade(1, fade);
    }
    
    IEnumerator PlayText()
    {
        foreach (char c in myText) 
        {
            my.text += c;
            yield return new WaitForSeconds (speed);
        }

        my.DOFade(0, fade);
        
        foreach (char c in teddyText) 
        {
            teddy.text += c;
            yield return new WaitForSeconds (speed);
        }

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }
}
