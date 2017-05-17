using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VGF;

namespace Catopus.Model
{
    [System.Serializable]
    public class PlanetModel : AbstractModel<PlanetModel>
    {
        public int PlanetId;
        public float Radius;

        public int Level = 1;
        /// <summary>
        /// Was the planet observed?
        /// </summary>
        public bool Observed,
                /// <summary>
                /// The visited. Was the planet visited? Now it can be visited only once
                /// </summary>
                Visited,
                    HasPopulation,
                    HasResources,
            
            LevelObserved,
            PopulationObserved,
            ResourcesObserved;

        #region quest parameters
        public Reward Reward = new Reward();
        public bool HasQuest,
                    QuestCompleted;

        public int QuestId;
        #endregion

        public PlanetModel()
        {

        }

        public PlanetModel(PlanetModel model)
        {
            SetValues(model);
        }

        public override void SetValues(PlanetModel model)
        {
            PlanetId = model.PlanetId;
            Radius = model.Radius;

            Observed = model.Observed;
            Visited = model.Visited;

            LevelObserved = model.LevelObserved;
            PopulationObserved = model.PopulationObserved;
            ResourcesObserved = model.ResourcesObserved;

            Level = model.Level;            
            HasPopulation = model.HasPopulation;
            HasResources = model.HasResources;
            LevelObserved = model.LevelObserved;
            PopulationObserved = model.PopulationObserved;
            ResourcesObserved = model.ResourcesObserved;

            #region quest parameters
            Reward = model.Reward;
            HasQuest = model.HasQuest;
            QuestCompleted = model.QuestCompleted;

            QuestId = model.QuestId;
            #endregion
        }
    }
}