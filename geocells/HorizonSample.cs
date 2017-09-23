namespace geocells
{
    public class HorizonSample : ILocation
    {
        public long Inline { get; set; }
        public long Crossline { get; set; }
        public double Z { get; set; }
        public double Porosity { get; set; }
        public double Amplitude { get; set; }

        public double X => Inline;
        public double Y => Crossline;

        // Output
        public double DistanceToNearestWell { get; set; }
    }
}
