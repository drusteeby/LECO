namespace LECO.Models
{
    public record City : Coordinates
    {
        public City(string name, double longitude, double latitude) : base(longitude, latitude)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
