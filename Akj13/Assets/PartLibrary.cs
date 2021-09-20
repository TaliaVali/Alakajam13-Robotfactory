using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PartLibrary : MonoBehaviour
{
    private static PartLibrary _instance;
    public static PartLibrary Instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType<PartLibrary>();
            return _instance;
        }
    }
    
    public Sprite[] heads, bodies, legs, armsLeft, armsRight;

    public static Sprite GetSprite(RobotPart.BodyType type) => Instance.getSprite(type);
    public Sprite getSprite( RobotPart.BodyType type )
    {
        switch (type)
        {
            case RobotPart.BodyType.Head:
                return heads[Random.Range(0, heads.Length)];
            case RobotPart.BodyType.Body:
                return bodies[Random.Range(0, bodies.Length)];
            case RobotPart.BodyType.Legs:
                return legs[Random.Range(0, legs.Length)];
            case RobotPart.BodyType.ArmLeft:
                return armsLeft[Random.Range(0, armsLeft.Length)];
            case RobotPart.BodyType.ArmRight:
                return armsRight[Random.Range(0, armsRight.Length)];
            default: return null;
        }
    }
    
}
