using System;
using System.Collections.Generic;
using UnityEngine;

namespace Catopus.ButtonInput
{
    public class InputController : MonoBehaviour
    {
        //It should be a singletone to deny multiple reactionson single input event
        static InputController instance;

        public static event Action OnAccelerateButtonDown,
            OnGoToNearestPlanetButtonDown;        

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckKeyEvent(KeyCode.Space, OnAccelerateButtonDown);
            CheckKeyEvent(KeyCode.LeftControl, OnGoToNearestPlanetButtonDown);
        }

        void CheckKeyEvent(KeyCode key, Action evt)
        {
            if (Input.GetKeyDown(key))
                if (evt != null)
                    evt();
        }
    }
}