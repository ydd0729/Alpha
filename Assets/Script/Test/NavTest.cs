using UnityEngine;
using Yd.Gameplay.Object;

public class NavTest : MonoBehaviour
{
    public Transform target;
    public float stoppingDistance = 4;
    
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        character.Controller.NavigateTo(target.position, stoppingDistance);
    }
}