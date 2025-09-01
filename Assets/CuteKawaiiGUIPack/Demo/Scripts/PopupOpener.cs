// Copyright (C) 2024 ricimi. All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using UnityEngine;

namespace Ricimi
{
    // This class is responsible for creating and opening a popupHint of the
    // given prefab and adding it to the UI canvas of the current scene.
    public class PopupOpener : MonoBehaviour
    {
        public GameObject popupPrefab;
        public GameObject bg;
        public Canvas canvas;

        protected Canvas m_canvas;
        protected GameObject m_popup;

        protected void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();
            if(m_canvas == null)
            {
                m_canvas = canvas;
            }
        }

        private GameObject m_popupPrefab;
        public virtual void OpenPopup()
        {
            m_popup = Instantiate(popupPrefab, m_canvas.transform, false);
            m_popup.SetActive(true);
            m_popup.GetComponent<Popup>().Open();
        }

        public virtual void ShowPopup(Popup popup)
        {
            if(bg != null)
            {
                bg.gameObject.SetActive(true);
            }
            
            popup.gameObject.SetActive(true);
            //m_popupPrefab = popup.AddBackground1();
            //popup.GetComponent<Popup>().Open();
        }

        public virtual void ShowPopup()
        {
            if (bg != null)
            {
                bg.gameObject.SetActive(true);
            }

            popupPrefab.gameObject.SetActive(true);
            //m_popupPrefab = popup.AddBackground1();
            //popup.GetComponent<Popup>().Open();
        }
        public virtual void HidePopup(Popup popup)
        {
            if(bg != null){

                bg.gameObject.SetActive(false);

            }
            
            popup.gameObject.SetActive(false);
            //popup.RemoveBackground(m_popupPrefab);
            //popup.RemoveBackground();
            //popup.GetComponent<Popup>().Open();
        }

        
    }
}
