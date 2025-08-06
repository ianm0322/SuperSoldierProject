using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrSuperSoldier
{
    public class LevelManager : Manager
    {
        public GameObject Player { get; set; }

        public GameObject LevelObject { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            GatherLevelObjects(SceneManager.GetActiveScene());
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene currentScene)
        {
            GatherLevelObjects(currentScene);
        }
        private void GatherLevelObjects(Scene scene)
        {
            var rootObjects = scene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                switch (obj.tag)
                {
                    case "Player":
                        Player = obj;
                        break;
                    case "LevelObject":
                        LevelObject = obj;
                        break;
                }
            }
        }
    }
}
