/* InstantVR Unity VR extension
 * Copyright (c) 2016 by Passer VR
 * author: Pascal Serrarens
 * email: support@passervr.com
 * version: 3.7.0
 * date: January 17, 2017
 * 
 * - Updated HelpURL
 */

using UnityEngine;

namespace IVR {

#if UNITY_ANDROID
    [HelpURL("http://passervr.com/documentation/instantvr-extensions/gear-vr/")]
#else
    [HelpURL("http://passervr.com/documentation/instantvr-extensions/oculus-rift/")]
#endif
    public class IVR_UnityVR : IVR_Extension {
    }
}