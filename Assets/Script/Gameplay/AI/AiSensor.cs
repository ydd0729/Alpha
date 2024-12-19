using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Yd.DebugExtension;
using Yd.Gameplay.Object;
using Yd.PhysicsExtension;

[ExecuteAlways]
public class AiSensor : MonoBehaviour
{

    private const float groundTolerance = -1e-1f;
    private const uint maxDetectedObjectsNum = 50;

    public Vector3 offset;
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    private readonly Collider[] colliders = new Collider[maxDetectedObjectsNum];
    private readonly List<GameObject> objects = new();
    private Character character;
    private int count;
    private Mesh mesh;
    private float scanInterval;
    private float scanTimer;

    [CanBeNull] private GameObject target;
    [CanBeNull] public GameObject Target
    {
        get => target;
        private set
        {
            if (target == value)
            {
                return;
            }
            target = value;
            TargetChanged?.Invoke(value);
        }
    }

    private Vector3 Origin => transform.position + (Vector3)(transform.localToWorldMatrix * offset);
    public IReadOnlyList<GameObject> Objects => objects;

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, Origin, transform.rotation);
        }

        Gizmos.DrawWireSphere(Origin, distance);
        // Debug.Log(count);
        // for (var i = 0; i < count; ++i)
        // {
        //     DebugE.DrawSphere(colliders[i].transform.position, 0.5f, Color.red, 8);
        // }

        // Gizmos.color = Color.green;

        foreach (var obj in objects)
        {
            // Debug.Log(obj.name);
            // Gizmos.DrawSphere(obj.transform.position, 0.2f);
            DebugE.DrawSphere(obj.transform.position, 0.2f, Color.green, 6);
        }
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    public event Action<GameObject> TargetChanged;

    public void Initialize(Character character)
    {
        this.character = character;
    }

    private void Scan()
    {
        count = PhysicsE.OverlapSphereNonAlloc(Origin, distance, colliders, layers, QueryTriggerInteraction.Collide);

        objects.Clear();
        for (var i = 0; i < count; ++i)
        {
            var go = colliders[i].gameObject;
            if (go == gameObject)
            {
                continue;
            }
            if (IsInSight(go))
            {
                objects.Add(go);
            }
        }
        Target = objects.Count > 0 ? objects[0] : null;
    }

    public bool IsInSight(GameObject target)
    {
        var origin = Origin;
        var dest = target.transform.position;
        var dir = dest - origin;
        if (dir.y < groundTolerance || dir.y > height)
        {
            return false;
        }

        dir.y = 0;
        var deltaAngle = Vector3.Angle(dir, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;

        return !Physics.Linecast(origin, dest, occlusionLayers);
    }

    private Mesh CreateWedgeMesh()
    {
        var mesh = new Mesh();

        var segments = 10;
        var numTriangles = segments * 4 + 2 + 2;
        var numVertices = numTriangles * 3;

        var vertices = new Vector3[numVertices];
        var triangles = new int[numVertices];


        var bottomCenter = Vector3.zero;
        var bottomLeft = Quaternion.Euler(0.0f, -angle, 0.0f) * Vector3.forward * distance;
        var bottomRight = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward * distance;

        var topCenter = bottomCenter + Vector3.up * height;
        var topRight = bottomRight + Vector3.up * height;
        var topLeft = bottomLeft + Vector3.up * height;

        var vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        var currentAngle = -angle;
        var deltaAngle = angle * 2 / segments;
        for (var i = 0; i < segments; i++, currentAngle += deltaAngle)
        {
            bottomLeft = Quaternion.Euler(0.0f, currentAngle, 0.0f) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0.0f, currentAngle + deltaAngle, 0.0f) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
        }

        for (var i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}