// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.StateMachines.Platformer
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays an idle animation.
    /// </summary>
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Platformer - Idle State")]
    [HelpURL(Strings.ExampleAPIDocumentationURL + nameof(StateMachines) + "." + nameof(Platformer) + "/" + nameof(IdleState))]
    public sealed class IdleState : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimationClip _Idle;
        [SerializeField] private AnimationClip _Walk;
        [SerializeField] private AnimationClip _Run;
        [SerializeField] private float _WalkSpeed;
        [SerializeField] private float _RunSpeed;

        /************************************************************************************************************************/

        public override float MovementSpeed => Creature.Brain.IsRunning ? _RunSpeed : _WalkSpeed;

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            if (Creature.Brain.MovementDirection == 0)
            {
                Creature.Animancer.Play(_Idle);
            }
            else if (Creature.Brain.IsRunning)
            {
                Creature.Animancer.Play(_Run);
            }
            else
            {
                Creature.Animancer.Play(_Walk);
            }
        }

        /************************************************************************************************************************/
    }
}
