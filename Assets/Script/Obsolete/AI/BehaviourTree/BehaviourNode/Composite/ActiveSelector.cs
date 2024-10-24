using System;

namespace Yd.AI.BehaviourTree
{
    public class ActiveSelector : Serial
    {
        protected override void OnTick()
        {
            base.OnTick();

            var activeEnumerator = Children.GetEnumerator();

            while (activeEnumerator.MoveNext())
            {
                activeEnumerator.Current!.Tick();

                switch (activeEnumerator.Current.State)
                {
                    case NodeState.Failed:
                        {
                            continue;
                        }
                    case NodeState.Running:
                    case NodeState.Success:
                    case NodeState.Aborted:
                        {
                            var currentNode = Enumerator.Current;
                            if (currentNode != null && currentNode != activeEnumerator.Current && currentNode.IsRunning)
                            {
                                currentNode.Abort();
                            }

                            Enumerator = activeEnumerator;
                            State = activeEnumerator.Current.State;
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