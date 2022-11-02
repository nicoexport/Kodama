using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable {
    [CreateAssetMenu(fileName = "new Device Map", menuName = "DeviceMap")]
    public class DeviceMapSo : ScriptableObject {
        public List<string> DeviceNames;
        public List<GeneralDeviceType> DeviceTypes;
        public Dictionary<string, GeneralDeviceType> DeviceMappings;
    }

    [Serializable]
    public enum GeneralDeviceType {
        Pc,
        Mac,
        DualShock,
        Xbox,
        Nintendo
    }
}