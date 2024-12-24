using UnityEngine;

namespace Script.Gameplay.GameplayObject.Item
{
    public interface IInteractive
    {
        bool Interact(GameObject other);
    }
}