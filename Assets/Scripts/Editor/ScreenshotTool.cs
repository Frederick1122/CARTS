
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Core
{
    public class ScreenshotTool : EditorWindow
    {
        [MenuItem("Tools/Screenshot Tool")]
        public static void ShowWindow()
        {
            GetWindow<ScreenshotTool>("Screenshot Tool");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Take Screenshot"))
            {
                TakeScreenshot();
            }
        }
        
        private void TakeScreenshot()
        {
            string directoryPath = Path.Combine(Application.dataPath, "Screenshots");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            string filePath = Path.Combine(directoryPath, fileName);

            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log("Screenshot saved to: " + filePath);
        }
    }
}

