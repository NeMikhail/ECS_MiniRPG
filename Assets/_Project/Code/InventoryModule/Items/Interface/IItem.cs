using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    public interface IItem
    {
        string Id { get; }
        string Title { get; }
        Sprite Icon { get; }
        int MaxStack { get; }
    }
}
