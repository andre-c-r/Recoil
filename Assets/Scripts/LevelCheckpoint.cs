using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoint : Collectable
{
    protected override void CollectableEffect()
    {
        GameController.Checkpoint = transform.position;
    }
}
