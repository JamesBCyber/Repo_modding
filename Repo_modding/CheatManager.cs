using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Repo_modding
{
    public class CheatManager
    {
        private static CheatManager instance;

        private GameObject Controller;
        private GameObject Collision;
        private PlayerController PlayerController;
        private PlayerHealth PlayerHealth;
        private FieldInfo FieldGodMode;
        private FieldInfo FieldExtraJump;

        private Vector3 PlayerPosition;

        public static bool UpdatePlayerSignal = false;
        private int PositionStabilizeTimer;
        private static bool C_GodModeEnabled = false;
        private static bool C_InfEnergyEnabled = false;
        private static bool C_InfJumpsEnabled = false;
        private static bool C_CollisionEnabled = true;
        private static bool C_PreventTumble = false;

        public static void Start()
        {
            MelonLogger.Msg("Cheats Starting Point");
            CheatManager.instance = new CheatManager();
        }
        public static void UpdatePlayer()
        {
            CheatManager Cheats = CheatManager.instance;

            Cheats.Controller = GameObject.Find("Player").transform.Find("Controller").gameObject;
            if (!Cheats.Controller)
            {
                MelonLogger.Msg("Could not Find Player Object");
                return;
            }
            Cheats.Collision = Cheats.Controller.gameObject.transform.Find("Collision").gameObject;
            Cheats.PlayerController = Cheats.Controller.gameObject.GetComponent<PlayerController>();
            Cheats.PlayerHealth = Cheats.PlayerController.playerAvatar.gameObject.GetComponent<PlayerHealth>();
            Cheats.FieldGodMode = typeof(PlayerHealth).GetField("godMode", BindingFlags.NonPublic | BindingFlags.Instance);
            Cheats.FieldExtraJump = typeof(PlayerController).GetField("JumpExtraCurrent", BindingFlags.NonPublic | BindingFlags.Instance);

            Cheats.PlayerController.DebugEnergy = C_InfEnergyEnabled;
            Cheats.FieldGodMode.SetValue(Cheats.PlayerHealth, C_GodModeEnabled);

            MelonLogger.Msg("Player Updated");

        }

        public static void toggleGodMode()
        {
            C_GodModeEnabled = !C_GodModeEnabled;
            CheatManager.instance.FieldGodMode.SetValue(CheatManager.instance.PlayerHealth, C_GodModeEnabled);
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

        public static void OnUpdate()
        {
            if (CheatManager.instance.PositionStabilizeTimer > 0)
            {
                CheatManager.instance.Controller.transform.transform.position = CheatManager.instance.PlayerPosition;
                CheatManager.instance.PositionStabilizeTimer -= 1;
            }
            if (UpdatePlayerSignal) { CheatManager.UpdatePlayer(); }
            if (C_InfJumpsEnabled) { CheatManager.instance.FieldExtraJump.SetValue(CheatManager.instance.PlayerController, 1); }


            // Cheat Keys
            if (Input.GetKeyDown(KeyCode.F1)) { toggleGodMode(); }
            if (Input.GetKeyDown(KeyCode.F2)) { toggleInfEngergy(); }
            if (Input.GetKeyDown(KeyCode.F3)) { toggleInfJump(); }
            if (Input.GetKeyDown(KeyCode.F4)) { toggleTumble(); }
            if (Input.GetKeyDown(KeyCode.F5)) { verticalShiftUp(); }
            if (Input.GetKeyDown(KeyCode.F6)) { verticalShiftDown(); }
            if (Input.GetKeyDown(KeyCode.F9)) { toggleCollision(); }

        }



    }
}
