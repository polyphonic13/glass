namespace polyworks
{
    using UnityEngine;

    public class RotationAgent : MonoBehaviour
    {
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;

        private void FixedUpdate()
        {
            transform.Rotate(XSpeed * Time.deltaTime, YSpeed * Time.deltaTime, ZSpeed * Time.deltaTime);
        }
    }
}
