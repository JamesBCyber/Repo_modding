﻿using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Repo_modding
{

    public class CheatManager
    {
        private static CheatManager instance;
        public static GameObject CheatUI;

        private GameObject Player;
        private GameObject Controller;
        private GameObject Collision;
        private PlayerController PlayerController;
        private GameObject PlayerAvatar;
        private PhysGrabber PhysGrabber;
        private PlayerHealth PlayerHealth;
        private GameObject PostProcessing;
        private static FieldInfo FieldGodMode;
        private static FieldInfo FieldExtraJump;
        private static float OldStrength;

        private Vector3 PlayerPosition;
        private int PositionStabilizeTimer;

        public static bool UpdatePlayerSignal = false;
        public static bool C_GodModeEnabled = false;
        public static bool C_InfEnergyEnabled = false;
        public static bool C_InfJumpsEnabled = false;
        public static bool C_PreventTumble = false;
        public static bool C_CollisionEnabled = true;
        public static bool C_ZeroGravityEnabled = false;
        public static bool C_StrengthEnabled = false;

        public static void Start()
        {
            
            CheatManager.instance = new CheatManager();
            FieldGodMode = typeof(PlayerHealth).GetField("godMode", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldExtraJump = typeof(PlayerController).GetField("JumpExtraCurrent", BindingFlags.NonPublic | BindingFlags.Instance);
            

        }

        public static void UpdatePlayer()
        {
            CheatManager Cheats = CheatManager.instance;

            Cheats.Player = GameObject.Find("Player");
            Cheats.Controller = Cheats.Player.transform.Find("Controller").gameObject;
            Cheats.Collision = Cheats.Controller.gameObject.transform.Find("Collision").gameObject;
            Cheats.PlayerController = Cheats.Controller.gameObject.GetComponent<PlayerController>();
            Cheats.PlayerAvatar = Cheats.PlayerController.playerAvatar.gameObject;
            Cheats.PlayerHealth = Cheats.PlayerAvatar.GetComponent<PlayerHealth>();
            Cheats.PostProcessing = GameObject.Find("Post Processing");
            Cheats.PhysGrabber = Cheats.PlayerAvatar.GetComponent<PhysGrabber>();
            CheatManager.OldStrength = Cheats.PhysGrabber.grabStrength;

            Cheats.PlayerController.DebugEnergy = C_InfEnergyEnabled;
            FieldGodMode.SetValue(Cheats.PlayerHealth, C_GodModeEnabled);

            CheatManager.UpdatePlayerSignal = false;
            MelonLogger.Msg("Player Updated");

        }

        public static void toggleGodMode()
        {
            C_GodModeEnabled = !C_GodModeEnabled;
            FieldGodMode.SetValue(CheatManager.instance.PlayerHealth, C_GodModeEnabled);
            MelonLogger.Msg($"GodMode set to {C_GodModeEnabled}");
        }
        public static void toggleInfEngergy()
        {
            C_InfEnergyEnabled = !C_InfEnergyEnabled;
            CheatManager.instance.PlayerController.DebugEnergy = C_InfEnergyEnabled;
            MelonLogger.Msg($"DebugEnergy set to {C_InfEnergyEnabled}");
        }
        public static void toggleInfJump()
        {
            C_InfJumpsEnabled = !C_InfJumpsEnabled;
            MelonLogger.Msg($"InfinateJumps set to {C_InfJumpsEnabled}");
        }
        public static void toggleTumble()
        {
            C_PreventTumble = !C_PreventTumble;
            CheatManager.instance.PlayerController.DebugNoTumble = C_PreventTumble;
            MelonLogger.Msg($"PreventTumble set to {C_PreventTumble}");
        }
        public static void verticalShiftUp()
        {
            CheatManager.instance.PlayerPosition = CheatManager.instance.Controller.transform.position;
            CheatManager.instance.PlayerPosition += Vector3.up * 5;
            CheatManager.instance.PositionStabilizeTimer = 5;
            MelonLogger.Msg("Vertical Shift UP");
        }
        public static void verticalShiftDown()
        {
            CheatManager.instance.PlayerPosition = CheatManager.instance.Controller.transform.position;
            CheatManager.instance.PlayerPosition += Vector3.down * CheatManager.instance.PlayerPosition.y;
            CheatManager.instance.PositionStabilizeTimer = 5;
            MelonLogger.Msg("Vertical Shift DOWN");
        }
        public static void toggleCollision()
        {
            C_CollisionEnabled = !C_CollisionEnabled;
            CheatManager.instance.Collision.SetActive(C_CollisionEnabled);
        }
        public static void toggleZeroGravity()
        {
            C_ZeroGravityEnabled = !C_ZeroGravityEnabled;
        }
        public static void IncreaseSpeed()
        {
            CheatManager Cheats = CheatManager.instance;
            Cheats.PlayerController.MoveSpeed += 2;
            Cheats.PlayerController.CrouchSpeed += 1;
            Cheats.PlayerController.SprintSpeed += 5;
            MelonLogger.Msg($"Move Speed set to {Cheats.PlayerController.CrouchSpeed}x");

        }
        public static void DecreaseSpeed()
        {
            CheatManager Cheats = CheatManager.instance;
            if (Cheats.PlayerController.MoveSpeed > 0)
            {
                Cheats.PlayerController.MoveSpeed -= 2;
                Cheats.PlayerController.CrouchSpeed -= 1;
                Cheats.PlayerController.SprintSpeed -= 5;
            }

            MelonLogger.Msg($"Move Speed set to {Cheats.PlayerController.CrouchSpeed}x");
        }
        public static void togglePostProcessing()
        {
            bool active = !CheatManager.instance.PostProcessing.activeSelf;
            CheatManager.instance.PostProcessing.SetActive(active);
        }
        private static void toggleStrength()
        {
            C_StrengthEnabled = !C_StrengthEnabled;
            PhysGrabber phys = CheatManager.instance.PhysGrabber;
            if (C_StrengthEnabled)
            {
                phys.grabStrength = CheatManager.OldStrength;
            }
            else
            {
                phys.grabStrength = 5;
            }
            MelonLogger.Msg($"Set Strength to {phys.grabStrength}");
        }
        public static void disableMapOverlay()
        {
            GameObject.Find("Over Layer").SetActive(false);
        }
        public static void OnUpdate()
        {
            if (CheatManager.instance.PositionStabilizeTimer > 0)
            {
                CheatManager.instance.Controller.transform.transform.position = CheatManager.instance.PlayerPosition;
                CheatManager.instance.PositionStabilizeTimer -= 1;
            }
            if (UpdatePlayerSignal) { CheatManager.UpdatePlayer(); }
            if (C_InfJumpsEnabled) { FieldExtraJump.SetValue(CheatManager.instance.PlayerController, 1); }
            if (C_ZeroGravityEnabled) { CheatManager.instance.PlayerController.AntiGravity(0.1f); }


            // Cheat Keys
            if (Input.GetKeyDown(KeyCode.F1)) { toggleGodMode(); }
            if (Input.GetKeyDown(KeyCode.F2)) { toggleInfEngergy(); }
            if (Input.GetKeyDown(KeyCode.F3)) { toggleInfJump(); }
            if (Input.GetKeyDown(KeyCode.F4)) { toggleTumble(); }
            if (Input.GetKeyDown(KeyCode.F5)) { verticalShiftUp(); }
            if (Input.GetKeyDown(KeyCode.F6)) { verticalShiftDown(); }
            if (Input.GetKeyDown(KeyCode.F8)) { toggleStrength(); }
            if (Input.GetKeyDown(KeyCode.F9)) { toggleCollision(); }
            if (Input.GetKeyDown(KeyCode.F10)) { toggleZeroGravity(); }
            if (Input.GetKeyDown(KeyCode.Equals)) { IncreaseSpeed(); }
            if (Input.GetKeyDown(KeyCode.Minus)) { DecreaseSpeed(); }
            if (Input.GetKeyDown(KeyCode.Alpha0)) { togglePostProcessing(); }
            if (Input.GetKeyDown(KeyCode.Alpha9)) { disableMapOverlay(); }

        }

    }
 
}
