using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class Hook : MonoBehaviour
{
    public Transform hookedTransform;

    private Camera mainCamera;
    private CircleCollider2D _circleCollider2D;

    private int length, strength, fishCount;

    private bool canMove = true;
    private Tweener cameraTween;
    
    List<Fish> hookedFishes = new List<Fish>();
    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        mainCamera = Camera.main;
        _circleCollider2D = GetComponent<CircleCollider2D>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;

        }
    }

    public void StartFishing()
    {

        length = IdleManager.instance.length - 20;
        strength = IdleManager.instance.strength;
        fishCount = 0;

        float time = (-length) * 0.1f;

        //Transform camera position to given y value
        cameraTween = mainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false).OnUpdate(delegate 
        {
            // Code for making hook dive while going to given y position with camera
            if (mainCamera.transform.position.y <= -11)
                transform.SetParent(mainCamera.transform);

        }).OnComplete(delegate
        {
            // Code for making hook come back after it reaches its destination meanwhile catching fishes on the road
            _circleCollider2D.enabled = true;
            cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= -25f)
                {
                    StopFishing();
                }
            });
        });

        ScreenManager.instance.ChangeScreen(Screens.GAME);
        _circleCollider2D.enabled = false;
        canMove = true;
        hookedFishes.Clear();
    }

    void StopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                // To not pull the hook to surface
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            _circleCollider2D.enabled = true;
            int totalGain = 0;
            for (int i = 0; i < hookedFishes.Count; i++)
            {
                hookedFishes[i].transform.SetParent(null);
                hookedFishes[i].ResetFish();
                totalGain += hookedFishes[i].Type.price;
            }

            IdleManager.instance.totalGain = totalGain;
            ScreenManager.instance.ChangeScreen(Screens.END);
            
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && fishCount != strength)
        {
            fishCount++;
            Fish component = other.GetComponent<Fish>();
            component.Hooked();
            hookedFishes.Add(component);
            //Hook'u parent yapıyor ve taşıyor
            other.transform.SetParent(transform);
            other.transform.position = hookedTransform.position;
            other.transform.rotation = hookedTransform.rotation;
            other.transform.localScale = Vector3.one;

            //
            other.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90).SetLoops(1,LoopType.Yoyo).OnComplete(
                delegate
                {
                    other.transform.rotation = Quaternion.identity;
                    
                });
            if (fishCount == strength)
            {
                StopFishing();
            }
        }
    }
}