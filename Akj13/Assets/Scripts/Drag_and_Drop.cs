using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_and_Drop : MonoBehaviour
{
    private Vector3 dragOffset;
    private static Camera _cam;
    public static Camera cam
    {
        get
        {
            if (!_cam) _cam = Camera.main;
            return _cam;
        }
    }

    private Rigidbody2D _rb2D;
    public SnapPart snapTarget;

    public Rigidbody2D rb2D
    {
        get
        {
            if (!_rb2D) _rb2D = GetComponent<Rigidbody2D>();
            return _rb2D;
        }
    }

    public event Action onDragStarted;

    private void OnMouseDown()
    {
        if (!RobotGame.WeCanClick) return;
        dragOffset = transform.position - GetMousePos();
        rb2D.isKinematic = true;
        if(onDragStarted!=null) onDragStarted.Invoke();
    }


    private void OnMouseDrag()
    {
        if (!RobotGame.WeCanClick) return;
        transform.position = GetMousePos() + dragOffset;
    }

    void OnMouseUp()
    {
        if (!RobotGame.WeCanClick) return;
        if (snapTarget)
        {
            transform.position = snapTarget.transform.position;
            transform.rotation = Quaternion.identity;
            snapTarget.Snapped(this);
        }
        else
        {
            rb2D.isKinematic = false;
        }

        rb2D.angularVelocity = 0f;
        rb2D.velocity = Vector2.zero;
    }

    Vector3 GetMousePos ()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }


}
