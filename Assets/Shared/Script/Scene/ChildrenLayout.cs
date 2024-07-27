using System.Collections.Generic;
using UnityEngine;

namespace Shared.Scene
{
    public class ChildrenLayout : MonoBehaviour
    {
        [SerializeField] private Vector2 spacing;
        [SerializeField] private int maxColumn;

        
        public void Organize()
        {
            if (maxColumn == 0)
            {
                Debug.LogWarning("Column == 0");
                return;
            }
            
            List<Transform> children = new List<Transform>();
            
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            int maxRow = children.Count / maxColumn + (children.Count % maxColumn == 0 ? 0 : 1);

            float halfWidth = (maxColumn - 1) * spacing.x / 2;
            float halfLength = (maxRow - 1) * spacing.y / 2;

            int column = 0;
            int row = 0;

            foreach (Transform child in children)
            {
                Vector3 offsetFromCenter = new Vector3(halfWidth - column * spacing.x, 0, halfLength - row * spacing.y);

                column += 1;
                if (column == maxColumn)
                {
                    column = 0;
                    row += 1;
                }
                
                child.SetPositionAndRotation(transform.position + offsetFromCenter, transform.rotation);
            }
        }
    }
}
