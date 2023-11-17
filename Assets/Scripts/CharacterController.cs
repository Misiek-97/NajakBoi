using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool _isGrounded;
    public HealthBar healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    private bool _isDead;


    private void Start()
    {
        healthBar.character = this;
        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isDead) return;
        
        ControlCharacter();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamaged(20);
        }
    }

    private void ControlCharacter()
    {
        // Move the character left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the character is on the ground (you may need to adjust the ground check position)
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Jump when the spacebar is pressed and the character is on the ground
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            _isDead = true;
            GameManager.Instance.PlayerDeath();
        }
    }
    public void GetDamaged(int dmg)
    {
        currentHealth -= dmg;
        

        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            _isDead = true;
            GameManager.Instance.PlayerDeath();
        }
    }

  


}
