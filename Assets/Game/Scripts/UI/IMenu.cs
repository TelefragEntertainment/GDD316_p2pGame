using UnityEngine;

namespace Game
{
    public interface IMenu
    {
        public bool IsOpen { get; } 
        public void Open();
        public void Close();
    }
}
