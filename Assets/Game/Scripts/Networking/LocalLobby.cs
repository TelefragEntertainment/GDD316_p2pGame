using Game.Networking;
using Mirror;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LocalLobby : MonoBehaviour, IMenu
    {
		public bool IsOpen {  get; private set; }

		[SerializeField] private GameObject localCOntainer;
		[SerializeField] private TMP_Text statusTxt;

		public void StartHost()
		{
			GameNetManager.Instance.OnHostStartedSuccess += HandleHostSuccess;
			statusTxt.text = "Attempting HOST on: " + NetworkManager.singleton.networkAddress + "...";
			NetworkManager.singleton.StartHost();
		}

		private void OnDisable()
		{
			GameNetManager.Instance.OnHostStartedSuccess -= HandleHostSuccess;
		}

		public void Open()
		{
			IsOpen = true;
			localCOntainer.SetActive(true);
			statusTxt.text = "Network Address: " + NetworkManager.singleton.networkAddress;
		}

		public void Close()
		{
			localCOntainer.SetActive(false);
			IsOpen = false;
		}

		private void HandleHostSuccess()
		{
			statusTxt.text = "Hosting on: " + NetworkManager.singleton.networkAddress;
		}
	}
}
