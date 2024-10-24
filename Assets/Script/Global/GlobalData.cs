using UnityEngine;
using Yd.Audio;
using Yd.Pattern;

public class GlobalData : MonoSingleton<GlobalData>
{
    [SerializeField] private AudioData audioResourceData;

    public AudioData Audio => audioResourceData;
}