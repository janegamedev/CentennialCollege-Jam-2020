using System;
using DG.Tweening;
using UnityEngine;
using Variables;

public class WorldRotation : MonoBehaviour
{
    public BoolVariable actionInProgress;
    private GridManager _gridManager;
    
    private int _rotation;

    private void Awake()
    {
        _gridManager = GetComponent<GridManager>();
        actionInProgress.value = false;
    }

    public void RotateRoom(Vector2Int rot)
    {
        actionInProgress.SetValue(true);
        _rotation = (int) rot.x;
        
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * _rotation);
        rotation *= transform.rotation;
        
        _gridManager.RotateGrid(_rotation);
        transform.DORotateQuaternion(rotation, 0.5f).SetEase(Ease.OutBack).OnComplete(OnRotationEnd);
        transform.DOScale(0.85f, 0.25f).SetEase(Ease.OutBack).OnComplete(OnScaleEnd);
    }

    private void OnScaleEnd()
    {
        transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
    }

    private void OnRotationEnd()
    {
        foreach (TileInstance instance in _gridManager.TilesToRotate)
        {
            instance.OnRoomRotated(_rotation, 0.2f);
        }

        Invoke(nameof(OnTileRotationEnd), .3f);
    }

    private void OnTileRotationEnd()
    {
        actionInProgress.SetValue(false);
        _gridManager.SimulatePhysics();
    }
}