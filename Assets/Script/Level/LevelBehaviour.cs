using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yd.Audio;
using Yd.Gameplay.Behavior;
using Yd.Gameplay.Object;
using Yd.Manager;

public class LevelBehaviour : MonoBehaviour
{
    private static readonly int Ready = Animator.StringToHash("Ready");
    private static readonly int StartHash = Animator.StringToHash("Start");

    [SerializeField] private PlayerCharacterSpawnPoint spawnPoint;
    [SerializeField] private Character boss;
    [SerializeField] private GameplayArea bossArea;
    [SerializeField] private ChestGroup[] chestGroups;
    [SerializeField] private GameUi ui;
    [SerializeField] private Chest swordChest;


    private PlayerCharacterController controller;

    private Character playerCharacter;

    private bool playerEntered;
    private bool PlayerEntered
    {
        get => playerEntered;
        set
        {
            if (playerEntered != value)
            {
                playerEntered = value;
                if (playerEntered)
                {
                    boss.Target = playerCharacter.gameObject;
                    boss.Animator.SetBool(Ready, true);
                    boss.BehaviorGraphAgent.SetVariableValue(variableName: "Target", playerCharacter.gameObject);
                    boss.Controller.LookAt(playerCharacter.transform.position);
                    boss.Controller.RotateToVelocity = false;
                }
                else
                {
                    boss.Target = null;
                    boss.BehaviorGraphAgent.SetVariableValue<GameObject>(variableName: "Target", null);
                    boss.Animator.SetBool(Ready, false);
                    boss.Controller.RotateToVelocity = true;
                }
            }
        }
    }

    private void Awake()
    {
        playerCharacter = spawnPoint.Spawn().GetComponent<Character>();
        playerCharacter.Dead += () => CoroutineTimer.SetTimer
            (_ => ui.SetActiveDeadPanel(true), 1, CoroutineTimerLoopPolicy.Once);

        boss.Dead += () => {
            CoroutineTimer.SetTimer
            (
                _ => {
                    ui.SetActiveFinishPanel(true);
                    controller.PlayerActions.DisableInput();
                },
                2,
                CoroutineTimerLoopPolicy.Once
            );
        };

        controller = (PlayerCharacterController)playerCharacter.Controller;

        ui.SetActiveMainMenu(true);
        ui.SetActiveHud(false);
        ui.SetActiveDeadPanel(false);
        ui.SetActiveMessagePanel(false);
        ui.SetActiveFinishPanel(false);

        ui.MessageClosed += () => controller.PlayerActions.EnableInput();
    }

    private void Start()
    {
        swordChest.Opened += () => {
            CoroutineTimer.SetTimer
            (
                _ => {
                    ui.SetActiveMessagePanel(true);
                    controller.PlayerActions.DisableInput();
                },
                1.5f
            );
            swordChest.AudioManager.PlayOneShot(AudioId.BookPage, AudioChannel.SFX);
        };

        foreach (var chestGroup in chestGroups)
        {
            chestGroup.Start();
        }

        controller.PlayerActions.DisableInput();
        ui.StartGameClicked += () => {
            ui.SetActiveMainMenu(false);
            controller.SetActiveFollowCamera(true);
        };
        ui.BackClicked += () => {
            CoroutineTimer.CancelAll();
            var currentScene = SceneManager.GetActiveScene();
            // SceneManager.LoadScene(currentScene.name);
            SceneManager.LoadScene(currentScene.buildIndex);
        };
    }

    private void Update()
    {
        if (playerCharacter.IsDead || boss.IsDead)
        {
            return;
        }

        if (playerCharacter.transform.position.y < 70f)
        {
            playerCharacter.Die();
            return;
        }

        foreach (var chestGroup in chestGroups)
        {
            chestGroup.Update();
        }

        PlayerEntered = Vector3.Distance(playerCharacter.transform.position, bossArea.transform.position) <= bossArea.Radius;
        if (PlayerEntered)
        {
            boss.Controller.LookAt(playerCharacter.transform.position);
        }
    }

    public void OnCameraBlendFinished()
    {
        Debug.Log("Blend Finished");
        playerCharacter.Animator.SetBool(StartHash, true);
        CoroutineTimer.SetTimer
        (
            _ => {
                ui.SetActiveHud(true);
                controller.PlayerActions.EnableInput();
            },
            2,
            CoroutineTimerLoopPolicy.Once
        );
    }
}

[Serializable]
public struct ChestGroup
{
    [SerializeField] private Chest[] chests;
    [SerializeField] private Character[] guards;

    private bool isAllDead;

    public void Start()
    {
        foreach (var chest in chests)
        {
            chest.Openable = false;
        }
    }

    public void Update()
    {
        if (isAllDead)
        {
            return;
        }

        isAllDead = true;
        foreach (var guard in guards)
        {
            if (!guard.IsDead && guard.isActiveAndEnabled)
            {
                isAllDead = false;
                break;
            }
        }

        if (isAllDead)
        {
            foreach (var chest in chests)
            {
                chest.Openable = true;
            }
        }
    }
}