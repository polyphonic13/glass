namespace Polyworks
{
    using UnityEngine;

    [RequireComponent(typeof(BezierFollow))]
    [RequireComponent(typeof(LookAtCollection))]
    public class CurveMovement : MonoBehaviour
    {
        public CubicBezierCurve[] Curves;
        public LookAtCollection LookAtTargets;

        public bool IsAutoStart;
        private BezierFollow bezierFollow;

        public void OnCurveCompleted(int index)
        {
            Debug.Log("CurveMovement/OnCurveCompleted, index = " + index);
            if (LookAtTargets == null)
            {
                return;
            }
            LookAtTargets.IncrementTarget();
        }

        public void OnMovementCompleted(bool isCompleted)
        {
            Debug.Log("CurveMovement/OnMovementCompleted, isCompleted = " + isCompleted);
            LookAtTargets.SetTarget(0);
        }

        private void Start()
        {
            LookAtTargets = GetComponent<LookAtCollection>();
            bezierFollow = GetComponent<BezierFollow>();
            bezierFollow.SetCurves(Curves);

            if (!IsAutoStart)
            {
                return;
            }
            bezierFollow.Go(OnMovementCompleted, OnCurveCompleted);
        }
    }
}
