using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UpdateTmpFromString : UIPropertyUpdater
{
    public float speed, fade;
    public StringVariable target;
    public TextMeshProUGUI targetText;
    
    [BoxGroup("SOUND")] public AudioClip thoughtsSound;
    [BoxGroup("SOUND")] public AudioManager audioManager;

    [ContextMenu("Raise")]
    public override void Raise()
    {
        targetText.text = "";
        targetText.DOFade(1, .2f);
        StartCoroutine(nameof(PlayText));
    }

    IEnumerator PlayText()
    {
        audioManager.CallSfx(thoughtsSound);
        
        foreach (char c in target.value) 
        {
            targetText.text += c;
            yield return new WaitForSeconds (speed);
        }

        targetText.DOFade(0, fade);
    }
}