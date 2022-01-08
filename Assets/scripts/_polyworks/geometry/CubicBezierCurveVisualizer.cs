namespace Polyworks
{
    using UnityEngine;

    [RequireComponent(typeof(CubicBezierCurve))]
    public class CubicBezierCurveVisualizer : MonoBehaviour
    {
        #region public members
        public float pointFrequency = 0.025f;
        public float pointSize = 0.05f;
        public Color pointColor = Color.white;
        public Color handleColor = Color.red;
        #endregion

        #region private members
        private CubicBezierCurve curve;
        private Vector3 gizmosPosition;

        private float t;
        #endregion

        #region private methods
        private void OnDrawGizmos()
        {
            if (curve == null)
            {
                curve = GetComponent<CubicBezierCurve>();
            }
            if (curve.points == null || curve.points.Length != CubicBezierCurve.NUM_POINTS || ShapeUtils.GetHasNullPoints(curve.points, CubicBezierCurve.NUM_POINTS))
            {
                return;
            }

            Gizmos.color = pointColor;

            for (t = 0; t < 1; t += pointFrequency)
            {
                gizmosPosition = Mathf.Pow(1 - t, 3) * (curve.points[0].position) +
                    3 * Mathf.Pow(1 - t, 2) * t * (curve.points[1].position) +
                    3 * (1 - t) * Mathf.Pow(t, 2) * (curve.points[2].position) +
                    Mathf.Pow(t, 3) * (curve.points[3].position);

                Gizmos.DrawSphere(gizmosPosition, pointSize);
            }
            // \mathbf {B} (t)=(1-t)^{3}\mathbf {P} _{0}+3(1-t)^{2}t\mathbf {P} _{1}+3(1-t)t^{2}\mathbf {P} _{2}+t^{3}\mathbf {P} _{3}{\mbox{ , }}0\leq t\leq 1.
            Gizmos.color = handleColor;

            Gizmos.DrawLine(ShapeUtils.Get2dPointFromTransform(curve.points[0]), ShapeUtils.Get2dPointFromTransform(curve.points[1]));
            Gizmos.DrawLine(ShapeUtils.Get2dPointFromTransform(curve.points[2]), ShapeUtils.Get2dPointFromTransform(curve.points[3]));

            float length = ShapeUtils.GetCurveLength(this.curve);
            // Handles.Label(transform.position, length.ToString());
        }
        #endregion
    }
}
