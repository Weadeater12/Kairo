using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using MelonLoader;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity;
using Il2CppNinjaKiwi.LiNK;

using BTD_Mod_Helper.Api.Towers;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Utils;
using Il2CppSystem.IO;
using MelonLoader;
using Octokit;
using static Il2CppTMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Il2CppAssets.Scripts.Data.Quests;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons.Behaviors;
using static Il2CppSystem.TypeIdentifiers;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity.Display.Animation;

namespace Kairo
{
   
    public class KairoHero : ModHero
    {
        
        public override string BaseTower => TowerType.MonkeyAce;

        public override int Cost => 1800;

        public override string DisplayName => "Kairo";
        public override string Title => "Air Striker";
        public override string Level1Description => "Shoot 1 dart at a fast rate with low Popping Power.";

        public override string Description =>
            "Commands all air Units and strike most Bloons in his path";
        public override string NameStyle => TowerType.Gwendolin; // Yellow colored
        public override string BackgroundStyle => TowerType.Etienne; // Yellow colored
        public override string GlowStyle => TowerType.StrikerJones; // Yellow colored

        public override string Portrait => "Portrait";
        public override string Icon => "Portrait";
        public override string Square => "Square";
        public override string Button => "Button";
        public override int MaxLevel => 20;
        public override float XpRatio => 1.0f;

        public override int Abilities => 2;

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var tower = towerModel.GetBehavior<AirUnitModel>();
            var attackairunitmodel = towerModel.GetBehavior<AttackAirUnitModel>();
            var weapons = attackairunitmodel.weapons[0];
            var projectile = weapons.projectile;
            weapons.emission = Game.instance.model.GetTowerFromId("MonkeyAce-004").GetBehavior<AttackAirUnitModel>().weapons[0].emission;
            projectile.GetBehavior<TravelStraitModel>().speed *= 1.75f;
            projectile.pierce = 4;
            projectile.GetDamageModel().damage = 1;
            weapons.rate = 0.25f;
            tower.behaviors[0].Cast<PathMovementModel>().speed *= 0.75f;
            tower.behaviors[0].Cast<PathMovementModel>().rotation *= 0.75f;
            attackairunitmodel.fireWithoutTarget = false;
            attackairunitmodel.range = 999999;
  
        }
    }
}
