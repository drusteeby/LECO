using LECO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LECO
{
    //https://chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
    public class CityPermutationCalculator
    {
        public static IList<IList<City>> CalculateCityPermutations(IEnumerable<City> list, CancellationToken cancellationToken)
        {
            return Permute(list.ToArray(), cancellationToken);
        }

        private static IList<IList<City>> Permute(City[] nums, CancellationToken cancellationToken) => 
            DoPermute(nums, 0, nums.Length - 1, new List<IList<City>>(), cancellationToken);

        private static IList<IList<City>> DoPermute(City[] nums, int start, int end, IList<IList<City>> list, CancellationToken cancellationToken)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<City>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, list, cancellationToken);
                    Swap(ref nums[start], ref nums[i]);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return list;
        }

        private static void Swap(ref City a, ref City b) => (b, a) = (a, b);

    }
}
