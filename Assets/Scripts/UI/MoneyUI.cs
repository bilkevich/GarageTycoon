using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            moneyText.text = $"Money: ${GameManager.Instance.Money:N0}";
        }
    }
}