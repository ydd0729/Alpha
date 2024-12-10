using Script.Gameplay.GameplayObject.Item;
using TMPro;
using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.Object;

public class Chest : Actor, IInteractive
{
    private static readonly int Open = Animator.StringToHash("Open");

    public TextMeshProUGUI text;
    public Animator animator;

    private bool opened;

    private void Awake()
    {
        text.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opened && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " enter");
            text.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!opened && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " exit");
            text.enabled = false;
        }
    }

    public bool Interact()
    {
        if (!opened)
        {
            animator.SetTrigger(Open);
            opened = true;
            text.enabled = false;
            AudioManager.PlayOneShot(AudioId.Collect, AudioChannel.SFX);
        }

        return true;
    }
}