using System;

namespace ReimbursementCalculation
{
    class Program
    {
        // NOTE: this is currently not going to work if there are ever 3 projects on the same day.
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


        }

        /* Going to assume that Begin is a nonincreasing array of the starts of the projects.
         *  End is a nonincreasing array of the ends of the projects.
         * isHighCost is an array representing if each respective project
         * from the arrays is a highcost city or a low cost city
         */
        int CalculateCost(int[] begin, int[]end, bool[] isHighCost)
        {
            // if the lengths are not the same, or there is no length
            if (begin.Length != end.Length || begin.Length != isHighCost.Length || begin.Length == 0) return 0;

            int cost = 0;
            // stores previous day. set to 0 when there hasnt been a previous day
            int previous = 0;

            int currLength;

            for(int i = 0; i < begin.Length; i++)
            {
                // adding for the travel gap between the previous,
                // or changing to full days from travel if overlap
                if(previous != 0)
                {
                    if(begin[i] == previous)
                    {

                    }
                }

                currLength = end[i] - begin[i];

                // one or two day project - travel day
                if(currLength < 2)
                {
                    if (isHighCost[i])
                    {
                        cost += 55 * (currLength + 1);
                    }
                    else
                    {
                        cost += 45 * (currLength + 1);
                    }
                }
                // more than 2 day project. 2 travel days plus full days in between.
                else
                {
                    if (isHighCost[i])
                    {
                        cost += 110 + 85 * (currLength - 2);
                    }
                    else
                    {
                        cost += 90 + 75 * (currLength - 2);
                    }
                }

                //

                previous = end[i];
            }



            return cost;
        }
    }


}
