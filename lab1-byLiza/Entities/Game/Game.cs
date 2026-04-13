using System;

namespace GameSimulation
{
    public abstract class Game
    {
        public string Name { get; }
        public bool IsInstalled { get; set; }
        public bool IsRunning { get; set; }

        public int Size { get; }
        public Hardware Requirements { get; }

        public List<Save> Saves { get; } = new();

        public Game(string name, int size, Hardware req)
        {
            Name = name;
            Size = size;
            Requirements = req;
        }
    }
}