using System;
using System.Collections.Generic;

namespace GameSimulation
{
    /// Фабрика для створення різних типів ігор
    public class GameFactory : IGameFactory
    {
        private static readonly Dictionary<string, Func<string, int, Hardware, Game>> GameCreators
            = new(StringComparer.OrdinalIgnoreCase)
            {
                { "RPG", (name, size, req) => new RPGGame(name, size, req) },
                { "Strategy", (name, size, req) => new StrategyGame(name, size, req) },
                { "Adventure", (name, size, req) => new AdventureGame(name, size, req) }
            };

        public Game CreateGame(string gameType, string name, int size, Hardware requirements)
        {
            if (string.IsNullOrWhiteSpace(gameType))
                throw new ArgumentException("Game type cannot be empty", nameof(gameType));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Game name cannot be empty", nameof(name));

            if (size <= 0)
                throw new ArgumentException("Game size must be greater than 0", nameof(size));

            if (requirements == null)
                throw new ArgumentNullException(nameof(requirements));

            if (!GameCreators.TryGetValue(gameType, out var creator))
                throw new ArgumentException(
                    $"Unknown game type: {gameType}. Available types: {string.Join(", ", GetAvailableGameTypes())}",
                    nameof(gameType));

            return creator(name, size, requirements);
        }

        public IEnumerable<string> GetAvailableGameTypes()
        {
            return GameCreators.Keys;
        }
    }
}