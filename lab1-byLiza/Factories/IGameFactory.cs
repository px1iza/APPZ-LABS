namespace GameSimulation
{
    /// Интерфейс для фабрики створення ігор
    public interface IGameFactory
    {
        Game CreateGame(string gameType, string name, int size, Hardware requirements);

        /// Отримує доступні типи ігор
        IEnumerable<string> GetAvailableGameTypes();
    }
}