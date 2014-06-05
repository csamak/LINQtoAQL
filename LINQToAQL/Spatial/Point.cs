namespace LINQToAQL.Spatial
{
    /// <summary>
    ///     TODO: asdf
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double X { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double Y { get; private set; }
    }
}