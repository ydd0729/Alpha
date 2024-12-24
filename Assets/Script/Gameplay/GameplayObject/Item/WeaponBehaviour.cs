using System;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private Collider collider;

    private void Awake()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Detected?.Invoke(other);
    }

    public void StartDetection(LayerMask mask)
    {
        collider.enabled = true;
        collider.includeLayers = mask;
    }

    public void EndDetection()
    {
        collider.enabled = false;
    }

    public event Action<Collider> Detected;
}