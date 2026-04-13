using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSimulation
{
    class Program
    {
        static void Main()
        {
            var rules = new GameRulesService();
            var gameService = new GameService(rules);
            var playerService = new PlayerService();

            gameService.OnMessage += Console.WriteLine;
            playerService.OnMessage += Console.WriteLine;
            rules.OnMessage += Console.WriteLine;

            Console.Write("Введіть ім'я гравця: ");
            string name = Console.ReadLine() ?? "Гравець";

            var player = playerService.RegisterPlayer(name);

            Console.WriteLine("Увійти в акаунт? (y/n)");
            string login = Console.ReadLine() ?? "";

            if (login.ToLower() == "y")
            {
                playerService.Login(player);
            }
            player.Account.OnMessage += Console.WriteLine;

            var pc = new Device("PC", Platform.PC, OperatingSystemType.Windows, new Hardware(8, 16, 6), 100);
            pc.Controllers.Add(new Controller());
            pc.Controllers.Add(new Controller());

            var mobile = new Device("Phone", Platform.Mobile, OperatingSystemType.Android, new Hardware(4, 4, 2), 50);

            pc.OnMessage += Console.WriteLine;
            mobile.OnMessage += Console.WriteLine;

            var devices = new List<Device> { pc, mobile };

            var storeGames = new List<Game>()
            {
                new RPGGame("Skyrim", 20, new Hardware(4, 8, 4)),
                new StrategyGame("Civilization", 30, new Hardware(4, 8, 4)),
                new AdventureGame("Uncharted", 15, new Hardware(2, 4, 2))
            };

            while (true)
            {
                Console.WriteLine("\n===== ЛАУНЧЕР =====");
                Console.WriteLine($"👤 Гравець: {player.Name}");

                foreach (var d in devices)
                    Console.WriteLine($"  💻 {d.Name} [{d.Platform}/{d.OS}] | HDD: {d.FreeHdd} GB вільно");

                Console.WriteLine("1. Мої ігри");
                Console.WriteLine("2. Магазин");
                Console.WriteLine("3. Встановити гру");
                Console.WriteLine("4. Запустити гру");
                Console.WriteLine("5. Зупинити гру");
                Console.WriteLine("6. Зберегти");
                Console.WriteLine("7. Завантажити");
                Console.WriteLine("8. Додати свою гру");
                Console.WriteLine("9. Стрім з мобільного");
                Console.WriteLine("10. Перевірити чи можна запустити гру");
                Console.WriteLine("11. Грати в мультиплеєр");
                Console.WriteLine("0. Вийти");

                Console.Write("\nОбери дію: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": ShowMyGames(player); break;
                    case "2": ShowStore(playerService, storeGames); break;
                    case "3": InstallGame(player, gameService, SelectDevice(devices)); break;
                    case "4": RunGame(player, gameService, SelectDevice(devices)); break;
                    case "5": StopGame(player, gameService); break;
                    case "6": SaveGame(player, gameService); break;
                    case "7": LoadGame(player, gameService); break;
                    case "8": AddCustomGame(playerService); break;
                    case "9": Stream(mobile, pc, player); break;
                    case "10": CheckCanRun(player, rules, SelectDevice(devices)); break;
                    case "11": PlayMultiplayer(player, rules, SelectDevice(devices)); break;
                    case "0": Console.WriteLine("До побачення!"); return;
                    default: Console.WriteLine("❌ Невідома команда"); break;
                }
            }
        }

        static Device SelectDevice(List<Device> devices)
        {
            Console.WriteLine("\n=== ОБЕРІТЬ ДЕВАЙС ===");

            for (int i = 0; i < devices.Count; i++)
                Console.WriteLine($"  {i + 1}. {devices[i].Name} [{devices[i].Platform}/{devices[i].OS}]");

            Console.Write("Вибір: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= devices.Count)
                return devices[idx - 1];

            Console.WriteLine("⚠️ Невірний вибір, використовується перший девайс");
            return devices[0];
        }

        static void Stream(Device mobile, Device pc, Player player)
        {
            if (!player.Account.IsLoggedIn)
            {
                Console.WriteLine("❌ Спочатку увійдіть в акаунт");
                return;
            }

            if (player.CurrentGame == null || !player.CurrentGame.IsRunning)
            {
                Console.WriteLine("❌ Немає запущеної гри для стріму");
                return;
            }

            // 🔥 ГОЛОВНА ПЕРЕВІРКА
            if (player.CurrentDevice != mobile)
            {
                Console.WriteLine("❌ Стрім можливий тільки якщо гра запущена на телефоні");
                return;
            }

            if (mobile.IsStreaming)
            {
                mobile.StopStream();
                return;
            }

            mobile.StartStream(pc, player.CurrentGame);
        }

        static void CheckCanRun(Player player, GameRulesService rules, Device device)
        {
            var game = SelectGame(player);
            if (game == null) return;

            var result = rules.CanRun(game, device);

            Console.WriteLine(result
                ? $"✅ {game.Name} можна запустити на {device.Name}"
                : $"❌ {game.Name} не можна запустити на {device.Name}");
        }

        static void ShowMyGames(Player player)
        {
            Console.WriteLine("\n=== МОЇ ІГРИ ===");

            if (player.Games == null || player.Games.Count == 0)
            {
                Console.WriteLine("Немає ігор у бібліотеці");
                return;
            }

            foreach (var g in player.Games)
            {
                string status = g.IsInstalled ? "[✔]" : "[ ]";
                string running = g.IsRunning ? " 🟢" : "";
                string saves = g.Saves.Count > 0 ? $" 💾{g.Saves.Count}" : "";

                Console.WriteLine($"  - {g.Name} {status}{running}{saves}");
            }
        }

        static void ShowStore(PlayerService playerService, List<Game> storeGames)
        {
            Console.WriteLine("\n=== МАГАЗИН ===");

            for (int i = 0; i < storeGames.Count; i++)
            {
                var g = storeGames[i];
                Console.WriteLine($"  {i + 1}. {g.Name} ({g.Size} GB)");
            }

            Console.Write("Оберіть гру (або 0): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index == 0) return;

            if (index >= 1 && index <= storeGames.Count)
                playerService.AddGameToLibrary(storeGames[index - 1]);
        }

        static void InstallGame(Player player, GameService gameService, Device device)
        {
            var game = SelectGame(player, true);
            if (game == null) return;

            gameService.Install(game, device);
        }

        static void RunGame(Player player, GameService gameService, Device device)
        {
            var game = SelectGame(player);
            if (game == null) return;

            gameService.Run(game, device, player);
        }

        static void StopGame(Player player, GameService gameService)
        {
            if (player.CurrentGame == null)
            {
                Console.WriteLine("❌ Немає запущеної гри");
                return;
            }

            gameService.Stop(player.CurrentGame, player);
        }

        static void SaveGame(Player player, GameService gameService)
        {
            if (player.CurrentGame == null)
            {
                Console.WriteLine("❌ Гра не запущена");
                return;
            }

            gameService.Save(player.CurrentGame);
        }

        static void LoadGame(Player player, GameService gameService)
        {
            if (!player.Account.IsLoggedIn)
            {
                Console.WriteLine("❌ Увійдіть в акаунт");
                return;
            }

            if (player.CurrentGame == null)
            {
                Console.WriteLine("❌ Гра не запущена");
                return;
            }

            var game = player.CurrentGame;

            if (game.Saves.Count == 0)
            {
                Console.WriteLine("❌ Немає збережень");
                return;
            }

            Console.WriteLine("\n=== ЗБЕРЕЖЕННЯ ===");

            for (int i = 0; i < game.Saves.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {game.Saves[i].SavedAt:HH:mm:ss} - {game.Saves[i].Description}");
            }

            Console.Write("Оберіть збереження: ");

            if (!int.TryParse(Console.ReadLine(), out int index) ||
                index < 1 || index > game.Saves.Count)
            {
                Console.WriteLine("❌ Невірний вибір");
                return;
            }

            gameService.Load(game, game.Saves[index - 1]);
        }

        static Game? SelectGame(Player player, bool onlyNotInstalled = false)
        {
            if (player.Games == null || player.Games.Count == 0)
            {
                Console.WriteLine("❌ Немає ігор");
                return null;
            }

            var games = player.Games.ToList();

            if (onlyNotInstalled)
                games = games.Where(g => !g.IsInstalled).ToList();

            if (games.Count == 0)
            {
                Console.WriteLine("❌ Немає доступних ігор");
                return null;
            }

            Console.WriteLine("\n=== СПИСОК ІГОР ===");

            for (int i = 0; i < games.Count; i++)
                Console.WriteLine($"  {i + 1}. {games[i].Name}");

            Console.Write("Вибір: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index == 0)
                return null;

            if (index < 1 || index > games.Count)
                return null;

            return games[index - 1];
        }

        static void AddCustomGame(PlayerService playerService)
        {
            Console.WriteLine("\n=== ДОДАТИ ГРУ ===");

            if (playerService.CurrentPlayer == null)
            {
                Console.WriteLine("❌ Немає користувача");
                return;
            }

            Console.Write("Назва: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Розмір: ");
            int size = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("CPU: ");
            int cpu = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("RAM: ");
            int ram = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("GPU: ");
            int gpu = int.Parse(Console.ReadLine() ?? "0");

            var game = new RPGGame(name, size, new Hardware(cpu, ram, gpu));

            playerService.AddGameToLibrary(game);
        }

        static void PlayMultiplayer(Player player, GameRulesService rules, Device device)
        {
            if (player.CurrentGame == null || !player.CurrentGame.IsRunning)
            {
                Console.WriteLine("❌ Немає гри");
                return;
            }

            if (!rules.CanPlayMultiplayer(player.CurrentGame, device))
                return;

            Console.WriteLine($"🎮 Мультиплеєр у {player.CurrentGame.Name}");
        }
    }
}