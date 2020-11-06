using System;
using System.Collections;
using DG.Tweening;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldRotation : MonoBehaviour
{
    [BoxGroup("VARIABLES")] public GridManagerVariable currentGrid;
    [BoxGroup("VARIABLES")] public BoolVariable actionInProgress;
    
    private int _rotation;
    private float instant = 0f, defaultSpeed = 0.5f;

    public void RotateRoom(Vector2Int rot)
    {
        actionInProgress.SetValue(true);
        _rotation = (int) rot.x;
        
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * _rotation);
        rotation *=  currentGrid.value.transform.rotation;
        
        currentGrid.value.RotateGrid(_rotation);
        currentGrid.value.transform.DORotateQuaternion(rotation, defaultSpeed).SetEase(Ease.OutBack).OnComplete(OnRotationEnd);
        currentGrid.value.transform.DOScale(0.85f,  defaultSpeed/2).SetEase(Ease.OutBack).OnComplete(OnScaleEnd);
    }
    
    private void OnScaleEnd()
    {
        currentGrid.value.transform.DOScale(1, defaultSpeed/2).SetEase(Ease.OutBack);
    }

    private void OnRotationEnd()
    {
        foreach (ObjectInstance obj in currentGrid.value.RotationRequired)
        {
            if(obj == null) continue;
            
            obj.OnRoomRotated(_rotation, defaultSpeed/3);
        }

        Invoke(nameof(OnTileRotationEnd), defaultSpeed);
    }

    private void OnTileRotationEnd()
    {
        actionInProgress.SetValue(false);
        currentGrid.value.SimulatePhysics();
    }
}