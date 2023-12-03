using System.Collections;
using TMPro;
using UnityEngine;

public class SpriteTextController : MonoBehaviour
{
    public float activationDistance = 5f; // Adjust as needed
    public TextMeshProUGUI[] textMeshProObjects;

    private bool[] textActivated;
    private int activeTextIndex = -1;

    private void Start()
    {
        InitializeTextActivationStatus();
        DeactivateAllTextObjects();
    }

    private void Update()
    {
        // Check if the player is within the specified distance
        if (IsPlayerWithinDistance())
        {
            // Check for player clicks
            if (Input.GetMouseButtonDown(0))
            {
                // Handle click and activate/deactivate TextMeshPro objects accordingly
                HandleClick();
            }
        }
        else
        {
            // Player is out of range, deactivate all text
            DeactivateAllTextObjects();
        }
    }

    private void InitializeTextActivationStatus()
    {
        // Initialize the boolean array to track text activation status
        textActivated = new bool[textMeshProObjects.Length];
        for (int i = 0; i < textActivated.Length; i++)
        {
            textActivated[i] = false;
        }
    }

    private bool IsPlayerWithinDistance()
    {
        // Replace this with your actual logic to check if the player is within the distance
        // You might use Vector3.Distance, Physics.CheckSphere, or another method depending on your setup
        return true;
    }

    private void HandleClick()
    {
        // Increment the active text index
        activeTextIndex++;

        // Check if the index is within the bounds of the array
        if (activeTextIndex < textMeshProObjects.Length && !textActivated[activeTextIndex])
        {
            // Activate the next TextMeshPro object
            ActivateTextObject(activeTextIndex);
            textActivated[activeTextIndex] = true;
        }
        else
        {
            // Player clicked after the last text or on a previously activated text, deactivate all text
            DeactivateAllTextObjects();
            activeTextIndex = -1; // Reset active text index
        }
    }

    private void ActivateTextObject(int index)
    {
        // Deactivate all other TextMeshPro objects
        DeactivateAllTextObjects();

        // Activate the specified TextMeshPro object
        if (index >= 0 && index < textMeshProObjects.Length)
        {
            textMeshProObjects[index].gameObject.SetActive(true);
        }
    }

    private void DeactivateAllTextObjects()
    {
        // Deactivate all TextMeshPro objects
        foreach (var textMeshProObject in textMeshProObjects)
        {
            textMeshProObject.gameObject.SetActive(false);
        }
    }
}