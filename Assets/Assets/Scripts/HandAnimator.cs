using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Component which reads input values and drives the thumbstick, trigger, and grip transforms
    /// to animate a controller model.
    /// </summary>
    public class ControllerAnimator : MonoBehaviour
    {

        [SerializeField]
        XRInputValueReader<float> m_GripInput = new XRInputValueReader<float>("Grip");

        Animator handAnim = null;
        public NearFarInteractor nearFarInteractor = null;
        void OnEnable()
        {

            handAnim = GetComponentInChildren<Animator>();

            m_GripInput?.EnableDirectActionIfModeUsed();

            
        }

        void OnDisable()
        {

            m_GripInput?.DisableDirectActionIfModeUsed();
        }

        void Update()
        {
           
            if (m_GripInput != null)
            {
                var gripVal = m_GripInput.ReadValue();

                handAnim.SetFloat("Trigger", gripVal);
            }
            CheckAlyxHover();
        }

        private void CheckAlyxHover()
        {
            if (nearFarInteractor == null || handAnim == null) return;

            if (nearFarInteractor.interactablesHovered.Count > 0)
            {
                var currentHover = nearFarInteractor.interactablesHovered[0];

                if (currentHover is AlyxGrabInteractable)
                {
                    handAnim.SetBool("Hovering", true);
                    return; 
                }
            }

            handAnim.SetBool("Hovering", false);
        }

    }
}

