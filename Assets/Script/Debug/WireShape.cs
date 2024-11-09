using System.Collections.Generic;
using UnityEngine;

namespace Yd.Extension
{
    public static class WireShape
    {
        public static Vector3[] Sphere(Vector3 origin, float radius, int segment)
        {
            if (segment <= 0)
            {
                segment = (int)Mathf.Ceil(Mathf.PI * radius * 16);
            }

            List<Vector3> points = new();

            var unitAngleY = 360f / segment;
            var unitAngleX = 180f / segment;

            var reversedUnitRotationY = Quaternion.AngleAxis(-unitAngleY, Vector3.up);

            for (var i = 1; i <= segment; i++)
            {
                var rotationX = Quaternion.AngleAxis(unitAngleX * i, Vector3.right); // 绕 x 轴转 i 下

                for (var j = 1; j <= segment; j++)
                {
                    var rotationY = Quaternion.AngleAxis(unitAngleY * j, Vector3.up); // 绕 y 轴转 j 下
                    var start = rotationY * rotationX * Vector3.up * radius;

                    var end = reversedUnitRotationY * start; // 左边那个点
                    points.Add(origin + start);
                    points.Add(origin + end);

                    var normal = Vector3.Cross(Vector3.up, start); // 切面的法线
                    end = Quaternion.AngleAxis(-unitAngleX, normal) * start; // 上边那个点
                    points.Add(origin + start);
                    points.Add(origin + end);
                }
            }

            return points.ToArray();
        }

        public static Vector3[] CircularArea(Vector3 origin, float radius, float height, int segment)
        {
            if (segment <= 0)
            {
                segment = (int)Mathf.Ceil(Mathf.PI * radius * 16);
            }

            List<Vector3> points = new();

            var unitAngleY = 360f / segment;
            var unitHeight = 2 * Mathf.PI / segment * radius;
            var reversedUnitRotationY = Quaternion.AngleAxis(-unitAngleY, Vector3.up);

            for (float i = 0; i < height + unitHeight; i += unitHeight)
            {
                var currentHeight = new Vector3(0, Mathf.Min(i, height), 0);

                for (var j = 1; j <= segment; j++)
                {
                    var rotationY = Quaternion.AngleAxis(unitAngleY * j, Vector3.up); // 绕 y 轴转 j 下

                    var start = rotationY * Vector3.right * radius + currentHeight;
                    var end = reversedUnitRotationY * start; // 左边那个点

                    points.Add(origin + start);
                    points.Add(origin + end);

                    if (currentHeight.y + unitHeight < height + unitHeight)
                    {
                        end = start;
                        end.y = Mathf.Min(currentHeight.y + unitHeight, height);

                        points.Add(origin + start);
                        points.Add(origin + end);
                    }
                }
            }

            return points.ToArray();
        }
    }
}