using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yd.AI.BehaviourTree
{
    public class BehaviourTree
    {
        public BehaviorNode Root;

        public void Reset()
        {
            Root.Reset();
        }

        public void Tick()
        {
            Root.Tick();
        }

        public void Abort()
        {
            Root.Abort();
        }
    }

    public class BehaviourTreeBuilder
    {
        private Stack<BehaviorNode> buildStack = new();
        public BehaviourTree Tree { get; private set; } = new();


        public BehaviourTreeBuilder Add(BehaviorNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }

            if (Tree.Root == null)
            {
                Tree.Root = node;
            }
            else if (buildStack.TryPeek(out var top))
            {
                switch (top)
                {
                    case Composite composite:
                        composite.Add(node);
                        break;
                    case Decorator decorator:
                        decorator.Add(node);
                        break;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return this;
        }

        public BehaviourTreeBuilder Push(BehaviorNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }

            Add(node);

            if (node is Composite or Decorator)
            {
                buildStack.Push(node);
            }
            else
            {
                throw new InvalidOperationException();
            }

            return this;
        }

        public BehaviourTreeBuilder Pop()
        {
            buildStack.Pop();

            return this;
        }

        public void Clear()
        {
            if (buildStack.Count != 0)
            {
                Debug.LogWarning("the build stack is not empty!");
                buildStack.Clear();
            }

            Tree = new BehaviourTree();
        }
    }
}