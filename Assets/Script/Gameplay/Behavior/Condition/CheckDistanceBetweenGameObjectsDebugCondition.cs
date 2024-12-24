using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable] [GeneratePropertyBag]
[Condition
(
    name: "Check Distance Between Game Objects Debug",
    story: "distance between [Source] and [Target] [Operator] [Threshold] (Debug)",
    category: "Conditions",
    id: "e61fcc602cd8567158c8208c945bca53"
)]
public class CheckDistanceBetweenGameObjectsDebugCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Source;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;
    [SerializeReference] public BlackboardVariable<float> Threshold;

    public override bool IsTrue()
    {
        if (Source.Value == null || Target.Value == null)
        {
            if (Source.Value == null)
            {
                Debug.LogWarning("[Check Distance Between Game Objects Debug] Source is null");
            }

            if (Target.Value == null)
            {
                Debug.LogWarning("[Check Distance Between Game Objects Debug] Target is null");
            }

            return true;
        }

        var distance = Vector3.Distance(Source.Value.transform.position, Target.Value.transform.position);
        var result = ConditionUtils.Evaluate(distance, Operator, Threshold.Value);

        // Debug.Log
        // (
        //     $"[Check Distance Between Game Objects Debug] Distance between {Source.Value.name} and {Target.Value.name} is {distance}, result = {result}"
        // );

        return result;
    }
}