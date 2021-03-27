using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public abstract class CameraState : IDisposable {
    public virtual void Dispose() {
    }
    public abstract void LateUpdate();

    public virtual void Start() {
        // optionally overridden
    }

    public enum State {
        Following,
        Static
    }
}
