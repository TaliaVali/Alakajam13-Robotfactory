using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPart : MonoBehaviour
{
    public enum BodyType{Head, Body, Legs, ArmLeft, ArmRight}
    public BodyType bodyType;
    
    private Rigidbody2D _rb2D;
    public Rigidbody2D rb2D
    {
        get
        {
            if (!_rb2D) _rb2D = GetComponent<Rigidbody2D>();
            return _rb2D;
        }
    }
    
    private SpriteRenderer _renderer;
    public SpriteRenderer Renderer
    {
        get
        {
            if (!gameObject) return null;
            if (!_renderer) _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    public bool randomize = true;

    void Start()
    {
        if (randomize) Renderer.sprite = PartLibrary.GetSprite(bodyType);
        var boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = Renderer.sprite.rect.size / Renderer.sprite.pixelsPerUnit;
        RobotGame.onStartingRound += SelfDestruct;
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        RobotGame.onStartingRound -= SelfDestruct;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(rb2D.worldCenterOfMass,0.1f);
    }

}
