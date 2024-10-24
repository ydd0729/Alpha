namespace Yd.AI.BehaviourTree
{
    public abstract class Decorator : BehaviorNode
    {
        protected BehaviorNode Node;

        public void Add(BehaviorNode node)
        {
            Node = node;
        }

        protected override void OnReset()
        {
            base.OnReset();
            Node?.Reset();
        }

        protected override void OnAbort()
        {
            base.OnAbort();
            Node?.Abort();
        }
    }
}