using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheKiwiCoder
{
    public class PhaseOneSelector : CompositeNode
    {
        protected int current = 0;
        [SerializeField]
        protected int prevPatternIdx = 0;

        protected override void OnStart()
        {
            if (current >= children.Count)
                current = 0;
            Debug.Log("Start");
        }

        protected override void OnStop()
        {
            Debug.Log("Stop");
        }

        protected override State OnUpdate()
        {
            var child = children[0];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    break;
            }

            child = children[Random.Range(1, children.Count)];
            
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    return State.Failure;
            }

            return State.Failure;
        }
    }
}
