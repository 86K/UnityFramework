namespace UnityFramework.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;
    
    public class UIImageListener : MonoBehaviour
    {
        [SerializeField] private Sprite m_EnterSprite, m_DownSprite, m_ExitSprite;
        [Tooltip("所有绑定UISelector的Image的Parent")]
        [SerializeField] private Transform m_GroupParent;
        Image m_Image;
        
        void Awake() 
        {
            m_Image = GetComponent<Image>();
        }

        void Start()
        {
            UIEventListener.Get(gameObject).m_OnPointerEnter = OnPointerEnter;
            UIEventListener.Get(gameObject).m_OnPointerDown = OnPointerDown;
            UIEventListener.Get(gameObject).m_OnPointerExit = OnPointerExit;
        }

        void OnPointerEnter(GameObject _gameObject)
        {
            if (m_EnterSprite != null)
                m_Image.sprite = m_EnterSprite;
        }

        void OnPointerExit(GameObject _gameObject)
        {
            if (m_ExitSprite != null)
                m_Image.sprite = m_ExitSprite;
        }

        void OnPointerDown(GameObject _gameObject)
        {
            if (m_GroupParent != null)
            {
                UIImageListener[] uISelectors = m_GroupParent.GetComponentsInChildren<UIImageListener>();
                for (int i = 0; i < uISelectors.Length; i++)
                {
                    uISelectors[i].OnPointerExit(uISelectors[i].gameObject);
                }
                
                if (m_DownSprite != null)
                {
                    m_Image.sprite = m_DownSprite;
                }
            }
            else
            {
                if (m_Image.sprite == m_ExitSprite)
                {
                    m_Image.sprite = m_DownSprite;
                }
                else if (m_Image.sprite == m_DownSprite)
                {
                    m_Image.sprite = m_ExitSprite;
                }
            }
        }
    }
}