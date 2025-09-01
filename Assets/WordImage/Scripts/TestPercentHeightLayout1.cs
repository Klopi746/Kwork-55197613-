using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.AudioSettings;
using YG;

public class TestCustomPercentLayout : MonoBehaviour
{
    public BoosterManager bosterManager;

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
        if (YG2.envir.device == YG2.Device.Mobile || Application.isMobilePlatform)
        {
            //uiPc.SetActive(true);
            
            UpdateLayout();
        }
        else
        {
            //uiPc.SetActive(false);
            UpdateLayoutPC();
        }

        //if (isPc)
        //{
        //    UpdateLayoutPC();
        //    Debug.Log("UpdateLayoutPC ");
        //} else
        //{
        //    UpdateLayout();
        //    Debug.Log("UpdateLayout ");
        //}
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

        //canvasScaler.referenceResolution = new Vector2(1920, 1080); // ����������� ���������� ��� ��
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

      
    }
}