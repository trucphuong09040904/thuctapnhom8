using UnityEngine;
using UnityEngine.UI;
using System;

public class LV8BossBase : MonoBehaviour
{
    public int maxHealth = 5000;
    protected int currentHealth;
    public Slider healthBar;
    public event Action<LV8BossBase> OnBossDefeated;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void SetHealthBar(Slider healthBarUI)
    {
        this.healthBar = healthBarUI;
        this.healthBar.maxValue = maxHealth;
        this.healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnBossDefeated?.Invoke(this); // Gửi sự kiện cho `BossManager`
        Destroy(gameObject);
    }
}
