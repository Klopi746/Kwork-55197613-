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
    //��
    public RectTransform contentPanel;
    public float fixedWidthPC = 800f;// �������� ������
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
        // ������������� Reference Resolution ��� CanvasScaler
        parentRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        canvasScaler.referenceResolution = new Vector2(1920, 1080); // ����������� ���������� ��� ��
        //Debug.Log("canvasScaler " + canvasScaler);
        //// ����������� CanvasScaler ��� ����������� ���������������
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 1f; // ������������ �� ������



        //// ���������� parentRect � ��������� ������
        parentRect.anchorMin = new Vector2(0.5f, 0f);
        parentRect.anchorMax = new Vector2(0.5f, 1f);
        parentRect.pivot = new Vector2(0.5f, 0.5f);
        parentRect.sizeDelta = new Vector2(fixedWidthPC, 0); // ������ ������������ �������
        parentRect.anchoredPosition = Vector2.zero;

        //// �������� ������ �������
        float parentHeight = canvas.GetComponent<RectTransform>().rect.height;

        //// ��������� �����
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

                // ����������� ����� ��� �����
                blockRect.anchorMin = new Vector2(0f, 1f); // ����� ������ ��������
                blockRect.anchorMax = new Vector2(1f, 1f); // ����������� �� ������ ��������
                blockRect.pivot = new Vector2(0.5f, 1f);

                // ������������� ������ � �������
                blockRect.sizeDelta = new Vector2(0, blockHeight); // ������ 0, �.�. ������������� �� ��������
                blockRect.anchoredPosition = new Vector2(0, -currentY);

                // ������������� ������������ ������ �����
                blockRect.sizeDelta = new Vector2(fixedWidthPC, blockHeight);



                currentY += blockHeight;
            }
            else
            {
                float blockHeight = parentHeight * (heightPercentagesPC[i] / 100f);

                // ����������� ����� ��� �����
                blockRect.anchorMin = new Vector2(0f, 1f); // ����� ������ ��������
                blockRect.anchorMax = new Vector2(1f, 1f); // ����������� �� ������ ��������
                blockRect.pivot = new Vector2(0.5f, 1f);

                // ������������� ������ � �������
                blockRect.sizeDelta = new Vector2(0, blockHeight); // ������ 0, �.�. ������������� �� ��������
                blockRect.anchoredPosition = new Vector2(0, -currentY);

                // ������������� ������������ ������ �����
                blockRect.sizeDelta = new Vector2(fixedWidthPC * 1.1f, blockHeight);



                currentY += blockHeight;
            }
            // ��������� auto-sizing ��� �����������, ������� ����� ����������� ����
            if (blockRect.GetComponent<LayoutElement>() != null)
            {
                blockRect.GetComponent<LayoutElement>().preferredWidth = fixedWidthPC;
            }

            // �������� ����������
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

        // ������������� Reference Resolution ��� CanvasScaler
        parentRect = GetComponent<RectTransform>();
        if (parentRect == null) return;

        canvas = GetComponentInParent<Canvas>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        canvasScaler.referenceResolution = new Vector2(1080, 1920); // ����������� ���������� ��� ��

        float parentHeight = parentRect.rect.height;
        if (parentHeight <= 0)
        {
            Debug.LogWarning("Parent height is 0!");
            return;
        }

        // ����������, ���� ������ �� ����������
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

            // ������� ������ ��� ���������
            //Debug.Log($"Block {i} ({blockRect.name}): height = {blockHeight}, position = {blockRect.anchoredPosition}");
        }
    }
}