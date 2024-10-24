namespace Yd.AI.BehaviourTree
{
    public class Inverter : Decorator
    {
        protected override void OnTick()
        {
            base.OnTick();

            Node.Tick();

            State = Node.State switch
            {
                NodeState.Success => NodeState.Failed,
                NodeState.Failed => NodeState.Success,
                _ => Node.State
            };
        }
    }
}