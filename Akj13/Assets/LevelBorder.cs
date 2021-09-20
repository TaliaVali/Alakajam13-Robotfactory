using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBorder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var rb2D = other.GetComponent<Rigidbody2D>();
        rb2D.position = RobotGame.GetRandomSpawnPosition();
        rb2D.angularVelocity = 0f;
        rb2D.velocity = Vector3.zero;
    }
}
