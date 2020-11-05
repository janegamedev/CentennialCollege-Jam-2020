using DG.Tweening;
using Scriptables;
using UnityEngine;

public class ObjectInstance : MonoBehaviour
{
    public BoolVariable actionInProgress;
    [HideInInspector] public Object data;
    public Vector2Int gridPos;
    public Tile tile;
    public Ease ease;
    public GameEvent onPlayerMoved;
    
    public void SetObject(Object d)
    {
        data = d;
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

        if (data.isCharacter)
        {
            onPlayerMoved.Raise();
        }
    }

    public void DestroyObject()
    {
        tile.RemoveObject(this);
        Destroy(gameObject);
    }
}