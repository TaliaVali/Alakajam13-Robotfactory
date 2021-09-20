using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPart : MonoBehaviour
{
    public RobotPart.BodyType snapping;
    public SpriteRenderer correctSpriteRenderer;

    public static List<SnapPart> snapParts = new List<SnapPart>();

    private Collider2D potentialSnap;

    public RobotPart snapped;
    
    private SpriteRenderer _renderer;
    public SpriteRenderer Renderer
    {
        get
        {
            if (!_renderer) _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    void Start()
    {
        snapParts.Add(this);
        RobotGame.onStartingRound += () =>
        {
            Renderer.enabled = true;
        };
    }

    public static bool WinCheck()
    {
        foreach (var snapPart in snapParts)
        {
            if (!snapPart.snapped) return false;
            if (snapPart?.snapped?.Renderer.sprite != snapPart?.correctSpriteRenderer.sprite)
            {
                return false;
            }
        }
        return true;
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
        UnSnap();
        Renderer.enabled = false;
        snapped = dragAndDrop.GetComponent<RobotPart>();
        dragAndDrop.onDragStarted -= DragReact;
        dragAndDrop.onDragStarted += DragReact;
    }

    void DragReact()
    {
        Renderer.enabled = true;
        snapped = null;
    }

    public void UnSnap()
    {
        if (snapped != null)
        {
            snapped.rb2D.isKinematic = false;
            snapped.rb2D.AddForce(Random.insideUnitCircle.normalized * 15f, ForceMode2D.Impulse);
            snapped.GetComponent<Drag_and_Drop>().onDragStarted -= DragReact;
            snapped = null;
        }
        Renderer.enabled = true;
    }
}
