using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RobotGame : MonoBehaviour
{
    private static RobotGame _instance;
    public static RobotGame Instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType<RobotGame>();
            return _instance;
        }
    }

    public static event Action onStartingRound; 
    
    public Vector2 spawnBounds;

    public RandomBlueprint targetBlueprint;
    public RobotPart partsPrefab;

    public Vector2Int rangeHeads = new Vector2Int(2,4), 
        rangeBodies= new Vector2Int(2,4), 
        rangeLegs= new Vector2Int(1,3), 
        rangeArms= new Vector2Int(1,3);
    
    [ContextMenu("Start Round")]
    public void StartRound()
    {
        if(onStartingRound != null) onStartingRound.Invoke();
        targetBlueprint.Shuffle();

        List<RobotPart> newParts = new List<RobotPart>();
        partsPrefab.gameObject.SetActive(false);
        partsPrefab.randomize = false;
        var winHead = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        winHead.Renderer.sprite = targetBlueprint.head.sprite;
        winHead.bodyType = RobotPart.BodyType.Head;
        newParts.Add(winHead);
        var winBody = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        winBody.Renderer.sprite = targetBlueprint.body.sprite;
        winBody.bodyType = RobotPart.BodyType.Body;
        newParts.Add(winBody);
        var winLegs = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        winLegs.Renderer.sprite = targetBlueprint.legs.sprite;
        winLegs.bodyType = RobotPart.BodyType.Legs;
        newParts.Add(winLegs);
        var winArmLeft = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        winArmLeft.Renderer.sprite = targetBlueprint.armleft.sprite;
        winArmLeft.bodyType = RobotPart.BodyType.ArmLeft;
        newParts.Add(winArmLeft);
        var winArmRight = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        winArmRight.Renderer.sprite = targetBlueprint.armright.sprite;
        winArmRight.bodyType = RobotPart.BodyType.ArmRight;
        newParts.Add(winArmRight);

        SpawnRandomPartsIntoList(rangeHeads, RobotPart.BodyType.Head, ref newParts);
        SpawnRandomPartsIntoList(rangeBodies, RobotPart.BodyType.Body, ref newParts);
        SpawnRandomPartsIntoList(rangeLegs, RobotPart.BodyType.Legs, ref newParts);
        SpawnRandomPartsIntoList(rangeArms, RobotPart.BodyType.ArmLeft, ref newParts);
        SpawnRandomPartsIntoList(rangeArms, RobotPart.BodyType.ArmRight, ref newParts);
        
        partsPrefab.gameObject.SetActive(true);
        partsPrefab.randomize = true;

        StartCoroutine(EnableBodyParts(newParts));
    }

    void SpawnRandomPartsIntoList(Vector2Int range, RobotPart.BodyType bodyType, ref List<RobotPart> list)
    {
        int rand = Random.Range(range.x, range.y);
        for (int i = 0; i < rand; i++)
        {
            var part = Instantiate(partsPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            part.randomize = true;
            part.bodyType = bodyType;
            list.Add(part);
        }
    }

    IEnumerator EnableBodyParts(List<RobotPart> list)
    {
        int randIndex = Random.Range(0, list.Count);
        var part = list[randIndex];
        part.gameObject.SetActive(true);
        list.RemoveAt(randIndex);
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        if (list.Count > 0) StartCoroutine(EnableBodyParts(list));
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, spawnBounds);
    }

    public static Vector2 GetRandomSpawnPosition()
    {
        Vector2 minBounds = Instance.transform.TransformPoint(-Instance.spawnBounds / 2f);
        Vector2 maxBounds = Instance.transform.TransformPoint(Instance.spawnBounds / 2f);

        return new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
    }
}
