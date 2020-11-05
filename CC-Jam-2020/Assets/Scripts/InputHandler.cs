using System;
using Scriptables;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public BoolVariable actionInProgress;
    public WorldRotation worldRotation;
    public GridManager gridManager;
    
    private void Update()
    {
        if(actionInProgress.value) return;
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            if (gridManager.TryMoveObject(gridManager.character, Vector2Int.right))
                gridManager.Invoke(nameof(gridManager.SimulatePhysics), .5f);
               

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            if (gridManager.TryMoveObject(gridManager.character, Vector2Int.left))
                gridManager.Invoke(nameof(gridManager.SimulatePhysics), .5f);

        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
            worldRotation.RotateRoom(Vector2Int.right);
        
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.DownArrow))
            worldRotation.RotateRoom(Vector2Int.left);
    }
}