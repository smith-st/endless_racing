namespace BikeParts
{
    public interface IRoadContactListener
    {
        void StartContactWithRoad(string tag);
        void StopContactWithRoad(string tag);
    }
}
