using Script.Gameplay.GameplayObject.Item;
using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.Object;

public class Chest : Actor, IInteractive
{
    private static readonly int Open = Animator.StringToHash("Open");

    public Canvas canvas;
    public Animator animator;

    private bool opened;

    private void Awake()
    {
        canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opened && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " enter");
            canvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!opened && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " exit");
            canvas.enabled = false;
        }
    }

    public bool Interact()
    {
        if (!opened)
        {
            animator.SetTrigger(Open);
            opened = true;
            canvas.enabled = false;
            AudioManager.PlayOneShot(AudioId.Collect, AudioChannel.SFX);
        }

        return true;
    }
}