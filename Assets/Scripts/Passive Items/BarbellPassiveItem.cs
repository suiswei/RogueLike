using UnityEngine;

public class BarbellPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMight *= 1 + passiveItemData.Multiplier / 100f;
    }
}
