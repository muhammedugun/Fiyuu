// Refactor 23.08.24
using UnityEngine;

/// <summary>
/// Seviyedeki d��man say�s�yla ilgili i�lemleri y�netmekten sorumlu
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
    /// Seviyedeki d��man say�s�n� hesapla
    /// </summary>
    private void CalculateEnemyCount()
    {
        _enemyCount = FindObjectsOfType<Enemy>().Length;
        EventBus.Publish(EventType.EnemyCountUpdated);
    }

    /// <summary>
    /// Seviyedeki mevcut d��man say�s�n� g�ncelle
    /// </summary>
    private void UpdateEnemyCount()
    {
        _enemyCount--;
        if (_enemyCount <= 0)
            EventBus.Publish(EventType.AllEnemiesDead);
        EventBus.Publish(EventType.EnemyCountUpdated);
    }
}
