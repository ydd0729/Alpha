using System;

namespace Yd.AI.BehaviourTree
{
    public class Sequence : Serial
    {
        protected override void OnTick()
        {
            base.OnTick();

            while (Enumerator.MoveNext())
            {
                Enumerator.Current!.Tick(false);

                switch (Enumerator.Current.State)
                {
                    
                    case NodeState.Success:
                    {
                        continue;
                    }
                    case NodeState.Running:
                    case NodeState.Failed:
                    case NodeState.Aborted:
                    {
                        State = Enumerator.Current.State;
                        return;
                    }
                    case NodeState.None:
                    case NodeState.Init:
                    default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

            State = NodeState.Success;
        }
    }
}