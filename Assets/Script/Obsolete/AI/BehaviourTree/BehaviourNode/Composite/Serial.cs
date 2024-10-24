using System.Collections.Generic;

namespace Yd.AI.BehaviourTree
{
    public abstract class Serial : Composite
    {
        protected List<BehaviorNode>.Enumerator Enumerator;

        protected override void OnReset()
        {
            base.OnReset();

            Enumerator = Children.GetEnumerator();
        }
    }
}