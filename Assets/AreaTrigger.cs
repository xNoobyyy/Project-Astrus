using UnityEngine;
using System;

public class TriggerEvent : MonoBehaviour
{
    public static event Action<string> OnAreaEntered; // Event, das ausgelöst wird
    private bool hasEntered = false; // Sicherstellen, dass es nur einmal passiert
    public string areaIdentifier;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasEntered && other.CompareTag("Player")) // Nur beim ersten Mal und nur für den Spieler
        {
            hasEntered = true;
            Debug.Log("Spieler hat den Bereich zum ersten Mal betreten!" + areaIdentifier);
            OnAreaEntered?.Invoke(areaIdentifier); // Event auslösen
        }
    }
}

