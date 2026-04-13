using System;

namespace GameSimulation
{
    public class GameService
    {
        public Game? CurrentRunningGame { get; private set; }

        private readonly GameRulesService _rules;

        public event Action<string>? OnMessage;

        public GameService(GameRulesService rules)
        {
            _rules = rules;
        }

        public void Install(Game game, Device device)
        {
            if (game.IsInstalled)
            {
                OnMessage?.Invoke("⚠️ Гра вже встановлена");
                return;
            }

            if (!_rules.CanRun(game, device))
            {
                OnMessage?.Invoke("Цю гру не можна встановити на цей пристрій");
                return;
            }

            if (device.FreeHdd < game.Size)
            {
                OnMessage?.Invoke($"Недостатньо місця на HDD (потрібно {game.Size} GB, доступно {device.FreeHdd} GB)");
                return;
            }

            device.FreeHdd -= game.Size;
            game.IsInstalled = true;

            OnMessage?.Invoke($"{game.Name} встановлено (залишилось {device.FreeHdd} GB)");
        }

        public void Run(Game game, Device device, Player player)
        {
            if (!game.IsInstalled)
            {
                OnMessage?.Invoke("Гра не встановлена");
                return;
            }

            if (!player.Account.IsLoggedIn)
            {
                OnMessage?.Invoke("Увійдіть в акаунт");
                return;
            }

            if (CurrentRunningGame != null)
            {
                OnMessage?.Invoke($"Вже запущена гра '{CurrentRunningGame.Name}'. Спочатку зупиніть її.");
                return;
            }

            if (!device.Hardware.Meets(game.Requirements))
            {
                OnMessage?.Invoke(
                    $"Апаратне забезпечення не відповідає вимогам гри.\n" +
                    $"   Потрібно: CPU={game.Requirements.Cpu}, RAM={game.Requirements.Ram}, GPU={game.Requirements.Gpu}\n" +
                    $"   Є: CPU={device.Hardware.Cpu}, RAM={device.Hardware.Ram}, GPU={device.Hardware.Gpu}");
                return;
            }

            if (!_rules.CanRun(game, device))
                return;

            game.IsRunning = true;
            player.CurrentGame = game;
            CurrentRunningGame = game;
            player.CurrentDevice = device;

            OnMessage?.Invoke($"🚀 {game.Name} запущено");
        }

        public void Stop(Game game, Player player)
        {
            if (!game.IsRunning)
            {
                OnMessage?.Invoke("Гра не запущена");
                return;
            }

            game.IsRunning = false;
            player.CurrentGame = null;
            CurrentRunningGame = null;
            player.CurrentDevice = null;

            OnMessage?.Invoke($"{game.Name} зупинено");
        }

        public void Save(Game game)
        {
            if (!game.IsRunning)
            {
                OnMessage?.Invoke("Немає запущеної гри");
                return;
            }

            game.Saves.Add(new Save());
            OnMessage?.Invoke($"💾 Збережено ({game.Saves.Count} збережень всього)");
        }

        public void Load(Game game, Save save)
        {
            if (!game.IsRunning)
            {
                OnMessage?.Invoke("Гра не запущена");
                return;
            }

            OnMessage?.Invoke($"📂 Завантажено збереження від {save.SavedAt:HH:mm:ss} ({save.Description})");
        }
    }
}