using UnityEngine;
using UnityEngine.UI;

public class EnemyCountManager : MonoBehaviour
{
    [SerializeField] private Text enemyCountText;
    [HideInInspector] public int _enemyCount;

    private void Start()
    {
        CalculateEnemyCount();
        UpdateEnemyCountText();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.EnemyDied, UpdateEnemyCount);
        
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.EnemyDied, UpdateEnemyCount);
       
    }

    private void CalculateEnemyCount()
    {
        _enemyCount = FindObjectsOfType<Enemy>().Length;
    }

    private void UpdateEnemyCount()
    {
        _enemyCount--;
        UpdateEnemyCountText();
        if (_enemyCount <= 0)
            EventBus.Publish(EventType.AllEnemiesDead);
    }

    private void UpdateEnemyCountText()
    {
        enemyCountText.text = _enemyCount.ToString();
    }
}
