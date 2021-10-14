using System;

namespace ReimbursementCalculation
{
    class Program
    {
        // NOTE: this is currently not going to work if there are ever 3 projects on the same day.
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DateTime date1 = new DateTime(2008, 4, 10);
            DateTime date2 = new DateTime(2008, 6, 10);
            int thisDate = (date2.Date - date1.Date).Days;
            Console.WriteLine(thisDate);


            // Sample data - not quite enough to warrant a resource file.

            // Sample 1
            DateTime[] Starts1 = new DateTime[1];
            Starts1[0] = new DateTime(2015, 9, 1);

            DateTime[] Ends1 = new DateTime[1];
            Ends1[0] = new DateTime(2015, 9, 3);

            bool[] Costs1 = new bool[1];
            Costs1[0] = false;

            // Sample 2
            DateTime[] Starts2 = new DateTime[3];
            Starts2[0] = new DateTime(2015, 9, 1);
            Starts2[1] = new DateTime(2015, 9, 2);
            Starts2[2] = new DateTime(2015, 9, 6);

            DateTime[] Ends2 = new DateTime[3];
            Ends2[0] = new DateTime(2015, 9, 1);
            Ends2[1] = new DateTime(2015, 9, 6);
            Ends2[2] = new DateTime(2015, 9, 8);

            bool[] Costs2 = new bool[3];
            Costs2[0] = false;
            Costs2[0] = true;
            Costs2[0] = false;

            // Sample 3
            DateTime[] Starts3 = new DateTime[3];
            Starts3[0] = new DateTime(2015, 9, 1);
            Starts3[1] = new DateTime(2015, 9, 5);
            Starts3[2] = new DateTime(2015, 9, 8);

            DateTime[] Ends3 = new DateTime[3];
            Ends3[0] = new DateTime(2015, 9, 3);
            Ends3[1] = new DateTime(2015, 9, 7);
            Ends3[2] = new DateTime(2015, 9, 8);

            bool[] Costs3 = new bool[3];
            Costs3[0] = false;
            Costs3[0] = true;
            Costs3[0] = true;

            // Sample 4
            DateTime[] Starts4 = new DateTime[4];
            Starts4[0] = new DateTime(2015, 9, 1);
            Starts4[1] = new DateTime(2015, 9, 1);
            Starts4[2] = new DateTime(2015, 9, 2);
            Starts4[3] = new DateTime(2015, 9, 2);

            DateTime[] Ends4 = new DateTime[4];
            Ends4[0] = new DateTime(2015, 9, 1);
            Ends4[1] = new DateTime(2015, 9, 1);
            Ends4[2] = new DateTime(2015, 9, 2);
            Ends4[3] = new DateTime(2015, 9, 3);

            bool[] Costs4 = new bool[3];
            Costs4[0] = false;
            Costs4[0] = true;
            Costs4[0] = true;

            // Printing initial screen
            //PrintSet(Start1, Start);
            //PrintSet(End1, End);



        }

        // Prints DateTime array to console
        static void PrintSet(DateTime[] starts, DateTime[] ends, bool[] Costs, int setNo)
        {
            Console.WriteLine("Set " + setNo + ":");
            for (int i = 0; i < starts.Length; i++)
            {
                Console.WriteLine("Project " + (i+1) + ": Start Date:" + starts[i] + " End Date: " + ends[i]);
            }
            Console.WriteLine("Respective Costs:");
            for (int i = 0; i < starts.Length; i++)
            {
                if(Costs[i]) Console.WriteLine("Cost: " + (i + 1) + ": High Cost.");
                else Console.WriteLine("Cost: " + (i + 1) + ": High Cost.");
            }
        }

        /* Takes Date array and returns int array of days
         */

        int[] DatetoIntArray(DateTime[] date)
        {
            int[] retDate = new int[date.Length];
            for(int i = 0; i < date.Length;i++)
            {
                retDate[i] = date[i].Day;
            }
            return retDate;
        }

        /* Going to assume that Begin is a nonincreasing array of the starts of the projects.
         * End is a nonincreasing array of the ends of the projects.
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
