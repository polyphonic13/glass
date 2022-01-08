namespace Polyworks
{
    // https://en.wikipedia.org/wiki/B%C3%A9zier_curve
    [System.Serializable]
    public struct CubicBezierCurveCollection
    {
        public string name;
        public CubicBezierCurve[] curves;
    }

    [System.Serializable]
    public class CubicBezierCurve : Shape2D
    {
        #region public static consts
        public static readonly int NUM_POINTS = 4;
        #endregion

        #region unity methods
        private void Awake()
        {
            this.length = ShapeUtils.GetCurveLength(this);
        }
        #endregion
    }

}
