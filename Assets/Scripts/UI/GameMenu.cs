using Mirror;
using UnityEngine;

namespace UI
{
    public class GameMenu : NetworkBehaviour
    {
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void Disconnect()
        {
            if (isServer)
            {
                NetworkManager.singleton.StopHost();
            }
            else if(isClient)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}