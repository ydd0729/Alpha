using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable] [GeneratePropertyBag]
[Condition
(
    name: "GameObject is valid",
    story: "[GameObject] is valid",
    category: "Variable Conditions",
    id: "652c3778af091ebdae812e7103085b3c"
)]
public class GameObjectIsValidCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> GameObject;

    public override bool IsTrue()
    {
        return GameObject.Value != null;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}