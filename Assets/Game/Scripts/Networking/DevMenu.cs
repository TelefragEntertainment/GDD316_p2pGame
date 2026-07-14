using Game;
using Game.Lobby;
using UnityEngine;

namespace Game
{
    public class DevMenu : MonoBehaviour, IMenu
	{
		public bool IsOpen { get; private set; }

		[Header("Menu")]
        [SerializeField] private GameObject menu;

        [Header("Relay Select")]
        [SerializeField] private SteamLobby steamLobby;

        private IMenu openRelayMenu;

		public void Open()
		{
            menu.SetActive(!menu.activeSelf);
		}

        public void Close() { } // Not used

		public void OpenRelayMenu(GameObject menuObject)
        {
            if(menuObject.TryGetComponent<IMenu>(out IMenu menu))
            {
                if (openRelayMenu != null)
                {
                    openRelayMenu.Close(); 
                    openRelayMenu = null;
				}

                if (!menu.IsOpen)
                {
                    menu.Open();
                    openRelayMenu = menu;
                }
                else
                {
                    menu.Close();
					openRelayMenu = null;
				}
			}
            else
            {
                Debug.LogError("DevMenu:: Open menu menuObject does not impliment IMenu");
            }
        }
    }
}
