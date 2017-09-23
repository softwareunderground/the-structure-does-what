using System;

namespace geocells
{
    public static class LocationExtensions
    {
        // public static double Distance(this ILocation a, ILocation b) =>
        //     Math.Sqrt(DistanceSquared(a, b));
  
        public static double DistanceSquared(this ILocation a, ILocation b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }
    }
}
