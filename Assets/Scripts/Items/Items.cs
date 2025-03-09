using System;
using System.Collections.Generic;
using Items.Items;
using Items.Items.ArmorItems;
using Items.Items.AxeItems;
using Items.Items.BowItems;
using Items.Items.CombatItems;

namespace Items {
    public class ItemList {
        /*
         * Astrus
         * Boat
         * BottleOfWater
         * Coal
         * Domilitant
         * Extric
         * Fire
         * Fireplace
         * Firestone
         * Glomtom
         * InvisibilityPotion
         * Iron
         * Liana
         * SpecialFlower
         * Stick
         * Stone
         * Torch
         * Wood
         * ExtricARmor
         * IronARmor
         * Lvl1Armor
         * Lvl2Armor
         * Lvl3Armor
         * IronAxe
         * StoneAxe
         * FireGlomtomBow
         * GlomtomBow
         * IronBow
         * StoneBow
         * GlomtomSword
         * IronSword
         * StoneSword
         * IronPickaxe
         * StonePickaxe
         */

        public static List<(string, Type)> items = new() {
            ("astrus", typeof(Astrus)),
            ("boat", typeof(Boat)),
            ("bottleOfWater", typeof(BottleOfWater)),
            ("coal", typeof(Coal)),
            ("domilitant", typeof(Domilitant)),
            ("extric", typeof(Extric)),
            ("fire", typeof(Fire)),
            ("fireplace", typeof(Fireplace)),
            ("firestone", typeof(Firestone)),
            ("glomtom", typeof(Glomtom)),
            ("invisibilityPotion", typeof(InvisibilityPotion)),
            ("iron", typeof(Iron)),
            ("liana", typeof(Liana)),
            ("specialFlower", typeof(SpecialFlower)),
            ("stick", typeof(Stick)),
            ("stone", typeof(Stone)),
            ("torch", typeof(Torch)),
            ("wood", typeof(Wood)),
            ("extricArmor", typeof(ExtricArmor)),
            ("ironArmor", typeof(IronArmor)),
            ("lvl1Armor", typeof(Lvl1Amour)),
            ("lvl2Armor", typeof(Lvl2Amour)),
            ("lvl3Armor", typeof(Lvl3Amour)),
            ("ironAxe", typeof(IronAxe)),
            ("stoneAxe", typeof(StoneAxe)),
            ("fireGlomtomBow", typeof(FireGlomtomBow)),
            ("glomtomBow", typeof(GlomtomBow)),
            ("ironBow", typeof(IronBow)),
            ("stoneBow", typeof(StoneBow)),
            ("glomtomSword", typeof(GlomtomSword)),
            ("ironSword", typeof(IronSword)),
            ("stoneSword", typeof(StoneSword)),
            ("ironPickaxe", typeof(IronPickaxe)),
            ("stonePickaxe", typeof(StonePickaxe))
        };
    }
}