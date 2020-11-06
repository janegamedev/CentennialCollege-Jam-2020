using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenController : MonoBehaviour
{
    public Vector2 targetPosition = Vector2.zero;
    [Range(0.5f, 10.0f)] public float duration = 1.0f;
    public Ease ease = Ease.Linear;
    [Range(1.0f, 500.0f)] public float defaultScale, targetScale;
    public Vector3 targetAngle;
    public DoTweenType doTweenType = DoTweenType.MOVEMENT;

    public bool playOnAwake;

    private Sequence _sequence;
    public enum DoTweenType {
        MOVEMENT,
        SCALE,
        ROTATION,
    }

    private void OnEnable()
    {
        _sequence = DOTween.Sequence();
        if(playOnAwake)
            CallDoTween();
    }

    private void CallDoTween()
    {
        switch (doTweenType)
        {
            case DoTweenType.MOVEMENT:
                _sequence.Append(transform.DOLocalMove(targetPosition, duration).SetEase(ease)).SetLoops(-1, LoopType.Yoyo);
                break;
            case DoTweenType.SCALE:
                _sequence.Append(transform.DOScale(targetScale, duration).SetEase(ease)).SetLoops(-1, LoopType.Yoyo);
                break;
            case DoTweenType.ROTATION:
                _sequence.Append(transform.DOLocalRotate(targetAngle, duration).SetEase(ease)).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDisable()
    {
        _sequence.Kill();
    }
}
