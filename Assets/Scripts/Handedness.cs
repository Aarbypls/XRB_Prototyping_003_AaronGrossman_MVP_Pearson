using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Handed
{
    Left,
    Right
}

public class Handedness : MonoBehaviour
{
    public Handed handed;
    [SerializeField] private GameEventManager _gameManager;
    [SerializeField] private GameObject[] _leftHandedObjects;
    [SerializeField] private GameObject[] _rightHandedObjects;

    private void Start()
    {
        handed = _gameManager._handedness;
        
        if (handed == Handed.Left)
        {
            foreach (var obj in _leftHandedObjects)
            {
                obj.SetActive(true);
            }

            foreach (var obj in _rightHandedObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (var obj in _rightHandedObjects)
            {
                obj.SetActive(true);
            }

            foreach (var obj in _leftHandedObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
