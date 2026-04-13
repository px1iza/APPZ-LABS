using System;

namespace GameSimulation
{
    public class GameRulesService
    {
        public event Action<string>? OnMessage;

        public bool CanRunStrategy(Device device)
        {
            if (device.Platform != Platform.PC)
            {
                OnMessage?.Invoke("Стратегія тільки на ПК");
                return false;
            }

            if (device.OS != OperatingSystemType.Windows)
            {
                OnMessage?.Invoke("Стратегія тільки на Windows");
                return false;
            }

            return true;
        }

        public bool CanRunRPG(Device device)
        {
            return true;
        }

        public bool CanRunAdventure(Device device)
        {
            return true;
        }

        public bool CanRun(Game game, Device device)
        {
            if (game is RPGGame)
                return CanRunRPG(device);

            if (game is StrategyGame)
                return CanRunStrategy(device);

            if (game is AdventureGame)
                return CanRunAdventure(device);

            OnMessage?.Invoke("Невідомий тип гри");
            return false;
        }

        public bool CanPlayMultiplayer(Game game, Device device)
        {
            if (game is not RPGGame)
            {
                OnMessage?.Invoke("Мультиплеєр доступний тільки в RPG");
                return false;
            }

            if (device.Controllers == null || device.Controllers.Count < 2)
            {
                OnMessage?.Invoke("Потрібно мінімум 2 контролери");
                return false;
            }

            return true;
        }
    }
}