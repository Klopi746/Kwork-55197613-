using System.Collections;
using System.Collections.Generic;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimation : MonoBehaviour
{
    public GameObject coinPrefab; // ������ ������
    public RectTransform coinIcon; // UI-������ ������
    public int coinCount = 10; // ���������� ����� ��� ������
    public float animationDuration = 1f; // ������������ �������� ����� ������
    public float spawnDelay = 0.1f; // �������� ����� ������� �����
    public Canvas canvas;

    public PopupOpener popupOpener;

    // ���������� ��� ������
    public void OnVictory()
    {
        popupOpener.OpenPopup();
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        // ����� Canvas � anchoredPosition (������������ Canvas)
        Vector2 center = Vector2.zero; // (0, 0) � ����� Canvas � ��������� �����������

        for (int i = 0; i < coinCount; i++)
        {
            // ������� ������ ��� �������� ������� Canvas
            GameObject coin = Instantiate(coinPrefab, canvas.transform);
            RectTransform coinRect = coin.GetComponent<RectTransform>();
            coinRect.anchoredPosition = center; // ������������� � ����� Canvas
            StartCoroutine(MoveCoinToIcon(coinRect));
            yield return new WaitForSeconds(spawnDelay); // �������� ����� �������
        }
    }

    private IEnumerator MoveCoinToIcon(RectTransform coin)
    {
        // �������� ������� ������ � ������� �����������
        Vector3 targetPos = coinIcon.position;
        Vector3 startPos = coin.transform.position;
        float elapsedTime = 0f;

        // �������� �������� ������ � ������
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            // ���������� ������� ������������ (����� �������� �� ������ ������, ��������, EaseInOut)
            coin.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // ������������� ��������� ������� � ���������� ������
        coin.transform.position = targetPos;


        // �������� ����������
        RemoveUIComponents coinRemoveComponent = coin.GetComponent<RemoveUIComponents>();
        coinRemoveComponent.RemoveUIElement();

        GameManager.Instance.data.Coins += 2;
    }
}
