using Mirror;

namespace UI
{
    public class GameMenu : NetworkBehaviour
    {
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