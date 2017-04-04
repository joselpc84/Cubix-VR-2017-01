/* InstantVR Hand Movements Base
 * author: Pascal Serrarnes
 * email: support@passervr.com
 * version: 3.7.0
 * date: January 15, 2017
 * 
 * - Added stretchlessTarget
 */

using UnityEngine;
using System.Collections;

namespace IVR {

    public class IVR_HandMovementsBase : IVR_Movements {
        [HideInInspector]
        public IVR_HandController selectedController;
        [HideInInspector]
        public Transform stretchlessTarget;

        public Vector3 storedCOM;
        public Vector3 grabLocation;
        public GameObject grabbedObject = null;

        public virtual void UpdateAnimation() { }
        public virtual void MoveTo(IVR_HandController handController, Vector3 position, Quaternion rotation) { }
        public virtual IEnumerator LetGoAnimation(IVR_HandController handController) {
            yield return null;
        }
    }
}