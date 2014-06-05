namespace LINQToAQL.Spatial
{
    /// <summary>
    ///     An AQL <c>Line</c>
    /// </summary>
    public class Line
    {
        /// <summary>
        ///     Creates an AQL <c>Line</c>
        /// </summary>
        /// <param name="first">The first of two points describing the line</param>
        /// <param name="second">The second of two points describing the line</param>
        public Line(Point first, Point second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        ///     Gets the first point describing the line
        /// </summary>
        /// <returns>The first point describing the line</returns>
        public Point First { get; private set; }

        /// <summary>
        ///     Gets the second point describing the line
        /// </summary>
        /// <returns>The second point describing the line</returns>
        public Point Second { get; private set; }
    }
}