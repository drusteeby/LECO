using LECO;
namespace LECO.Tests
{
    [TestClass]
    public class CityPermutationCalculatorTests
    {
        [TestMethod]
        public void ThreeCites_ShouldEqual_SixTotalPermutations()
        {
            var test = new CityPermutationCalculator();
            var result = test.CalculateCityPermutations(new List<City>
            {
                new City("CityA",0,0),
                new City("CityB",0,0),
                new City("CityC",0,0)
            },
            new CancellationToken());

            Assert.AreEqual(6, result.Count);
        }

        [TestMethod]
        public void FourCites_ShouldEqual_24TotalPermutations()
        {
            var test = new CityPermutationCalculator();
            var result = test.CalculateCityPermutations(new List<City>
            {
                new City("CityA",0,0),
                new City("CityB",0,0),
                new City("CityC",0,0),
                new City("CityD",0,0)
            },
            new CancellationToken());

            Assert.AreEqual(24, result.Count);
        }

        [TestMethod]
        public async Task ShortestDistance_ShouldBeOptimal()
        {
            var test = new CityPermutationCalculator();
            var result = test.CalculateCityPermutations(new List<City>
            {
                new City("CityA",0,0),
                new City("CityB",3,4),
                new City("CityC",3,5),
                new City("CityD",3,6)
            },
            new CancellationToken());

            var distanceCalculator = new CityDistanceCalculator();

            var shortestDistance = await distanceCalculator.GetShortestDistanceAsync(result);

            Assert.AreEqual(7,shortestDistance.TotalDistance);
        }
    }
}