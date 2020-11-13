using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BepInEx;
using UnityEngine;

namespace BepAwkwardsModPack
{
    [BepInPlugin("awkwardsModPackVecter","awkwardArtistsModPack", "1.0.2")]
    [BepInProcess("Vecter.exe")]
    public class Class1 : BaseUnityPlugin
    {

        public bool hasAdded;
        public bool enterDepressed;
        public bool specialEnter;
        public bool warningSkipYN;
        public string warningSkipYNString;
        public OptionsMenu2 menu;
        public string firstUpgradeYNString;
        public bool firstUpgradeYN;
        public bool onOffFps;
        public string onOffFpsString;
        public static PauseMenu pause;

        
            void Start()
            {
                hasAdded = false;
            if (!File.Exists(Path.Combine(Application.dataPath, @"..\Mods\warning.json")))
            {
                File.WriteAllText(Path.Combine(Application.dataPath, @"..\Mods\warning.json"), "TurnedOff...");
            }
            var readTheWarning = File.ReadAllText(Path.Combine(Application.dataPath, @"..\Mods\warning.json"));
            switch (readTheWarning)
            {
                case "TurnedOn...":
                    warningSkipYN = true;
                    break;
                case "TurnedOff...":
                    warningSkipYN = false;
                    break;
                default:
                    warningSkipYN = false;
                    break;
            }

        }
            void ResumeMe()
            {
                go.GeneralGameState.GetComponent<MonitorRamUsage>().ClearRam(true);
                if (GameObject.Find("TrackResetCanvas").GetComponent<NewRunCountdown>().timeRemaining.TotalSeconds > 0.0)
                {
                    Time.timeScale = 1f;
                    go.GeneralGameState.isPauseMenuShowing = false;
                    go.GeneralGameState.pauseMenu.SetActive(false);
                }
            }
            void OnApplicationQuit()
            {
                if (firstUpgradeYN)
                {
                    go.GeneralGameState.UserInfo.ShipLevel = ShipLevel.Level4;
                    firstUpgradeYN = false;

                }
            }
            void Update()
            {
                if (go.GeneralGameState.isOptionsMenuShowing)
                {
                    UIStuff();
                    UpdateFPS();
                }
                if (specialEnter && Input.GetKeyUp(KeyCode.Escape))
                {
                    pause = Resources.FindObjectsOfTypeAll<PauseMenu>().First();

                    QuitRunButton();
                    GeneralGameState.isMainMenuShowing = true;
                    Time.timeScale = 1f;
                    go.GeneralGameState.isPauseMenuShowing = false;
                    go.GeneralGameState.pauseMenu.SetActive(false);
                    go.GeneralGameState.mainMenu.SetActive(true);
                    pause.isMenuVisible = false;
                    ResumeMe();
                    specialEnter = false;



                }

            }


        public void UIStuff()
        {

            menu = Resources.FindObjectsOfTypeAll<OptionsMenu2>().First<OptionsMenu2>();


            if (!hasAdded)
            {
                BepVecterModCore.VecterModCore.ModSettings.Add("- AwkwardsModPack");
                BepVecterModCore.VecterModCore.ModSettings.Add("- BackToFirstUpgrade");
                BepVecterModCore.VecterModCore.ModSettings.Add("- FPScounter");
                BepVecterModCore.VecterModCore.ModSettings.Add("- GoToColourChanger");
                hasAdded = true;
            }

            //UI Descriptions
            switch (menu.SelectedOption)
            {
                case "AwkwardsModPack":
                    menu.SelectedOption += "\nWelcome to awkwards mod pack!\ni made this for all the smaller ideas i had\nwithout the inconvenience of\n a ton of smaller mods.";
                    break;


                case "BackToFirstUpgrade":

                    if (go.GeneralGameState.UserInfo.ShipLevel < ShipLevel.Level4 && !firstUpgradeYN)
                    {
                        menu.SelectedOption += "\nyou must be level 4 to use this mod.";
                    }
                    else
                    {
                        if (File.Exists(Application.dataPath + "../Mods/BackTo1stUpgradeMod.dll"))
                        {
                            menu.SelectedOption += "\nYou already have this mod by itself...\n\nsorry :)";
                        }
                        else
                        {
                            menu.SelectedOption += "\nPuts you back to First Upgrade for a challenge >:)\n\n" + firstUpgradeYNString;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Return) && !enterDepressed)
                    {
                        switch (firstUpgradeYN)
                        {
                            case true:
                                firstUpgradeYN = false;
                                break;
                            case false:
                                firstUpgradeYN = true;
                                break;
                        }
                        enterDepressed = true;
                    }
                    if (Input.GetKeyUp(KeyCode.Return) && enterDepressed)
                    {
                        enterDepressed = false;
                    }
                    break;

                case "FPScounter":

                    menu.SelectedOption += "\nDisplays your current FPS in game.\n\n" + onOffFpsString;
                    if (Input.GetKeyDown(KeyCode.Return) && !enterDepressed)
                    {
                        switch (onOffFps)
                        {
                            case true:
                                onOffFps = false;
                                break;
                            case false:
                                onOffFps = true;
                                break;
                        }
                        enterDepressed = true;
                    }
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        enterDepressed = false;
                    }
                    break;

                case "GoToColourChanger":
                    menu.SelectedOption += "\ntakes you straight to the colour editor you\ncan usually only access through pause.\n\n[ take me there ]";
                    if (Input.GetKeyDown(KeyCode.Return) && !enterDepressed)
                    {
                        go.GeneralGameState.colourPickerMenu.SetActive(true);
                        go.GeneralGameState.mainMenu.SetActive(false);
                        go.GeneralGameState.optionsMenu.SetActive(false);
                        specialEnter = true;
                        enterDepressed = true;
                    }
                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        enterDepressed = false;
                    }

                    break;


            }

            //EnabledDisabled Options
            switch (warningSkipYN)
            {
                case true:
                    warningSkipYNString = "[ on ] /   off  ";
                    break;
                case false:
                    warningSkipYNString = "  on   / [ off ] ";
                    break;

            }
            switch (onOffFps)
            {
                case true:
                    onOffFpsString = "[ on ] /   off  ";
                    break;
                case false:
                    onOffFpsString = "  on   / [ off ] ";
                    break;
            }
            switch (firstUpgradeYN)
            {
                case true:
                    firstUpgradeYNString = "[ enabled ] /   disabled  ";
                    go.GeneralGameState.UserInfo.ShipLevel = ShipLevel.Level1;
                    break;
                case false:
                    firstUpgradeYNString = "  enabled   / [ disabled ]";
                    go.GeneralGameState.UserInfo.ShipLevel = ShipLevel.Level4;
                    break;
            }




        }

        public void UpdateFPS()
        {

            if (onOffFps)
            {
                var newGraphy = Resources.FindObjectsOfTypeAll<Tayx.Graphy.GraphyManager>().First();
                newGraphy.FpsModuleState = Tayx.Graphy.GraphyManager.ModuleState.TEXT;

            }
            else
            {
                var newGraphy = Resources.FindObjectsOfTypeAll<Tayx.Graphy.GraphyManager>().First();
                newGraphy.FpsModuleState = Tayx.Graphy.GraphyManager.ModuleState.OFF;
            }

        }
        public void QuitRunButton()
        {
            go.GeneralGameState.GetComponent<MonitorRamUsage>().ClearRam(true);
            if (go.GeneralGameState.training && !go.GeneralGameState.UserInfo.LeaderboardUnlocked)
            {
                for (int i = 0; i < go.PlayerShip.Count; i++)
                {
                    if (go.PlayerScore.GetScore(i) > GeneralGameState.unlockLeaderboardpoints)
                    {
                        go.GeneralGameState.UserInfo.LeaderboardUnlocked = true;
                        go.GeneralGameState.SaveUserInformation(false);
                    }
                }
            }
            if (GeneralGameState.useShadow == null && go.PlayerScore.isPersonalBest && !go.GeneralGameState.training && SeedAPI.VrifySeed(long.Parse(go.GeneralGameState.seed.ToString()), false, null) == long.Parse(go.GeneralGameState.seed.ToString()))
            {
                for (int j = 0; j < go.PlayerShip.Count; j++)
                {
                    go.PlayerShip[j].GetComponent<ShadowRecording>().SaveRecording();
                }
            }
            Time.timeScale = 1f;
            go.GeneralGameState.seed = 0L;
            go.GeneralGameState.training = false;
            PubSubHub.PostMessage<RestartLevel>(new RestartLevel(false, null));
        }

    }
}
