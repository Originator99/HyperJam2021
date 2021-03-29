using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RadarPing : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOFade(0, 2.5f).OnComplete(delegate () {
            Destroy(gameObject);
        });
    }
}
