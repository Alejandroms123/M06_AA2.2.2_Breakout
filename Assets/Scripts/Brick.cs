using UnityEngine;
using static PowerUps;

public class Brick : MonoBehaviour
{
    public int _points = 1;
    [SerializeField] private GameObject _powerUpPrefab;
    private SpriteRenderer _sr;
    public BrickGenerator Generator { get; set; }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int row, int col, int totalRows, int totalColumns)
    {
        _points = Mathf.RoundToInt(Mathf.Lerp(totalRows, 1, (float)row / (totalRows - 1)));

        Color baseColor;
        if (row < 2) baseColor = Color.red;
        else if (row < 4) baseColor = Color.yellow;
        else baseColor = Color.green;

        Color.RGBToHSV(baseColor, out float h, out float s, out float v);
        int strength = Mathf.Min(col, totalColumns - 1 - col);
        s -= 0.1f * strength;
        s = Mathf.Clamp01(s);

        if (_sr != null)
            _sr.color = Color.HSVToRGB(h, s, v);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.AddScore(_points);

        if (_powerUpPrefab != null && Random.value <= 0.1f)
        {
            PowerUpType type = (PowerUpType)Random.Range(0, 3);
            GameObject pu = Instantiate(_powerUpPrefab, transform.position, Quaternion.identity);
            PowerUps powerUpScript = pu.GetComponent<PowerUps>();
            if (powerUpScript != null)
                powerUpScript.Initialize(type);
        }

        Generator.NotifyBrickDestroyed(this);

        Destroy(gameObject);
    }
}