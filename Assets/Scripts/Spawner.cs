using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Fish _fishPrefab;

    [SerializeField] private Fish.FishType[] _fishTypes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        for (int i = 0; i < _fishTypes.Length; i++)
        {
            int num = 0;
            //
            while (num < _fishTypes[i].fishCount)
            {
                Fish _fish = UnityEngine.Object.Instantiate<Fish>(_fishPrefab);
                _fish.Type = _fishTypes[i];
                _fish.ResetFish();
                num++;
            }
        }
    }
}
