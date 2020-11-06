using System;
using Scriptables;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public BoolVariable actionInProgress;
    public WorldRotation worldRotation;
    public GridManagerVariable currentGrid;
    
    private void Update()
    {
        if(actionInProgress.value || currentGrid.value == null) return;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            if (currentGrid.value.TryMoveObject(currentGrid.value.character, Vector2Int.right))
                currentGrid.value.Invoke(nameof(currentGrid.value.SimulatePhysics), .5f);
               

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            if (currentGrid.value.TryMoveObject(currentGrid.value.character, Vector2Int.left))
                currentGrid.value.Invoke(nameof(currentGrid.value.SimulatePhysics), .5f);

        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
            worldRotation.RotateRoom(currentGrid.value.eRotation);
        
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.DownArrow))
            worldRotation.RotateRoom(currentGrid.value.qRotation);
    }
}