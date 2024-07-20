using System;
using Shared.Extension;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyFirstAARPG
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] private float timeToRotate = 0.1f;

        public virtual void Initialize(Character inCharacter)
        {
            Character = inCharacter;
            FollowCharacterVisualPosition();
        }

        protected virtual void Update()
        {
            FollowCharacterVisualPosition();

            if (LocalMoveDirection != Vector3.zero)
            {
                if (CharacterTargetRotation != rotationTask.Target)
                {
                    rotationTask.SetTask(VisualObjectRotation, CharacterTargetRotation, timeToRotate);
                }
                else if (rotationTask.Executing)
                {
                    VisualObjectRotation = rotationTask.Execute(Time.deltaTime);
                }
                
                Character.CharacterAnimator.Walk();
            }
            else
            {
                Character.CharacterAnimator.Stand();
            }
        }

        protected Character Character { get; private set; }
        
        protected Vector3 LocalMoveDirection { get; set; } = Vector3.zero;
        
        private void FollowCharacterVisualPosition()
        {
            transform.position = Character.VisualObject.transform.position;
        }

        private Quaternion VisualObjectRotation
        {
            get => Character.VisualObject.transform.rotation;
            set => Character.VisualObject.transform.rotation = value;
        }
        
        private Quaternion CharacterTargetRotation =>
            Quaternion.LookRotation(transform.TransformDirection(LocalMoveDirection).Ground());

        private RotationTask rotationTask;

        private struct RotationTask
        {
            private Quaternion origin;
            // private float angle;
            private float rotationTime;
            private float timePassed;

            public Quaternion Target { get; private set; }
            public bool Executing { get; private set; }

            public void SetTask(Quaternion inOrigin, Quaternion inTarget, float timeToRotateHalfCircle)
            {
                origin = inOrigin;
                Target = inTarget;
                // angle = Quaternion.Angle(inOrigin, inTarget);
                rotationTime = timeToRotateHalfCircle /* * angle / 180 */;
                timePassed = 0;
                Executing = true;
            }

            public Quaternion Execute(float deltaTime)
            {
                timePassed += deltaTime;
                if (timePassed >= rotationTime)
                {
                    Executing = false;
                }

                return Quaternion.Slerp(origin,Target, Math.Min(timePassed / rotationTime, 1));
            }
        }
    }
}