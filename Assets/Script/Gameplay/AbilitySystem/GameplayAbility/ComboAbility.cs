using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Yd.Gameplay.AbilitySystem
{
    public abstract class ComboAbility : GameplayAbility
    {
        private int comboCounter;
        protected ComboAbility(
            GameplayAbilityData data, GameplayAbilitySystem owner, [CanBeNull] GameplayAbilitySystem source
        ) : base(data, owner, source)
        {
        }

        protected int ComboCounter
        {
            get => comboCounter;
            set
            {
                if (comboCounter != value)
                {
                    comboCounter = value;
                    Debug.LogWarning($"combo #{ComboCounter}");
                }
            }
        }
        protected ComboAbilityData ComboAbilityData => (ComboAbilityData)Data;
        protected bool CanDetectCombo => ComboCounter < ComboAbilityData.MaxCombo && DetectingCombo;
        private bool DetectingCombo { get; set; }
        protected bool ComboApproved { get; set; }

        protected TaskCompletionSource<bool> ComboWaiter
        {
            get;
            private set;
        }

        protected virtual Task<bool> Combo()
        {
            ComboWaiter = new();

            if (ComboCounter == 0)
            {
                ComboCounter = 1;
                ComboWaiter.SetResult(ComboCounter < ComboAbilityData.MaxCombo);
            }

            return ComboWaiter.Task;
        }

        public override void StopExecution()
        {
            base.StopExecution();

            DetectingCombo = false;
            ComboCounter = 0;
            ComboWaiter.SetResult(false);
            ComboApproved = false;
        }

        protected virtual void StartComboDetection()
        {
            Debug.LogWarning("Combo Detection Starts");
            DetectingCombo = true;
            ComboApproved = false;
        }

        protected virtual void StopComboDetection()
        {
            Debug.LogWarning("Combo Detection Ends");
            DetectingCombo = false;
            ComboApproved = false;
        }
    }
}