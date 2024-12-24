using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable] [GeneratePropertyBag]
[Condition
    (name: "GameObject is null", story: "[Object] is null", category: "Conditions", id: "00fe72d2620dc678ef325552b27a524b")]
public class GameObjectIsNullCondition : Condition
{
    [FormerlySerializedAs("GameObject")] [SerializeReference] public BlackboardVariable<GameObject> Object;

    public override bool IsTrue()
    {
        return Object.Value == null;
    }
}