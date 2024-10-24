using System;

namespace Yd.AI.BehaviourTree
{
    public class Selector : Serial
    {
        protected override void OnTick()
        {
            base.OnTick();

            while (Enumerator.MoveNext())
            {
                Enumerator.Current!.Tick(false);

                switch (Enumerator.Current.State)
                {
                   
                    case NodeState.Failed:
                    {
                        continue;
                    }
                    case NodeState.Running:
                    case NodeState.Success:
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

            State = NodeState.Failed;
        }
    }
}