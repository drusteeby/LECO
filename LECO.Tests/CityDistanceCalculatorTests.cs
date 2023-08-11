using LECO.Models;

namespace LECO.Tests
{
    [TestClass]
    public class CityDistanceCalculatorTests
    {
        [TestMethod]
        public void DistanceBetweenTwoCities_ShouldEqualFive()
        {
            var result = CityDistanceCalculator.CalculateDistance(new List<City>
            {
                new City("Gotham",0,0),
                new City("St. Joeseph", 3,4)
            });

            Assert.AreEqual(5.0, result);
        }


        [TestMethod]
        public void DistanceBetweenThreeCities_ShouldEqualTen()
        {
            var result = CityDistanceCalculator.CalculateDistance(new List<City>
            {
                new City("Gotham",0,0),
                new City("St. Joeseph", 3,4),
                new City("Ann Arbor", 6,8)
            });

            Assert.AreEqual(10.0, result);
        }

    }
}
