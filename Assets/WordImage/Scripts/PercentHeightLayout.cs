using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CustomPercentLayout : MonoBehaviour
{
    public BoosterManager bosterManager;

    public RectTransform[] blocks;
    public float[] heightPercentages;
    public float[] heightPercentagesPC;
    private RectTransform parentRect;
    private float lastParentHeight;
    private Canvas canvas;
    //ПК
    public RectTransform contentPanel;
    public float fixedWidthPC = 800f;// Желаемая ширина
    public bool isPc = true;
    CanvasScaler canvasScaler;

    void Start()
    {
        
       
        if (isPc)
        {
            UpdateLayoutPC();
            Debug.Log("UpdateLayoutPC ");
        } else
        {
            UpdateLayout();
            Debug.Log("UpdateLayout ");
        }
        //UpdateLayout();
        //UpdateLayoutPC();

        bosterManager.UpdateBoosterUI();
    }

    void OnRectTransformDimensionsChange()
    {
        if (isPc)
        {
            UpdateLayoutPC();

        }
        else
        {
            UpdateLayout();
        }
        //UpdateLayout();
        //UpdateLayoutPC();
    }

    public float referenceHeight = 1080f;

    void UpdateLayoutPC()
    {
        // Устанавливаем Reference Resolution для CanvasScaler
        parentRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        canvasScaler.referenceResolution = new Vector2(1920, 1080); // Стандартное разрешение для ПК
        //Debug.Log("canvasScaler " + canvasScaler);
        //// Настраиваем CanvasScaler для корректного масштабирования
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 1f; // Масштабируем по ширине



        //// Центрируем parentRect и фиксируем ширину
        parentRect.anchorMin = new Vector2(0.5f, 0f);
        parentRect.anchorMax = new Vector2(0.5f, 1f);
        parentRect.pivot = new Vector2(0.5f, 0.5f);
        parentRect.sizeDelta = new Vector2(fixedWidthPC, 0); // Высота определяется экраном
        parentRect.anchoredPosition = Vector2.zero;

        //// Получаем высоту канваса
        float parentHeight = canvas.GetComponent<RectTransform>().rect.height;

        //// Обновляем блоки
        float currentY = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
            {
                Debug.LogWarning($"Block {i} is null!");
                continue;
            }
            RectTransform blockRect = blocks[i];
            if (i != 3)
            {

                float blockHeight = parentHeight * (heightPercentagesPC[i] / 100f);

                // Настраиваем якоря для блока
                blockRect.anchorMin = new Vector2(0f, 1f); // Якорь вверху родителя
                blockRect.anchorMax = new Vector2(1f, 1f); // Растягиваем по ширине родителя
                blockRect.pivot = new Vector2(0.5f, 1f);

                // Устанавливаем размер и позицию
                blockRect.sizeDelta = new Vector2(0, blockHeight); // Ширина 0, т.к. растягивается по родителю
                blockRect.anchoredPosition = new Vector2(0, -currentY);

                // Принудительно ограничиваем ширину блока
                blockRect.sizeDelta = new Vector2(fixedWidthPC, blockHeight);



                currentY += blockHeight;
            }
            else
            {
                float blockHeight = parentHeight * (heightPercentagesPC[i] / 100f);

                // Настраиваем якоря для блока
                blockRect.anchorMin = new Vector2(0f, 1f); // Якорь вверху родителя
                blockRect.anchorMax = new Vector2(1f, 1f); // Растягиваем по ширине родителя
                blockRect.pivot = new Vector2(0.5f, 1f);

                // Устанавливаем размер и позицию
                blockRect.sizeDelta = new Vector2(0, blockHeight); // Ширина 0, т.к. растягивается по родителю
                blockRect.anchoredPosition = new Vector2(0, -currentY);

                // Принудительно ограничиваем ширину блока
                blockRect.sizeDelta = new Vector2(fixedWidthPC * 1.1f, blockHeight);



                currentY += blockHeight;
            }
            // Отключаем auto-sizing для компонентов, которые могут растягивать блок
            if (blockRect.GetComponent<LayoutElement>() != null)
            {
                blockRect.GetComponent<LayoutElement>().preferredWidth = fixedWidthPC;
            }

            // Логируем информацию
            //Debug.Log($"Block {i} ({blockRect.name}): height = {blockHeight}, " +
            //         $"sizeDelta = {blockRect.sizeDelta}, " +
            //         $"real width = {blockRect.rect.width}, " +
            //         $"position = {blockRect.anchoredPosition}, " +
            //         $"canvas.scaleFactor = ");
            //}
        }
       
    }
    void UpdateLayout()
    {

        // Устанавливаем Reference Resolution для CanvasScaler
        parentRect = GetComponent<RectTransform>();
        if (parentRect == null) return;

        canvas = GetComponentInParent<Canvas>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        canvasScaler.referenceResolution = new Vector2(1080, 1920); // Стандартное разрешение для ПК

        float parentHeight = parentRect.rect.height;
        if (parentHeight <= 0)
        {
            Debug.LogWarning("Parent height is 0!");
            return;
        }

        // Пропускаем, если высота не изменилась
        if (Mathf.Approximately(parentHeight, lastParentHeight))
            return;

        lastParentHeight = parentHeight;
        float currentY = 0;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null) continue;

            float blockHeight = parentHeight * (heightPercentages[i] / 100f);
            RectTransform blockRect = blocks[i];

            blockRect.anchorMin = new Vector2(0, 1);
            blockRect.anchorMax = new Vector2(1, 1);
            blockRect.sizeDelta = new Vector2(0, blockHeight);
            blockRect.anchoredPosition = new Vector2(0, -currentY);

            currentY += blockHeight;

            // Отладка только при изменении
            //Debug.Log($"Block {i} ({blockRect.name}): height = {blockHeight}, position = {blockRect.anchoredPosition}");
        }
    }
}