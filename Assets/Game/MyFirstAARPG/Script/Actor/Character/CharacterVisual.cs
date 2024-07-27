using Shared.Extension;
using UnityEngine;

namespace MyFirstAARPG
{
    public class CharacterVisual : MonoBehaviour
    {
        [SerializeField] private Vector3 controllerFollowOffset = Vector3.up;
        [SerializeField] private float cameraVerticalArmLength = 0.45f;

        public Vector3 ControllerFollowOffset => controllerFollowOffset;
        public float CameraVerticalArmLength => cameraVerticalArmLength;

        public void Initialize(Character character)
        {
            transform.SetPositionAndRotation(character.transform);
        }
    }
}
