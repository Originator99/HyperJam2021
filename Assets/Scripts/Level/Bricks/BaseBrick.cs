using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class BaseBrick : MonoBehaviour {
    public string ID {
        get {
            return brickData != null ? brickData.gridNodeID : "";
        }
    }

    public BrickType currentType;
    
    [SerializeField]
    protected SpriteRenderer gfx, selectedState, pathState;
    [SerializeField]
    protected GameObject destroyEffect;
    [SerializeField]
    protected BrickData brickData;
    [SerializeField]
    protected Animator animator;

    protected SignalBus _signalBus;
    public void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    private void OnEnable() {
        if(animator == null) {
            animator = GetComponent<Animator>();
        }
    }

    public void Initialize(BrickData data) {
        if(data != null) {
            brickData = data;
            ToggleSelectState(false);

            if(data.type == BrickType.PATH) {
                SwitchToPath();
            } else {
                SwitchToOriginalState();
            }
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
            Debug.LogError("Cannot initialize Brick, data is null");
        }
    }

    public void ResetBrick() {
        Initialize(brickData);
    }

    public void SwitchToOriginalState() {
        currentType = brickData != null ? brickData.type : BrickType.NORMAL;

        if(gfx != null) {
            gfx.enabled = true;
        } else {
            Debug.LogWarning("original Sprite renderer is null for brick : " + transform.name);
        }
        if(pathState != null) {
            pathState.enabled = false;
        } else {
            Debug.LogWarning("path sprite renderer is null for brick : " + transform.name);
        }

        OnSwitchedToOriginal();
    }

    public void SwitchToPath() {
        currentType = BrickType.PATH;
        if(gfx != null) {
            gfx.enabled = false;
        } else {
            Debug.LogWarning("Original Sprite renderer is null for brick : " + transform.name);
        }
        if(pathState != null) {
            pathState.enabled = true;
        } else {
            Debug.LogWarning("path sprite renderer is null for brick : " + transform.name);
        }

        OnSwitchedToPath();
    }

    public void ToggleSelectState(bool state) {
        if(selectedState != null) {
            if(!selectedState.gameObject.activeSelf) {
                selectedState.gameObject.SetActive(true);
            }
            if(animator != null) {
                animator.SetBool("Selected", state);
            }
            selectedState.enabled = state;
        } else {
            Debug.LogWarning("Selected state GO not found, reference added?");
        }
    }

    public void SwitchPositions(Vector2 new_position) {
        transform.position = new_position;
    }

    public void DoShuffleHint() {
        if(animator != null) {
            animator.SetTrigger("ShuffleHint");
        }
    }

    public virtual void DestroyBrick() {
        if(destroyEffect != null) {
            Instantiate(destroyEffect, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        }
        SwitchToPath();
    }

    public virtual void OnSwitchedToPath() {
    
    }
    public virtual void OnSwitchedToOriginal() {
        
    }
}
