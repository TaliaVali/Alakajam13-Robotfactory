using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public static bool WeCanClick = true;
    
    public Vector2 spawnBounds;

    public RandomBlueprint targetBlueprint;
    public RobotPart partsPrefab;
    public Animatable shutterAnimatable;

    public Button okButton;

    public ScrollingPanel textPanel;
    public Text levelText;
    public string rememberRobotText, buildRobotText, youDidGreatText, notCorrectText;

    public Vector2Int rangeHeads = new Vector2Int(2,4), 
        rangeBodies= new Vector2Int(2,4), 
        rangeLegs= new Vector2Int(1,3), 
        rangeArms= new Vector2Int(1,3);
    
    [ContextMenu("Start Round")]
    public void StartRound()
    {
        WeCanClick = true;
        if(onStartingRound != null) onStartingRound.Invoke();
        targetBlueprint.Shuffle();
        
        shutterAnimatable.Play(1);

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

        dontSpawn = true;
        textPanel.SetString(rememberRobotText);
        okButton.onClick.RemoveListener(SetDontSpawnToFalse);
        okButton.onClick.AddListener(SetDontSpawnToFalse);
        StartCoroutine(EnableBodyParts(newParts));
    }

    void SetDontSpawnToFalse()
    {
        shutterAnimatable.Play(0);
        dontSpawn = false;
        okButton.onClick.RemoveListener(CheckForWin);
        okButton.onClick.AddListener(CheckForWin);
        textPanel.SetString(buildRobotText);
    }

    void CheckForWin()
    {
        WeCanClick = false;
        okButton.onClick.RemoveListener(CheckForWin);
        shutterAnimatable.Play(1);
        bool won = SnapPart.WinCheck();
        textPanel.SetString(won ? youDidGreatText : notCorrectText);
        if (won)
        {
            okButton.onClick.RemoveListener(Levelup);
            okButton.onClick.AddListener(Levelup);
        }
        else
        {
            okButton.onClick.RemoveListener(Retry);
            okButton.onClick.AddListener(Retry);
        }
    }

    void Retry()
    {
        WeCanClick = true;
        foreach(var snap in SnapPart.snapParts) snap.UnSnap();
        okButton.onClick.RemoveListener(Retry);
        shutterAnimatable.Play(0);
        textPanel.SetString(buildRobotText);
        okButton.onClick.RemoveListener(CheckForWin);
        okButton.onClick.AddListener(CheckForWin);
    }

    public int level = 0;
    void Levelup()
    {
        okButton.onClick.RemoveListener(Levelup);
        level++;
        levelText.text = level.ToString("00");
        if (level % 2 == 0)
        {
            rangeHeads.y++;
        }

        if (level % 3 == 0)
        {
            rangeHeads.x++;
            rangeBodies.x++;
            rangeBodies.y++;
            rangeArms.y++;
            rangeLegs.y++;
        }

        if (level % 4 == 0)
        {
            rangeArms.x++;
            rangeLegs.x++;
        }
        StartRound();
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

    public bool dontSpawn;
    
    IEnumerator EnableBodyParts(List<RobotPart> list)
    {
        while (dontSpawn) yield return null;
        okButton.onClick.RemoveListener(SetDontSpawnToFalse);
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
