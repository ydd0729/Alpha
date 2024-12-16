using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable] [GeneratePropertyBag]
[Condition
(
    name: "Check Distance between GameObjects",
    story: "distance between [Source] and [Target] [Operator] [Threshold]",
    category: "Conditions",
    id: "0d7b248e7b9bcac76d6d281dab1e526f"
)]
public class CheckDistanceBetweenGameObjectsCondition : Condition
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
            return false;
        }

        var distance = Vector3.Distance(Source.Value.transform.position, Target.Value.transform.position);
        return ConditionUtils.Evaluate(distance, Operator, Threshold.Value);
    }
    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}