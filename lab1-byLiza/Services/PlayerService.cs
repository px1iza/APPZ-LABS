using System;

namespace GameSimulation
{
    public class PlayerService
    {
        public Player? CurrentPlayer { get; private set; }

        public event Action<string>? OnMessage;

        public Player RegisterPlayer(string name)
        {
            var player = new Player(name);
            OnMessage?.Invoke($"Гравця {name} створено");
            return player;
        }

        public void Login(Player player)
        {
            player.Account.Login();
            CurrentPlayer = player;

            OnMessage?.Invoke($"Користувач {player.Name} увійшов в акаунт");
        }

        public void Logout()
        {
            if (CurrentPlayer == null)
            {
                OnMessage?.Invoke("Немає активного користувача");
                return;
            }

            CurrentPlayer.Account.Logout();
            OnMessage?.Invoke($"Користувач {CurrentPlayer.Name} вийшов з акаунта");

            CurrentPlayer = null;
        }

        public void AddGameToLibrary(Game game)
        {
            if (CurrentPlayer == null)
            {
                OnMessage?.Invoke("Користувач не авторизований");
                return;
            }

            CurrentPlayer.Games.Add(game);

            OnMessage?.Invoke($"Гру {game.Name} додано до бібліотеки");
        }

        public void ShowGames()
        {
            if (CurrentPlayer == null)
            {
                OnMessage?.Invoke("Користувач не авторизований");
                return;
            }

            OnMessage?.Invoke("🎮 Ігри користувача:");

            foreach (var game in CurrentPlayer.Games)
            {
                string status = "";

                if (game.IsInstalled)
                    status += " [ВСТАНОВЛЕНА]";
                else
                    status += " [НЕ ВСТАНОВЛЕНА]";

                if (game.IsRunning)
                    status += " [ЗАПУЩЕНА]";

                OnMessage?.Invoke($"- {game.Name}{status}");
            }
        }
    }
}