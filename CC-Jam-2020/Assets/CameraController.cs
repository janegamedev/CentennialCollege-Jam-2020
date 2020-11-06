using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scriptables;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridManagerVariable currentGrid;
    public Ease ease;
    public float defaultFOV = 20f, targetFOV = 10f;
    private Camera _camera;
    private Vector3 _roomPos, _defaultPos;

    private float _t, speed = .5f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _defaultPos = transform.position;
    }

    public void OnNewRoom()
    {
        transform.DOMove(_defaultPos, .5f).SetEase(ease).OnComplete(ZoomIn);
        if(_camera.orthographicSize < defaultFOV)
            _camera.DOOrthoSize(defaultFOV, .5f).SetEase(ease);
    }

    public void ZoomIn()
    {
        _roomPos = currentGrid.value.transform.position;
        _roomPos.z = _defaultPos.z;

        transform.DOMove(_roomPos, .5f).SetEase(ease);
        
        if(_camera.orthographicSize > targetFOV)
            _camera.DOOrthoSize(targetFOV, .5f).SetEase(ease);
    }
}
