using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState :IDisposable {
    public abstract void Update();

    public virtual void Start() {
        // optionally overridden
    }

    public virtual void FixedUpdate() {
        //optional overridden
    }

    public virtual void Dispose() {
        // optionally overridden
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {
        // optionally overridden
    }
}
