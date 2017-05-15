using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VGF.UintyUI;

namespace Catopus.UI
{
    public class RPGLabelValueGroup : LabelValueGroup
    {
        //[SerializeField]
        //protected Text BalancedValue;
        [SerializeField]
        protected Button UpgradeButton;

        public void SetValue(int value, bool btnActive)
        {
            SetValue(value);
            //BalancedValue.TrySetInt(balancedValue);
            UpgradeButton.SetActivity(btnActive);
        }
    }

    public static class RPGLabelValueGroupExtensions
    {
        /// <summary>
        /// Do SetValue if this object is not null
        /// </summary>
        /// <param name="g"></param>
        /// <param name="value"></param>
        /// <param name="balancedValue"></param>
        /// <param name="btnActive"></param>
        public static void SetValueSafe(this RPGLabelValueGroup g, int value, bool btnActive)
        {
            if (g == null)
                return;
            g.SetValue(value, btnActive);
        }
    }
}