// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>
    /// Base class for the various states a <see cref="Brains.Creature"/> can be in and actions they can perform.
    /// </summary>
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Brains - Creature State")]
    [HelpURL(Strings.ExampleAPIDocumentationURL + nameof(StateMachines) + "." + nameof(Brains) + "/" + nameof(CreatureState))]
    public abstract class CreatureState : StateBehaviour<CreatureState>,
        IOwnedState<CreatureState>
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Creature _Creature;

        /// <summary>The <see cref="Brains.Creature"/> that owns this state.</summary>
        public Creature Creature
        {
            get => _Creature;
            set
            {
                // If this was the active state in the previous creature, force it back to Idle.
                if (_Creature != null &&
                    _Creature.StateMachine.CurrentState == this)
                    _Creature.Idle.ForceEnterState();

                _Creature = value;
            }
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            _Creature = Editor.AnimancerEditorUtilities.GetComponentInHierarchy<Creature>(gameObject);
        }
#endif

        /************************************************************************************************************************/

        public StateMachine<CreatureState> OwnerStateMachine => _Creature.StateMachine;

        /************************************************************************************************************************/
    }
}
