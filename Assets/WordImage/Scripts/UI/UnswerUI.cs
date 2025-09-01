using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UnswerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI letterText;
    public int index = 0;
    public Image bgImage;
    public Color originalColor;

    // Параметры тряски
    [SerializeField] private float shakeDuration = 0.5f; // Длительность тряски
    [SerializeField] private float shakeMagnitude = 10f; // Сила тряски
    private Vector2 originalAnchoredPosition; // Исходная позиция в anchoredPosition
    private RectTransform rectTransform;

    public static Action<int> OnKeyPressed;
    public TextMeshProUGUI LetterText
    {
        get { return letterText; }
        set { letterText = value; }
    }

    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(OnClick);
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        //Debug.Log(originalAnchoredPosition + " originalAnchoredPosition");

        // Ждём один кадр, чтобы HorizontalLayoutGroup успел рассчитать позиции
        StartCoroutine(InitializePosition());
    }

    private void OnClick()
    {
        OnKeyPressed?.Invoke(index);
    }

    private IEnumerator InitializePosition()
    {
        // Ждём завершения первого кадра
        yield return new WaitForEndOfFrame();
        // Сохраняем anchoredPosition после того, как макет применился
        originalAnchoredPosition = rectTransform.anchoredPosition;
        //Debug.Log($"{originalAnchoredPosition} originalAnchoredPosition");
    }

    // Метод для вызова тряски (можно вызывать в нужный момент)
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            bgImage.color = Color.red;
            // Генерируем случайное смещение
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            // Применяем смещение к anchoredPosition
            rectTransform.anchoredPosition = originalAnchoredPosition + new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null; // Ждём следующий кадр
        }

        // Возвращаем элемент в исходную anchoredPosition
        rectTransform.anchoredPosition = originalAnchoredPosition;
        bgImage.color = originalColor;
        GameManager.Instance.uiManager.RemoveUnswerUiByIndex(index);
    }
}
