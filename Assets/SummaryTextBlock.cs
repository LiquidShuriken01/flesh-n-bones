using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SummaryTextBlock : MonoBehaviour
{
    private Dictionary<string, string> info = new Dictionary<string, string>();

    public void AddEntry(string source, string modifiers)
    {
        info[source] = modifiers;
        UpdateText();
    }

    public void RemoveEntry(string source) 
    {
        info.Remove(source);
        UpdateText();
    }

    private void UpdateText()
    {
        string text_block = string.Empty;
        foreach(KeyValuePair<string, string> kvp in info)
        { 
            text_block += kvp.Value;
        }
        gameObject.GetComponent<TMP_Text>().text = text_block;
    }
}
