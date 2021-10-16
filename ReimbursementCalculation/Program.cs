using System;
using System.Collections.Generic;

namespace ReimbursementCalculation
{
    // Set contains a set of projects 
    public class Set
    {
        DateTime[] Starts;
        DateTime[] Ends;
        bool[] Costs;
        int SetNo;
        int Cost;

        public Set(DateTime[] starts, DateTime[] ends, bool[] costs, int setNo)
        {   
            // check for same lengths of begin dates, end dates, and costs
            if(starts.Length == ends.Length && starts.Length == costs.Length)
            {
                Starts = starts;
                Ends = ends;
                Costs = costs;
                SetNo = setNo;
                Cost = 0;
            }
        }

        // Prints DateTime array to console
        public void PrintSet()
        {
            Console.WriteLine("Set " + SetNo + ":");
            for (int i = 0; i < Starts.Length; i++)
            {
                Console.WriteLine("Project " + (i + 1) + ": Start Date:" + Starts[i].ToString("MM/dd/yyyy") + " End Date: " + Ends[i].ToString("MM/dd/yyyy"));
            }
            Console.WriteLine("Respective Costs:");
            for (int i = 0; i < Starts.Length; i++)
            {
                if (Costs[i]) Console.WriteLine("Cost: " + (i + 1) + ": High Cost.");
                else Console.WriteLine("Cost: " + (i + 1) + ": Low Cost.");
            }
            Console.WriteLine("\n");
        }

        // calculate or return calculated cost
        public int getCost()
        {
            if(Cost != 0)
            {
                return Cost;
            }
            else
            {
                int[] begin = DatetoIntArray(Starts);
                int[] end = DatetoIntArray(Ends);
                return Cost =  CalculateCost(begin, end);
            }
        }

        /* Takes Date array and returns int array of days
        */
        public static int[] DatetoIntArray(DateTime[] date)
        {
            int[] retDate = new int[date.Length];
            for (int i = 0; i < date.Length; i++)
            {
                retDate[i] = date[i].Day;
            }
            return retDate;
        }

        /* Going to assume that Begin is a nonincreasing array of the starts of the projects.
         * End is a nonincreasing array of the ends of the projects.
         * Costs is an array representing if each respective project
         * from the arrays is a highcost city or a low cost city
         */
        public int CalculateCost(int[] begin, int[] end)
        {
            // if the lengths are not the same, or there is no length
            if (begin.Length != end.Length || begin.Length != Costs.Length || begin.Length == 0) return 0;

            int cost = 0;
            // stores previous day. set to 0 when there hasnt been a previous day
            int previousDay = 0;
            // gap between end of last and beginning of current (not the amount of days between)
            int currentGap;
            // stores price from previous day.
            int previousPrice = 0;

            int currLength;

            for (int i = 0; i < begin.Length; i++)
            {
                currLength = end[i] - begin[i];
                // adding for the travel gap between the previous,
                // or changing to full days from travel if overlap

                if (previousDay != 0)
                {
                    currentGap = begin[i] - previousDay;
                    if (currentGap == 0)
                    {   
                        // change previous day to the highest full day of the comparison of projects
                        if(Costs[i] || Costs[i-1])
                        {
                            cost += 85 - previousPrice;
                            previousPrice = 85;
                        }
                        else
                        {
                            cost += Math.Max(75 - previousPrice, 0);
                            previousPrice = 75;
                        }
                        // Don't count this project day again
                        if(currLength == 0) continue;
                        // Take one travel day off of the project calculation that is going to happen below/
                        else
                        {
                            if (Costs[i]) cost -= 55;
                            else cost -= 45;
                        }

                    }
                    // no days between - add the amount to make this day a full day (from a travel day) and the previous day
                    // to full day from whatever it was
                    if(currentGap == 1)
                    {
                        // making this day a full day (same for high vs low)
                        cost += 30;
                        // making previous day a full day (if it wasnt already)
                        if (Costs[i - 1])
                        {
                            cost += 85 - previousPrice;
                        }
                        else
                        {
                            cost += 75 - previousPrice;
                        }

                        if (Costs[i])
                        {
                            previousPrice = 85;
                        }
                        else
                        {
                            previousPrice = 75;
                        }
                    }
                    // one day in between - add a travel day of the highest value between the two projects
                    //
                    if(currentGap == 2)
                    {
                        if(Costs[i] || Costs[i-1])
                        {
                            cost += 55;
                        }
                        else
                        {
                            cost += 45;
                        }
                    }
                    // add travel days for both in between travel days
                    if(currentGap > 2)
                    {
                        if (Costs[i]) cost += 55;
                        else cost += 45;
                        if (Costs[i - 1]) cost += 55;
                        else cost += 45;
                    }
                }


                // Below is the project section itself. Everything above accounted for the gaps/ overlap
                //Console.WriteLine(currLength);

                // one or two day project - travel day
                if (currLength < 2)
                {
                    if (Costs[i])
                    {
                        cost += 55 * (currLength + 1);
                        previousPrice = 55;
                    }
                    else
                    {
                        cost += 45 * (currLength + 1);
                        previousPrice = 45;
                    }
                }
                // more than 2 day project. 2 travel days plus full days in between.
                if(currLength >= 2)
                {
                    if (Costs[i])
                    {
                        cost += 110 + 85 * (currLength - 1);
                        previousPrice = 55;
                    }
                    else
                    {
                        cost += 90 + 75 * (currLength - 1);
                        previousPrice = 45;
                    }
                }

                previousDay = end[i];
            }

            // need to append travel days for before the first project and after the last project

            // base for the first and last travel day
            cost += 90;

            // must account for if several projects are on the first/last day, and prioritize high cost project
            int beginorend = begin[0];
            int it = 0;

            // comparator below has to be in this order
            while (it < begin.Length && begin[it] == beginorend)
            {
                if (Costs[it])
                {
                    // additional amount if first day is high cost
                    cost += 10;
                    break;
                }
                it++;
            }

            it = end.Length - 1;
            beginorend = end[it];

            // comparator below has to be in this order
            while (it >= 0 && end[it] == beginorend)
            {
                if (Costs[it])
                {
                    // additional amount if last day is high cost
                    cost += 10;
                    break;
                }
                it--;
            }

            return cost;
        }
    }

    class Program
    {
        static void Main(string[] args)
        { 
            // Sample data - not quite enough to warrant a resource file.

            List<Set> SetList = new List<Set>();

            // Sample 1
            DateTime[] Starts1 = new DateTime[1];
            Starts1[0] = new DateTime(2015, 9, 1);

            DateTime[] Ends1 = new DateTime[1];
            Ends1[0] = new DateTime(2015, 9, 3);

            bool[] Costs1 = new bool[1];
            Costs1[0] = false;

            SetList.Add(new Set(Starts1, Ends1, Costs1, 1));

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
            Costs2[1] = true;
            Costs2[2] = false;

            SetList.Add(new Set(Starts2, Ends2, Costs2, 2));

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
            Costs3[1] = true;
            Costs3[2] = true;

            SetList.Add(new Set(Starts3, Ends3, Costs3, 3));

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

            bool[] Costs4 = new bool[4];
            Costs4[0] = false;
            Costs4[1] = false;
            Costs4[2] = true;
            Costs4[3] = true;

            SetList.Add(new Set(Starts4, Ends4, Costs4, 4));

            // Console display portion below

            int input;
            Console.WriteLine(" --- Select a set by entering its number to see the Project Reimbursement ---");
            foreach (Set S in SetList)
            {
                S.PrintSet();
            }

            Console.WriteLine("Select a set by entering its number to see the Project Reimbursement:");
            while (true)
            {
                try
                {
                    input = Int32.Parse(Console.ReadLine());
                    if (input > 0 && input < 5)
                    {
                        Console.WriteLine("Set " + (input) + " Cost: " + SetList[input - 1].getCost());
                    }
                    else
                    {
                        Console.WriteLine("Please enter an integer within the range of sets provided.");
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter an integer.");
                }
            }

        }
    }
}
