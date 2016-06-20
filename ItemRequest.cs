using System;

namespace DiabloInterfaceAPI
{
    public class ItemRequest
    {
        public enum Slot
        {
            Helm,
            Armor,
            Weapon,
            Shield,
            WeaponSwap,
            ShieldSwap,
            Gloves,
            Boots,
            Belt,
            Rings,
            Amulet
        }

        public string EquipmentSlot { get; private set; }

        public ItemRequest(Slot itemSlot)
        {
            EquipmentSlot = ItemSlotToString(itemSlot);
        }

        public static string ItemSlotToString(Slot itemSlot)
        {
            switch (itemSlot)
            {
                case Slot.Helm: return "helm";
                case Slot.Armor: return "armor";
                case Slot.Weapon: return "weapon";
                case Slot.Shield: return "shield";
                case Slot.WeaponSwap: return "weapon2";
                case Slot.ShieldSwap: return "shield2";
                case Slot.Gloves: return "gloves";
                case Slot.Boots: return "boots";
                case Slot.Belt: return "belt";
                case Slot.Rings: return "rings";
                case Slot.Amulet: return "amulet";

                default: throw new ArgumentException("itemSlot");
            }
        }
    }
}
