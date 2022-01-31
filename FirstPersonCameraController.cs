using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity_FirstPersonController
{
    /// <summary>
    /// Put this on the camera.
    /// </summary>
    public class FirstPersonCameraController : MonoBehaviour
    {
        [Tooltip("If this is true, the parent is used for horizontal rotations")]
        public bool rotateParent = true;
        public float sensitivity = 1f;

        [Space(20)]
        [SerializeField, ReadOnly] private float currentVertical;
        [SerializeField, ReadOnly] private float currentHorizontal;
        
        [Space(20)]
        public float minVertical = 0;
        public float maxVertical = 0;
        
        [Space(20)]
        public bool invertVertical = false;
        public bool invertHorizontal = false;

        private void Start() {
            // Get initial values
            Vector3 eulerAngles = this.transform.eulerAngles;
            
            this.currentVertical = eulerAngles.x;
            this.currentHorizontal = eulerAngles.y;
        }

        private void FixedUpdate() {
            // Calculate mouse inputs
            this.currentVertical -= Input.GetAxis("Mouse Y") * sensitivity * (this.invertVertical ? -1 : 1);
            this.currentHorizontal += Input.GetAxis("Mouse X") * sensitivity * (this.invertHorizontal ? -1 : 1);
            
            // Enforce vertical limits
            if (this.currentVertical > this.maxVertical) this.currentVertical = this.maxVertical;
            else if (this.currentVertical < this.minVertical) this.currentVertical = this.minVertical;

            // Apply rotations to camera/parent
            if (rotateParent && this.transform.parent != null) {
                this.transform.localRotation = Quaternion.Euler(this.currentVertical, 0, 0);
                this.transform.parent.localRotation = Quaternion.Euler(0, this.currentHorizontal, 0);
                
            } else {
                this.transform.localRotation = Quaternion.Euler(this.currentVertical, this.currentHorizontal, 0);
            }
        }
    }
}