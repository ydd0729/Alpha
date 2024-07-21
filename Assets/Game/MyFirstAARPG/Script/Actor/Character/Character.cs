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

    [SerializeField] private GameObject followCameraPrefab;
    [SerializeField] private GameObject PlayerInputPrefab;
    [SerializeField] private GameObject visualPrefab;
    [SerializeField] private GameObject controllerPrefab;

    [SerializeField] private bool isAI;

    private void Awake()
    {
        Destroy(visualObject);
        visualObject = Instantiate(visualPrefab, visualObjectsMountPoint);
        visualObject.transform.SetPositionAndRotation(transform);

        var animator = visualObject.GetComponent<Animator>();
        var animationClipEvent = visualObject.AddComponent<AnimationClipEvent>();
        characterAnimator.Animator = animator;
        characterAnimator.AnimationClipEvent = animationClipEvent;

        Destroy(controllerObject);
        controllerObject = Instantiate(controllerPrefab, controllersMountPoint);

        var playerController = controllerObject.GetComponent<PlayerController>();
        var aiController = controllerObject.GetComponent<AIController>();

        if (!isAI)
        {
            followCameraObject ??= Instantiate(followCameraPrefab, transform);
            followCamera = followCameraObject.GetComponent<CinemachineCamera>();
            
            PlayerInputObject ??= Instantiate(PlayerInputPrefab, transform);
            playerActions = PlayerInputObject.GetComponent<PlayerActions>();

            playerController.Initialize(this, playerActions, followCamera);
            playerController.enabled = true;
            aiController.enabled = false;
        }
        else
        {
            aiController.Initialize(this);
            playerController.enabled = false;
            aiController.enabled = true;
        }
    }

    public CharacterAnimator CharacterAnimator => characterAnimator;
    public GameObject VisualObject => visualObject;

    private PlayerActions playerActions;
    private CinemachineCamera followCamera;
    
    private GameObject followCameraObject;
    private GameObject PlayerInputObject;
    private GameObject visualObject;
    private GameObject controllerObject;
}