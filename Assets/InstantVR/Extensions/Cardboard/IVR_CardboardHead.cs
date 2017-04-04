/* InstantVR Cardboard head controller
 * author: Pascal Serrarens
 * email: support@passervr.com
 * version: 3.7.0
 * date: January 15, 2017
 *
 * - Updated to Google VR SDK v1.1.0
 */

using UnityEngine;

namespace IVR {

    public class IVR_CardboardHead : IVR_Controller {
#if UNITY_ANDROID
        private GvrViewer cardboard;
#endif
        //[HideInInspector]
        public GameObject cameraRoot;
        //[HideInInspector]
        private Transform cameraTransform;

        [HideInInspector]
        private Vector3 neck2eyes;

        [HideInInspector]
        private Vector3 cardboardStartPoint;
        [HideInInspector]
        private Vector3 baseStartPoint;
        [HideInInspector]
        private Quaternion cardboardStartRotation;
        [HideInInspector]
        private float baseStartAngle;

        [HideInInspector]
        private Vector3 localNeckOffset;

        [HideInInspector]
        private ControllerInput controller;
#if INSTANTVR_ADVANCED
        [HideInInspector]
        private IVR_VicoVRHead vicoVRHead;
#endif

        void Start() {
            // This dummy code is here to ensure the checkbox is present in editor
        }

        public override void StartController(InstantVR ivr) {
            base.StartController(ivr);
#if UNITY_ANDROID
            if (extension == null)
                extension = ivr.GetComponent<IVR_Cardboard>();

            Camera camera = CheckCamera();
            if (camera != null) {
                cameraTransform = camera.transform;
                neck2eyes = HeadUtils.GetNeckEyeDelta(ivr);

                cameraTransform.gameObject.SetActive(false);

                GvrViewer cardboardPrefab = Resources.Load<GvrViewer>("CardboardPrefab");
                cardboard = Instantiate(cardboardPrefab);
                if (cardboard == null)
                    Debug.LogError("Could not instantiate Cardboard. CardboardCameraRig is missing?");
                else {
                    cameraRoot = cardboard.gameObject;
                    cameraRoot.transform.parent = ivr.transform;

                    cameraRoot.transform.position = transform.position;
                    cameraRoot.transform.rotation = ivr.transform.rotation;

                    GvrHead gvrHead = cardboard.GetComponentInChildren<GvrHead>();
                    if (gvrHead != null) {
                        camera = Camera.main;
                        cameraTransform = Camera.main.transform;
                    }
                }
            
            }
            controller = Controllers.GetController(0);
#if INSTANTVR_ADVANCED
            vicoVRHead = GetComponent<IVR_VicoVRHead>();
#endif
#endif
        }

        private Camera CheckCamera() {
            Camera camera = transform.GetComponentInChildren<Camera>();
            if (camera == null) {
                GameObject cameraObj = new GameObject("First Person Camera");
                cameraObj.transform.SetParent(transform);
                camera = cameraObj.AddComponent<Camera>();
                camera.nearClipPlane = 0.05F;
            }

            return camera;
        }

        public override void UpdateController() {
#if UNITY_ANDROID
            if (enabled) {
                SetHeadTargets();
            } else
                tracking = false;
#endif
        }

        private void SetHeadTargets() {
#if UNITY_ANDROID
            if (cardboard.HeadPose != null) {
                tracking = true;
                transform.rotation = cameraTransform.rotation;
                Vector3 target2camera = transform.position + neck2eyes - cameraTransform.position;
                cameraRoot.transform.position += target2camera; 
#if INSTANTVR_ADVANCED
                if (vicoVRHead != null && vicoVRHead.isTracking()) {
                    transform.position = vicoVRHead.position;
                }
#endif
            }
#endif
        }

        private void UpdateInput() {
            controller.right.buttons[0] = GvrViewer.Instance.Triggered;
        }
    }
}