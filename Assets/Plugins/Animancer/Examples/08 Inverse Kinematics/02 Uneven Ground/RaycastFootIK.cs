// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0414 // Field is assigned but its value is never used.
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.InverseKinematics
{
    /// <summary>
    /// Demonstrates how to use Unity's Inverse Kinematics (IK) system to adjust a character's feet according to the
    /// terrain they are moving over.
    /// </summary>
    [AddComponentMenu(Strings.MenuPrefix + "Examples/Inverse Kinematics - Raycast Foot IK")]
    [HelpURL(Strings.APIDocumentationURL + ".Examples.InverseKinematics/RaycastFootIK")]
    public sealed class RaycastFootIK : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private AnimationClip _Animation;
        [SerializeField] private float _RaycastOriginY = 0.5f;
        [SerializeField] private float _RaycastEndY = -0.2f;

        /************************************************************************************************************************/

        private Transform _LeftFoot;
        private Transform _RightFoot;

        /************************************************************************************************************************/

        /// <summary>Public property for a UI Toggle to enable or disable the IK.</summary>
        public bool ApplyAnimatorIK
        {
            get { return _Animancer.Layers[0].ApplyAnimatorIK; }
            set { _Animancer.Layers[0].ApplyAnimatorIK = value; }
        }

        /************************************************************************************************************************/
#if UNITY_2019_1_OR_NEWER
        /************************************************************************************************************************/

        private AnimatedProperty _LeftFootWeight;
        private AnimatedProperty _RightFootWeight;

        /************************************************************************************************************************/
            
        private void Awake()
        {
            _LeftFootWeight = new AnimatedProperty(_Animancer, AnimatedProperty.Type.Float, "LeftFootIK");
            _RightFootWeight = new AnimatedProperty(_Animancer, AnimatedProperty.Type.Float, "RightFootIK");

            _Animancer.Play(_Animation);

            // Tell Unity that OnAnimatorIK needs to be called every frame.
            ApplyAnimatorIK = true;

            // Get the foot bones.
            _LeftFoot = _Animancer.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            _RightFoot = _Animancer.Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        /************************************************************************************************************************/

        // Note that due to limitations in the Playables API, Unity will always call this method with layerIndex = 0.
#pragma warning disable IDE0060 // Remove unused parameter.
        private void OnAnimatorIK(int layerIndex)
#pragma warning restore IDE0060 // Remove unused parameter.
        {
            UpdateFootIK(_LeftFoot, AvatarIKGoal.LeftFoot, _LeftFootWeight, _Animancer.Animator.leftFeetBottomHeight);
            UpdateFootIK(_RightFoot, AvatarIKGoal.RightFoot, _RightFootWeight, _Animancer.Animator.rightFeetBottomHeight);
        }

        /************************************************************************************************************************/

        private void UpdateFootIK(Transform footTransform, AvatarIKGoal goal, float weight, float footBottomHeight)
        {
            var animator = _Animancer.Animator;
            animator.SetIKPositionWeight(goal, weight);
            animator.SetIKRotationWeight(goal, weight);

            if (weight == 0)
                return;

            // Get the local up direction of the foot.
            var rotation = animator.GetIKRotation(goal);
            var localUp = rotation * Vector3.up;

            var position = footTransform.position;
            position += localUp * _RaycastOriginY;

            var distance = _RaycastOriginY - _RaycastEndY;

            RaycastHit hit;
            if (Physics.Raycast(position, -localUp, out hit, distance))
            {
                // Use the hit point as the desired position.
                position = hit.point;
                position += localUp * footBottomHeight;
                animator.SetIKPosition(goal, position);

                // Use the hit normal to calculate the desired rotation.
                var rotAxis = Vector3.Cross(localUp, hit.normal);
                var angle = Vector3.Angle(localUp, hit.normal);
                rotation = Quaternion.AngleAxis(angle, rotAxis) * rotation;

                animator.SetIKRotation(goal, rotation);
            }
            else// Otherwise simply stretch the leg out to the end of the ray.
            {
                position += localUp * (footBottomHeight - distance);
                animator.SetIKPosition(goal, position);
            }
        }

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        private void Awake()
        {
#if UNITY_EDITOR
            Debug.LogError("This example requires the Animation Job system implemented in Unity 2019.1" +
                " in order to access the IK weight curves in the AnimationClip." +
                " See the documentation for more information: " + Strings.DocsURLs.UnevenGround);

            UnityEditor.EditorApplication.isPlaying = false;
            if (UnityEditor.EditorWindow.focusedWindow != null)
                UnityEditor.EditorWindow.focusedWindow.ShowNotification(
                    new GUIContent("Animation Jobs require Unity 2019.1+\nCheck the Console for details"));
#else
            Application.Quit();
#endif
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
