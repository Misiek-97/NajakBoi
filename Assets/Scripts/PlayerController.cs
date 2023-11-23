using StarterAssets;
using UnityEngine;
using Weapons;

public class PlayerController : MonoBehaviour, IDamageable
{

    public PlayerId playerId;
    public HealthBar healthBar;
    public MovementBar movementBar;
    public float maxHealth = 100f;
    public float currentHealth;
    public float currentMovement;
    public float maxMovement = 100f;

    public ThirdPersonController controller;

    private bool _isDead;


    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.player = this;
        healthBar.UpdateHealth();

        currentMovement = maxMovement;

        movementBar.player = this;
        movementBar.UpdateMovement();
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

    public bool UseMovement(float amount)
    {
        currentMovement -= amount;
        movementBar.UpdateMovement();
        if(currentMovement <= 0f)
        {           
            currentMovement = maxMovement;
            GameManager.Instance.EndTurn();
            return true;
        }
        return false;
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
