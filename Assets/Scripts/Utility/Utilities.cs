using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Utility
{
    public static class Utilities
    {

        public static IEnumerator ActionAfterDelay(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback();
        }

        /// <summary>
        /// /// Takes in a levelData and returns the first world containing this levelData in its LevelDatas List.
        /// </summary>
        /// <param name="levelData">levelData to Check</param>
        /// <param name="saveDataSo">game SessionData to check in</param>
        /// <returns></returns>
        public static WorldData GameSessionGetWorldDataFromLevelData(LevelData levelData, SaveDataSo saveDataSo)
        {
            foreach (WorldData worldData in saveDataSo.WorldDatas)
            {
                foreach (LevelData level in worldData.LevelDatas)
                {
                    if (levelData == level)
                        return worldData;
                }
            }
            return null;
        }

        /// <summary>
        ///  Takes in a levelData and a gameSessionData and returns the successor of the passed in leveldata
        /// </summary>
        /// <param name="levelData">Level you want to get the successor from</param>
        /// <param name="saveDataSo"> the game session data you are referring to</param>
        /// <returns></returns>
        public static LevelData GameSessionGetNextLevelData(LevelData levelData, SaveDataSo saveDataSo)
        {
            var resultLevelData = levelData;

            var worldDatas = saveDataSo.WorldDatas;

            for (int i = 0; i < worldDatas.Count; i++)
            {
                var levelDatas = worldDatas[i].LevelDatas;

                for (int j = 0; j < levelDatas.Count; j++)
                {
                    if (levelDatas[j] == levelData)
                    {
                        if (j < levelDatas.Count - 1)
                        {
                            resultLevelData = levelDatas[j + 1];
                            return resultLevelData;
                        }

                        // if its the last level of the world and its not the last world load the next world.
                        else if (i < worldDatas.Count - 1)
                        {
                            resultLevelData = worldDatas[i + 1].LevelDatas[0];
                            return resultLevelData;
                        }
                        // if its the last level of the last world load the credits.
                        else
                        {
                            Debug.Log("last level passed in");
                            return null;
                        }
                    }
                }
            }
            Debug.Log("Could not find next Level");
            return null;
        }

        public static WorldData GetWorldDataFromWorldDataSO(WorldDataSO worldDataSO, SaveDataSo saveDataSo)
        {
            foreach (WorldData worldData in saveDataSo.WorldDatas)
            {
                if (worldData.WorldName == worldDataSO.WorldName)
                {
                    return worldData;
                }
            }
            return null;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element, Camera camera)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.rect.position, camera, out var result);
            return result;
        }

        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}

