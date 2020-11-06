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
    private bool isResetting;
    private float instant = 0f, defaultSpeed = 0.5f;

    public void RotateRoom(Vector2Int rot)
    {
        actionInProgress.SetValue(true);
        _rotation = (int) rot.x;
        
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * _rotation);
        rotation *=  currentGrid.value.transform.rotation;
        
        currentGrid.value.RotateGrid(_rotation);
        currentGrid.value.transform.DORotateQuaternion(rotation, isResetting? instant : defaultSpeed).SetEase(Ease.OutBack).OnComplete(OnRotationEnd);
        currentGrid.value.transform.DOScale(0.85f, isResetting? instant : defaultSpeed/2).SetEase(Ease.OutBack).OnComplete(OnScaleEnd);
    }

    public void ResetRoom()
    {
        Vector3 desireRotation = currentGrid.value.playerInitRotation;

        int times;
        Vector2Int dir;
        
        float current = currentGrid.value.transform.rotation.z;
        if (current > 0)
        {
            times = (int) (current / 90);
            dir = Vector2Int.right;
        }
        else
        {
            times = 1;
            dir = Vector2Int.left;
        }

        for (int i = 0; i < times; i++)
        {
            RotateRoom(dir);
        }
    }

    IEnumerator Rotate(int times, Vector2Int dir)
    {
        int t = 0;
        isResetting = true;
        
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            t++;
            RotateRoom(dir);
            
            if(t == times)
                break;
        }

        isResetting = false;
    }
    private void OnScaleEnd()
    {
        currentGrid.value.transform.DOScale(1, isResetting? instant : defaultSpeed/2).SetEase(Ease.OutBack);
    }

    private void OnRotationEnd()
    {
        foreach (ObjectInstance obj in currentGrid.value.RotationRequired)
        {
            obj.OnRoomRotated(_rotation, isResetting? instant : defaultSpeed/3);
        }

        Invoke(nameof(OnTileRotationEnd), isResetting? instant : defaultSpeed/3);
    }

    private void OnTileRotationEnd()
    {
        actionInProgress.SetValue(false);
        currentGrid.value.SimulatePhysics();
    }
}