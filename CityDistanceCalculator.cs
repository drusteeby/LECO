using LECO.Models;
using Prism.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

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

        public async Task<CalculatedPermutation> GetShortestDistanceAsync(IList<IList<City>> permutations, CancellationToken cancellationToken = default)
        {
            var _calculatedDistances = new ConcurrentBag<CalculatedPermutation>();
            await Parallel.ForEachAsync(permutations, async (list, cancellationToken) =>
            {
                try
                {
                    var calculationTask = Task.Factory.StartNew(() => _calculatedDistances.Add(new CalculatedPermutation(list, CalculateDistance(list, cancellationToken))), cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    await calculationTask;
                }
                finally
                {
                    Interlocked.Increment(ref _numberProcessed);
                    RaisePropertyChanged(nameof(NumberProcessed));
                }
            });

            return _calculatedDistances.Min();
        }

        public double CalculateDistance(IList<City> list, CancellationToken cancellationToken = default)
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
