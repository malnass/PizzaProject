using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaProject
{
    class main
    {
        public static int C;
        public static int L;
        public static int H;
        public static int R;

        public static int score = 0;
        public static int slices = 0;
        public static char[,] pizza;
        public static List<string> outputList;


        static void Main(string[] args)
        {
            outputList = new List<string>();
            String name = "c_medium"; // The name of the imput file (without .in extension)  


            String[] lines = System.IO.File.ReadAllLines(name + @".in");

            string  firstline;

            firstline = lines[0];
            lines=lines.Where(l => l != lines[0]).ToArray();
            String[] vars;
            vars = firstline.Split(' ');

            R = Convert.ToInt32(vars[0]);
            C = Convert.ToInt32(vars[1]);
            L = Convert.ToInt32(vars[2]);
            H = Convert.ToInt32(vars[3]);

            pizza = new char[R,C];
           // System.IO.File.WriteAllLines(name + @".in", lines.Skip(1).ToArray());

            int i = 0;
            int j;
            foreach (string _line in lines)
            {
                //char[] letters = _line;
                j = 0;
                foreach (char letter in _line)
                {
                    pizza[i,j] = letter;
                    j++;
                }
                i++;
            }
            //Console.WriteLine( pizza[0,0]);

            DVD(0, 0, R - 1, C - 1); // Solves the problem recursively.
            Console.WriteLine("Score: " + score);
            Console.WriteLine("Slices: " + slices);

            // Create output file.
            List<string> finaloutputline = new List<string>();
            finaloutputline.Add(slices.ToString());
            foreach (string outputline in outputList)
            {
                finaloutputline.Add(outputline);
            }
            //System.IO.File.WriteAllText("C:/Users/User/source/repos/PizzaProject/PizzaProject" + name + ".out", slices.ToString());
            System.IO.File.WriteAllLines(name + ".out", finaloutputline);



        }

        public static void DVD(int startI, int startJ, int endI, int endJ)
        {

            PizzaCut maxProb = partition(startI, startJ, endI, endJ); // Get the best position to cut.

            if (maxProb == null)
            {
                return;
            }

            if (maxProb.horizontal)
            {
                DVD(startI, startJ, startI + maxProb.i, endJ); // Call DVD for the upper slice
                DVD(startI + maxProb.i + 1, startJ, endI, endJ); // Call DVD for the lower slice
            }
            else
            {
                DVD(startI, startJ, endI, startJ + maxProb.i); // Call DVD for the left slice.
                DVD(startI, startJ + maxProb.i + 1, endI, endJ); // Call DVD for the right slice.
            }

        }


        public static PizzaCut partition(int startI, int startJ, int endI, int endJ)
        {

            if (startI == endI && startJ == endJ)
            {
                return null;
            }

            // Check if this slice satisfies the conditions.

            double[] pt = check(startI, startJ, endI, endJ);

            if (pt[1] == 1)
            { // If all conditions are satisfied take the whole slice as a slice.

                Console.WriteLine("SLICE: " + startI + " " + startJ + " " + endI + " " + endJ);
                outputList.Add(startI + " " + startJ + " " + endI + " " + endJ);

                score +=Convert.ToInt32( pt[2]);
                slices++;

                return null;

            }
            else if (pt[1] == 0)
            {
                return null;
            }


            List<PizzaCut> list = new List<PizzaCut>(); // List that contains all possible cuts, vertical and horizontal.

            double prob;

            // Cut vertical.

            int over = endJ - startJ;

            for (int i = 0; i < over; i++)
            {

                double p1 = check(startI, startJ, endI, startJ + i)[0]; // Probability for the left slice.
                double p2 = check(startI, startJ + i + 1, endI, endJ)[0]; // Probability for the right slice.

                prob = p1 * p2;

                list.Add(new PizzaCut(i, false, prob)); // false indicates that this cut is vertical.
            }

            // Cut horizontal.

            over = endI - startI;

            for (int i = 0; i < over; i++)
            {

                double p1 = check(startI, startJ, startI + i, endJ)[0]; // Probability for the upper slice.
                double p2 = check(startI + i + 1, startJ, endI, endJ)[0]; // Probability for the lower slice.

                prob = p1 * p2;

                list.Add(new PizzaCut(i, true, prob)); // false indicates that this cut is horizontal.
            }

            // Search for the best probability and cut the pizza there.

            PizzaCut maxProb = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].probability > maxProb.probability)
                {
                    maxProb = list[i];
                }
            }

            return new PizzaCut(maxProb.i, maxProb.horizontal, maxProb.probability);

        }

        public static double[] check(int startI, int startJ, int endI, int endJ)
        {

            int T = 0; // Number of tomatoes taken in this slice.
            int M = 0; // Number of mushrooms taken in this slice.

            for (int i = startI; i <= endI; i++)
            {
                for (int j = startJ; j <= endJ; j++)
                {
                    if (pizza[i,j]=='T')
                    {
                        T++;
                    }
                    else
                    {
                        M++;
                    }
                }
            }

            int total = T + M; // Number of pieces taken in this slice.

            double t = 0; // Tomatoes left to add to reach L.
            if (T < L)
            {
                t = L - T;
            }

            double m = 0; // Mushrooms left to add to reach L.
            if (M < L)
            {
                m = L - M;
            }

            double s = 0; // Pieces left to add to reach M. 
            if (H > total)
            {
                s = H - total;
            }

            double s2 = 0; // How more pieces than M.
            if (H < total)
            {
                s2 = total - H;
            }

            double q = 0; // 0 if L condition is satisfied, 1 otherwise.
            if (t == 0 && m == 0)
            {
                q = 1;
            }

            double tick = 0; // 2 if both conditions are satisfied, 1 if only M condition is satisfied, 0 otherwise.
            if (T >= L && M >= L)
            {
                if (total > H)
                {
                    tick = 2;
                }
                else
                {
                    tick = 1;
                }
            }

            double tipos = (1 / 3.0) * ((1 - ((t + m) / (2.0 * L))) + q * (1 - s / H) + (s2 / (T + M)) * ((Math.Min(T, M) * 1.0) / Math.Max(T, M)));

            double[] res = { tipos, tick, total };

            return res;

        }
    }
}

