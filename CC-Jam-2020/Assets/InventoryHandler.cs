using System.Collections;
using System.Collections.Generic;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    [BoxGroup("UI")] public Image keyImage, soulImage;

    [BoxGroup("SOUL")] public Sprite hasSoulSprite, noSoulSprite;
    [BoxGroup("SOUL")] public float hasSoulOpacity, noSoulOpacity;
    
    [BoxGroup("KEY")] public Sprite hasKeySprite, noKeySprite;
    [BoxGroup("KEY")] public float hasKeyOpacity, noKeyOpacity;
    
    [BoxGroup("VARIABLES")] public BoolVariable hasKey, hasSoul;

    public void OnKeyUpdate()
    {
        keyImage.sprite = hasKey.value ? hasKeySprite : noKeySprite;
        Color c = Color.white;
        c.a = hasKey.value ? hasKeyOpacity : noKeyOpacity;
        keyImage.color = c;
    }
    
    public void OnSoulUpdate()
    {
        soulImage.sprite = hasSoul.value ? hasSoulSprite : noSoulSprite;
        Color c = Color.white;
        c.a = hasSoul.value ? hasSoulOpacity : noSoulOpacity;
        soulImage.color = c;
    }
}
