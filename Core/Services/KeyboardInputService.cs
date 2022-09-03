using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Larje.Core.Services
{
    [BindService(typeof(KeyboardInputService))]
    public class KeyboardInputService : Service
    {
        [SerializeField] private KeyCode[] _keysToDetect;

        public Action<KeyCode> OnKeyDown;
        public Action<KeyCode> OnKeyUp;


        public override void Init(){}

        public void Update()
        {
            foreach (KeyCode key in _keysToDetect) 
            {
                if (Input.GetKeyDown(key)) OnKeyDown?.Invoke(key);
                if (Input.GetKeyUp(key)) OnKeyUp?.Invoke(key);
            }
        }
    }
}