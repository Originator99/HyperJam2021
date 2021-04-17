using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour {
    public string IDOnGrid {
        get {
            if(data != null)
                return data.gridNodeID;
            return "";
        }
    }

    public Vector2 WorldPosition {
        get {
            if(data != null)
                return data.worldPosition;
            return Vector2.zero;
        }
    }

    public BrickType currentType;

    public BrickData data;

    private SpriteRenderer spriteRender;
    private new BoxCollider2D collider;

    private void Start() {
        InitializeBrick(data);
    }

    public void InitializeBrick(BrickData data) {
        if(data != null) {
            this.data = data;

            transform.position = data.worldPosition;
            spriteRender = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();

            ChangeBrickState(data.type);

            gameObject.SetActive(true);

            Invoke(nameof(AnimateBrick), Random.Range(0f, 1f));
        } else {
            gameObject.SetActive(false);
        }
    }

    public void ChangePosition(Vector3 newPostion) {
        data.worldPosition = newPostion;
        transform.position = data.worldPosition;
    }

    public void DestroyBrick() {
        if(data.renderData != null && data.renderData.destroyEffect != null) {
            Instantiate(data.renderData.destroyEffect, transform.position, Quaternion.identity);
        }
        ChangeBrickState(BrickType.PATH);
    }

    public void ChangeBrickState(BrickType type) {
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
            case BrickType.END:
                SwitchToEndBrick();
                break;
            case BrickType.SAFE_PATH:
                SwitchToSafePath();
                break;
        }
    }

    private void SwitchToBomb() {
        SwitchToNormal();
        //spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0.15f); // to remove later
    }

    private void SwitchToPath() {
        
        if(spriteRender != null) {
            spriteRender.enabled = false;
        } else {
            Debug.LogWarning("Sprite renderer is null for brick : " + transform.name);
        }

        if(collider != null) {
            collider.isTrigger = true;
        } else {
            Debug.LogWarning("Collider is null for brick : " + transform.name);
        }
    }

    private void SwitchToNormal() {
        if(data.renderData != null) {
            
            if(spriteRender != null) {
                spriteRender.enabled = true;
                spriteRender.sprite = data.renderData.brickSprite;
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 1f); // to remove later

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

    private void SwitchToEndBrick() {
        SwitchToNormal();
    }

    private void SwitchToSafePath() {
        spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0.5f);
    }

    private void AnimateBrick() {
        Vector2 ogScale = transform.localScale;
        transform.DOScale(new Vector2(ogScale.x + 0.25f, ogScale.y + 0.25f), 0.5f).OnComplete(delegate () {
            transform.DOScale(ogScale, 0.15f);
        });
    }
}


public enum BrickType {
    NORMAL,
    BOMB,
    PATH,
    SAFE_PATH,
    END
}

[System.Serializable]
public class BrickData {
    public string gridNodeID;
    public BrickGraphicData renderData;
    public BrickType type;
    public Vector2 worldPosition;
}

[System.Serializable]
public class BrickGraphicData {
    public Sprite brickSprite;
    public GameObject destroyEffect;
}
