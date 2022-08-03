using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField] GameObject logEntryPrefab;
    [SerializeField] Sprite warningSprite;

    private List<LogEntry> activeLogs = new List<LogEntry>();

    public void AddLog(Item item, float logAge)
    {
        string text = $"Added '{item.name}' to Inventory.";
        AddLog(LogEntryType.Info, text, logAge, item.itemIcon);
    }

    public void AddLog(LogEntryType logEntryType, string text, float logAge = 2f, Sprite sprite = null)
    {
        GameObject logEntryObject = Instantiate(logEntryPrefab, transform);
        LogEntry logEntry = logEntryObject.GetComponent<LogEntry>();

        Sprite logSprite = sprite;
        if (sprite == null)
        {
            switch (logEntryType)
            {
                case LogEntryType.Warning:
                    logSprite = warningSprite;
                    break;
                case LogEntryType.Info:
                    break;
                default:
                    break;
            }
        }

        activeLogs.Add(logEntry);
        logEntry.SetLogData(logSprite, text, logAge, RemoveLog);
    }

    void RemoveLog(LogEntry logEntry)
    {
        // TODO: Add Fade animations with DOTween
        activeLogs.Remove(logEntry);
        Destroy(logEntry.gameObject);
    }
}

public enum LogEntryType
{
    Warning,
    Info,
}
