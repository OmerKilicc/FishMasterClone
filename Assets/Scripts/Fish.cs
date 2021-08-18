using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEditor;
using Random = System.Random;

public class Fish : MonoBehaviour
{
    [Serializable]
    public class FishType
    {
        public int price;
        
        public float fishCount;

        public float minLenght = 30;

        public float maxLenght = 50;
        
        public float colliderRadius;
        
        public Sprite sprite;
    }
    
    CircleCollider2D coll;
    
    SpriteRenderer _renderer;
    
    float sceenLeft;
    
    Tweener _tweener;

    public Fish.FishType type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            coll.radius = type.colliderRadius;
            _renderer.sprite = type.sprite;
        }
    }

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        sceenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        ResetFish();
    }

    private void Update()
    {
        
        
    }


    void ResetFish()
    {
        if (_tweener != null)
        {
            _tweener.Kill(false);
        }

        float num = UnityEngine.Random.Range(type.minLenght, type.maxLenght);
        coll.enabled = true;
        
        //Where our fish will be
        Vector3 position = transform.position;
        position.y = num;
        position.x = sceenLeft;
        transform.position = position;

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 Vect = new Vector2(-position.x,y);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        //infiinty loop
        _tweener = transform.DOMove(Vect, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay)
            .OnStepComplete(
                delegate
                {
                    Vector3 localScale = transform.localScale;
                    localScale.x = -localScale.x;
                    transform.localScale = localScale;
                });
    }

    void Hooked()
    {
        coll.enabled = false;
        _tweener.Kill(false);
    }

    
    
}
