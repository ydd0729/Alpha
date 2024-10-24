using System;
using System.Collections.Generic;

namespace Yd.AI.BehaviourTree
{
    public enum ParallelPolicy
    {
        WaitAllSuccess, // wait for all nodes to exit, succeed if all nodes succeed.
        WaitAnySuccess, // wait for all nodes to exit, succeed if any node succeeds.
        AnySuccess, // succeed when any node succeeds and abort other running nodes.
        AllSuccess, // fail when any node fails and abort other running nodes.
    }

    public abstract class Parallel : Composite
    {
        protected ParallelPolicy Policy;

        protected override void OnTick()
        {
            base.OnTick();

            ParallelStates result = new();
            foreach (var child in Children)
            {
                child.Tick(false);
                result.Add(child);
            }

            switch (Policy)
            {
                case ParallelPolicy.WaitAllSuccess:
                {
                    if (result.GetCount(NodeState.Running) == 0)
                    {
                        State = result.GetCount(NodeState.Failed) == 0 ? NodeState.Success : NodeState.Failed;
                    }

                    break;
                }
                case ParallelPolicy.WaitAnySuccess:
                {
                    if (result.GetCount(NodeState.Running) == 0)
                    {
                        State = result.GetCount(NodeState.Success) != 0 ? NodeState.Success : NodeState.Failed;
                    }

                    break;
                }
                case ParallelPolicy.AnySuccess:
                {
                    if (result.GetCount(NodeState.Success) != 0)
                    {
                        State = NodeState.Success;
                    }
                    else if (result.GetCount(NodeState.Running) == 0)
                    {
                        State = NodeState.Failed;
                    }

                    break;
                }
                case ParallelPolicy.AllSuccess:
                {
                    if (result.GetCount(NodeState.Failed) != 0)
                    {
                        State = NodeState.Failed;
                    }
                    else if (result.GetCount(NodeState.Running) == 0)
                    {
                        State = NodeState.Success;
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private struct ParallelStates
        {
            private Dictionary<NodeState, int> stateCount;

            public void Add(BehaviorNode node)
            {
                switch (node.State)
                {
                    case NodeState.Running:
                    case NodeState.Success:
                    case NodeState.Failed:
                    case NodeState.Aborted:
                    {
                        if (!stateCount.TryAdd(node.State, 1))
                        {
                            stateCount[node.State] += 1;
                        }

                        break;
                    }
                    case NodeState.None:
                    case NodeState.Init:
                    default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public int GetCount(NodeState state)
            {
                if (!stateCount.TryGetValue(state, out var count))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return count;
            }
        }
    }

    public class ParallelWaitAllSuccess : Parallel
    {
        public ParallelWaitAllSuccess()
        {
            Policy = ParallelPolicy.WaitAllSuccess;
        }
    }

    public class ParallelWaitAnySuccess : Parallel
    {
        public ParallelWaitAnySuccess()
        {
            Policy = ParallelPolicy.WaitAnySuccess;
        }
    }

    public class ParallelAnySuccess : Parallel
    {
        public ParallelAnySuccess()
        {
            Policy = ParallelPolicy.AnySuccess;
        }
    }

    public class ParallelAllSuccess : Parallel
    {
        public ParallelAllSuccess()
        {
            Policy = ParallelPolicy.AllSuccess;
        }
    }
}