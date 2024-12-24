using UnityEngine;
using Yd.Audio;
using Yd.Gameplay.AbilitySystem;
using Yd.Gameplay.Object;
using Yd.PhysicsExtension;

public class LavaFireBall : Actor
{
    [SerializeField] private ParticleSystem impactPs;
    [SerializeField] private GameplayEffectData fireballDamageEffect;
    [SerializeField] private float damageRadius;

    private readonly Collider[] colliders = new Collider[10];
    private bool damageApplied;

    private bool impact;

    private AudioSource whoosh;
    public GameObject Owner { get; set; }

    private void Start()
    {
        whoosh = AudioManager.PlayOneShot(AudioId.FireballWhoosh, AudioChannel.World);
    }

    private void Update()
    {
        if (!impact && impactPs.particleCount != 0)
        {
            impact = true;
        }
        else if (impact && !damageApplied)
        {
            var count = PhysicsE.OverlapSphereNonAlloc
            (
                transform.position,
                damageRadius,
                colliders,
                LayerMaskE.Character,
                QueryTriggerInteraction.Ignore,
                true,
                Color.red,
                Color.gray,
                8
            );

            for (var i = 0; i < count; i++)
            {
                var go = colliders[i].gameObject;

                if (go.CompareTag(Owner.tag))
                {
                    continue;
                }

                var abilitySystem = go.GetComponent<Character>().Controller.AbilitySystem;
                abilitySystem.ApplyGameplayEffectAsync(fireballDamageEffect, null);
            }

            damageApplied = true;

            if (whoosh.enabled)
            {
                whoosh.Stop();
                whoosh.enabled = false;
            }

            AudioManager.PlayOneShot(AudioId.FireballExplosion, AudioChannel.World);

            Destroy(gameObject, 3f);
        }
    }
}