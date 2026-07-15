using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class GameInfo : MonoBehaviour
    {
        public string Key => key;

        [SerializeField] private TMP_Text label;
        [SerializeField] private TMP_Text text;

        private string key;
        private string value;

        public void SetInfo(string newKey, string newValue)
        {
            key = newKey;
            value = newValue;
            label.text = key;
            text.text = value;
        }
    }
}
