using System.Collections.Generic;
using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.AbilitySystem;
using Yd.Pattern;

public class GlobalData : MonoSingleton<GlobalData>
{
    [SerializeField] private AudioData audioResourceData;
    [SerializeField] private GameplayAttributes attributes;

    public AudioData Audio => audioResourceData;
    public IReadOnlyDictionary<GameplayAttributeTypeEnum, GameplayAttributeTypeSO> Attributes => attributes.Attrributes;
}