using Unity.Cinemachine;
using UnityEngine;

namespace MyFirstAARPG
{
    public class Character : MonoBehaviour
    {
        // Fields
        
        [SerializeField] private GameObject followCameraPrefab; // Prefabs
        [SerializeField] private GameObject characterVisualPrefab;
        [SerializeField] private GameObject controllerPrefab;

        [SerializeField] private bool isAI;
        
        private CharacterVisual characterVisual;
        private CharacterController characterController;
        private CharacterAnimation characterAnimation;
        
        private GameObject followCameraObject; // Subobjects
        private GameObject characterVisualObject;
        private GameObject controllerObject;

        // Properties
        
        public CharacterVisual CharacterVisual => characterVisual;
        public CharacterAnimation CharacterAnimation => characterAnimation;
        public CharacterController CharacterController => characterController;
        public GameObject CharacterVisualObject => characterVisualObject;
        
        // Event Functions

        private void Awake()
        {
            characterAnimation = GetComponent<CharacterAnimation>();
            characterVisual = SetupVisual();
            var controller = SetupController();
            SetupAnimation(controller);
        }

        private void SetupAnimation(CharacterControllerBase controllerBase)
        {
            var animator = characterVisualObject.GetComponent<Animator>();
            var animationClipEvent = characterVisualObject.GetComponent<AnimationClipEvent>();
            var characterAnimation = GetComponent<CharacterAnimation>();
            characterAnimation.Initialize(animator, animationClipEvent, controllerBase);
        }

        private void OnDestroy()
        {
            Destroy(followCameraObject);
            Destroy(characterVisualObject);
            Destroy(controllerObject);
        }

        // Methods

        private CharacterControllerBase SetupController()
        {
            Destroy(controllerObject);
            controllerObject = Instantiate(controllerPrefab, transform);

            var playerController = controllerObject.GetComponent<PlayerCharacterController>();
            var aiController = controllerObject.GetComponent<AICharacterController>();

            if (!isAI)
            {
                SetupFollowCamera(playerController);
                var playerActions = SetupPlayerInput();

                playerController.Initialize(this, playerActions);
                
                playerController.enabled = true;
                aiController.enabled = false;

                return playerController;
            }
            else
            {
                aiController.Initialize(this);
                
                playerController.enabled = false;
                aiController.enabled = true;

                return aiController;
            }
        }

        private PlayerActions SetupPlayerInput()
        {
            var playerActions = GetComponent<PlayerActions>();
            playerActions.Initialize();
            return playerActions;
        }

        private void SetupFollowCamera(PlayerCharacterController characterController)
        {
            followCameraObject ??= Instantiate(followCameraPrefab, transform);
            var followCamera = followCameraObject.GetComponent<CinemachineCamera>();
            followCamera.Target = new CameraTarget() { TrackingTarget = characterController.transform, CustomLookAtTarget = false };

            var cinemachineThirdPersonFollow = followCameraObject.GetComponent<CinemachineThirdPersonFollow>();
            cinemachineThirdPersonFollow.VerticalArmLength = characterVisual.CameraVerticalArmLength;
        }

        private CharacterVisual SetupVisual()
        {
            Destroy(characterVisualObject);
            characterVisualObject = Instantiate(characterVisualPrefab, transform);

            characterVisual = characterVisualObject.GetComponent<CharacterVisual>();
            characterVisual.Initialize(this);

            characterController = characterVisualObject.GetComponent<CharacterController>();

            return characterVisual;
        }
    }
}