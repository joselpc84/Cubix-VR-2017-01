/* InstantVR Head Movements
 * author: Pascal Serrarens
 * email: support@passervr.com
 * version: 3.7.1
 * date: January 29, 2017
 * 
 * - set default eye position to (0, 0.13, 0.13)
 */
using UnityEngine;

using IVR;

public static class HeadUtils {

    public static Vector3 GetNeckEyeDelta(InstantVR ivr) {
        Animator animator = ivr.characterTransform.GetComponent<Animator>();
        if (animator != null) {
            Transform neckBone = animator.GetBoneTransform(HumanBodyBones.Neck);
            Transform leftEyeBone = animator.GetBoneTransform(HumanBodyBones.LeftEye);
            Transform rightEyeBone = animator.GetBoneTransform(HumanBodyBones.RightEye);

            if (neckBone != null && leftEyeBone != null && rightEyeBone != null) {
                Vector3 centerEyePosition = (leftEyeBone.transform.position + rightEyeBone.transform.position) / 2;
                Vector3 worldNeckEyeDelta = (centerEyePosition - neckBone.position);
                Vector3 localNeckEyeDelta = ivr.headTarget.InverseTransformDirection(worldNeckEyeDelta);
                return localNeckEyeDelta;
            }
        }
        return new Vector3(0, 0.13F, 0.13F);
    }

    public static Vector3 GetHeadEyeDelta(InstantVR ivr) {
        Animator animator = ivr.characterTransform.GetComponent<Animator>();
        if (animator != null) {
            Transform neckBone = animator.GetBoneTransform(HumanBodyBones.Neck);
            Transform leftEyeBone = animator.GetBoneTransform(HumanBodyBones.LeftEye);
            Transform rightEyeBone = animator.GetBoneTransform(HumanBodyBones.RightEye);
            if (neckBone != null && leftEyeBone != null && rightEyeBone != null) {
                Vector3 centerEyePosition = (leftEyeBone.position + rightEyeBone.position) / 2;
                Vector3 worldHeadEyeDelta = (centerEyePosition - neckBone.position);
                Vector3 localHeadEyeDelta = ivr.headTarget.InverseTransformDirection(worldHeadEyeDelta);
                return localHeadEyeDelta;
            }
        }

        return new Vector3(0, 0.13F, 0.13F); ;
    }

    public static Vector3 CalculateNeckPosition(Vector3 eyePosition, Quaternion eyeRotation, Vector3 eye2neck) {
        Vector3 neckPosition = eyePosition + eyeRotation * eye2neck;
        return neckPosition;
    }

}
