using System.Collections;
using System.Collections.Generic;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimation : MonoBehaviour
{
    public GameObject coinPrefab; // Префаб монеты
    public RectTransform coinIcon; // UI-иконка монеты
    public int coinCount = 10; // Количество монет для спавна
    public float animationDuration = 1f; // Длительность анимации одной монеты
    public float spawnDelay = 0.1f; // Задержка между спавном монет
    public Canvas canvas;

    public PopupOpener popupOpener;

    // Вызывается при победе
    public void OnVictory()
    {
        popupOpener.OpenPopup();
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        // Центр Canvas в anchoredPosition (относительно Canvas)
        Vector2 center = Vector2.zero; // (0, 0) — центр Canvas в локальных координатах

        for (int i = 0; i < coinCount; i++)
        {
            // Создаем монету как дочерний элемент Canvas
            GameObject coin = Instantiate(coinPrefab, canvas.transform);
            RectTransform coinRect = coin.GetComponent<RectTransform>();
            coinRect.anchoredPosition = center; // Устанавливаем в центр Canvas
            StartCoroutine(MoveCoinToIcon(coinRect));
            yield return new WaitForSeconds(spawnDelay); // Задержка между спавном
        }
    }

    private IEnumerator MoveCoinToIcon(RectTransform coin)
    {
        // Получаем позицию иконки в мировых координатах
        Vector3 targetPos = coinIcon.position;
        Vector3 startPos = coin.transform.position;
        float elapsedTime = 0f;

        // Анимация движения монеты к иконке
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            // Используем плавную интерполяцию (можно заменить на другую кривую, например, EaseInOut)
            coin.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Устанавливаем финальную позицию и уничтожаем монету
        coin.transform.position = targetPos;


        // Получаем компоненты
        RemoveUIComponents coinRemoveComponent = coin.GetComponent<RemoveUIComponents>();
        coinRemoveComponent.RemoveUIElement();

        GameManager.Instance.data.Coins += 2;
    }
}
