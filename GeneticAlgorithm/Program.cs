using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GeneticAlgorithm
{
    class Program
    {
        private const double min = 0;
        private const double max = 100;
        private static double mutationCont = 0.5;
        private static double errorRate = 0.1;
        private static double expectedResult = 20;

        private static Random random = new Random();
        private static Stopwatch stopwatch;
        private static List<Solution> solutions = new List<Solution>();
      
        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        private static double CalculateValue(double x, double y, double z)
        {
            return (4 * x + 8 * y + 5 * z)/ 3 * Math.Sqrt(y) + 2 * Math.Sqrt(x) * Math.Pow(z,2);
        }

        private static double fitness(double value)
        {
            double calculation = value - expectedResult;

            if (calculation == 0) return 9999;
            return Math.Abs(1 / calculation);
        }

        private static void generateSolutions()
        {
            for (int i = 0; i < 500; i++)
            {
                double x = RandomNumberBetween(min, max);
                double y = RandomNumberBetween(min, max);
                double z = RandomNumberBetween(min, max);
                double calculatedValue = CalculateValue(x, y, z);
                double fitnessValue = fitness(calculatedValue);
                Solution solution = new Solution(x, y, z, calculatedValue, fitnessValue);
                solutions.Add(solution);
            }
        }

        private static void mutation(Solution solution)
        {
            int whichVariable = random.Next(0, 3);
            switch(whichVariable) {
                case 0:
                    solution.x = solution.x + expectedResult * (RandomNumberBetween(-mutationCont, mutationCont)/100);
                    break;

                case 1:
                    solution.y = solution.y + expectedResult * (RandomNumberBetween(-mutationCont, mutationCont) / 100);
                    break;

                case 2:
                    solution.z = solution.z + expectedResult * (RandomNumberBetween(-mutationCont, mutationCont) / 100);
                    break;
            }
        }

        private static void listPopulation()
        {
            for (int i = 0; i <= 50; i++)
            {
                Solution solution = (Solution)solutions[i];
                double error = solution.result / expectedResult * 100;
                String formatedError = String.Format("{0:0.0000}", error);
                Console.WriteLine($"({i}.) x = {solution.x},  y = {solution.y}, z = {solution.z}, " +
                                 $"result =  {solution.result},  match = {formatedError} %");
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n...........................................................................................................");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void generations()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n...........................................................................................................\n");
            Console.WriteLine("Starting population\n...........................................................................................................\n");
            Console.ForegroundColor = ConsoleColor.White;
            listPopulation();

            int i = 0;
            while(stopwatch.Elapsed.Minutes < 1)
            {
                i++;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Current generation: " + i+ "\n...........................................................................................................\n");
                
                solutions = createNextGeneration();
                solutions.Sort();
                solutions.Reverse();

                Console.ForegroundColor = ConsoleColor.White;
                listPopulation();

                Solution solution = solutions[0];
                double error = solution.result / expectedResult * 100;
                if (error >= 100 - errorRate && error <= 100 + errorRate) {break;}
            }
        }

        private static List<Solution> createNextGeneration()
        {
            List<Solution> nextSolutions = new List<Solution>();

            for(int i=0;i<=500;i++)
            {
                int chosenSol1 = -1;
                int chosenSol2 = -1;

                do
                {
                    chosenSol1 = random.Next(0, 51);
                    chosenSol2 = random.Next(0, 51);
                }
                while (chosenSol1 == chosenSol2);

                double x = solutions[chosenSol1].x;
                double y = solutions[chosenSol2].y;
                double z = solutions[chosenSol1].z;
                Solution solution = new Solution(x, y, z);
                mutation(solution);

                double result = CalculateValue(x, y, z);
                double calculatedFitness = fitness(result);

                solution.result = result;
                solution.fitness = calculatedFitness;
                nextSolutions.Add(solution);
            }
            return nextSolutions;
        }   

        private static void communication()
        {
            Console.WriteLine("The equation is: (4 * x + 8 * y + 5 * z)/ 3 * sqrt(y) + 2 * sqrt(x) * z^2).\n");
            Console.WriteLine("What is your desired result to the equation above?");
            Console.Write("My result is: ");
            double desiredResult = Convert.ToDouble(Console.ReadLine());
            expectedResult = desiredResult;

            Console.WriteLine(".........................................................................................");
            Console.Write("Accepted error rate (in %): ");
            double acceptedError = Convert.ToDouble(Console.ReadLine());
            errorRate = acceptedError;

            Console.WriteLine(".........................................................................................");
            Console.Write("rate of mutations (in %): ");
            double mutationRate = Convert.ToDouble(Console.ReadLine());
            mutationCont = mutationRate;
        }

        private static void evolution()
        {
            generateSolutions();
            solutions.Sort();
            solutions.Reverse();
            generations();
        }

        private static void conclusion(Stopwatch stopwatch)
        {
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine("The expected result is: " + expectedResult);
            Console.WriteLine("The best calculated result: " + solutions[0].result);
            Console.WriteLine("Time of calculation : " + ts.Minutes + " minutes " + ts.Seconds + " seconds " + ts.Milliseconds + " milliseconds.");
            Console.WriteLine("Variables\n....................................................................");
            Console.WriteLine("X = " + solutions[0].x);
            Console.WriteLine("Y = " + solutions[0].y);
            Console.WriteLine("Z = " + solutions[0].z);
        }

        static void Main(string[] args) {
            stopwatch = new Stopwatch();
            communication();

            stopwatch.Start();
            evolution();
            stopwatch.Stop();

            conclusion(stopwatch);
        }
    }
}
