using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehavior : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _faceSprite;
    
    public void SetFace(string faceSpriteName)
    {
        _faceSprite.sprite = AttackPatterns.attackPatterns[faceSpriteName];
    }
}
