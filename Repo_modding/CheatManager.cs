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
        private GameObject _MainController;
        private GameObject _PlayerCollision;
        private PlayerController _PlayerController;
        private PlayerHealth _PlayerHealth;
        private FieldInfo _godModeInfo;
        private FieldInfo _extraJumpCurrentInfo;

        private Vector3 _PlayerPos;

        private int PosStablize;
        private bool godModeEnabled = false;
        private bool infEnergyEnabled = false;
        private bool infJumpsEnabled = false;
        private bool CollisionEnabled = true;
        private bool PreventTumble = false;

        public void UpdatePlayer()
        {
            // update GameObject Player
            // get child GameObject Controller from parent GameObject Player
            // originalGameObject.transform.GetChild(0).gameObject
            _MainController = GameObject.Find("Player").transform.Find("Controller").gameObject;
            if (!_MainController)
            {
                MelonLogger.Msg("Could not Find Player Object");
                return;
            }
            _PlayerCollision = _MainController.gameObject.transform.Find("Collision").gameObject;
            _PlayerController = _MainController.gameObject.GetComponent<PlayerController>();
            _PlayerHealth = _PlayerController.playerAvatar.gameObject.GetComponent<PlayerHealth>();
            // binding for private variables
            _godModeInfo = typeof(PlayerHealth).GetField("godMode", BindingFlags.NonPublic | BindingFlags.Instance);
            _extraJumpCurrentInfo = typeof(PlayerController).GetField("JumpExtraCurrent", BindingFlags.NonPublic | BindingFlags.Instance);

            // apply modifiers from previous player
            _PlayerController.DebugEnergy = infEnergyEnabled;
            _godModeInfo.SetValue(_PlayerHealth, godModeEnabled);

            MelonLogger.Msg("Player Updated");

        }

        public void toggleGodMode()
        {
            godModeEnabled = !godModeEnabled;
            _godModeInfo.SetValue(_PlayerHealth, godModeEnabled);
            MelonLogger.Msg($"GodMode set to {godModeEnabled}");
        }
        public void toggleInfEngergy()
        {
            infEnergyEnabled = !infEnergyEnabled;
            _PlayerController.DebugEnergy = infEnergyEnabled;
            MelonLogger.Msg($"DebugEnergy set to {infEnergyEnabled}");
        }
        public void toggleInfJump()
        {
            infJumpsEnabled = !infJumpsEnabled;
            MelonLogger.Msg($"InfinateJumps set to {infJumpsEnabled}");
        }
        public void toggleTumble()
        {
            PreventTumble = !PreventTumble;
            _PlayerController.DebugNoTumble = PreventTumble;
            MelonLogger.Msg($"PreventTumble set to {PreventTumble}");
        }
        public void verticalShiftUp()
        {
            _PlayerPos = _MainController.transform.position;
            _PlayerPos += Vector3.up * 5;
            PosStablize = 5;
            MelonLogger.Msg("Vertical Shift UP");
        }
        public void verticalShiftDown()
        {
            _PlayerPos = _MainController.transform.position;
            _PlayerPos += Vector3.down * _PlayerPos.y;
            PosStablize = 5;
            MelonLogger.Msg("Vertical Shift DOWN");
        }
        public void toggleCollision()
        {
            CollisionEnabled = !CollisionEnabled;
            _PlayerCollision.SetActive(CollisionEnabled);
        }

        public void OnUpdate()
        {
            if (PosStablize > 0)
            {
                _MainController.transform.transform.position = _PlayerPos;
                PosStablize -= 1;
            }
            if (Globals.UpdatePlayer) { UpdatePlayer(); Globals.UpdatePlayer = false; }
            if (infJumpsEnabled) { _extraJumpCurrentInfo.SetValue(_PlayerController, 1); }


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
