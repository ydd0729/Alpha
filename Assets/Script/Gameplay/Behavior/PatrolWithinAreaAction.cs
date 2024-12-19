using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Yd.Algorithm;
using Yd.Collection;
using Yd.Gameplay.Object;
using Action = Unity.Behavior.Action;

namespace Yd.Gameplay.Behavior
{
    [Serializable] [GeneratePropertyBag]
    [NodeDescription
    (
        name: "Patrol within Area",
        story: "[Agent] patrols at [PatrolArea]",
        category: "Action",
        id: "c54ed989b918f27ce63295bea9b0ff71"
    )]
    public class PatrolWithinAreaAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<PatrolArea> PatrolArea;
        [SerializeReference] public BlackboardVariable<float> WaitTimeMinInclusive;
        [SerializeReference] public BlackboardVariable<float> WaitTimeMaxInclusive;

        private Character character;
        private GameplayCharacterController controller;

        private float timer;
        private float waitTime;
        private SRangeFloat waitTimeRange = new();

        protected override Status OnStart()
        {
            if (Agent.Value == null)
            {
                LogFailure("No agent assigned.");
                return Status.Failure;
            }

            if (PatrolArea.Value == null)
            {
                LogFailure("No patrol area assigned.");
                return Status.Failure;
            }

            if (PatrolArea.Value.Radius == 0)
            {
                LogFailure("Radius cannot be zero.");
                return Status.Failure;
            }

            character = Agent.Value.GetComponent<Character>();
            if (ReferenceEquals(character, null))
            {
                LogFailure("Agent has no character.");
                return Status.Failure;
            }

            controller = character.Controller;

            timer = 0;
            waitTime = 0;
            waitTimeRange.Set(WaitTimeMinInclusive.Value, WaitTimeMaxInclusive.Value);

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (controller.IsNavigating || timer < waitTime)
            {
                timer += Time.deltaTime;

            }
            else
            {
                timer = 0;
                waitTime = waitTimeRange.Random();

                var target = GetNext();
                controller.NavigateTo(target, 0.1f);
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
            controller?.StopNavigation();
        }

        private Vector3 GetNext()
        {
            switch(PatrolArea.Value.Type)
            {

                case PatrolAreaType.Circle:
                {
                    var randomInCircle = RandomE.RandomInCircle(PatrolArea.Value.Radius);

                    var target = PatrolArea.Value.Center;
                    target.x += randomInCircle.x;
                    target.z += randomInCircle.y;

                    return target;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum PatrolAreaType
    {
        Circle
    }
}