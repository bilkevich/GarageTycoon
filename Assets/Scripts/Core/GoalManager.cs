using Cooperative;
using TMPro;
using UnityEngine;

namespace Core
{
    public class GoalManager : MonoBehaviour
    {
        public static GoalManager Instance { get; private set; }

        [Header("Goal")]
        [SerializeField] private float targetMoney = 50000f;

        [Header("UI")]
        [SerializeField] private TMP_Text goalText;
        
        [SerializeField] private CooperativeMapObject cooperativeMapObject;
        private bool isCompleted;

        public bool IsCompleted => isCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            UpdateGoal();
        }

        private void Update()
        {
            if (isCompleted)
                return;

            if (GameManager.Instance == null)
                return;

            UpdateGoal();
        }

        private void UpdateGoal()
        {
            float currentMoney = GameManager.Instance.Money;

            if (currentMoney >= targetMoney)
            {
                CompleteGoal();
                return;
            }

            goalText.text =
                $"GOAL\n" +
                $"Reach ${targetMoney:N0}\n" +
                $"Money: ${currentMoney:N0} / ${targetMoney:N0}";
        }

        private void CompleteGoal()
        {
            isCompleted = true;

            goalText.text =
                $"GOAL COMPLETED\n" +
                $"You reached ${targetMoney:N0}!";

            Debug.Log($"Goal completed: Reach ${targetMoney:N0}");
            cooperativeMapObject.UnlockCooperative();
        }
    }
}