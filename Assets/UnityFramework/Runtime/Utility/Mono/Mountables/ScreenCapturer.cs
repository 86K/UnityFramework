using System;
using System.IO;
using UnityEngine;

namespace UnityFramework.Runtime
{
    [AddComponentMenu("itsxwz/Tool/Screen Capturer")]
    public class ScreenCapturer : MonoBehaviour
    {
        [Tooltip("The keycode that trigger sceen capture.")]
        [SerializeField] KeyCode m_KeyCode = KeyCode.C;
        string m_FolderPath;
        string m_FilePath;

        void Awake()
        {
            m_FolderPath = Application.dataPath + "../ScreenCapture";
            if (!Directory.Exists(m_FolderPath))
            {
                Directory.CreateDirectory(m_FolderPath);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(m_KeyCode))
            {
                m_FilePath = Path.Combine(m_FolderPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png").Replace("\\", "/");
                if (File.Exists(m_FilePath))
                {
                    File.Delete(m_FilePath);
                }

                ScreenCapture.CaptureScreenshot(m_FilePath, ScreenCapture.StereoScreenCaptureMode.BothEyes);
            }
        }
    }
}