using System;
using UnityEngine;

namespace MyFirstAARPG
{
    public enum AnimationClipEventType
    {
        StepLeft,
        StepRight,
        StepLeftMiddle,
        StepRightMiddle
    }

    public sealed class AnimationClipEvent : MonoBehaviour
    {
        public event EventHandler<StepEventArgs> Step;
        
        private void StepLeft()
        {
            // Debug.Log("OnStepLeft");
            OnStep(StepEventArgs.StepLeft);
        }
        
        private void StepRight()
        {
            // Debug.Log("OnStepRight");
            OnStep(StepEventArgs.StepRight);
        }
        
        private void StepLeftMiddle()
        {
            // Debug.Log("OnStepLeftMiddle");
            OnStep(StepEventArgs.StepLeftMiddle);
        }
        
        private void StepRightMiddle()
        {
            // Debug.Log("OnStepRightMiddle");
            OnStep(StepEventArgs.StepRightMiddle);
        }

        private void OnStep(StepEventArgs e)
        {
            Step?.Invoke(this, e);
        }
    }

    public class StepEventArgs : EventArgs
    {
        public AnimationClipEventType AnimationClipEventType;

        public static StepEventArgs StepLeft => stepLeft ??= new() { AnimationClipEventType = AnimationClipEventType.StepLeft };
        public static StepEventArgs StepRight => stepRight ??= new() { AnimationClipEventType = AnimationClipEventType.StepRight };
        public static StepEventArgs StepLeftMiddle => stepLeftMiddle ??= new() { AnimationClipEventType = AnimationClipEventType.StepLeftMiddle };
        public static StepEventArgs StepRightMiddle => stepRightMiddle ??= new() { AnimationClipEventType = AnimationClipEventType.StepRightMiddle };

        private static StepEventArgs stepLeft;
        private static StepEventArgs stepRight;
        private static StepEventArgs stepLeftMiddle;
        private static StepEventArgs stepRightMiddle;

    }
}