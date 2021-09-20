using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPart : MonoBehaviour
{
    public RobotPart.BodyType snapping;
    public SpriteRenderer correctSpriteRenderer;

    private Collider2D potentialSnap;
    
    private SpriteRenderer _renderer;
    public SpriteRenderer Renderer
    {
        get
        {
            if (!_renderer) _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var drag = other.GetComponent<Drag_and_Drop>();
        var part = other.GetComponent<RobotPart>();
        if (drag && part && part.bodyType == snapping)
        {
            potentialSnap = other;
            drag.snapTarget = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == potentialSnap)
        {
            other.GetComponent<Drag_and_Drop>().snapTarget = null;
        }
    }

    public void Snapped(Drag_and_Drop dragAndDrop)
    {
        Renderer.enabled = false;
        dragAndDrop.onDragStarted += () => Renderer.enabled = true;
    }
}
