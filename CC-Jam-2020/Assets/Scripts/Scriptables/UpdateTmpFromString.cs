using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpdateTmpFromString : UIPropertyUpdater
{
    public float speed, fade;
    public StringVariable target;
    public TextMeshProUGUI targetText;

    [ContextMenu("Raise")]
    public override void Raise()
    {
        targetText.text = "";
        targetText.DOFade(1, .2f);
        StartCoroutine(nameof(PlayText));
    }

    IEnumerator PlayText()
    {
        foreach (char c in target.value) 
        {
            targetText.text += c;
            yield return new WaitForSeconds (speed);
        }

        targetText.DOFade(0, fade);
    }
}