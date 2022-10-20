using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Level_Selection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelSelectLightManager : MonoBehaviour
{
   [SerializeField] private List<Light2D> _lights = new List<Light2D>();

   private void OnEnable()
   {
      LevelSelect.OnLevelSelectStarted += HandleLevelSelectStarted;
      WorldSelectSocket.OnButtonSelectedAction += HandleWorldIconSelected;
   }

   private void OnDisable()
   {
      LevelSelect.OnLevelSelectStarted -= HandleLevelSelectStarted;
      WorldSelectSocket.OnButtonSelectedAction -= HandleWorldIconSelected;
   }

   private void HandleWorldIconSelected(WorldData worldData, Transform transform1)
   {
      SetLightColorAll(worldData.Style.MenuLightColor);  
   }

   private void HandleLevelSelectStarted(WorldData worldData)
   {
      SetLightColorAll(worldData.Style.MenuLightColor);
   }

   private void SetLightColorAll(Color color)
   {
      foreach (var light2D in _lights)
      {
         light2D.color = color;
      }
   }
}
