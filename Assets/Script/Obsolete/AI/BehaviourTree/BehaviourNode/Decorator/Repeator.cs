using System;

namespace Yd.AI.BehaviourTree
{
    public class Repeator : Decorator
    {
        private int limit;
        private int count;

        public Repeator(int limit)
        {
            this.limit = limit;
            count = 0;
        }

        protected override void OnReset()
        {
            base.OnReset();
            count = 0;
        }

        protected override void OnTick()
        {
            base.OnTick();

            while (count++ < limit)
            {
                Node.Tick();

                switch (Node.State)
                {
                    case NodeState.Success:
                    case NodeState.Failed:
                    case NodeState.Aborted:
                    {
                        State = Node.State;
                        return;
                    }
                    case NodeState.Running:
                    {
                        continue;
                    }
                    case NodeState.None:
                    case NodeState.Init:
                    default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            State = Node.State;
        }
    }
}