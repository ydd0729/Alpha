using UnityEngine;
using Yd.Gameplay.Object;

public class SwordTest : MonoBehaviour
{
    public Character character;

    private void Start()
    {
        character.Weapon = Weapon.Sword;
    }
}