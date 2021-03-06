﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Catopus.UI
{
    public class GameOverStupid : MonoBehaviour
    {
        [SerializeField]
        Text GameOverCase;
        [SerializeField]
        GameObject GameOverWindow;

        string caseLost = "Вы сбились с курса и затерялись в межзвездном пространстве.",
            caseBreakdown = "Вы не справились с управлением и врезались в планету",
            caseOutOfFuel = "В жизни поправимо всё. Всё корме пустого топливного бака в открытом космосе.";

        Planet[] planets;        

        // Use this for initialization
        void Start()
        {
            planets = FindObjectsOfType<Planet>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void FixedUpdate()
        {
            bool toofar = true;
            /*
            if (Spaceship.Instance == null)
            {
                //Debug.Log(Spaceship.Instance);
                return;
            }
            */
            foreach (var p in planets)
            {
                //Debug.Log(Spaceship.Instance);
                //Debug.Log(p);

                float d = Vector3.Distance(Spaceship.Instance.transform.position, p.transform.position);
                if (d < 70)
                {
                    toofar = false;
                }
            }

            if (toofar)
            {
                GameOverWindow.SetActive(true);
                GameOverCase.text = caseLost;
            }
            else if (Spaceship.Instance.State == SpaceShipState.Dead)
            {
                GameOverWindow.SetActive(true);
                GameOverCase.text = caseBreakdown;
            }
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void CheckOutOfFuel()
        {
            if (PlayerController.Instance.FuelCurrent == 0)
            {
                GameOverWindow.SetActive(true);
                GameOverCase.text = caseOutOfFuel;
            }
        }
    }
}