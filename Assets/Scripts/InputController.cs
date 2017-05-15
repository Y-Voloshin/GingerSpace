using System;
using System.Collections.Generic;
using UnityEngine;
using VGF;

namespace Catopus.ButtonInput
{
    public class InputController : MonoBehaviour
    {
        //It should be a singletone to deny multiple reactionson single input event
        static InputController instance;

        public static event Action OnAccelerateButtonDown,
            OnGoToNearestPlanetButtonDown,
            OnSave,
            OnLoad,
            OnLoadInit;        

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

            CheckKeyEvent(KeyCode.S, OnSave);
            CheckKeyEvent(KeyCode.L, OnLoad);
            CheckKeyEvent(KeyCode.I, OnLoadInit);
        }

        void CheckKeyEvent(KeyCode key, Action evt)
        {
            if (Input.GetKeyDown(key))
                evt.CallEventIfNotNull();
        }
    }
}