using System;
using Script.Gameplay.GameplayObject.Item;
using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.AbilitySystem;
using Yd.Gameplay.Object;

public class Chest : Actor, IInteractive
{
    private static readonly int Open = Animator.StringToHash("Open");

    public Canvas canvas;
    public Animator animator;
    public ParticleSystem glowVfx;
    public ParticleSystem openVfx;
    public Weapon weapon;
    public GameplayEffectData[] effects;

    private bool openable;

    private bool opened;
    public bool Openable
    {
        get => openable;
        set
        {
            if (openable == value)
            {
                return;
            }
            openable = value;

            if (value && !opened)
            {
                glowVfx.gameObject.SetActive(true);
                AudioManager.PlayOneShot(AudioId.ChestUnlock, AudioChannel.SFX);
            }
            else
            {
                glowVfx.gameObject.SetActive(false);
                canvas.enabled = false;
            }
        }
    }

    private void Awake()
    {
        canvas.enabled = false;
        openVfx.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opened && Openable && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " enter");
            canvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!opened && Openable && other.gameObject.CompareTag("Player"))
        {
            // Debug.Log(other.gameObject.name + " exit");
            canvas.enabled = false;
        }
    }

    public bool Interact(GameObject other)
    {
        if (!opened && Openable)
        {
            animator.SetTrigger(Open);

            opened = true;
            Openable = false;

            openVfx.gameObject.SetActive(true);
            openVfx.Play();

            AudioManager.PlayOneShot(AudioId.Collect, AudioChannel.SFX);

            var controller = other.GetComponent<GameplayCharacterController>();
            if (controller != null)
            {
                if (weapon != Weapon.None)
                {
                    controller.Character.GrantWeapon(weapon);
                    controller.Character.Weapon = weapon;
                }
                if (effects.Length != 0 && controller.AbilitySystem != null)
                {
                    foreach (var effect in effects)
                    {
                        controller.AbilitySystem.ApplyGameplayEffectAsync(effect, null);
                    }
                }
            }

            Opened?.Invoke();
        }

        return true;
    }

    public event Action Opened;
}