using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class KeyboardPlayerInput : IPlayerInput {
    public bool any { get; private set; }
    public float xAxis { get; private set; }
    public float yAxis { get; private set; }
    public bool PrimaryActionDown { get; private set; }
    public bool SecondaryActionDown { get; private set; }

    public void Poll(float dt) {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        PrimaryActionDown = Input.GetButtonDown("PrimaryAction");
        SecondaryActionDown = Input.GetButton("SecondaryAction");
    }
}
