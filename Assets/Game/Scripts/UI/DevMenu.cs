using Game;
using Game.Lobby;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class DevMenu : MonoBehaviour, IMenu
	{
        public static DevMenu Instance => instance;

		public bool IsOpen { get; private set; }

        private static DevMenu instance;

		[Header("Menu")]
        [SerializeField] private GameObject menu;

        [Header("Relay Select")]
        [SerializeField] private SteamLobby steamLobby;

        [Header("Game Info")]
        [SerializeField] private TMP_Text gameInfoTxt;
        [SerializeField] private TMP_InputField nameInput;
        private Dictionary<string, string> infoDict = new Dictionary<string, string>();

        private IMenu openRelayMenu;

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
            nameInput.placeholder.GetComponent<TMP_Text>().text = $"{PlayerPrefs.GetString("name", "New Player")} (change name here)";
		}

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

        /// <summary>
        /// Create or set a GameInfo item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetInfo(string key, string value)
        {
            if (infoDict.ContainsKey(key)) 
            {
                infoDict[key] = value;
            }
            else
            {
                infoDict.Add(key, value);
            }
            string s = "";
            foreach(KeyValuePair<string, string> kvp in infoDict)
            {
                s += $"{kvp.Key} : {kvp.Value}\n";
            }
            gameInfoTxt.text = s;
        }

        public void OnPlayerSetName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;
            Debug.Log("DevMenu:: Setting player name: " +  newName);
            nameInput.ReleaseSelection();
            PlayerPrefs.SetString("name", newName);
			nameInput.placeholder.GetComponent<TMP_Text>().text = $"{PlayerPrefs.GetString("name", "New Player")} (change name here)";
		}
	}
}
