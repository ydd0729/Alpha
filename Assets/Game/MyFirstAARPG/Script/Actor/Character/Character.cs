using MyFirstAARPG;
using Shared.Collections;
using Shared.Extension;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform controllersMountPoint;
    [SerializeField] private Transform visualObjectsMountPoint;
    
    [SerializeField] private CharacterAnimator characterAnimator;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private CinemachineCamera followCamera;
    
    [SerializeField] private GameObject visualPrefab;
    [SerializeField] private GameObject visualObject;    
    
    [SerializeField] private GameObject controllerPrefab;
    [SerializeField] private GameObject controllerObject;
    
    [SerializeField] private bool isAI;
    
    private void Awake()
    {
        Destroy(visualObject);
        visualObject = Instantiate(visualPrefab, visualObjectsMountPoint);
        
        var animator = visualObject.GetComponent<Animator>();
        var animationClipEvent = visualObject.AddComponent<AnimationClipEvent>();
        
        characterAnimator.Animator = animator;
        characterAnimator.AnimationClipEvent = animationClipEvent;
        
        Destroy(controllerObject);
        controllerObject = Instantiate(controllerPrefab, controllersMountPoint);
        
        if (!isAI)
        {
            var playerController = controllerObject.GetComponent<PlayerController>();
            playerController.Initialize(this, playerActions, followCamera);
        }
        
        var aiController = controllerObject.GetComponent<AIController>();
        aiController.Initialize(this);
    }

    public CharacterAnimator CharacterAnimator => characterAnimator;
    public GameObject VisualObject => visualObject;
}
