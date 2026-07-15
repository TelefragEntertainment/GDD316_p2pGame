using UnityEngine;
using Mirror;
using System;

namespace Game.Networking
{
    /// <summary>
    /// Derrived class of Mirror.NetworkManager
    /// </summary>
    public class GameNetManager : NetworkManager
    {
        public static GameNetManager Instance => instance;

        public Action OnHostStartedSuccess;
        public Action OnClientConnectSuccess;
		public Action<NetworkConnectionToClient> OnAddPlayer;

        private static GameNetManager instance;

		public override void Awake()
		{
			base.Awake();
            instance = this;
		}

		public override void Start()
		{
			base.Start();
			DevMenu.Instance.SetInfo("Network Status", "Offline");
		}

		public override void OnStartHost()
		{
			Debug.Log("GNM:: OnStartHost");
			base.OnStartHost();
            OnHostStartedSuccess?.Invoke();
		}

		public override void OnClientConnect()
		{
			Debug.Log("GNM:: OnClientConnect");
			base.OnClientConnect();
            OnClientConnectSuccess?.Invoke();
		}

		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			base.OnServerAddPlayer(conn);
			Debug.Log("GNM:: OnServerAddPlayer");
			OnAddPlayer?.Invoke(conn);
		}
    }
}