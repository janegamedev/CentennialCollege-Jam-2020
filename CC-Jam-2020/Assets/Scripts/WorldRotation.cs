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
    private bool _isResetting;
    private float instant = 0f, defaultSpeed = 0.5f;

    public void RotateRoom(Vector2Int rot)
    {
        actionInProgress.SetValue(true);
        _rotation = (int) rot.x;
        
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * _rotation);
        rotation *=  currentGrid.value.transform.rotation;
        
        currentGrid.value.RotateGrid(_rotation);
        currentGrid.value.transform.DORotateQuaternion(rotation, _isResetting? instant : defaultSpeed).SetEase(Ease.OutBack).OnComplete(OnRotationEnd);
        currentGrid.value.transform.DOScale(0.85f, _isResetting? instant : defaultSpeed/2).SetEase(Ease.OutBack).OnComplete(OnScaleEnd);
    }
    
    private void OnScaleEnd()
    {
        currentGrid.value.transform.DOScale(1, _isResetting? instant : defaultSpeed/2).SetEase(Ease.OutBack);
    }

    private void OnRotationEnd()
    {
        foreach (ObjectInstance obj in currentGrid.value.RotationRequired)
        {
            obj.OnRoomRotated(_rotation, _isResetting? instant : defaultSpeed/3);
        }

        Invoke(nameof(OnTileRotationEnd), _isResetting? instant : defaultSpeed/3);
    }

    private void OnTileRotationEnd()
    {
        actionInProgress.SetValue(false);
        currentGrid.value.SimulatePhysics();
    }
}