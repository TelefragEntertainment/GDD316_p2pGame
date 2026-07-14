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

        private static GameNetManager instance;

		public override void Awake()
		{
			base.Awake();
            instance = this;
		}

		public override void OnStartHost()
		{
            base.OnStartHost();
            OnHostStartedSuccess?.Invoke();
		}
    }
}