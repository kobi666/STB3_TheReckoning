// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>
    /// The numerical details of a <see cref="Creature"/>.
    /// </summary>
    [Serializable]
    public sealed class CreatureStats
    {
        /************************************************************************************************************************/

        [SerializeField]
        private float _WalkSpeed = 2;
        public float WalkSpeed => _WalkSpeed;

        [SerializeField]
        private float _RunSpeed = 4;
        public float RunSpeed => _RunSpeed;

        public float GetMoveSpeed(bool isRunning) => isRunning ? _RunSpeed : _WalkSpeed;

        /************************************************************************************************************************/

        [SerializeField]
        private float _TurnSpeed = 360;
        public float TurnSpeed => _TurnSpeed;

        /************************************************************************************************************************/

        // Max health.
        // Strength, dexterity, intelligence.
        // Carrying capacity.
        // Etc.

        /************************************************************************************************************************/
    }
}
