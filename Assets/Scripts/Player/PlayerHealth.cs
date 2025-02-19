using System;
using System.Collections;
using System.Collections.Generic;
using Logic.Events;
using UnityEngine;
using UnityEngine.UI;
using Utils.WhiteFlash;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 100; 
    public int currentHealth; 
    public Slider healthSlider; 
    public Transform respawnPointStrand; 
    public Transform respawnPointPlateau; 
    private Rigidbody2D rb;
    bool plateau = false;
    private Coroutine regenCoroutine;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        currentHealth = maxHealth;
        if (healthSlider != null) {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void OnEnable() {
        EventManager.Instance.Subscribe<PlayerDamageEvent>(HandlePlayerDamage);
    }

    private void OnDisable() {
        EventManager.Instance.Unsubscribe<PlayerDamageEvent>(HandlePlayerDamage);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            TakeDamage(20); 
        }
    }

    private void HandlePlayerDamage(PlayerDamageEvent e) {
        var knockbackDir = (Vector2)(transform.position - e.Source.position).normalized;
        rb.AddForce(knockbackDir * 5f, ForceMode2D.Impulse);
        GetComponent<SpriteFlashEffect>().StartWhiteFlash();
        TakeDamage(e.Damage);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthSlider != null) {
            healthSlider.value = currentHealth;
        }
        if (regenCoroutine != null) {
            StopCoroutine(regenCoroutine);
        }
        regenCoroutine = StartCoroutine(StartRegeneration());
        if (currentHealth <= 0) {
            Respawn();
        }
    }

    public void Heal(int amount) {
        // Heilung anwenden
        currentHealth += amount;
        
        // Gesundheit darf nicht über maxHealth steigen
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Lebensbalken aktualisieren
        if (healthSlider != null) {
            healthSlider.value = currentHealth;
        }
    }

    private void Respawn() {
        if (plateau) {
            transform.position = respawnPointPlateau.position;
        } else{
            transform.position = respawnPointStrand.position;
        }
        currentHealth = maxHealth;
        if (healthSlider != null) {
            healthSlider.value = currentHealth;
        }
    }
    private IEnumerator StartRegeneration()
    {
        yield return new WaitForSeconds(20);
        while (currentHealth < maxHealth)
        {
            currentHealth++;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healthSlider.value = currentHealth;
            yield return new WaitForSeconds(1f); 
        }
        regenCoroutine = null; 
    }
}