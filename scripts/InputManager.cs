using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Player_inputs playerInput;
    public Player_inputs.OnFootActions onFoot;

    private PlayerMotors motor;
    private PlayerLook look;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new Player_inputs();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotors>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.processLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
