using UnityEngine;
using TMPro;
using System.Collections;

public class DisplayStats : MonoBehaviour
{
    public TextMeshProUGUI framerateText;
    public TextMeshProUGUI memoryText;

    void Start()
    {
        // Make sure the TextMeshProUGUI components are assigned in the Unity Editor
        if (framerateText == null || memoryText == null)
        {
            Debug.LogError("TextMeshProUGUI components not assigned in the inspector!");
            return;
        }

        // Set up a coroutine to update the stats every second
        StartCoroutine(UpdateStats());
    }

    IEnumerator UpdateStats()
    {
        while (true)
        {
            // Update framerate
            float fps = 1f / Time.deltaTime;
            framerateText.text = "Framerate: " + Mathf.Round(fps) + " FPS";

            // Update total memory usage
            long totalMemory = System.GC.GetTotalMemory(false);
            memoryText.text = "Memory Usage: " + BytesToMegabytes(totalMemory).ToString("F2") + " MB";

            // Wait for one second before updating again
            yield return new WaitForSeconds(0.1f);
        }
    }

    float BytesToMegabytes(long bytes)
    {
        return bytes / (1024f * 1024f);
    }
}
