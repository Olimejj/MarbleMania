using UnityEngine;

public class MarbleControl: MonoBehaviour
{
    
    public float walkSpeed;
    public float maxWalk;
    public float turnSpeed;
    public float jumpForce;
    public float probeLen;
    public bool grounded;
    public LayerMask whatIsGround;

    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Rigidbody rigi;
    private MarbleControls ctrl;

    void jump(UnityEngine.InputSystem.InputAction.CallbackContext context){
        if (grounded){
            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
    void Awake() {
        rigi = GetComponent<Rigidbody>();
        ctrl = new MarbleControls();
        ctrl.Enable();
        ctrl.Marble.Jump.started += jump;
    }

    void OnDisable(){
        ctrl.Disable();
    }

    private void FixedUpdate(){

        grounded = Physics.Raycast(this.transform.position, Vector3.down, probeLen, whatIsGround);


        moveInput = ctrl.Marble.Move.ReadValue<Vector2>();

        if(moveInput.magnitude > 0.1f){
            Vector3 moveForward = moveInput.y * this.transform.forward;
            Vector3 moveRight = moveInput.x * this.transform.right;
            Vector3 moveVector = moveForward + moveRight;
            rigi.AddForce(moveVector * walkSpeed * Time.deltaTime);

            rigi.linearVelocity = Vector3.ClampMagnitude(rigi.linearVelocity, maxWalk);
        }
    }

}
