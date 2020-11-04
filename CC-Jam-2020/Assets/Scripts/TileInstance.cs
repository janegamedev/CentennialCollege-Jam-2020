using System;
using DG.Tweening;
using UnityEngine;
using Variables;

[System.Serializable]
public class TileInstance : MonoBehaviour
{
    public BoolVariable actionInProgress;
    [HideInInspector] public Tile data;
    [HideInInspector] public Vector2Int gridPos;
    public Ease ease;

    public void SetTile(GridManager gm, Tile d, Vector2Int pos)
    {
        data = d;
        gridPos = pos;
    }

    public void OnRoomRotated(int rot, float delay)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, -90 * rot);
        rotation *= transform.rotation;
        transform.DORotateQuaternion(rotation, delay).SetEase(ease);
    }

    public void Move(Vector3 pos)
    {
        if(!actionInProgress.value)
            actionInProgress.SetValue(true);
        transform.DOMove(pos, 0.5f).SetEase(Ease.OutBack).OnComplete(OnMoveEnd);
    }

    private void OnMoveEnd()
    {
        if(actionInProgress.value)
            actionInProgress.SetValue(false);
    }
}