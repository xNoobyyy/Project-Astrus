using UnityEngine;
using UnityEngine.UI;
 
 public class PlayerHealth : MonoBehaviour
 {
     public int maxHealth = 100; // Maximale Gesundheit
     public int currentHealth;  // Aktuelle Gesundheit
     public Slider healthSlider; // Referenz zum Lebensbalken
     public Transform respawnPoint; // Checkpoint-Position
 
     void Start()
     {
         // Spieler startet mit voller Gesundheit
         currentHealth = maxHealth;
 
         // Slider initialisieren
         if (healthSlider != null)
         {
             healthSlider.maxValue = maxHealth;
             healthSlider.value = currentHealth;
         }
     }
     void Update()
     {
         // Schaden bei Tastendruck ("L")
         if (Input.GetKeyDown(KeyCode.L))
         {
             TakeDamage(20); // Spieler nimmt 20 Schaden
         }
     }
     public void TakeDamage(int damage)
     {
         // Schaden anwenden
         currentHealth -= damage;
 
         // Gesundheit darf nicht unter 0 fallen
         currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
 
         // Lebensbalken aktualisieren
         if (healthSlider != null)
         {
             healthSlider.value = currentHealth;
         }
 
         // Wenn Gesundheit 0 erreicht, Spieler zurücksetzen
         if (currentHealth <= 0)
         {
             Respawn();
         }
     }
 
     void Respawn()
     {
         // Spieler an den Checkpoint zurücksetzen
         if (respawnPoint != null)
         {
             transform.position = respawnPoint.position;
         }
 
         // Gesundheit wiederherstellen
         currentHealth = maxHealth;
 
         // Lebensbalken aktualisieren
         if (healthSlider != null)
         {
             healthSlider.value = currentHealth;
         }
     }
 }
 