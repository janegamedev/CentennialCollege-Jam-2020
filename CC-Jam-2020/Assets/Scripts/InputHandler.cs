using System;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public BoolVariable actionInProgress;
    public WorldRotation worldRotation;
    public GridManagerVariable currentGrid;
    public GameManager gameManager;
    [BoxGroup("SOUND")] public AudioClip rotateSound, moveSound;
    [BoxGroup("SOUND")] public AudioManager audioManager;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            gameManager.OnEsp();
        
        if(actionInProgress.value || currentGrid.value == null) return;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            if (currentGrid.value.TryMoveObject(currentGrid.value.character, Vector2Int.right))
            {
                currentGrid.value.Invoke(nameof(currentGrid.value.SimulatePhysics), .5f);
                audioManager.CallSfx(moveSound);
            }
        
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            if (currentGrid.value.TryMoveObject(currentGrid.value.character, Vector2Int.left))
            { 
                currentGrid.value.Invoke(nameof(currentGrid.value.SimulatePhysics), .5f);
                audioManager.CallSfx(moveSound);
            }


        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            worldRotation.RotateRoom(currentGrid.value.eRotation);
            audioManager.CallSfx(rotateSound);
        }


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            worldRotation.RotateRoom(currentGrid.value.qRotation);
            audioManager.CallSfx(rotateSound);
        }
            
        
    }
}