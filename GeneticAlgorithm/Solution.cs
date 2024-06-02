using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GeneticAlgorithm
{
    public class Solution : IComparable<Solution>
    {
        public double result;
        public double x;
        public double y;
        public double z;
        public double fitness;

        public Solution(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.result = 0;
            this.fitness = 0;
        }

        public Solution(double x, double y, double z, double result, double fitness) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.result = result;
            this.fitness = fitness;
        }

        public int CompareTo( Solution other)
        {
            return this.fitness.CompareTo(other.fitness);
        }
    }
}
