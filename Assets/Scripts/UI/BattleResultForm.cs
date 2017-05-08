using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using VGF.UintyUI;
using Catopus.Battle;

namespace Catopus.UI
{
    public class BattleResultForm : MonoBehaviour
    {
        [SerializeField]
        Text ResultDescription,
            ExpaValue,
            FuelValue,
            Message;

        [SerializeField]
        GameObject ExpaCaption, FuelCaption;



        string Win = "Вы одержали победу",
            Lose = "Вы потерпели поражение";

        void OnEnable()
        {
            UpdateInfo();
        }

        void UpdateInfo()
        {
            var r = BattleResultModel.LastBattleResult;
            ResultDescription.TrySetText(r.Victory ? Win: Lose);

            UpdateIntLabelValue(r.Reward.Fuel, FuelCaption, FuelValue);
            UpdateIntLabelValue(r.Reward.Expa, ExpaCaption, ExpaValue);

            Message.TrySetText(r.Message);
        }

        void UpdateIntLabelValue(int value, GameObject lCaption, Text lValue)
        {
            if (value == 0)
            {
                lCaption.TrySetActive(false);
                lValue.TryClear();
            }
            else
            {
                lCaption.TrySetActive(true);
                lValue.TrySetInt(value);
            }
            
        }
    }
}