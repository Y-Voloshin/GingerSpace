using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catopus.Model
{
    public class PlayerSaveData
    {

        PlayerParameters p = new PlayerParameters();
        Vector3 pos;
        Quaternion rot;

        public void LoadData()
        {
            pos = Spaceship.Instance.transform.position;
            rot = Spaceship.Instance.transform.rotation;
            p.CopyValuesFrom(PlayerController.Instance.Parameters);
        }

        public void SaveData()
        {
            pos = Spaceship.Instance.transform.position;
            rot = Spaceship.Instance.transform.rotation;
            PlayerController.Instance.Parameters.CopyValuesFrom(p);
        }

    }
}
