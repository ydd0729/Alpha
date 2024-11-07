using JetBrains.Annotations;
using UnityEngine;

namespace Yd.Gameplay.Object
{
    public class HumanoidCharacter : Character
    {
        [SerializeField] private GameObject leftFoot;
        [SerializeField] private GameObject rightFoot;

        [CanBeNull] public GameObject LeftFoot => leftFoot;
        [CanBeNull] public GameObject RightFoot => rightFoot;
    }
}