using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public BrickType currentType;

    private BrickData data;

    private SpriteRenderer spriteRender;
    private new BoxCollider2D collider;

    public void InitializeBrick(BrickData data) {
        if(data != null) {
            this.data = data;

            transform.position = data.worldPosition;
            spriteRender = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();

            ChangeBrickState(data.type);

            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    public void DestroyBrick() {
        if(data.renderData != null && data.renderData.destroyEffect != null) {
            Instantiate(data.renderData.destroyEffect, transform.position, Quaternion.identity);
        }
        ChangeBrickState(BrickType.PATH);
    }

    private void ChangeBrickState(BrickType type) {
        currentType = type;

        switch(type) {
            case BrickType.BOMB:
                SwitchToBomb();
                break;
            case BrickType.NORMAL:
                SwitchToNormal();
                break;
            case BrickType.PATH:
                SwitchToPath();
                break;
        }
    }

    private void SwitchToBomb() {
        spriteRender.enabled = false;
    }

    private void SwitchToPath() {
        
        if(spriteRender != null) {
            spriteRender.enabled = false;
        } else {
            Debug.LogWarning("Sprite renderer is null for brick : " + transform.name);
        }

        if(collider != null) {
            collider.enabled = false;
        } else {
            Debug.LogWarning("Collider is null for brick : " + transform.name);
        }
    }

    private void SwitchToNormal() {
        if(data.renderData != null) {
            
            if(spriteRender != null) {
                spriteRender.enabled = true;
                spriteRender.sprite = data.renderData.brickSprite;
            } else {
                Debug.LogError("Cannot render brick, SpriteRender is null. Is component added?");
            }

            if(collider != null) {
                collider.enabled = true;
            } else {
                Debug.LogError("Collider is null for brick : " + transform.name + " Collisions will not happen");
            }

        } else {
            Debug.LogError("Cannot render to switch to normal state as the render data is null.. switching to path..");
            ChangeBrickState(BrickType.PATH);
        }
    }
}


public enum BrickType {
    NORMAL,
    BOMB,
    PATH
}

[System.Serializable]
public class BrickData {
    public BrickGraphicData renderData;
    public BrickType type;
    public Vector2 worldPosition;
}

[System.Serializable]
public class BrickGraphicData {
    public Sprite brickSprite;
    public GameObject destroyEffect;
}
