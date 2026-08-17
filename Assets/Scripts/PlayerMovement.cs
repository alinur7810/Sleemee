using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour{
    [Header("Movement Parameters")]
    [SerializeField] private float slimeSpeed = 8f;
    [SerializeField] private float jumpPower = 12f;

    [Header("GRound Check")]
    [SerializeField] private Transform groundCP; 
    [SerializeField] private float groundCR = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horInput;
    private bool isGround;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        horInput = 0f;
        if (Keyboard.current != null){
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed){
                horInput = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed){
                horInput = 1f;
            }

            if ( (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && isGround ){
                Jump();
            }
        }
    }

    private void FixedUpdate(){
        isGround = Physics2D.OverlapCircle(groundCP.position, groundCR, groundLayer);
        rb.linearVelocity = new Vector2(horInput * slimeSpeed, rb.linearVelocity.y);
    }
    private void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }
    private void OnDrawGizmosSelected(){
        if (groundCP != null){
            Gizmos.color=Color.red;
            Gizmos.DrawWireSphere(groundCP.position, groundCR);
        }
    }
}
