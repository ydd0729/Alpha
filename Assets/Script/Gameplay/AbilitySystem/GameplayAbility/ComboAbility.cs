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
        protected bool CanDetectCombo => ComboCounter < ComboAbilityData.MaxCombo && ComboInterval;
        private bool ComboInterval { get; set; }
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
                ComboWaiter.SetResult(true);
            }

            return ComboWaiter.Task;
        }

        public override void StopExecution()
        {
            base.StopExecution();

            ComboInterval = false;
            ComboCounter = 0;
            ComboWaiter = null;
            ComboApproved = false;
        }

        protected virtual void StartComboDetection()
        {
            Debug.LogWarning("Combo Detection Starts");
            ComboInterval = true;
            ComboApproved = false;
        }

        protected virtual void EndComboDetection()
        {
            Debug.LogWarning("Combo Detection Ends");
            ComboInterval = false;
            ComboApproved = false;
            // if (!ComboWaiter.Task.IsCompleted)
            // {
            //     ComboWaiter.SetResult(false);
            // }
        }
    }
}