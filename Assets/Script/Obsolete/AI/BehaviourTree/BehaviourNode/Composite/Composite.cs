using System.Collections.Generic;
using System.Linq;

namespace Yd.AI.BehaviourTree
{
    public abstract class Composite : BehaviorNode
    {
        protected readonly List<BehaviorNode> Children = new();

        public void Add(BehaviorNode node)
        {
            Children.Add(node);
        }
        
        public void AddRange(List<BehaviorNode> nodes)
        {
            Children.AddRange(nodes);
        }
        
        protected override void OnReset()
        {
            base.OnReset();

            foreach (var child in Children.Where(child => child.IsExited))
            {
                child.Reset();
            }
        }

        protected override void OnAbort()
        {
            base.OnAbort();

            foreach (var child in Children.Where(child => child.IsRunning))
            {
                child.Abort();
            }
        }
    }
}