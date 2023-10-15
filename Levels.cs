using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Towers;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Utils;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace Kairo
{
    public class Levels
    {
        public class Level2 : ModHeroLevel<KairoHero>
        {
            public override string Description => "All air Units on map gain 20% Attack Speed buff";

            public override int Level => 2;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(new RateSupportModel("AirAttackSpeed", 0.8f, true, "AirAttackSpeed", true, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
              {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
              }), "AirAttackSpeed", null));
            }
        }
        public class Level3 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Stronger Darts Ability: Projectiles now seek, gain more pierce and can pop every Bloon Type but cancel all other attacks.";

            public override int Level => 3;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var abilityModel = Game.instance.model.GetTowerFromId("Ezili 3").GetAbility().Duplicate();
                abilityModel.RemoveBehavior<ActivateAttackModel>();
                abilityModel.name = "AbilityModel_StrongerDartsAbility";
                abilityModel.displayName = "Stronger Darts Ability";
                abilityModel.description = "Projectile now seek, gain more pierce and can pop every Bloon Type but cancel all other attacks.";
                abilityModel.icon = GetSpriteReference("Ability1Icon");
                abilityModel.cooldown = 45f;
                abilityModel.RemoveBehaviors<CreateSoundOnAbilityModel>();
                abilityModel.RemoveBehaviors<CreateEffectOnAbilityModel>();
                abilityModel.AddBehavior(Game.instance.model.GetTowerFromId("Quincy 3").GetAbility().GetBehavior<CreateEffectOnAbilityModel>());
                abilityModel.AddBehavior(Game.instance.model.GetTowerFromId("Quincy 3").GetAbility().GetBehavior<CreateSoundOnAbilityModel>());
                abilityModel.GetBehavior<CreateEffectOnAbilityModel>().effectModel.lifespan = 10;

                var activateAttackModel = new ActivateAttackModel("ActivateAttackModel_StrongerDartsAbility", 10, true, new Il2CppReferenceArray<AttackModel>(1), false, true, false, false, false);
                abilityModel.AddBehavior(activateAttackModel);

                var attackModel = activateAttackModel.attacks[0] = attackairunitmodel.Duplicate();

                activateAttackModel.AddChildDependant(attackModel);
            
                var weaponModel = attackModel.weapons[0];
                weaponModel.rate *= 0.65f;
                var projectileModel = weaponModel.projectile;
                projectileModel.pierce *= 1.35f;
                projectileModel.GetBehavior<TravelStraitModel>().lifespan *= 1.75f;
                projectileModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-003").GetBehavior<AttackAirUnitModel>().weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate());
                projectileModel.display = Game.instance.model.GetTowerFromId("MonkeyAce-003").GetBehavior<AttackAirUnitModel>().weapons[0].projectile.display;
                projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                if (towerModel.tiers.Max() > 3 && towerModel.tiers.Max() < 9)
                {
                    projectileModel.GetDamageModel().damage *= 2;
                }
                if (towerModel.tiers.Max() > 8)
                {
                    projectileModel.GetDamageModel().damage *= 3;
                }
                if (towerModel.tiers.Max() > 4)
                {
                    projectileModel.pierce *= 1.15f;
                }
                if (towerModel.tiers.Max() > 10)
                {
                    projectileModel.pierce *= 1.25f;
                    projectileModel.GetDamageModel().damage *= 2;
                }
                if (towerModel.tiers.Max() > 11)
                {
                    var ex = Game.instance.model.GetTowerFromId("BombShooter").GetDescendant<CreateProjectileOnContactModel>().Duplicate();
                    var fx = Game.instance.model.GetTowerFromId("BombShooter").GetDescendant<CreateEffectOnContactModel>().Duplicate();
                    var so = Game.instance.model.GetTowerFromId("BombShooter").GetDescendant<CreateSoundOnProjectileCollisionModel>().Duplicate();
                    ex.projectile.GetDamageModel().damage = 3;
                    projectileModel.AddBehavior(ex);
                    projectileModel.AddBehavior(fx);
                    projectileModel.AddBehavior(so);
                }
                if (towerModel.tiers.Max() > 12)
                {
                    projectileModel.pierce *= 1.25f;
                    projectileModel.GetDamageModel().damage *= 1.75f;
                }
                if (towerModel.tiers.Max() > 13)
                {
                    weaponModel.rate *= 0.66f;
                }
                if (towerModel.tiers.Max() > 14)
                {
                 projectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 6;
                }
                if (towerModel.tiers.Max() > 14)
                {
                    projectileModel.GetDamageModel().damage *= 1.45f;
                }
                if (towerModel.tiers.Max() > 15)
                {
                    projectileModel.GetDamageModel().damage *= 1.25f;
                }
                if (towerModel.tiers.Max() > 16)
                {
                    projectileModel.GetDamageModel().damage *= 5f;
                    projectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 16;
                }
                if (towerModel.tiers.Max() > 17)
                {
                    weaponModel.rate *= 0.8f;
                }
                if (towerModel.tiers.Max() > 18)
                {
                    projectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 38;
                }
                if (towerModel.tiers.Max() > 19)
                {
                    projectileModel.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 65;
                    projectileModel.GetDamageModel().damage *= 3;
                    projectileModel.pierce *= 2.5f;
                    weaponModel.rate *= 0.85f;
                }
                towerModel.AddBehavior(abilityModel);
              
            }
        }
        public class Level4 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Add an additional machine gun. The Stronger Darts Ability still shoot 1 projectile but deal more damage";

            public override int Level => 4;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = attackairunitmodel.Duplicate();
                machineGun2.weapons[0].ejectY = -5;

                weapons.ejectY = 5;
                towerModel.AddBehavior(machineGun2);
            }
        }
        public class Level5 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Gain more pierce";

            public override int Level => 5;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                machineGun2.weapons[0].projectile.pierce += 2;
                projectile.pierce += 2;
               
            }
        }
        public class Level6 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Air units gain 1 extra Damage and 30% Attack Speed buff";

            public override int Level => 6;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                towerModel.RemoveBehavior<RateSupportModel>();
                towerModel.AddBehavior(new RateSupportModel("AirAttackSpeed", 0.85f, true, "AirAttackSpeed", true, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
             }), "AirAttackSpeed", null));
                towerModel.AddBehavior(new DamageSupportModel("AirDamage",false, 1f,  "AirDamage", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
            }), true, false, 0));
            }
        }
        public class Level7 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Deal more damage to ceramic Bloons.";

            public override int Level => 7;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];

            
                projectile.AddBehavior(new DamageModifierForTagModel("ceramic", "Ceramic", 1, 3, false, true));
                machineGun2.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("ceramic", "Ceramic", 1, 3, false, true));
            }
        }
        public class Level8 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Not even Camo Bloons can be stopped.";

            public override int Level =>8;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            }
        }
        public class Level9 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Add a triple gun and the Stronger Darts Ability deal even more damage";

            public override int Level => 9;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
               
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = attackairunitmodel.Duplicate();
                attackairunitmodel.weapons[0].ejectY = 7;
                machineGun3.weapons[0].ejectY = 0;
                machineGun2.weapons[0].ejectY = -7;
                towerModel.AddBehavior(machineGun3);

            }
        }
        public class Level10 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Highly Tech Darts Ability: Send a burst of hight tech dart that deal huge damage but low pierce.";

            public override int Level => 10;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = attackairunitmodel.Duplicate();
                var abilityModel = Game.instance.model.GetTowerFromId("Ezili 10").GetAbility(2).Duplicate();
                abilityModel.name = "AbilityModel_HightTechDartsAbility";
                abilityModel.displayName = "Hight Tech Darts Ability";
                abilityModel.description = "Send a burst of hight tech dart that deal huge damage";
                abilityModel.cooldown = 30;
                abilityModel.icon = GetSpriteReference("Ability2Icon");
                abilityModel.livesCost = 0;
                abilityModel.RemoveBehavior<ActivateAttackModel>();
                abilityModel.RemoveBehaviors<CreateSoundOnAbilityModel>();
                abilityModel.RemoveBehaviors<CreateEffectOnAbilityModel>();
                abilityModel.AddBehavior(Game.instance.model.GetTowerFromId("Ezili 10").GetAbility(2).GetBehavior<CreateEffectOnAbilityModel>());
                abilityModel.AddBehavior(Game.instance.model.GetTowerFromId("Ezili 10").GetAbility(2).GetBehavior<CreateSoundOnAbilityModel>());

                towerModel.AddBehavior(abilityModel);


                var activateAttackModel = new ActivateAttackModel("ActivateAttackModel_HightTechDartsAbility", 0f, true, new Il2CppReferenceArray<AttackModel>(1), false, false, false, false, true);
                abilityModel.AddBehavior(activateAttackModel);

                var attackModel = activateAttackModel.attacks[0] = Game.instance.model.GetTower(TowerType.MonkeyAce).GetBehavior<AttackAirUnitModel>().Duplicate();

                attackModel.weapons[0].projectile.GetDamageModel().damage = 28;
                attackModel.weapons[0].projectile.pierce = 6;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 2;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 2;
                if(towerModel.tiers.Max() > 12)
                {
                    attackModel.weapons[0].projectile.pierce *= 2.5f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage *= 2;
                    attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmission_", 12, 0, 360, null, false, false);
                }
                if (towerModel.tiers.Max() > 16)
                {
                    attackModel.weapons[0].projectile.pierce *= 3f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage *= 3;
                    attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmission_", 20, 0, 360, null, false, false);
                }
                if (towerModel.tiers.Max() > 19)
                {
                    attackModel.weapons[0].projectile.pierce *= 2.5f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage *= 3.25f;
                    attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmission_", 40, 0, 360, null, false, false);
                }
                attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<HightTechDarts>() };
                attackModel.weapons[0].projectile.scale *= 2;
                activateAttackModel.AddChildDependant(attackModel);
            }
        }
        public class HightTechDarts : ModDisplay
        {

            public override string BaseDisplay => Generic2dDisplay;
            public override float Scale => 1f;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "HightTechDarts");
            }
        }
        public class Level11 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Increase pierce and pop an extra Bloon layer.";

            public override int Level => 11;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];

                projectile.pierce += 3;
                projectile.GetDamageModel().damage += 1;
                machineGun2.weapons[0].projectile.pierce += 3;
                machineGun2.weapons[0].projectile.GetDamageModel().damage += 1;
                machineGun3.weapons[0].projectile.pierce += 3;
                machineGun3.weapons[0].projectile.GetDamageModel().damage += 1;

            }
        }
        public class Level12 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Drop a hight damage stunt bomb once in a while. Stronger Darts Ability projectile now explode on contact.";

            public override int Level => 12;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var bombbehavior = Game.instance.model.GetTowerFromId("MonkeyAce-030").GetAttackModel(1).Duplicate();

                bombbehavior.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var bomb = bombbehavior.weapons[0];
                bomb.projectile.AddBehavior(new DamageModel("dummy", 0, 0, false, false, false, BloonProperties.None, BloonProperties.None));
                bomb.projectile.GetDamageModel().CapDamage(0);
                bomb.RemoveBehavior<CheckAirUnitOverTrackModel>();
                var emissionovertimemodel = Game.instance.model.GetTowerFromId("MonkeyAce-030").GetAttackModel(1).weapons[0].emission.Duplicate();
                bomb.emission = new SingleEmmisionTowardsTargetModel("targetbloons", null, 0);
                bomb.rate = 1.5f;
                bomb.projectile.GetBehavior<FallToGroundModel>().timeToTake = 1.8f;
                var explosion = bomb.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                explosion.GetBehavior<DamageModel>().damage = 20;
                var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModel>().Duplicate();
                var stuntag = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModifierForTagModel>().Duplicate();
                explosion.collisionPasses = new[] { -1, 0 };
                var moabstun = Game.instance.model.GetTowerFromId("BombShooter-500").GetDescendant<SlowModel>().Duplicate();
                moabstun.lifespan = 0.5f;
                explosion.GetBehavior<DamageModel>().maxDamage = 999999;
                explosion.GetBehavior<DamageModel>().CapDamage(999999);
                explosion.AddBehavior(stun);
                explosion.AddBehavior(stuntag);
                explosion.AddBehavior(moabstun);
                towerModel.AddBehavior(bombbehavior);
            }
        }
        public class Level13 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Highly Tech Darts Ability improved and add a laser gun. Stronger Darts Ability enhanced.";

            public override int Level => 13;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var laserArray = attackairunitmodel.Duplicate();
         

                var balloflight = Game.instance.model.GetTowerFromId("BallOfLightTower").Duplicate();
                balloflight.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                balloflight.GetDescendant<LineProjectileEmissionModel>().dontUseTowerPosition = true;
                //balloflight.GetDescendant<LineProjectileEmissionModel>().displayPath.assetPath = new Assets.Scripts.Utils.PrefabReference() { guidRef = "d48587764ad63c84ea37e82f58bd05ad" };
                balloflight.GetAttackModel().weapons[0].AddBehavior(new FireFromAirUnitModel("fire"));

                var balloflightweapon = balloflight.GetAttackModel().weapons[0];
                balloflightweapon.projectile.GetDamageModel().damage = 2;
                balloflightweapon.projectile.pierce = 12;
                balloflightweapon.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                balloflightweapon.projectile.CapPierce(999999);
                balloflightweapon.projectile.maxPierce = 999999;
                balloflightweapon.projectile.GetDamageModel().maxDamage = 999999;
                balloflightweapon.projectile.GetDamageModel().CapDamage(999999);

                laserArray.SetWeapon(balloflightweapon);

                towerModel.AddBehavior(laserArray);
            }
        }
        public class Level14 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Air units shoot 50% faster, deal 2 more damage and 2 extra pierce.";

            public override int Level => 14;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];
                towerModel.RemoveBehavior<RateSupportModel>();
                towerModel.RemoveBehavior<DamageSupportModel>();
                towerModel.AddBehavior(new RateSupportModel("AirAttackSpeed", 0.75f, true, "AirAttackSpeed", true, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
             }), "AirAttackSpeed", null));
                towerModel.AddBehavior(new DamageSupportModel("AirDamage", false, 2f, "AirDamage", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
            }), true, false, 0));
                towerModel.AddBehavior(new PierceSupportModel("AirPierce", false, 2f, "AirPierce", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
           {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
           }), true, "AirPierce", ""));

            }
        }
        public class Level15 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Bombs stunt even more and deal more damage.";

            public override int Level => 15;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];

                var bomb = bombbehavior.weapons[0];
                var explosion = bomb.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                explosion.GetBehaviors<SlowModel>().ForEach(model => model.lifespan *= 2);
                explosion.GetDamageModel().damage = 40;
            }
        }
        public class Level16 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Add a fourth machine gun and laser gun improved.";

            public override int Level => 16;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var machineGun4 = attackairunitmodel.Duplicate();
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];

                laserArray.weapons[0].rate *= 0.85f;
                laserArray.weapons[0].projectile.GetDamageModel().damage += 2;
                laserArray.weapons[0].projectile.pierce += 10;
                machineGun3.weapons[0].ejectY =1.75f;
                machineGun4.weapons[0].ejectY = -1.75f;
                towerModel.AddBehavior(machineGun4);
            }
        }
        public class Level17 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Overall weapons enhanced";

            public override int Level => 17;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var machineGun4 = attackairunitmodel.Duplicate();
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];

                foreach (var weapon in towerModel.GetWeapons().ToList())
                {
                    weapon.rate *= 0.80f;
                    weapon.projectile.pierce *= 1.25f;
                    weapon.projectile.GetDamageModel().damage *= 1.25f;
                }

            }
        }
        public class Level18 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Add another laser gun.";

            public override int Level => 18;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var machineGun4 = attackairunitmodel.Duplicate();
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var laserArray2 = laserArray.Duplicate();
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];


                laserArray.weapons[0].ejectY = 7;
                laserArray2.weapons[0].ejectY = -7;

                towerModel.AddBehavior(laserArray2);
            }
        }
        public class Level19 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Bomb become nuke and destroy everything";

            public override int Level => 19;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var machineGun4 = attackairunitmodel.Duplicate();
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var laserArray2 = laserArray.Duplicate();
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];

                var bomb = bombbehavior.weapons[0];
                bomb.rate *= 0.8f;
                var explosion = bomb.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                explosion.GetBehaviors<SlowModel>().ForEach(model => model.lifespan *= 1.5f);
                explosion.GetDamageModel().damage = 150;
                explosion.radius *= 1.5f;
                bomb.projectile.GetBehavior<CreateEffectOnExhaustFractionModel>().effectModel.scale *= 1.5f;
            }
        }
        public class Level20 : ModHeroLevel<KairoHero>
        {
            public override string Description => "Become the ultimate DESTROYER OF WORLD. Every air units are way stronger";

            public override int Level => 20;

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var tower = towerModel.GetBehavior<AirUnitModel>();
                var attackairunitmodel = towerModel.GetBehaviors<AttackAirUnitModel>()[0];
                var weapons = attackairunitmodel.weapons[0];
                var projectile = weapons.projectile;
                var machineGun2 = towerModel.GetBehaviors<AttackAirUnitModel>()[1];
                var machineGun3 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                var machineGun5 = attackairunitmodel.Duplicate();
                var laserArray = towerModel.GetBehaviors<AttackAirUnitModel>()[4];
                var laserArray3 = laserArray.Duplicate();
                var bombbehavior = towerModel.GetBehaviors<AttackAirUnitModel>()[3];

                var bomb = bombbehavior.weapons[0];
                var explosion = bomb.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                explosion.GetBehaviors<SlowModel>().ForEach(model => model.lifespan *= 1.75f);
                explosion.GetDamageModel().damage = 500;
                explosion.radius *= 1.15f;
                bomb.projectile.GetBehavior<CreateEffectOnExhaustFractionModel>().effectModel.scale *= 1.15f;
                foreach (var weapon in towerModel.GetWeapons().ToList())
                {
                    weapon.rate *= 0.8f;
                    weapon.projectile.pierce *= 2f;
                    weapon.projectile.GetDamageModel().damage *= 1.75f;
                }
                machineGun5.weapons[0].ejectY = 0;
                towerModel.AddBehavior(machineGun5);
                laserArray3.weapons[0].ejectY = 0;
                towerModel.AddBehavior(laserArray3);
                towerModel.RemoveBehavior<RateSupportModel>();
                towerModel.RemoveBehavior<DamageSupportModel>();
                towerModel.RemoveBehavior<PierceSupportModel>();
                towerModel.AddBehavior(new RateSupportModel("AirAttackSpeed", 0.5f, true, "AirAttackSpeed", true, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
             }), "AirAttackSpeed", null));
                towerModel.AddBehavior(new DamageSupportModel("AirDamage", false, 3f, "AirDamage", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
            }), true, false, 0));
                towerModel.AddBehavior(new PierceSupportModel("AirPierce", false, 4f, "AirPierce", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
           {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeyAce, TowerType.HeliPilot }))
           }), true, "AirPierce", ""));
            }
        }
    }
}
