using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{
   
    [SerializeField] private Text enemyCountText;

    private EnemyCountManager _enemyCountManager;

    private void Start()
    {
        _enemyCountManager = FindObjectOfType<EnemyCountManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.EnemyCountUpdated, UpdateEnemyCountText);

    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.EnemyDied, UpdateEnemyCountText);

    }

    private void UpdateEnemyCountText()
    {
        enemyCountText.text = _enemyCountManager._enemyCount.ToString();
    }
}
