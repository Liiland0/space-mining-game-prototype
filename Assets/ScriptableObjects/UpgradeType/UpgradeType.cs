using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeType", menuName = "Scriptable Objects/UpgradeType")]
public class UpgradeType : ScriptableObject
{
    public UpgradeTypeEnum upgradeType;
    public UpgradeMathEnum upgradeMath;
    public float upgradeValue;
    public enum UpgradeTypeEnum
    {
        DrillPower,
        DrillSpeed,
        MaxSpeed,
        Thruster,
        FuelEfficiency
    }

    public enum UpgradeMathEnum
    {
        Additive,
        Multiplicative
    }
}