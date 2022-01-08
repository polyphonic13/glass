namespace Polyworks
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShapeUtils
    {
        #region public methods
        public static bool GetHasNullPoints(Transform[] points, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (points[i] == null)
                {
                    return true;
                }
            }
            return false;
        }

        public static Vector2 Get2dPointFromTransform(Transform target)
        {
            return new Vector2(target.position.x, target.position.y);
        }

        public static float GetLineLength(Transform[] points)
        {
            return Vector2.Distance(Get2dPointFromTransform(points[0]), Get2dPointFromTransform(points[1]));
        }

        // https://stackoverflow.com/questions/29438398/cheap-way-of-calculating-cubic-bezier-length
        public static float GetCurveLength(CubicBezierCurve curve)
        {
            float arcLength = 0.0f;

            if (curve.points[0] == null || curve.points[0] == null || curve.points[0] == null || curve.points[0] == null)
            {
                return arcLength;
            }

            ArcLengthUtil(curve.points[0].position, curve.points[1].position, curve.points[2].position, curve.points[3].position, 5, ref arcLength);
            return arcLength;
        }

        public static void ArcLengthUtil(Vector3 A, Vector3 B, Vector3 C, Vector3 D, uint subdiv, ref float L)
        {
            if (subdiv > 0)
            {
                Vector3 a = A + (B - A) * 0.5f;
                Vector3 b = B + (C - B) * 0.5f;
                Vector3 c = C + (D - C) * 0.5f;
                Vector3 d = a + (b - a) * 0.5f;
                Vector3 e = b + (c - b) * 0.5f;
                Vector3 f = d + (e - d) * 0.5f;

                // left branch
                ArcLengthUtil(A, a, d, f, subdiv - 1, ref L);
                // right branch
                ArcLengthUtil(f, e, c, D, subdiv - 1, ref L);
            }
            else
            {
                float controlNetLength = (B - A).magnitude + (C - B).magnitude + (D - C).magnitude;
                float chordLength = (D - A).magnitude;
                L += (chordLength + controlNetLength) / 2.0f;
            }
        }
        #endregion
    }
}
