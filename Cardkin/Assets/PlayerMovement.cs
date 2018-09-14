using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private CharacterController characterController;
    private Player player;
    // Use this for initialization
    public Vector3 velocity;

    public float gravity = 0.1f;

    public Transform lookDirection;

    public float walkSpeed = 4f;
    public float runSpeed = 6f;

    public float jumpVelocity = 1f;

    public LayerMask whatIsGround;

    public Transform playerModel;

    public float rotationPerSecond = 600f;

    public Transform trackingObject;

    

	void Start () {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 bottomPos = transform.position + characterController.center - (characterController.height / 2 - characterController.radius + characterController.skinWidth*2) * transform.up;
        Debug.DrawLine(transform.position, bottomPos, Color.red);
        bool isGrounded = Physics.CheckSphere(bottomPos, characterController.radius, whatIsGround, QueryTriggerInteraction.Ignore);

        bool jumpButtonPressed = Input.GetButtonDown("Jump");

        if (isGrounded && jumpButtonPressed)
        {
            velocity.y = jumpVelocity;
        } else if (isGrounded && !jumpButtonPressed && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else { velocity.y -= gravity * Time.deltaTime; }
        

        //Player Inputs
        float axisHorizontal = Input.GetAxis("Horizontal");
        float axisVertical = Input.GetAxis("Vertical");

        Vector3 movementVelocity;
        movementVelocity = (lookDirection.right * axisHorizontal + lookDirection.forward * axisVertical).normalized * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementVelocity *= runSpeed;
        }
        else
        {
            movementVelocity *= walkSpeed;
        }


        if (isGrounded)
        {
            velocity.x = movementVelocity.x*player.movementSpeedModifier;
            velocity.z = movementVelocity.z*player.movementSpeedModifier;
        }

        Quaternion desiredRotation;

        // 플레이어 모델이 어디를 바라볼지를 지금부터 결정한다.
        // 플레이어 모델은 즉시 어떤 방향을 바라보지 않고, 보고 싶은 방향(desiredRotation)으로 부드럽게 모델이 돌아갈 것이다.
        // (단 기절 상태일 때는 빼고)


        if (trackingObject != null)
        {
            // Player.trackingObject 가 있을시 그 물체를 바라보고 싶어한다. (어떤 물체를 바라보게하고 싶을 때 사용)
            desiredRotation = Quaternion.LookRotation(trackingObject.position-transform.position, transform.up);
        }
        else if (movementVelocity.magnitude == 0)
        {

            // 바라볼 물체가 없는데 플레이어가 움직이지도 않고 있다면 그냥 가만히 있고 싶어한다.
            desiredRotation = playerModel.rotation;
        }
        else
        {
            // 둘 다 아닐 시 움직이는 방향을 바로보고 싶어한다.
            desiredRotation = Quaternion.LookRotation(movementVelocity, transform.up);
        }

        if (player.isStunned)
        {
            // 플레이어가 기절했으므로 xz 이동을 아예 못하게 만든다. (기존 속도도 없애버림)
            velocity.x = 0;
            velocity.z = 0;
            characterController.Move(velocity);
            
        }
        else
        {
            // 원하는 방향으로 부드럽게 바라보게 만든다.
            playerModel.rotation = Quaternion.RotateTowards(playerModel.rotation, desiredRotation, rotationPerSecond * Time.deltaTime);
            characterController.Move(velocity);
        }
        
        
	}
}
