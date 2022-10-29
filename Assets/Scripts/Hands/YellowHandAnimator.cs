using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;

public class YellowHandAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float debug;
    [SerializeField] private ActionBasedController controller;

    private void Update()
    {
        debug = controller.selectAction.action.ReadValue<float>();
        animator.SetFloat("Blend", controller.selectAction.action.ReadValue<float>());
    }
}
