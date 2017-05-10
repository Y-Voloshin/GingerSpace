using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VGF.UintyUI;

namespace Catopus.UI
{
    public class CurrentPlanetInfo : MonoBehaviour
    {
        [SerializeField]
        Text VisitedValue,
            LevelValue,
            ResourcesValue,
            PopulationValue;

        GameObject go;

        public void Hide()
        {
            go.SetActive(false);
        }

        public void Show()
        {
            if (go == null)
                go = gameObject;
            go.SetActive(true);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            var p = Planet.Current;
            VisitedValue.SetHasNoUnknown(true, p.Visited);
            LevelValue.SetIntUnknown(p.LevelObserved, p.Level);
            ResourcesValue.SetHasNoUnknown(p.ResourcesObserved, p.HasResources);
            PopulationValue.SetHasNoUnknown(p.PopulationObserved, p.HasPopulation);
        }
    }

    public static class CatopusTextExtensions
    {
        static string yesT = "Да",
            no = "Нет",
            hasT = "Есть",
            unknown = "?";

        public static void SetYesNoUnknown(this Text t, bool known, bool yes)
        {
            if (t == null)
                return;
            t.text = known ? yes ? yesT : no : unknown;
        }

        public static void SetHasNoUnknown(this Text t, bool known, bool has)
        {
            if (t == null)
                return;
            t.text = known ? has ? hasT : no : unknown;
        }

        public static void SetIntUnknown(this Text t, bool known, int value)
        {
            if (t == null)
                return;
            t.text = known ? value.GetCachedIntString() : unknown;
        }
    }

}