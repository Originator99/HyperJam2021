using System;


public abstract class CameraState : IDisposable {
    public virtual void Dispose() {
    }
    public abstract void LateUpdate();
    public abstract void UpdateState(System.Object data);

    public virtual void Start() {
        // optionally overridden
    }

    public enum State {
        Following,
        Zoom,
        Static
    }
}
