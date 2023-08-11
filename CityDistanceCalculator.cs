using LECO.Models;
using Prism.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LECO
{
    public class CityDistanceCalculator : BindableBase
    {
        private int _numberProcessed;
        public int NumberProcessed
        {
            get => _numberProcessed;
            set => SetProperty(ref _numberProcessed, value);
        }

        /// <summary>
        /// Gets the shortest distance between a list of cites, starting and ending at the Home City
        /// </summary>
        /// <param name="HomeCity">City to start and end travel</param>
        /// <param name="cities">List of cities to travel to</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CalculatedPermutation> GetShortestDistanceAsync(City HomeCity, IList<City> cities, CancellationToken cancellationToken = default)
        {
            var permutations = CityPermutationCalculator.CalculateCityPermutations(cities, cancellationToken);

            if (HomeCity == null) 
            {
                return await GetShortestDistanceAsync(permutations, cancellationToken);
            }

            //could also use LINQ here to not mutate the passed in list. Would have to test for performance differences.
            //var newCityList = permutations.Select(list => list.Prepend(HomeCity).Append(HomeCity));
            foreach(var cityList in permutations) 
            {
                cityList.Add(HomeCity);
                cityList.Insert(0, HomeCity);
            }

            return await GetShortestDistanceAsync(permutations, cancellationToken);
        }

        public async Task<CalculatedPermutation> GetShortestDistanceAsync(IList<IList<City>> permutations, CancellationToken cancellationToken = default)
        {
            var _calculatedDistances = new ConcurrentBag<CalculatedPermutation>();
            await Parallel.ForEachAsync(permutations, cancellationToken, async (list, cancellationToken) =>
            {
                try
                {
                    await Task.Factory.StartNew(() => _calculatedDistances.Add(new CalculatedPermutation(list, CalculateDistance(list, cancellationToken))), cancellationToken); 
                    cancellationToken.ThrowIfCancellationRequested();
                }
                finally
                {

                    Interlocked.Increment(ref _numberProcessed);
                    RaisePropertyChanged(nameof(NumberProcessed));
                }
            });

            return _calculatedDistances.Min();
        }

        public static double CalculateDistance(IList<City> list, CancellationToken cancellationToken = default)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count <= 1)
            {
                return 0;
            }

            double distance = 0;

            for (var cityNum = 0; cityNum < list.Count - 1; cityNum++)
            {
                distance += Point.Subtract(list[cityNum].Point, list[cityNum + 1].Point).Length;
                cancellationToken.ThrowIfCancellationRequested();
            }

            return distance;
        }


    }
}
