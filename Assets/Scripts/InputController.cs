using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VGF;

namespace Catopus.ButtonInput
{
    public class InputController : MonoBehaviour
    {
        //It should be a singletone to deny multiple reactionson single input event
        static InputController instance;

        [SerializeField]
        EventSystem evSys;

        public static event Action OnAccelerateButtonDown,
            OnGoToNearestPlanetButtonDown,
            OnSave,
            OnLoad,
            OnLoadInit,
            
            OnLMBDown, OnLMBUp;        

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
            if (evSys == null)
                evSys = GameObject.FindObjectOfType<EventSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckKeyEvent(KeyCode.Space, OnAccelerateButtonDown);
            CheckKeyEvent(KeyCode.LeftControl, OnGoToNearestPlanetButtonDown);

            CheckKeyEvent(KeyCode.S, OnSave);
            CheckKeyEvent(KeyCode.L, OnLoad);
            CheckKeyEvent(KeyCode.I, OnLoadInit);

            if (!evSys.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                    OnLMBDown.CallEventIfNotNull();
                if (Input.GetMouseButtonUp(0))
                    OnLMBUp.CallEventIfNotNull();
            }
            //if (evSys.)
        }

        void CheckKeyEvent(KeyCode key, Action evt)
        {
            if (Input.GetKeyDown(key))
                evt.CallEventIfNotNull();
        }
    }
}