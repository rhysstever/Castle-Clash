using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInputActions playerControls;
	private InputAction move;

	public Rigidbody2D rb;
	private Vector2 moveDirection;
	private float moveSpeed;

	void Awake()
	{
		playerControls = new PlayerInputActions();
		rb = GetComponent<Rigidbody2D>();
		moveSpeed = GetComponent<Unit>().MoveSpeed();
	}

	// Start is called before the first frame update
	void Start()
	{
		moveDirection = Vector2.zero;
	}

	private void OnEnable()
	{
		move = playerControls.Player.Move;
		move.Enable();
	}

	private void OnDisable()
	{
		move.Disable();
	}

	// Update is called once per frame
	void Update()
	{
		moveDirection = move.ReadValue<Vector2>();
	}

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
	}
}
