using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseBrick : MonoBehaviour {
    public string ID {
        get {
            return brickData != null ? brickData.gridNodeID : "";
        }
    }

    public BrickType currentType;
    
    [SerializeField]
    protected SpriteRenderer gfx, selectedState;
    [SerializeField]
    protected GameObject destroyEffect;
    [SerializeField]
    protected BrickData brickData;

    protected SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus) {
        _signalBus = signalBus;
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
            Debug.LogWarning("Sprite renderer is null for brick : " + transform.name);
        }
    }

    public void SwitchToPath() {
        currentType = BrickType.PATH;
        if(gfx != null) {
            gfx.enabled = false;
        } else {
            Debug.LogWarning("Sprite renderer is null for brick : " + transform.name);
        }
    }

    public void ToggleSelectState(bool state) {
        if(selectedState != null) {
            if(!selectedState.gameObject.activeSelf) {
                selectedState.gameObject.SetActive(true);
            }
            selectedState.enabled = state;
        } else {
            Debug.LogWarning("Selected state GO not found, reference added?");
        }
    }

    public void SwitchPositions(Vector2 new_position) {
        transform.position = new_position;
    }

    public virtual void DestroyBrick() {
        if(destroyEffect != null) {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        SwitchToPath();
    }
}
