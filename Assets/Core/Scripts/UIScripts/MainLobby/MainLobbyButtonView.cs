using UnityEngine;
using UnityEngine.UI;

namespace PrSuperSoldier
{
    public class MainLobbyButtonView : MonoBehaviour
    {
        private Button _thisButton;

        private void Awake()
        {
            _thisButton = GetComponent<Button>();
        }
    }
}
