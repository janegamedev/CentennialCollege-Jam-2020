using System;
using DG.Tweening;
using UnityEngine;
using Variables;

[System.Serializable]
public class TileInstance : MonoBehaviour
{
    [HideInInspector] public Tile data;
    [HideInInspector] public Vector2Int gridPos;
    public Ease ease;
    private GridManager _gridManager;

    public void SetTile(GridManager gm, Tile d, Vector2Int pos)
    {
        data = d;
        gridPos = pos;
        _gridManager = gm;
    }

    public void OnRoomRotated(int rot, float delay)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, -90 * rot);
        rotation *= transform.rotation;
        transform.DORotateQuaternion(rotation, delay).SetEase(ease);
    }

    public void Move(Vector3 pos)
    {
        transform.DOMove(pos, 0.5f).SetEase(Ease.OutBack);
    }
}