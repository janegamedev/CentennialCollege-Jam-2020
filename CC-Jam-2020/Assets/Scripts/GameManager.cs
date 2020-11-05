using System;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [BoxGroup("SETTINGS")] public GridManager startingRoom;
    
    [BoxGroup("VARIABLES")] public GridManagerVariable currentGrid;
    [BoxGroup("VARIABLES")] public BoolVariable actionInProgress;
    [BoxGroup("VARIABLES")] public ObjectInstVariable currentPlayer;
    
    private void Start()
    {
        actionInProgress.SetValue(false);
        currentGrid.SetValue(startingRoom);
        currentPlayer.SetValue(currentGrid.value.SpawnCharacter());
    }
    
    public void OnPlayerMoved()
    {
        currentGrid.value.CheckForPlayer();
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Death");
        currentGrid.value.RemoveCharacter();
        currentGrid.SetValue(currentGrid.value.underworldRoom);
        currentGrid.value.AddCharacter(currentPlayer.value);
    }

    public void OnRoomFinished()
    {
        if(currentGrid.value.nextRoom != null) return;
        
        currentGrid.value.RemoveCharacter();
        Destroy(currentPlayer.value);
        currentGrid.SetValue(currentGrid.value.nextRoom);
        currentPlayer.SetValue(currentGrid.value.SpawnCharacter());
    }
}
