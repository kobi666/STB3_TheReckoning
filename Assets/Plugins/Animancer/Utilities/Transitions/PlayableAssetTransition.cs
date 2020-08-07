// Animancer // Copyright 2020 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> which holds a <see cref="PlayableAssetState.Transition"/>.
    /// </summary>
#if !UNITY_EDITOR
    [System.Obsolete(Validate.ProOnlyMessage)]
#endif
    [CreateAssetMenu(menuName = Strings.MenuPrefix + "Playable Asset Transition", order = Strings.AssetMenuOrder + 8)]
    [HelpURL(Strings.APIDocumentationURL + "/" + nameof(PlayableAssetTransition))]
    public class PlayableAssetTransition : AnimancerTransition<PlayableAssetState.Transition> { }
}
