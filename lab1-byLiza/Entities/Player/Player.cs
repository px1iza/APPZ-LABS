using System;

namespace GameSimulation
{
    public class Player
    {
        public string Name { get; }

        public GameAccount Account { get; } = new();

        public Game? CurrentGame { get; set; }

        public Device? CurrentDevice { get; set; }

        public List<Game> Games { get; } = new();

        public Player(string name) => Name = name;
    }
}