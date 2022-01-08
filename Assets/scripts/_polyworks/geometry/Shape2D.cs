namespace Polyworks
{
    using UnityEngine;

    [System.Serializable]
    public class Shape2D : MonoBehaviour
    {
        public Transform[] points;

        protected float length;

        public float Length
        {
            get
            {
                return length;
            }
        }
    }
}
