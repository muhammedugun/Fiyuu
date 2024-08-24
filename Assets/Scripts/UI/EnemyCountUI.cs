using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level içindeki düþman sayýsýný UI'da göstermekten sorumludur
/// </summary>
public class EnemyCountUI : MonoBehaviour
{
    [SerializeField] private Text _enemyCountText;

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
        _enemyCountText.text = _enemyCountManager._enemyCount.ToString();
    }
}
