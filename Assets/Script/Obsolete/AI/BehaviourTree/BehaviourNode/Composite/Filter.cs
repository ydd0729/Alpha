using System;

namespace Yd.AI.BehaviourTree
{
    public class Filter : BehaviorNode
    {
        private readonly Parallel condition;
        private readonly Sequence sequence;

        public Filter(Parallel condition, Sequence sequence)
        {
            this.condition = condition;
            this.sequence = sequence;
        }

        protected override void OnReset()
        {
            base.OnReset();

            condition.Reset();
            sequence.Reset();
        }

        protected override void OnTick()
        {
            base.OnTick();

            condition.Tick();

            switch (condition.State)
            {
                case NodeState.Success:
                {
                    sequence.Tick();
                    State = sequence.State;
                    break;
                }
                case NodeState.Running:
                case NodeState.Failed:
                case NodeState.Aborted:
                {
                    State = condition.State;
                    break;
                }
                case NodeState.None:
                case NodeState.Init:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnAbort()
        {
            base.OnAbort();
            
            condition.Abort();
            sequence.Abort();
        }
    }
}