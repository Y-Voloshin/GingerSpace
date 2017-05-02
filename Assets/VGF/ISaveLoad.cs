﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Voloshin game framework
/// </summary>
namespace VGF
{
    /// <summary>
    /// Interface for classes supporting save/load their states
    /// </summary>
    public interface ISaveLoad
    {
        /// <summary>
        /// Rewrite last saved state
        /// </summary>
        void Save();
        /// <summary>
        /// Load last saved state
        /// </summary>
        void Load();
        /// <summary>
        /// Create and save initial state
        /// </summary>
        void Init();
        /// <summary>
        /// Load initial state (reload, restart functions)
        /// </summary>
        void LoadInit();
    }
}