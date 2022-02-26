Accelerated User Reasoning: Operations, Research, Analysis for Cross Reality
(auroraXR)

auroraXR Unity3D API

Version: 2.2.0.0
Released: 2021-07-26

Developers: Stormfish Scientific Corporation (1) and DEVCOM Army Research Lab (2)

Authors: Theron T. Trout (1), Mark Dennison Jr. (2), Naththan Hahn (2), and Tiffany Raber (2)

https://www.stormfish.io
http://arl.army.mil/

Copyright (C) 2019, 2020, 2021 by Stormfish Scientific Corporation

All Rights Reserved

See LICENSE file for Terms of Use.

THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE
LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR
OTHER PARTIES PROVIDE THE PROGRAM “AS IS” WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE
ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU.
SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY
SERVICING, REPAIR OR CORRECTION. YOU ARE SOLELY RESPONSIBLE FOR DETERMINING
THE APPROPRIATENESS OF USING OR REDISTRIBUTING THE WORK AND ASSUME ANY
RISKS ASSOCIATED WITH YOUR EXERCISE OF PERMISSIONS UNDER THIS LICENSE.

----------------------------------------------------------------------------

For the auroraXR package to work appropriately, the following must be done:

--In Project Settings -> Player, add "USE_UNITYXR" to Scripting Define Symbols

--Install the XR Plugin Management Package (editor restart may be required)
--Download and install the Windows Mixed Reality Feature Tool (https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool) and enable the following features:
----Mixed Reality Toolkit Extensions
----Mixed Reality Toolkit Foundation
----Mixed Reality Standard Assets
----Mixed Reality OpenXR Plugin
----Microsoft Spatializer
--Under Project Settings, XR Plug-in Management, OpenXR sub-item
----for the PC Tab set Render Mode to Multi Pass, Depth Mode to 24 bit and enable Hand Tracking, Mixed Reality Features, and Motion Controller Model
----for the Windows Tab set Render Mode to Single Pass, Depth Mode to 16 bit and enable Hand Tracking, Mixed Reality Features, and Motion Controller Model
----for the Android Tab set Render Mode to Single Pass, Depth Mode to 16 bit, and enable Oculus Quest Support

--In the Manager Scene, change the following in the inspector on the MixedRealityToolkit GameObject:
----In the Camera submenu, change CameraSystemType to AURORA_MixedRealityCameraProfile
----In the Input submenu, change InputSystemType to AURORA_OpenXRInputSystemProfile
----In the Spatial Awareness submenu, change SpatialAwarenessSystemType to AURORA_SpatialAwarenessProfile
----In the Extensions submenu, change the profile to AURORA_ExtensionsProfile

--Set Layer 10 to "VR"
--Set Layer 13 to "NoPlayerCollide"
--Set Layer 14 to "Player"
--Set Layer 18 to "Terrain"
--Set Layer 20 to "Grabbable"