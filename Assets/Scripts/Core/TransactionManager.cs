using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class TransactionManager :MonoBehaviour
    {
        public static TransactionManager Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private TMP_Text transactionHistoryText;

        private List<string> transactions = new List<string>();
        [SerializeField] private InputActionReference showHistoryAction;
        [SerializeField] private GameObject transactionHistoryPanel;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /*private void Start()
        {
            GameManager.Instance.UserInput.inputSystemActions.UI.ShowTransactionHistory.performed += OnShowHistory;
        }*/
        
        private void OnEnable()
        {
            showHistoryAction.action.performed += OnShowHistory;
            showHistoryAction.action.Enable();
        }

        private void OnShowHistory(InputAction.CallbackContext obj)
        {
            transactionHistoryPanel.SetActive(!transactionHistoryPanel.activeInHierarchy);
        }

        private void OnDisable()
        {
            showHistoryAction.action.performed -= OnShowHistory;
            showHistoryAction.action.Disable();
        }

        public void AddTransaction(string description, float amount)
        {
            if (GameManager.Instance == null)
                return;

            string sign = amount >= 0 ? "+" : "-";
            float absoluteAmount = Mathf.Abs(amount);

            string transaction =
                $"Day {TimeManager.Instance.CurrentDay} | " +
                $"{description}  {sign}${absoluteAmount:N0}  " +
                $"Balance: ${GameManager.Instance.Money:N0}";

            transactions.Add(transaction);

            UpdateHistory();
        }

        private void UpdateHistory()
        {
            if (transactionHistoryText == null)
                return;

            transactionHistoryText.text = string.Join("\n\n", transactions);
        }
    }
}