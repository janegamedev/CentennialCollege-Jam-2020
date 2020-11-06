using System;
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

    private Sequence _sequence;

    private Animator _anim;
    private void OnEnable()
    {
        _sequence = DOTween.Sequence();
    }

    public void SetObject(Object d)
    {
        data = d;
        if (data.isCharacter)
            _anim = GetComponent<Animator>();
    }

    public void OnRoomRotated(int rot, float delay)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, -90 * rot);
        rotation *= transform.rotation;
        _sequence.Append(transform.DORotateQuaternion(rotation, delay).SetEase(ease));
    }

    public void SetRotation(Vector3 final, float delay)
    {
        _sequence.Append(transform.DORotate(final, delay).SetEase(ease));
    }

    public void Move(Vector3 pos)
    {
        if(!actionInProgress.value)
            actionInProgress.SetValue(true);
        _sequence.Append(transform.DOMove(pos, 0.5f).SetEase(Ease.OutBack).OnComplete(OnMoveEnd));

        if (data.isCharacter)
        {
            bool isFalling = Math.Abs(pos.y - transform.position.y) > 0.5f;
            
            if(!isFalling)
            {
                Vector3 scale = transform.localScale;
                scale.x = pos.x > transform.position.x ? 1 : -1;
                transform.localScale = scale;
            }
           
            _anim.SetBool(isFalling? "falling" : "walking", true);
        }
        
    }

    private void OnMoveEnd()
    {
        if(actionInProgress.value)
            actionInProgress.SetValue(false);

        if (data.isCharacter)
        {
            _anim.SetBool("walking", false);
            _anim.SetBool("falling", false);
            onPlayerMoved.Raise();
        }
    }

    private void OnDisable()
    {
        _sequence.Kill();
    }

    public void DestroyObject()
    {
        tile.RemoveObject(this);
        Destroy(gameObject);
    }
}