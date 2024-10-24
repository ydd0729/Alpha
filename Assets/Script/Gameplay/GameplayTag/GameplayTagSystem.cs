using System.Collections.Generic;
using UnityEngine;

namespace Yd.Gameplay
{
    public class GameplayTagSystem : MonoBehaviour
    {
        private HashSet<GameplayTag> tags = new();

        private void Awake()
        {
            tags.Clear();
        }

        public void Add(GameplayTag tag)
        {
            tags.Add(tag);
        }

        public void Remove(GameplayTag tag)
        {
            tags.Remove(tag);
        }

        public bool Contains(GameplayTag tag)
        {
            return tags.Contains(tag);
        }
    }
}