using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using Logic.Events;
using Player.Inventory;
using Player.Inventory.Slots;
using TextDisplay;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.WhiteFlash;

public class PlayerHealth : MonoBehaviour {
    public static PlayerHealth Instance;

    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Transform respawnPointStrand;
    public Transform respawnPointPlateau;
    private Rigidbody2D rb;
    public bool plateau;
    private Coroutine regenCoroutine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        currentHealth = maxHealth;
        if (healthSlider == null) return;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void OnEnable() {
        EventManager.Instance.Subscribe<PlayerDamageEvent>(HandlePlayerDamage);
        EventManager.Instance.Subscribe<PlayerAreaEnterEvent>(HandlePlayerAreaEnter);
    }

    private void OnDisable() {
        EventManager.Instance.Unsubscribe<PlayerDamageEvent>(HandlePlayerDamage);
        EventManager.Instance.Unsubscribe<PlayerAreaEnterEvent>(HandlePlayerAreaEnter);
    }

    private void HandlePlayerDamage(PlayerDamageEvent e) {
        if (AreaManager.Instance.LastOrCurrentArea.type == AreaType.Starter) return;

        var knockbackDir = (Vector2)(transform.position - e.Source.position).normalized;
        rb.AddForce(knockbackDir * 5f, ForceMode2D.Impulse);
        GetComponent<SpriteFlashEffect>().StartWhiteFlash();
        TakeDamage(e.Damage);
    }

    private void HandlePlayerAreaEnter(PlayerAreaEnterEvent e) {
        if (e.Area.type == AreaType.Plateau && !plateau) plateau = true;
    }

    public void TakeDamage(int damage) {
        var armor = (ArmorItem)PlayerInventory.Instance.Slots.FirstOrDefault(slot => slot is ArmorSlot)?.Item;
        damage /= (armor?.Lvl ?? 1);

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
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null) {
            healthSlider.value = currentHealth;
        }
    }

    private void Respawn() {
        var diedAt = transform.position;

        transform.position = plateau ? respawnPointPlateau.position : respawnPointStrand.position;

        currentHealth = maxHealth;
        if (healthSlider != null) {
            healthSlider.value = currentHealth;
        }

        EventManager.Instance.Trigger(new PlayerDeathEvent());
        EventManager.Instance.Trigger(new PlayerMoveEvent(diedAt, transform.position, transform));
    }

    private IEnumerator StartRegeneration() {
        yield return new WaitForSeconds(20);
        while (currentHealth < maxHealth) {
            Heal(1);
            yield return new WaitForSeconds(1f);
        }

        regenCoroutine = null;
    }
}