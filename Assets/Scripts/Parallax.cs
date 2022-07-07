using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveRate;
    public float moveMin = -1.0f;
    public float moveMax = 1.0f;
    public bool frozeY;

    private float _startX;
    private float _startY;

    private void Start()
    {
        var position = transform.position;
        _startX = position.x;
        _startY = position.y;
    }

    private void Update()
    {
        var t = transform;
        var movedPosition = t.position;

        var moveX = Math.Max(cameraTransform.position.x * moveRate, moveMin);
        moveX = Math.Min(moveX, moveMax);
        movedPosition.x = _startX + moveX;

        if (!frozeY)
        {
            var moveY = Math.Max(cameraTransform.position.y * moveRate, moveMin);
            moveY = Math.Min(moveY, moveMax);
            movedPosition.y  = _startY + moveY;
        }
        
        t.position = movedPosition;
    }
}