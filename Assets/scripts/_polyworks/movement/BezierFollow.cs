namespace Polyworks
{
    using System.Collections;
    using UnityEngine;

    [System.Serializable]
    public class BezierFollow : MonoBehaviour
    {
        public float speed = 1f;

        public bool IsAutoIncrement = true;
        public bool IsLooped = false;

        private int curveIndex = 0;

        private float tParam = 0;

        private Vector3 targetPosition;
        private bool isCoroutineAllowed = false;

        private CubicBezierCurve[] curves;
        private CubicBezierCurve currentCurve;
        private Vector3 p0;
        private Vector3 p1;
        private Vector3 p2;
        private Vector3 p3;
        private float currentSpeed;

        private System.Action<bool> allCompletedCallback;
        private System.Action<int> curveCompletedCallback;

        public void SetCurves(CubicBezierCurve[] curves)
        {
            this.curves = curves;
        }

        public void Go(System.Action<bool> completedCallback = null, System.Action<int> curveCallback = null)
        {
            if (curves.Length == 0)
            {
                return;
            }

            if (completedCallback != null)
            {
                allCompletedCallback = completedCallback;
            }

            if (curveCallback != null)
            {
                curveCompletedCallback = curveCallback;
            }
            isCoroutineAllowed = true;
        }

        private void FixedUpdate()
        {
            // Debug.Log("BezierFollow/FixedUpdate, isCoroutineAllowed = " + isCoroutineAllowed);
            if (!isCoroutineAllowed)
            {
                return;
            }
            StartCoroutine(followCurve());
        }

        private IEnumerator followCurve()
        {
            isCoroutineAllowed = false;

            currentCurve = curves[curveIndex];

            p0 = currentCurve.points[0].position;
            p1 = currentCurve.points[1].position;
            p2 = currentCurve.points[2].position;
            p3 = currentCurve.points[3].position;
            // Debug.Log("p0 / p1 / p2 / p3 = " + p0 + " / " + p1 + " / " + p2 + " / " + p3);
            float length = ShapeUtils.GetCurveLength(currentCurve);
            currentSpeed = speed / length;

            while (tParam < 1)
            {
                tParam += Time.deltaTime * currentSpeed;
                targetPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                    3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                    3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                    Mathf.Pow(tParam, 3) * p3;

                transform.position = targetPosition;
                yield return new WaitForFixedUpdate();
            }

            tParam = 0;

            notifyCurveCompleted(curveIndex);

            if (IsAutoIncrement)
            {
                if (curveIndex < curves.Length - 1)
                {
                    curveIndex++;
                    isCoroutineAllowed = true;
                }
                else
                {
                    curveIndex = 0;

                    if (IsLooped)
                    {
                        isCoroutineAllowed = true;
                    }
                    else
                    {
                        // notifyAllCompleted all lines completed
                        notifyAllCompleted(true);
                        allCompletedCallback = null;
                    }
                }
            }
            else
            {
                // trigger line completed
                notifyAllCompleted(false);
            }
        }

        private void notifyCurveCompleted(int index)
        {
            if (curveCompletedCallback == null)
            {
                return;
            }
            curveCompletedCallback(index);
        }

        private void notifyAllCompleted(bool value)
        {
            if (allCompletedCallback == null)
            {
                return;
            }
            allCompletedCallback(value);
        }
    }
}
