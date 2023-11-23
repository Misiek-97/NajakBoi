using StarterAssets;
using UnityEngine;
using Weapons;

public class PlayerController : MonoBehaviour
{

    public PlayerId playerId;
    public HealthBar healthBar;
    public float maxHealth = 100f;
    public float currentHealth;
    public float moveUsed;

    public ThirdPersonController controller;

    private bool _isDead;


    private void Start()
    {
        healthBar.player = this;
        currentHealth = maxHealth;
        healthBar.UpdateHealth();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            GetDamaged(currentHealth);
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            var p = other.gameObject.GetComponent<Projectile>();
            GetDamaged(p.damage);
            Destroy(p.gameObject);
        }
    }
    public void GetDamaged(float dmg)
    {
        currentHealth -= dmg;

        healthBar.UpdateHealth();

        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            _isDead = true;
            GameManager.Instance.PlayerDeath(playerId);
        }
    }

  


}
