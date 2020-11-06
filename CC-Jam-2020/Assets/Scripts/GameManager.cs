using System;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [BoxGroup("SETTINGS")] public GridManager startingRoom;
    
    [BoxGroup("VARIABLES")] public GridManagerVariable currentGrid;
    [BoxGroup("VARIABLES")] public BoolVariable actionInProgress, hasKey, hasSoul;
    [BoxGroup("VARIABLES")] public ObjectInstVariable currentPlayer;

    [BoxGroup("EVENTS")] public GameEvent onNewRoomAssign, onPlayerEnterUnderworld, onKeyUpdated, onSoulUpdated;

    public GameObject endCanvas;
    private GridManager prevRoom;
    
    private void Start()
    {
        hasKey.SetValue(false);
        hasSoul.SetValue(true);
        
        onKeyUpdated.Raise();
        onSoulUpdated.Raise();
        
        actionInProgress.SetValue(false);
        currentGrid.SetValue(startingRoom);
        currentPlayer.SetValue(currentGrid.value.SpawnCharacter());
        onNewRoomAssign.Raise();
    }
    
    public void OnPlayerMoved()
    {
        currentGrid.value.CheckForPlayer();
    }

    public void OnPlayerDeath()
    {
        hasKey.SetValue(false);
        onKeyUpdated.Raise();

        if(prevRoom != null)
            prevRoom.ResetRoom();
        
        currentGrid.value.RemoveCharacter();

        prevRoom = currentGrid.value;
        
        currentGrid.SetValue(currentGrid.value.underworldRoom);
        currentGrid.value.AddCharacter(currentPlayer.value);
       
        currentGrid.value.PlaceSoul();
        hasSoul.SetValue(false);
        onSoulUpdated.Raise();
           
        onPlayerEnterUnderworld.Raise();
    }
    
    public void OnPortalUse()
    {
        hasKey.SetValue(false);
        onKeyUpdated.Raise();
        
        prevRoom.ResetRoom();
        currentGrid.value.RemoveCharacter();
        
        prevRoom = currentGrid.value;
        currentGrid.SetValue(currentGrid.value.underworldRoom); // it wil be upperworld room
        
        /*currentGrid.value.PlaceKey();*/
        currentGrid.value.AddCharacter(currentPlayer.value);
        
        onPlayerEnterUnderworld.Raise();
    }

    public void OnRoomFinished()
    {
        if (currentGrid.value.nextRoom == null)
        {
            OnGameWin();
            return;
        }
        
        hasKey.SetValue(false);
        onKeyUpdated.Raise();
        
        prevRoom = null;
        Destroy(currentPlayer.value);
        
        currentGrid.SetValue(currentGrid.value.nextRoom);
        currentPlayer.SetValue(currentGrid.value.SpawnCharacter());
        onNewRoomAssign.Raise();
    }

    private void OnGameWin()
    {
        actionInProgress.SetValue(true);
        endCanvas.SetActive(true);
    }
}
