using System;
using System.Collections.Generic;

namespace GameSimulation
{
    public class Device
    {
        public string Name { get; }
        public Platform Platform { get; }
        public OperatingSystemType OS { get; }
        public Hardware Hardware { get; }
        public int FreeHdd { get; set; }

        public List<Controller> Controllers { get; } = new();

        public event Action<string>? OnMessage;

        public bool IsStreaming { get; private set; }
        public Device? StreamingTarget { get; private set; }
        public Game? StreamingGame { get; private set; }

        public Device(
            string name,
            Platform platform,
            OperatingSystemType os,
            Hardware hardware,
            int freeHdd)
        {
            Name = name;
            Platform = platform;
            OS = os;
            Hardware = hardware;
            FreeHdd = freeHdd;
        }

        public void StartStream(Device target, Game? currentGame)
        {
            if (Platform != Platform.Mobile)
            {
                Send("Стрім доступний тільки з мобільного пристрою");
                return;
            }

            if (IsStreaming)
            {
                Send("Стрім вже активний");
                return;
            }

            if (currentGame == null || !currentGame.IsRunning)
            {
                Send("Немає запущеної гри для стріму");
                return;
            }

            IsStreaming = true;
            StreamingTarget = target;
            StreamingGame = currentGame;

            Send($"Трансляція {currentGame.Name} з {Name} → {target.Name}");
        }

        public void StopStream()
        {
            if (!IsStreaming)
            {
                Send("Стрім не активний");
                return;
            }

            Send($"📴 Стрім {StreamingGame?.Name} зупинено");

            IsStreaming = false;
            StreamingTarget = null;
            StreamingGame = null;
        }

        private void Send(string message)
        {
            OnMessage?.Invoke(message);
        }
    }
}