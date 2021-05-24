﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

namespace Subnautica_Enhanced_Sleep
{
    public class Tiredness : MonoBehaviour
    {
        /*
        public static Dictionary<Player, float> tirednessDict;
        public static Dictionary<Player, float> tirednessLastUpdateH;
        public static Dictionary<Player, float> tirednessLastUpdateD;
        */
        public static float tiredness;
        public static float lastUpdate;
        public static bool isIngame = false;
        public static bool wasSleepIntroSent = false;

        public static void onEnable()
        {
            isIngame = true;
            /*
            tirednessDict = new Dictionary<Player, float>();
            tirednessLastUpdateH = new Dictionary<Player, float>();
            */
            tiredness = 0;
            lastUpdate = 0;
            //tirednessLastUpdateD = new Dictionary<Player, float>();
            Tiredness.LoadTiredness();
            Main.EnhancedSleepComp.Load();
            TirednessGui.Load();
            SleepGui.Load();

            if (!wasSleepIntroSent)
            {
                wasSleepIntroSent = true;
                gameStartSleepIntroNotification01.Play();
                gameStartSleepIntroNotification02.Play();
                gameStartSleepIntroNotification03.Play();
            }
        }

        public static void onDisable()
        {
            isIngame = false;
            /*
            tirednessDict = null;
            tirednessLastUpdateH = null;
            tirednessLastUpdateD = null;
            */
            tiredness = 0;
            lastUpdate = 0;
        }

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        public class PlayerTirednessPatch
        {
            public static bool MesmerizedScreenFX { get; private set; }

            public static void Postfix(Player __instance)
            {
                if (isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing /*&& tirednessDict != null && tirednessLastUpdateH != null *//* && tirednessLastUpdateD != null*/)
                {
                    /*
                    if (tirednessLastUpdateH.ContainsKey(__instance)&& tirednessDict.ContainsKey(__instance))
                    {
                    */
                    /*
                    if (tirednessLastUpdateH.ContainsKey(__instance) && tirednessLastUpdateD.ContainsKey(__instance) && tirednessDict.ContainsKey(__instance))
                    {
                    */
                    /*
                    tirednessDict.TryGetValue(__instance, out float playerTiredness);
                    tirednessLastUpdateH.TryGetValue(__instance, out float oldHour);
                    tirednessLastUpdateD.TryGetValue(__instance, out float oldDay);
                    float newHour = DayNightCycle.main.GetDayNightCycleTime();
                    float newDay = (float) Math.Floor(DayNightCycle.main.GetDay());
                    float newHour2 = newHour;
                    if (oldDay != newDay)
                    {
                        newHour2 += (newDay - oldDay);
                    }
                    float timePassed = newHour2 - oldHour;
                    float timePassedHours = (float)(timePassed / (1d / 24));
                    float timePassedMinutes = (float) (timePassedHours * 60);
                    float looseFactor = 4;
                    if (timePassedMinutes >= 1)
                    {
                        float tirednessAdded = (float)(timePassedMinutes / (43.2 / looseFactor));
                        float tirednessOut = (float) (playerTiredness + tirednessAdded);
                        tirednessDict[__instance] = tirednessOut;
                        tirednessLastUpdateH[__instance] = newHour;
                        tirednessLastUpdateD[__instance] = newDay;
                        float tirednessNotifyValue = 25f;
                        float tirednessWarningNotifyValue = 50f;
                        if (playerTiredness < tirednessNotifyValue && tirednessOut >= tirednessNotifyValue && tirednessOut < tirednessWarningNotifyValue)
                        {
                            tiredNotification.Play();
                        }
                        else if (playerTiredness < tirednessWarningNotifyValue && tirednessOut >= tirednessNotifyValue)
                        {
                            tiredWarningNotification.Play();
                        }
                    }
                    */
                    // NEW VARIANT
                    if ((float)Math.Floor(DayNightCycle.main.GetDay()) >= Main.config.timetosleep && !(lastUpdate > ((float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()))))
                    {
                        /*
                        tirednessDict.TryGetValue(__instance, out float playerTiredness);
                        tirednessLastUpdateH.TryGetValue(__instance, out float oldTime);
                        */
                        float playerTiredness = tiredness;
                        float oldTime = lastUpdate;
                        float newTime = (float)DayNightCycle.main.GetDayNightCycleTime() +
                                        (float)Math.Floor(DayNightCycle.main.GetDay());
                        float timePassed = newTime - oldTime;
                        float timePassedHours = (float)(timePassed / (1d / Main.config.timePassedHours));
                        float timePassedMinutes = (float)(timePassedHours * Main.config.timePassedMinutes);
                        float looseFactor = Main.config.sleeplooseFactor;
                        float recoverFactor = Main.config.sleeprecoverFactor;
                        if (SleepPatcher.isSleeping)
                        {
                            if (timePassedMinutes >= 1)
                            {
                                float tirednessAdded = (float)(timePassedMinutes / (43.2 / recoverFactor));
                                float tirednessOut = (float)(playerTiredness - tirednessAdded);
                                if (tirednessOut > 100)
                                {
                                    tirednessOut = 100;
                                }
                                else if (tirednessOut < 0)
                                {
                                    tirednessOut = 0;
                                }
                                /*
                                tirednessDict[__instance] = tirednessOut;
                                tirednessLastUpdateH[__instance] = newTime;
                                */
                                tiredness = tirednessOut;
                                lastUpdate = newTime;
                            }
                        }
                        else
                        {
                            if (timePassedMinutes >= 2)
                            {
                                float tirednessAdded = (float)(timePassedMinutes / (43.2 / looseFactor));
                                float tirednessOut = (float)(playerTiredness + tirednessAdded);
                                if (tirednessOut > 100)
                                {
                                    tirednessOut = 100;
                                }
                                else if (tirednessOut < 0)
                                {
                                    tirednessOut = 0;
                                }
                                /*
                                tirednessDict[__instance] = tirednessOut;
                                tirednessLastUpdateH[__instance] = newTime;
                                */
                                tiredness = tirednessOut;
                                lastUpdate = newTime;
                                float tirednessNotifyValue = 25f;
                                float tirednessWarningNotifyValue = 50f;
                                if (playerTiredness < tirednessNotifyValue &&
                                    tirednessOut >= tirednessNotifyValue &&
                                    tirednessOut < tirednessWarningNotifyValue)
                                {
                                    //tiredNotification.Play();
                                }
                                else if (playerTiredness < tirednessWarningNotifyValue &&
                                         tirednessOut >= tirednessNotifyValue)
                                {
                                    //tiredWarningNotification.Play();
                                }

                                if (tiredness >= 30)
                                {
                                    
                                }
                                if (tiredness >= 75 )
                                {
                                    GameInput.SetInvertMouse(false);
                                }
                                
                                if (tiredness >= 100)
                                {
                                    tiredness = 100;

                                    __instance.liveMixin.TakeDamage(Main.config.FullSleepdamage);
                                    DevConsole.SendConsoleCommand("hypnosison");
                                    __instance.lilyPaddlerHypnosis.cameraRotationSpeed = Main.config.cameraRotationSpeed;
                                    GameInput.SetInvertMouse(true);
                                    
                                }  
                                if (tiredness < 0)
                                {
                                    GameInput.SetInvertMouse(false);
                                    tiredness = 0;
                                    DevConsole.SendConsoleCommand("hypnosisoff");
                                   



                                }
                            }
                        }
                    }
                    else if ((lastUpdate > ((float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()))))
                    {

                    }
                    else
                    {
                        float newTime = (float)DayNightCycle.main.GetDayNightCycleTime() +
                                        (float)Math.Floor(DayNightCycle.main.GetDay());
                        //tirednessLastUpdateH[__instance] = newTime;
                        lastUpdate = newTime;
                    }
                    /*
                    }
                    else
                    {
                        if (tirednessDict.ContainsKey(__instance))
                        {
                            //tirednessDict.TryGetValue(__instance, out float playerTiredness);
                            //tirednessDict[__instance] = playerTiredness;
                            tirednessDict[__instance] = 0;
                            if (tirednessLastUpdateH.ContainsKey(__instance))
                            {
                                //tirednessLastUpdateH[__instance] = DayNightCycle.main.GetDayNightCycleTime();
                                tirednessLastUpdateH[__instance] = (float) DayNightCycle.main.GetDayNightCycleTime() + (float) Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                //tirednessLastUpdateH.Add(__instance, DayNightCycle.main.GetDayNightCycleTime());
                                tirednessLastUpdateH.Add(__instance, (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                        }
                        else
                        {
                            tirednessDict.Add(__instance, 0);
                            if (tirednessLastUpdateH.ContainsKey(__instance))
                            {
                                tirednessLastUpdateH[__instance] = (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay());
                            }
                            else
                            {
                                tirednessLastUpdateH.Add(__instance, (float)DayNightCycle.main.GetDayNightCycleTime() + (float)Math.Floor(DayNightCycle.main.GetDay()));
                            }
                        }
                    }
                    */

                }
            }
        }

        [HarmonyPatch(typeof(Survival))]
        [HarmonyPatch("Eat")]
        public class PlayerEatPatch
        {
            public static void Postfix(Survival __instance, GameObject useObj)
            {
                if ((UnityEngine.Object)useObj != (UnityEngine.Object)null)
                {

                   
                    TechType ttt = CraftData.GetTechType(useObj);
                    if (TechType.FilteredWater == ttt)
                    {
                        tiredness -= Main.config.Watertiredness;
                    }
                    TechType tttt = CraftData.GetTechType(useObj);
                    if (TechType.DisinfectedWater == tttt)
                    {
                        tiredness -= Main.config.Watertiredness;
                    }
                    TechType ttttt = CraftData.GetTechType(useObj);
                    if (TechType.BigFilteredWater == ttttt)
                    {
                        tiredness -= Main.config.Waterbigtiredness;
                    }
                    TechType tttttt = CraftData.GetTechType(useObj);
                    if (TechType.StillsuitWater == tttttt)
                    {
                        tiredness -= Main.config.Watertiredness;
                    }
                    
                }
            }
        }

        public static PDANotification gameStartSleepIntroNotification01 = new PDANotification();
        public static PDANotification gameStartSleepIntroNotification02 = new PDANotification();
        public static PDANotification gameStartSleepIntroNotification03 = new PDANotification();
        public static PDANotification tiredNotification = new PDANotification();
        public static PDANotification tiredWarningNotification = new PDANotification();

        public static void initAssets()
        {
            gameStartSleepIntroNotification01 = new PDANotification();
            gameStartSleepIntroNotification01.sound = null;
            gameStartSleepIntroNotification01.text = "For your safety, you have been injected with a sleep suppressing serum.";
            gameStartSleepIntroNotification02 = new PDANotification();
            gameStartSleepIntroNotification02.sound = null;
            gameStartSleepIntroNotification02.text = "Your need for sleep has been removed for 12 days.";
            gameStartSleepIntroNotification03 = new PDANotification();
            gameStartSleepIntroNotification03.sound = null;
            gameStartSleepIntroNotification03.text = "Please try to find a way to sleep as soon as possible.";
            tiredNotification = new PDANotification();
            tiredNotification.sound = null;
            tiredNotification.text = "You are getting tired. Getting some sleep is highly recommended.";
            tiredWarningNotification = new PDANotification();
            tiredWarningNotification.sound = null;
            tiredWarningNotification.text = "You are really tired. Find some sleep immediately.";
        }

        /*
        public class TirednessSOHData
        {
            public Dictionary<Player, float> tirednessDict;

            public TirednessSOHData()
            {
                if (Tiredness.tirednessDict != null)
                {
                    this.tirednessDict = Tiredness.tirednessDict;
                }
                else
                {
                    this.tirednessDict = new Dictionary<Player, float>();
                }
            }
        }

        public static Dictionary<Player,float> getTirednessDict()
        {
            if (Tiredness.tirednessDict != null)
            {
                return Tiredness.tirednessDict;
            }
            else
            {
                return new Dictionary<Player, float>();
            }
        }
        */

        /*
        [Serializable]
        internal class TirednessSaveData
        {
            public Dictionary<Player, float> TirednessDict { get; set; }
            public Dictionary<Player, float> TirednessLastUpdateH { get; set; }
        }
        */

        [Serializable]
        internal class TirednessSaveData
        {
            public float _tiredness { get; set; }
            public float _lastUpdate { get; set; }
            public bool _wasSleepIntroSent { get; set; }
        }

        public static void SaveTiredness()
        {
            TirednessSaveData saveData = new TirednessSaveData
            {
                /*
                TirednessDict = tirednessDict,
                TirednessLastUpdateH = tirednessLastUpdateH
                */
                _tiredness = tiredness,
                _lastUpdate = lastUpdate,
                _wasSleepIntroSent = wasSleepIntroSent,
            };
            string sSaveData = JsonConvert.SerializeObject(saveData);
            Directory.CreateDirectory(Main.GetSaveGameDir());
            File.Create(Main.GetSaveGameDir() + "/tiredness.json").Close();
            File.WriteAllText(Main.GetSaveGameDir() + "/tiredness.json", sSaveData);
        }

        public static void LoadTiredness()
        {
            if (File.Exists(Main.GetSaveGameDir() + "/tiredness.json"))
            {
                string loadData = File.ReadAllText(Main.GetSaveGameDir() + "/tiredness.json");
                TirednessSaveData saveDataT = JsonConvert.DeserializeObject<TirednessSaveData>(loadData);
                /*
                tirednessDict = saveDataT.TirednessDict;
                tirednessLastUpdateH = saveDataT.TirednessLastUpdateH;
                */
                tiredness = saveDataT._tiredness;
                lastUpdate = saveDataT._lastUpdate;
                wasSleepIntroSent = saveDataT._wasSleepIntroSent;
            }
            else
            {
                tiredness = 0;
                lastUpdate = (float)DayNightCycle.main.GetDayNightCycleTime() +
                             (float)Math.Floor(DayNightCycle.main.GetDay());
                wasSleepIntroSent = false;

            }
        }
    }
}
