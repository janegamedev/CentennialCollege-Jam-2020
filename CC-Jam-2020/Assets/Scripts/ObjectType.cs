using System;
using Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Variables/ObjectType")]
public class ObjectType : ScriptableObject
{
    public AudioClip clip;
    public StringVariable messageVariable;
    public GameEvent messageUpdated;

    public float precentage = .7f;
    public string[] messages;

    [NonSerialized] public bool FirstTimeCalled = false;
    
    public virtual void Interact(ObjectInstance inst)
    { 
        if(clip != null)
            AudioManager.Instance.CallSfx(clip);
        if(messages != null && Random.Range(0,1) >= precentage || !FirstTimeCalled)
            ChooseMessage();
    }

    private void ChooseMessage()
    {
        messageVariable.Set(messages[Random.Range(0, messages.Length)]);
        messageUpdated.Raise();
        FirstTimeCalled = true;
    }
}