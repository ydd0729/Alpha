using System.Linq;
using UnityEngine;

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

        var children = transform.Cast<Transform>().ToList();

        var maxRow = children.Count / maxColumn + (children.Count % maxColumn == 0 ? 0 : 1);

        var halfWidth = (maxColumn - 1) * spacing.x / 2;
        var halfLength = (maxRow - 1) * spacing.y / 2;

        var column = 0;
        var row = 0;

        foreach (var child in children)
        {
            var offsetFromCenter = new Vector3(halfWidth - column * spacing.x, 0, halfLength - row * spacing.y);

            column += 1;
            if (column == maxColumn)
            {
                column = 0;
                row += 1;
            }

            var cachedTransform = transform;
            child.SetPositionAndRotation(cachedTransform.position + offsetFromCenter, cachedTransform.rotation);
        }
    }
}