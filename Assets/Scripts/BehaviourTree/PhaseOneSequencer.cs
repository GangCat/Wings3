using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    public class PhaseOneSequencer : CompositeNode
    {
        protected int current = 0;

        protected override void OnStart()
        {
            current = current >= children.Count ? 0 : current;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    ++current;
                    return State.Success;
            }

            return State.Success;
        }
    }
}