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

    [Header("Wall Check")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] Vector2 wallJumpPower = new Vector2(10f, 12f);
    [SerializeField] private Transform wallCP;
    [SerializeField] private float wallCR = 0.2f;
    [SerializeField] private float wallStickCD = 0.30f;
    [SerializeField] private float wallJumpLD = 0.2f;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private float horInput;
    private bool isGround;

    private bool isTouchWall;
    private bool isWallSlide;
    private bool isFacingRight = true;
    private bool isWallJump;
    private bool canWallSlide = true;

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
            if (!isWallJump)
            {
                if (horInput > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (horInput < 0 && isFacingRight)
                {
                    Flip();
                }
            }
            
            if ( (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)){
                if (isGround){
                    Jump();
                }
                else if (isTouchWall || isWallSlide)
                {
                    WallJump();
                }
                
            }
        }
    }

    private void FixedUpdate(){
        isGround = Physics2D.OverlapCircle(groundCP.position, groundCR, groundLayer);
        isTouchWall = Physics2D.OverlapCircle(wallCP.position, wallCR, groundLayer);
        if (isTouchWall && !isGround && horInput != 0 && canWallSlide)
        {
            isWallSlide = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));

        }
        else
        {
            isWallSlide = false;
        }

        if (!isWallJump)
        {
            rb.linearVelocity = new Vector2(horInput * slimeSpeed, rb.linearVelocity.y);
        }
        
    }
    private void Jump(){
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }
    private void WallJump()
    {
        isWallJump = true;
        canWallSlide = false;

        float jumpDirect = isFacingRight ? -1f : 1f;
        rb.linearVelocity = new Vector2(jumpDirect * wallJumpPower.x, wallJumpPower.y);

        Flip();
        Invoke(nameof(StopWallJump), wallJumpLD);
        Invoke(nameof(ReenableWallSlide), wallStickCD);
    }

    private void StopWallJump()
    {
        isWallJump = false;
    }
    private void ReenableWallSlide()
    {
        canWallSlide = true;
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private void OnDrawGizmosSelected(){
        if (groundCP != null){
            Gizmos.color=Color.red;
            Gizmos.DrawWireSphere(groundCP.position, groundCR);
        }

        if (wallCP != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCP.position, wallCR);
        }

    }
}
