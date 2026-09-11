using UnityEngine;

namespace Core
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [Header("Time Settings")]
        [SerializeField] private float secondsPerDay = 60f;

        [SerializeField] private int daysPerMonth = 30;

        public int CurrentDay { get; private set; } = 1;
        public int CurrentMonth { get; private set; } = 1;

        // Total game time in seconds
        public float GameTime { get; private set; }

        private float dayTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            GameTime += deltaTime;
            dayTimer += deltaTime;

            if (dayTimer >= secondsPerDay)
            {
                dayTimer -= secondsPerDay;

                NextDay();
            }
        }

        private void NextDay()
        {
            CurrentDay++;

            Debug.Log($"New day started: Day {CurrentDay}, Month {CurrentMonth}");

            if (CurrentDay > daysPerMonth)
            {
                NextMonth();
            }
        }

        private void NextMonth()
        {
            CurrentDay = 1;
            CurrentMonth++;

            Debug.Log($"New month started: Month {CurrentMonth}");

            if (GarageManager.Instance != null)
            {
                GarageManager.Instance.ProcessMonthlyIncome();
            }
        }

        public void AdvanceDay()
        {
            GameTime += secondsPerDay;

            NextDay();
        }

        public float GetSecondsPerDay()
        {
            return secondsPerDay;
        }
    }
}