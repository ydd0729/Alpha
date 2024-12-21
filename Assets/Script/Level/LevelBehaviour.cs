using System;
using UnityEngine;
using Yd.Gameplay.Object;

public class LevelBehaviour : MonoBehaviour
{
    [SerializeField] ChestGroup[] chestGroups;

    void Start()
    {
        foreach (var chestGroup in chestGroups)
        {
            chestGroup.Start();
        }
    }

    void Update()
    {
        foreach (var chestGroup in chestGroups)
        {
            chestGroup.Update();
        }
    }
}

[Serializable]
public struct ChestGroup
{
    [SerializeField] private Chest[] chests;
    [SerializeField] private Character[] guards;

    private bool isAllDead;
    
    public void Start()
    {
        foreach (var chest in chests)
        {
            chest.Openable = false;
        }
    }

    public void Update()
    {
        if (isAllDead)
        {
            return;
        }
        
        isAllDead = true;
        foreach (var guard in guards)
        {
            if (!guard.Dead)
            {
                isAllDead = false;
                break;
            }
        }

        if (isAllDead)
        {
            foreach (var chest in chests)
            {
                chest.Openable = true;
            }
        }
    }
}
