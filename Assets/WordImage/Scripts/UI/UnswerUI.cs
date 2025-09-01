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

    // ��������� ������
    [SerializeField] private float shakeDuration = 0.5f; // ������������ ������
    [SerializeField] private float shakeMagnitude = 10f; // ���� ������
    private Vector2 originalAnchoredPosition; // �������� ������� � anchoredPosition
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

        // ��� ���� ����, ����� HorizontalLayoutGroup ����� ���������� �������
        StartCoroutine(InitializePosition());
    }

    private void OnClick()
    {
        OnKeyPressed?.Invoke(index);
    }

    private IEnumerator InitializePosition()
    {
        // ��� ���������� ������� �����
        yield return new WaitForEndOfFrame();
        // ��������� anchoredPosition ����� ����, ��� ����� ����������
        originalAnchoredPosition = rectTransform.anchoredPosition;
        //Debug.Log($"{originalAnchoredPosition} originalAnchoredPosition");
    }

    // ����� ��� ������ ������ (����� �������� � ������ ������)
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
            // ���������� ��������� ��������
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            // ��������� �������� � anchoredPosition
            rectTransform.anchoredPosition = originalAnchoredPosition + new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null; // ��� ��������� ����
        }

        // ���������� ������� � �������� anchoredPosition
        rectTransform.anchoredPosition = originalAnchoredPosition;
        bgImage.color = originalColor;
        GameManager.Instance.uiManager.RemoveUnswerUiByIndex(index);
    }
}
