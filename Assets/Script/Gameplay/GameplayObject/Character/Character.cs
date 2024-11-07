using System;
using UnityEngine;
using Yd.Extension;

namespace Yd.Gameplay.Object
{
    [RequireComponent(typeof(Animator))]
    public class Character : Actor
    {
        [SerializeField] private GameObject controllerPrefab;

        public Animator Animator { get; private set; }
        public CharacterController UnityCharacterController { get; private set; }

        public CharacterMovement Movement { get; private set; }

        public CharacterControllerBase Controller { get; private set; }

        public CharacterWeapon Weapon
        {
            get;
            protected set;
        }


        private void Awake()
        {
            Animator = GetComponent<Animator>();
            UnityCharacterController = GetComponent<CharacterController>();

            Movement = this.GetOrAddComponent<CharacterMovement>();
            Movement.Initialize(this);

            Controller = Instantiate(controllerPrefab, transform.parent).GetComponent<CharacterControllerBase>();
            // 在 Initialize 返回前就会执行 Awake ，不是想要的，把初始化放在 Initialize 中
            Controller.Initialize(this);
        }

        private void OnAnimatorMove()
        {
            AnimatorMoved?.Invoke();
        }

        public event Action AnimatorMoved;
    }

    public enum CharacterWeapon
    {
        None
    }
}