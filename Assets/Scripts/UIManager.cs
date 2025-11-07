using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Slider healthBar;
    public TextMeshProUGUI timerText;

    private float timeElapsed = 0f;
    private bool timerRunning = true;

    void Start()
    {
        if (healthBar != null)
            healthBar.value = 1f; // tam dolu başlasın
    }

    void Update()
    {
        if (timerRunning)
        {
            timeElapsed += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void UpdateHealth(float value)
    {
        if (healthBar != null)
            healthBar.value = Mathf.Clamp01(value);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}
