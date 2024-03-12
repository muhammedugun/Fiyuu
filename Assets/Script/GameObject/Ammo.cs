// Refactor 12.03.24
using TMPro;
using UnityEngine;

/// <summary>
/// Mühimmat ile ilgili
/// </summary>
public class Ammo : ExplosiveBase
{
    [Header("Ammo")]
    [Tooltip("Mühimmatın maddesi")]
    [SerializeField] internal AmmoMatter matter;
    [Tooltip("Mühimmat düştükten sonra ne kadar yakınındaki düşmanları korkutsun?")]
    [SerializeField] private float scareRadius;
    [SerializeField] private TextMeshProUGUI launchPowerText;
    [Tooltip("Trail Renderer'in görülmesini sağlayacak materyal")]
    [SerializeField] private Material visibleMaterial;

    internal Vector3 launchPos;
    internal float[] power = new float[8]{ 1f, 2f, 2f, 2f, 1f, 1f, 1f, 1f};
    internal bool isDestroyable;

    private TrailRenderer _trailRenderer;
    private void Awake()
    {
        _massMultiplier = power[(int)matter - 1];
    }
    protected override void Start()
    {
        base.Start();
        _trailRenderer = GetComponent<TrailRenderer>();
        launchPowerText = GameObject.Find("/UI/Canvas/LaunchPowerText").GetComponent<TextMeshProUGUI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ScareEnemies(scareRadius);
        DoDamage(collision);
        if (CheckSmash())
        {
            _trailRenderer.material = visibleMaterial;
            ShowLaunchPowerText();
            Smash(collision);
            Explode(collision);
            isExplode = true;
            if (isDestroyable)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Fırlatma gücünü fırlatılan noktada text olarak gösterir
    /// </summary>
    private void ShowLaunchPowerText()
    {
        // Dünya pozisyonunu ekran pozisyonuna çevir.
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, launchPos);
        // Ekran pozisyonunu RectTransform'ın yerel pozisyonuna çevir.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(launchPowerText.rectTransform.parent.GetComponent<RectTransform>(), screenPoint, null, out Vector2 localPoint);
        // Yeni pozisyonu ayarla.
        launchPowerText.rectTransform.anchoredPosition = localPoint;
    }

    /// <summary>
    /// Düşmanları korkut. Mühimmatın bulunduğu konumun çevresindeki düşmanların korkma animasyonlarını çalıştırır.
    /// </summary>
    private void ScareEnemies(float radius)
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.TryGetComponent<Enemy>(out var enemy);
            if (enemy != null)
                enemy.animator.SetTrigger("terrified");
        }
    }

}

/// <summary>
/// Mühimmatın maddesi
/// </summary>
public enum AmmoMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel,
    Fire,
    Ice,
    Explosion,
    Electric
}