using Game.Networking;
using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Lobby manager for Localhost games (testing)
	/// </summary>
    public class LocalLobby : MonoBehaviour, IMenu
    {
		public bool IsOpen {  get; private set; }

		[SerializeField] private GameObject localContainer;
		[SerializeField] private TMP_Text statusTxt;

		public void StartHost()
		{
			GameNetManager.Instance.OnHostStartedSuccess += HandleHostSuccess;
			GameNetManager.Instance.OnAddPlayer += HandleAddPlayer;
			statusTxt.text = "Attempting HOST on: " + NetworkManager.singleton.networkAddress + "...";
			NetworkManager.singleton.StartHost();
		}

		public void StartClient()
		{
			GameNetManager.Instance.OnClientConnectSuccess += HandleClientConnect;
			GameNetManager.Instance.OnAddPlayer += HandleAddPlayer;
			statusTxt.text = "Attempting CONNECT on: " + NetworkManager.singleton.networkAddress + "...";
			NetworkManager.singleton.StartClient();
		}

		private void OnDisable()
		{
			GameNetManager.Instance.OnHostStartedSuccess -= HandleHostSuccess;
			GameNetManager.Instance.OnAddPlayer -= HandleAddPlayer;
		}

		public void Open()
		{
			IsOpen = true;
			localContainer.SetActive(true);
			statusTxt.text = "Network Address: " + NetworkManager.singleton.networkAddress;
		}

		public void Close()
		{
			localContainer.SetActive(false);
			IsOpen = false;
		}

		private void HandleHostSuccess()
		{
			statusTxt.text = "Hosting on: " + NetworkManager.singleton.networkAddress;
		}

		private void HandleClientConnect()
		{
			statusTxt.text = "Connection on: " + NetworkManager.singleton.networkAddress;
		}

		private void HandleAddPlayer(NetworkConnectionToClient newConn)
		{
			string clients = "Clients\n";
			foreach (KeyValuePair<int, NetworkConnectionToClient> kvp in NetworkServer.connections)
			{
				int connectionId = kvp.Key;
				NetworkConnectionToClient conn = kvp.Value;
				string ipAddress = conn.address;

				clients += $" - ID: {connectionId} | IP: {ipAddress} | Ping: {conn.rtt}\n";
			}
			Debug.Log(newConn.identity);
			DevMenu.Instance.SetInfo("Connections", clients);
		}
	}
}
