using System;
using DG.Tweening;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldRotation : MonoBehaviour
{
    [BoxGroup("VARIABLES")] public GridManagerVariable currentGrid;
    [BoxGroup("VARIABLES")] public BoolVariable actionInProgress;

    private int _rotation;

    public void RotateRoom(Vector2Int rot)
    {
        actionInProgress.SetValue(true);
        _rotation = (int) rot.x;
        
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * _rotation);
        rotation *=  currentGrid.value.transform.rotation;
        
        currentGrid.value.RotateGrid(_rotation);
        currentGrid.value.transform.DORotateQuaternion(rotation, 0.5f).SetEase(Ease.OutBack).OnComplete(OnRotationEnd);
        currentGrid.value.transform.DOScale(0.85f, 0.25f).SetEase(Ease.OutBack).OnComplete(OnScaleEnd);
    }

    private void OnScaleEnd()
    {
        currentGrid.value.transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
    }

    private void OnRotationEnd()
    {
        foreach (ObjectInstance obj in currentGrid.value.RotationRequired)
        {
            obj.OnRoomRotated(_rotation, 0.2f);
        }

        Invoke(nameof(OnTileRotationEnd), .3f);
    }

    private void OnTileRotationEnd()
    {
        actionInProgress.SetValue(false);
        currentGrid.value.SimulatePhysics();
    }
}