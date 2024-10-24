using JetBrains.Annotations;
using UnityEngine;
using Yd.Extension;

namespace Yd.Gameplay.Object
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class Character : Actor
    {
        [SerializeField] private GameObject leftFoot;
        [SerializeField] private GameObject rightFoot;
        [SerializeField] private CharacterData data;

        public CharacterData Data => data;

        public Animator Animator { get; private set; }
        public CharacterController UnityCharacterController { get; private set; }

        public CharacterMovement Movement { get; private set; }

        public CharacterControllerBase Controller { get; private set; }

        public CharacterWeapon Weapon
        {
            get;
            protected set;
        }

        [CanBeNull] public GameObject LeftFoot => leftFoot;
        [CanBeNull] public GameObject RightFoot => rightFoot;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            UnityCharacterController = GetComponent<CharacterController>();

            Movement = this.GetOrAddComponent<CharacterMovement>();
            Movement.Initialize(this);

            Controller = Instantiate(Data.controllerPrefab, transform.parent).GetComponent<CharacterControllerBase>();
            // 在 Initialize 返回前就会执行 Awake ，不是想要的，把初始化放在 Initialize 中
            Controller.Initialize(this);
        }
    }

    public enum CharacterWeapon
    {
        None
    }
}