using System;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    [CreateAssetMenu(fileName = "Stage Info", menuName = "SuperSoldier/Stage Info")]
    public class StageInfoSO : ScriptableObject
    {
        public Sprite thumbnail;
        public string title;
        public string sceneName;
        public TimeSpan bestRecord;
        public bool isLocked;


        public string GetBestRecordAsString()
        {
            return bestRecord.ToString("mm:ss.ff");
        }


    }
}
