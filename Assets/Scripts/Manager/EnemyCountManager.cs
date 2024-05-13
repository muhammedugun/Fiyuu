using UnityEngine;


public class EnemyCountManager : MonoBehaviour
{
    
    [HideInInspector] public int _enemyCount;

    private void Start()
    {
        CalculateEnemyCount();
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
        EventBus.Publish(EventType.EnemyCountUpdated);
    }

    private void UpdateEnemyCount()
    {
        _enemyCount--;
        if (_enemyCount <= 0)
            EventBus.Publish(EventType.AllEnemiesDead);
        EventBus.Publish(EventType.EnemyCountUpdated);
    }

    
}
