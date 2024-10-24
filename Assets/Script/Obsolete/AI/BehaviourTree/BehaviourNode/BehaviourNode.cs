using UnityEngine;

namespace Yd.AI.BehaviourTree
{
    public enum NodeState
    {
        None,
        Init,
        Running,
        Success,
        Failed,
        Aborted
    }

    public abstract class BehaviorNode
    {
        public NodeState State { get; protected set; } = NodeState.None;

        public bool IsInit => State == NodeState.Init;
        public bool IsRunning => State == NodeState.Running;
        public bool IsExited => State is NodeState.Success or NodeState.Failed or NodeState.Aborted or NodeState.None;

        public void Reset()
        {
            if (IsExited)
                OnReset();
            else
                Debug.LogWarning($"Trying to reset a BehaviourNode which is {State.ToString()}");
        }

        protected virtual void OnReset()
        {
            State = NodeState.Init;
        }

        public void Tick(bool autoRest = true)
        {
            if (IsExited)
            {
                if (autoRest)
                {
                    Reset();
                }
                else
                {
                    Debug.LogWarning($"Trying to tick a BehaviourNode which is {State.ToString()}");
                    return;
                }
            }

            if (IsInit) State = NodeState.Running;

            OnTick();
        }

        protected virtual void OnTick()
        {
        }

        public void Abort()
        {
            if (IsRunning)
                OnAbort();
            else
                Debug.LogWarning($"Trying to abort a BehaviourNode which is {State.ToString()}");
        }

        protected virtual void OnAbort()
        {
            State = NodeState.Aborted;
        }
    }
}