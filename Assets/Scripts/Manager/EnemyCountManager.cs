// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Seviyedeki düþman sayýsýyla ilgili iþlemleri yönetmekten sorumlu
/// </summary>
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

    /// <summary>
    /// Seviyedeki düþman sayýsýný hesapla
    /// </summary>
    private void CalculateEnemyCount()
    {
        _enemyCount = FindObjectsOfType<Enemy>().Length;
        EventBus.Publish(EventType.EnemyCountUpdated);
    }

    /// <summary>
    /// Seviyedeki mevcut düþman sayýsýný güncelle
    /// </summary>
    private void UpdateEnemyCount()
    {
        _enemyCount--;
        if (_enemyCount <= 0)
            EventBus.Publish(EventType.AllEnemiesDead);
        EventBus.Publish(EventType.EnemyCountUpdated);
    }
}
