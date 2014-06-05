namespace LINQToAQL.Spatial
{
    /// <summary>
    ///     An AQL <c>Point</c> 
    /// </summary>
    public class Point
    {
        /// <summary>
        ///     Creates an AQL <c>Point</c>
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Gets the x-coordinate of the <c>Point</c>
        /// </summary>
        /// <returns>The x-coordinate</returns>
        public double X { get; private set; }

        /// <summary>
        ///     Gets the y-coordinate of the <c>Point</c>
        /// </summary>
        /// <returns>The y-coordinate</returns>
        public double Y { get; private set; }
    }
}