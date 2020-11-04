using System;
using UnityEngine;
using Variables;

public class InputHandler : MonoBehaviour
{
    public BoolVariable worldInRotation;
    public WorldRotation worldRotation;
    public GridManager gridManager;
    
    private void Update()
    {
        if(worldInRotation.value) return;
        
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            gridManager.MoveCharacter(Vector2Int.right);
        
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            gridManager.MoveCharacter(Vector2Int.left);
        
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
            worldRotation.RotateRoom(Vector2Int.right);
        
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.DownArrow))
            worldRotation.RotateRoom(Vector2Int.left);
    }
}