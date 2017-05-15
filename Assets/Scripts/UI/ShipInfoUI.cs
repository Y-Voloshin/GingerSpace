using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Catopus;

namespace Catopus.UI
{
    public class ShipInfoUI : MonoBehaviour
    {

        [SerializeField]
        LabelValueGroup Fuel, Expa; // ExpaLabel;
        //TODO: create a logic for LabeLValueGropu that hides label if value is 0

        // Use this for initialization
        void Start()
        {
            UpdateValues();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateValues()
        {
            Fuel.SetValueSafe(PlayerController.Instance.FuelCurrent.ToString()
            + " / " + PlayerController.Instance.FuelMax.ToString());

            //int xp = PlayerController.Instance.ExperiencePoints;
            //Debug.Log(PlayerController.Instance.ExperiencePoints);
            Expa.SetValueSafe(PlayerController.Instance.ExperiencePoints);
        }
    }
}
