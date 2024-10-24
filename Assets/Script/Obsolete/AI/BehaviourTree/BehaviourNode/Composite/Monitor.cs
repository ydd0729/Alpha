using System;

namespace Yd.AI.BehaviourTree
{
    public class Monitor : BehaviorNode
    {
        private readonly Parallel condition;
        private readonly Parallel parallel;

        public Monitor(Parallel condition, Parallel parallel)
        {
            this.condition = condition;
            this.parallel = parallel;
        }

        protected override void OnReset()
        {
            base.OnReset();

            condition.Reset();
            parallel.Reset();
        }

        protected override void OnTick()
        {
            base.OnTick();

            condition.Tick();

            switch (condition.State)
            {
                case NodeState.Success:
                {
                    parallel.Tick();
                    State = parallel.State;
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
            parallel.Abort();
        }
    }
}