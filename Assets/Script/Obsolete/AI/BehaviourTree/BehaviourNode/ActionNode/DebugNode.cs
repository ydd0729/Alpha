using UnityEngine;

namespace Yd.AI.BehaviourTree
{
    public class DebugNode : BehaviorNode
    {
        private string s;

        public DebugNode(string s)
        {
            this.s = s;
        }

        protected override void OnTick()
        {
            base.OnTick();
            Debug.Log(s);

            State = NodeState.Success;
        }
    }
}