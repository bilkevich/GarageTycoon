using Garage;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private float startingMoney = 20000f;

    public float Money { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Money = startingMoney;
    }

    public bool TrySpendMoney(float amount)
    {
        if (amount <= 0)
            return false;

        if (Money < amount)
            return false;

        Money -= amount;
        return true;
    }

    public void AddMoney(float amount)
    {
        if (amount <= 0)
            return;

        Money += amount;
    }
}
