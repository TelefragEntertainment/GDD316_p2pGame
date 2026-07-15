using UnityEngine;
using Mirror;
using Steamworks;
using Game.Networking;
using TMPro;
using UnityEngine.UI;
using Mirror.FizzySteam;

namespace Game.Lobby
{
    public class SteamLobby : MonoBehaviour, IMenu
    {
		public ulong LobbyId => lobbyId;                                     // public ID of current lobby
		public bool IsOpen { get; private set; }

		protected Callback<LobbyCreated_t> LobbyCreatedCallback;                  // Called when this client creates a lobby
        protected Callback<GameLobbyJoinRequested_t> JoinLobbyRequestCallback;  // Called when this client requests to join a lobby
        protected Callback<LobbyEnter_t> LobbyEnteredCallback;                    // Called when this client enters a lboby

        private const string HostAddressKey = "HostAddress";

        [SerializeField] private GameObject steamLobbyContainer;            // Contains the Steam lobby UI
		[SerializeField] private FizzySteamworks fizzyTransport;	// Transport layer for steam
        [SerializeField] private GameObject hostLobbyBtn;                   // Button to host lobby
        [SerializeField] private TMP_Text lobbyNameTxt;                     // Text displaying lobby name
        [SerializeField] private TMP_Text steamSatusText;                   
       
        private ulong lobbyId;                                              // ID of current lobby
		private GameNetManager gameNetManager;                      // gameNetManager in our scene

		private void Awake()
		{
			steamLobbyContainer.SetActive(false);
			IsOpen = false;
		}

        /// <summary>
        /// Host a new lobby
        /// </summary>
        public void StartHostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, gameNetManager.maxConnections);
        }

		/// <summary>
		/// Lobby was created by this client and we got this callback.
		/// </summary>
		/// <param name="callback">Callback data</param>
		private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if(callback.m_eResult != EResult.k_EResultOK)   // Handle errors
            {
                Debug.Log($"SteamLobby:: OnLobbyCreated error: {callback.m_eResult}");
                return;
            }

			Debug.Log("SteamLobby:: OnLobbyCreated SUCCESS");
            gameNetManager.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "", SteamUser.GetSteamID().ToString());
			SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", $"{SteamFriends.GetPersonaName().ToString()}'s Lobby");
		}

		/// <summary>
		/// This client attempted to join a lobby and received this callback
		/// </summary>
		/// <param name="callback">Callback data</param>
		private void OnJoinLobbyRequest(GameLobbyJoinRequested_t callback)
        {
			Debug.Log("SteamLobby:: OnJoinLobby request. Joining...");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
		}

		/// <summary>
		/// Any client enters the lobby that this client is in, including this client
		/// </summary>
		/// <param name="callback">Callback data</param>
		private void OnLobbyEntered(LobbyEnter_t callback)
        {
            hostLobbyBtn.SetActive(false);
            lobbyId = callback.m_ulSteamIDLobby;
            lobbyNameTxt.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name"); // Set text to value of 'name' key

            // Only execute on clients, not the host
            if (!NetworkServer.active)
            {
                gameNetManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
                gameNetManager.StartClient();   // Connects client to server
            }
        }

		public void Open()
		{
			IsOpen = true;
			steamLobbyContainer.SetActive(true);
			if (!SteamManager.Initialized)
			{
				steamSatusText.text = "Steam Status:Not Initialized";
				hostLobbyBtn.GetComponent<Button>().interactable = false;
				Debug.LogError("SteamLobby:: SteamManager not initialized!");
				return;
			}
			else
			{
				steamSatusText.text = "Steam Status: Ready!";
				hostLobbyBtn.GetComponent<Button>().interactable = true;
			}

			gameNetManager = FindAnyObjectByType<GameNetManager>();

			//Set transport
			fizzyTransport.enabled = true;
			gameNetManager.transport = fizzyTransport;

			//Initialize callbacks
			LobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
			JoinLobbyRequestCallback = Callback<GameLobbyJoinRequested_t>.Create(OnJoinLobbyRequest);
			LobbyEnteredCallback = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
		}

		public void Close()
		{
			IsOpen = false;
			steamLobbyContainer.SetActive(false);
			hostLobbyBtn.GetComponent<Button>().interactable = false;
		}
	}
}
