using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MarbleControl: MonoBehaviour
{
    
    public float walkSpeed;
    public float maxWalk;
    public float turnSpeed;
    public float jumpForce;
    public float probeLen;
    public bool grounded;
    public LayerMask whatIsGround;
    public bool isAlive;

    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Rigidbody rigi;
    private MarbleControls ctrl;
    
    private int keyCount;
    public int minKeys;

    public TextMeshProUGUI displayKeys;
    public TextMeshProUGUI displayTimer;
    private float timer;

    void jump(UnityEngine.InputSystem.InputAction.CallbackContext context){
        if (grounded){
            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void UpdateKeys(){
        displayKeys.text = "Keys: " + keyCount.ToString("00") + " / " + minKeys.ToString("00");
    }

    void UpdateTimer(){
        displayTimer.text = timer.ToString("000.00");
    }
    void Awake() {
        rigi = GetComponent<Rigidbody>();
        ctrl = new MarbleControls();
        ctrl.Enable();
        ctrl.Marble.Jump.started += jump;
        isAlive = true;
        keyCount = 0;
        UpdateKeys();
        timer = 0f;
        UpdateTimer();
    }

    void OnDisable(){
        ctrl.Disable();
    }

    private void FixedUpdate(){

        if (isAlive){
            grounded = Physics.Raycast(this.transform.position, Vector3.down, probeLen, whatIsGround);


            moveInput = ctrl.Marble.Move.ReadValue<Vector2>();
            rotateInput = ctrl.Marble.Turn.ReadValue<Vector2>();

            timer += Time.deltaTime;
            UpdateTimer();

            if(rotateInput.magnitude > 0.1f){
                Vector3 angleVelocity = new Vector3(0f, rotateInput.x * turnSpeed, 0f);
                Quaternion deltaRot = Quaternion.Euler(angleVelocity * Time.deltaTime);
                rigi.MoveRotation(rigi.rotation * deltaRot);
            }

            if(moveInput.magnitude > 0.1f){
                Vector3 moveForward = moveInput.y * this.transform.forward;
                Vector3 moveRight = moveInput.x * this.transform.right;
                Vector3 moveVector = moveForward + moveRight;
                rigi.AddForce(moveVector * walkSpeed * Time.deltaTime);

                rigi.linearVelocity = Vector3.ClampMagnitude(rigi.linearVelocity, maxWalk);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.transform.tag == "Key"){
            keyCount++;
            Destroy(other.gameObject);
            UpdateKeys();
        }
        else if(other.transform.tag == "Finish"){
            Debug.Log("door");
            if(keyCount >= minKeys){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                Debug.Log("you need more keys");
            }
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.tag == "Enemy"){
            isAlive = false;
        }
    }

}
