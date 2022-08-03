using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public LogController LogController;
    public GarbageSpawner GarbageSpawner;

    public float CollectionSpeed = 1f;

    private void OnValidate()
    {
        GarbageSpawner = GetComponent<GarbageSpawner>();
    }
}
