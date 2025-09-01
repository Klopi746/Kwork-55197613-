using TMPro;
using UnityEngine;
using YG;

public class TimerADSR : MonoBehaviour
{
    public float timeInSeconds = 300f; // Время в секундах
    private float remainingTime;

    public TextMeshProUGUI timerText; // UI текст для отображения времени
    public bool flagStartTimer = false;
    void Start()
    {
        remainingTime = timeInSeconds; // Инициализация оставшегося времени
        
    }

    void Update()
    {
        if (!flagStartTimer || YG2.saves.NOTads) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Ум réduction du temps restant
            if(timerText != null)
            {
                UpdateTimerText();
            }
            
        }
        else
        {
            remainingTime = 0; // Установка времени на 0, если прошло
            // Вы можете добавить логику, когда время вышло, например:
            TimerEnded();
        }

        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    void UpdateTimerText()
    {
        // Рассчитываем минуты и секунды
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool TimerEnded()
    {
        EndTimer();
        return false;
    }

    public void StartTimer()
    {
        flagStartTimer = true;
    }

    public void EndTimer()
    {
        flagStartTimer = false;
    }
}
