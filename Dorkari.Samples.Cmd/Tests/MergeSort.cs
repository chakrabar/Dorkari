using System;

namespace Dorkari.Samples.Cmd.Tests
{
    class MergeSort
    {
        public int number = 1;

        public void mergeSort(int[] sortArray, int lower, int upper)
        {
            int middle;
            if (upper == lower)
                return;
            else
            {
                middle = (lower + upper) / 2;
                mergeSort(sortArray, lower, middle);
                mergeSort(sortArray, middle + 1, upper);
                Merge(sortArray, lower, middle + 1, upper);
            }
        }

        public void Merge(int[] sortArray, int lower, int middle, int upper)
        {
            string[] temp = new string[sortArray.Length];
            int lowEnd = middle - 1;
            int low = lower;
            int n = upper - lower + 1;
            while ((lower <= lowEnd) && (middle <= upper))
            {
                if (sortArray[lower] <= sortArray[middle])
                {
                    temp[low] = sortArray[lower].ToString();
                    low++;
                    lower++;
                }
                else
                {
                    temp[low] = sortArray[middle].ToString();
                    low++;
                    middle++;
                }
            }
            while (lower <= lowEnd)
            {
                temp[low] = sortArray[lower].ToString();
                low++;
                lower++;
            }
            while (middle <= upper)
            {
                temp[low] = sortArray[middle].ToString();
                low++;
                middle++;
            }
            for (int i = 0; i < n; i++)
            {
                sortArray[upper] = Int32.Parse(temp[upper]);
                upper--;
            }
        }

        public void MergeStrings(string[] sortArray, int lower, int middle, int upper)
        {
            string[] temp = new string[sortArray.Length];
            int lowEnd = middle - 1;
            int low = lower;
            int n = upper - lower + 1;
            while ((lower <= lowEnd) && (middle <= upper)) //here
            {
                if (sortArray[lower].CompareTo(sortArray[middle]) < 1)
                {
                    temp[low] = sortArray[lower];
                    low++;
                    lower++;
                }
                else
                {
                    temp[low] = sortArray[middle];
                    low++;
                    middle++;
                }
            }
            while (lower <= lowEnd)
            {
                temp[low] = sortArray[lower].ToString();
                low++;
                lower++;
            }
            while (middle <= upper)
            {
                temp[low] = sortArray[middle].ToString();
                low++;
                middle++;
            }
            for (int i = 0; i < n; i++)
            {
                sortArray[upper] = temp[upper];
                upper--;
            }
        }
    }
}
