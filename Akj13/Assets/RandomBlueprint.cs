using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlueprint : MonoBehaviour
{
    public SpriteRenderer head, body, legs, armleft, armright;

    void Start()
    {
        Shuffle();
    }

    [ContextMenu("Shuffle")]
    public void Shuffle()
    {
        head.sprite = PartLibrary.GetSprite(RobotPart.BodyType.Head);
        body.sprite = PartLibrary.GetSprite(RobotPart.BodyType.Body);
        legs.sprite = PartLibrary.GetSprite(RobotPart.BodyType.Legs);
        armleft.sprite = PartLibrary.GetSprite(RobotPart.BodyType.ArmLeft);
        armright.sprite = PartLibrary.GetSprite(RobotPart.BodyType.ArmRight);
    }
    
    
}
