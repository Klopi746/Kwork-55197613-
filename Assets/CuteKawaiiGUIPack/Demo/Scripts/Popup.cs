// Copyright (C) 2024 ricimi. All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG.Example;

namespace Ricimi
{
    // This class is responsible for popupHint management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.
    public class Popup : MonoBehaviour
    {
        public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        public float destroyTime = 0.5f;

        private GameObject m_background;


        public void Open()
        {
            AddBackground();
        }

        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());
        }

        public void HideClose()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            RemoveBackground();
            StartCoroutine(RunPopupHide());
        }

        private IEnumerator RunPopupHide()
        {
            yield return new WaitForSeconds(destroyTime);   
            gameObject.SetActive(false);
        }

        // We destroy the popupHint automatically 0.5 seconds after closing it.
        // The destruction is performed asynchronously via a coroutine. If you
        // want to destroy the popupHint at the exact time its closing animation is
        // finished, you can use an animation event instead.
        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(destroyTime);
            Destroy(m_background);
            Destroy(gameObject);
        }

        public void AddBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            // Clone the material, which is the default UI material, to avoid changing it permanently.
            image.material = new Material(image.material);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = GetComponentInParent<Canvas>();
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        public GameObject AddBackground1()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            // Clone the material, which is the default UI material, to avoid changing it permanently.
            image.material = new Material(image.material);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = GetComponentInParent<Canvas>();
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());

            return m_background;
        }

        public void RemoveBackground()
        {
            if(m_background == null) return; 
            var image = m_background.GetComponent<Image>();
            if (image != null)
            {
                image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }

        public void RemoveBackground(GameObject m_background)
        {
            if (m_background == null) return;
            var image = m_background.GetComponent<Image>();
            if (image != null)
            {
                image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }
    }
}
