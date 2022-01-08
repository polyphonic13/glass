namespace Polyworks
{
    using UnityEngine;

    public class LookAtCollection : MonoBehaviour
    {
        public Transform[] Targets;
        public float Speed = 2f;

        private Transform currentTarget;
        private int currentIndex = 0;

        public void SetTarget(int index)
        {
            if (index > Targets.Length - 1)
            {
                return;
            }
            currentTarget = Targets[index];
        }

        public void IncrementTarget()
        {
            if (currentIndex >= Targets.Length - 1)
            {
                return;
            }
            currentIndex++;
            currentTarget = Targets[currentIndex];
        }

        private void Awake()
        {
            currentTarget = Targets[currentIndex];
        }

        private void FixedUpdate()
        {
            if (currentTarget == null)
            {
                return;
            }

            Vector3 relativePos = currentTarget.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Speed * Time.deltaTime);
        }
    }
}
