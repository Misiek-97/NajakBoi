using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Move the character left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Check if the character is on the ground (you may need to adjust the ground check position)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Jump when the spacebar is pressed and the character is on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
            GameManager.PlayerDeath();
        }
    }
}
