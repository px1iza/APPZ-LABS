using System;

namespace GameSimulation
{
    public class GameAccount
    {
        public bool IsLoggedIn { get; private set; }

        public event Action<string>? OnMessage;

        public void Login()
        {
            if (IsLoggedIn)
            {
                OnMessage?.Invoke("Користувач вже увійшов в акаунт");
                return;
            }

            IsLoggedIn = true;
            OnMessage?.Invoke("Вхід виконано успішно");
        }

        public void Logout()
        {
            if (!IsLoggedIn)
            {
                OnMessage?.Invoke("Користувач вже вийшов з акаунта");
                return;
            }

            IsLoggedIn = false;
            OnMessage?.Invoke("Вихід виконано успішно");
        }
    }
}