using LECO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LECO
{
    //https://chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
    public class CityPermutationCalculator
    {
        public IList<IList<City>> CalculateCityPermutations(IEnumerable<City> list, CancellationToken cancellationToken)
        {
            return Permute(list.ToArray(), cancellationToken);
        }

        static IList<IList<City>> Permute(City[] nums, CancellationToken cancellationToken)
        {
            var list = new List<IList<City>>();
            return DoPermute(nums, 0, nums.Length - 1, list, cancellationToken);
        }

        static IList<IList<City>> DoPermute(City[] nums, int start, int end, IList<IList<City>> list, CancellationToken cancellationToken)
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

        static void Swap(ref City a, ref City b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

    }
}
