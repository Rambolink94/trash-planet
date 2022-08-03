using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class LogEntry : MonoBehaviour
{
    [SerializeField] Image logImage;
    [SerializeField] TextMeshProUGUI logText;

    private float logAge;

    public void SetLogData(Sprite sprite, string text, float age, Action<LogEntry> removeCallback)
    {
        logImage.sprite = sprite;
        logText.SetText(text);
        logAge = age;

        gameObject.SetActive(true);
        StartCoroutine(WaitForExpiration(removeCallback));
    }

    IEnumerator WaitForExpiration(Action<LogEntry> removeCallback)
    {
        yield return new WaitForSeconds(logAge);

        removeCallback(this);
    }
}
