using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TypeWriter : MonoBehaviour
{
    public static TypeWriter instance;

    public AudioClip audioClip;
    public float Speed = 15f;

    [HideInInspector] public bool isWriting;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Run(string textToType, Text textLabel, UnityAction endEvent = null)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(textToType, textLabel, endEvent));
        isWriting = true;
    }

    IEnumerator TypeText(string textToType, Text textLabel, UnityAction endEvent)
    {
        float t = 0f;
        int charIndex = 0;

        int audioChannel = -1;
        if (AudioManager.instance != null)
            audioChannel = AudioManager.instance.PlayLoop(audioClip, 0.5f, 0f);

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * Speed;
            charIndex = Mathf.Clamp(Mathf.FloorToInt(t), 0, textToType.Length);
            textLabel.text = ProcessText(textToType.Substring(0, charIndex));
            yield return null;
        }

        textLabel.text = ProcessText(textToType);
        endEvent?.Invoke();

        isWriting = false;
        if (AudioManager.instance != null)
            AudioManager.instance.Stop(audioChannel);
    }

    // Wraps text between @ markers in yellow rich-text color tags.
    private string ProcessText(string inputText)
    {
        string result = "";
        bool inHighlight = false;
        int startIndex = 0;

        for (int i = 0; i < inputText.Length; i++)
        {
            if (inputText[i] != '@')
                continue;

            if (inHighlight)
            {
                result += "<color=yellow>" + inputText.Substring(startIndex, i - startIndex) + "</color>";
                inHighlight = false;
            }
            else
            {
                result += inputText.Substring(startIndex, i - startIndex);
                inHighlight = true;
            }

            startIndex = i + 1;
        }

        result += inHighlight
            ? "<color=yellow>" + inputText.Substring(startIndex) + "</color>"
            : inputText.Substring(startIndex);

        return result;
    }
}
