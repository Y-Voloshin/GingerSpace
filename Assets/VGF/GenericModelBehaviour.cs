using UnityEngine;
using System;

namespace VGF
{
    /// <summary>
    /// Controller for abstract models, providing save, load, reset model
    /// </summary>
    /// <typeparam name="T">AbstractModel child type</typeparam>
    public class GenericModelBehaviour<T> : CachedBehaviour, ISaveLoad where T : AbstractModel<T>, new()
    {
        [SerializeField]
        protected T InitModel;
        //[SerializeField]
        protected T CurrentModel, SavedModel;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public virtual void Init()
        {
            //Debug.Log(InitModel);
            if (InitModel == null)
                return;
            Debug.Log(gameObject.name + " : Init current model");
            CurrentModel = new T();
            CurrentModel.InitializeWith(InitModel);
            //Debug.Log(CurrentModel);
            //Debug.Log("Init saved model");
            SavedModel = new T();
            SavedModel.InitializeWith(InitModel);
        }

        public virtual void Load()
        {
            LoadFrom(SavedModel);
        }

        public virtual void LoadInit()
        {
            LoadFrom(InitModel);
        }

        void LoadFrom(T source)
        {
            if (source == null)
                return;
            CurrentModel.SetValues(source);
        }

        public virtual void Save()
        {
            if (CurrentModel == null)
                return;
            if (SavedModel == null)
                SavedModel.InitializeWith(CurrentModel);
            else
                SavedModel.SetValues(CurrentModel);
        }
    }
}