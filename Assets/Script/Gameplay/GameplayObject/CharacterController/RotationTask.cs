using System;
using UnityEngine;

namespace Yd.Gameplay.Object
{
    public struct RotationTask
    {
        private Quaternion origin;
        private float rotationTime;
        private float timePassed;

        public Quaternion Target
        {
            get; private set;
        }
        public bool Executing
        {
            get; private set;
        }

        public void SetTask(Quaternion inOrigin, Quaternion inTarget, float timeToRotateHalfCircle)
        {
            origin = inOrigin;
            Target = inTarget;
            rotationTime = timeToRotateHalfCircle;
            timePassed = 0;
            Executing = true;
        }

        public Quaternion Execute()
        {
            timePassed += Time.deltaTime;
            if (timePassed >= rotationTime)
            {
                Executing = false;
            }

            return Quaternion.Slerp(origin, Target, Math.Min(timePassed / rotationTime, 1));
        }
    }
}