using System;

public abstract class Tutorial :IDisposable {
    public enum Type {
        DASHBOARD,
        CHAPTER_1
    }
    public virtual void Dispose() {
    }

    public virtual void Start() {
        // optionally overridden
    }
}
