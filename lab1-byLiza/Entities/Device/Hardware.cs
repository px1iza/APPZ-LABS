using System;

namespace GameSimulation
{
    public class Hardware
    {
        public int Cpu { get; }
        public int Ram { get; }
        public int Gpu { get; }

        public Hardware(int cpu, int ram, int gpu)
        {
            Cpu = cpu;
            Ram = ram;
            Gpu = gpu;
        }

        public bool Meets(Hardware req)
        {
            return Cpu >= req.Cpu &&
                   Ram >= req.Ram &&
                   Gpu >= req.Gpu;
        }
    }
}