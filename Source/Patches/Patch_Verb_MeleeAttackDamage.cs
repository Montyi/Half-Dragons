using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace HalfDragons.Patch1
{
    [HarmonyPatch(typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget")]
    class Patch_ApplyMeleeDamageToTarget
    {
        [HarmonyPostfix]
        private static void AddDragonRageHediff(Verb_MeleeAttackDamage __instance)
        {
            try
            {
                if (!(__instance.Caster is Pawn caster))
                {
                    return;
                }
                if (!caster.IsEligableForDragonRage())
                {
                    //Log.Message("Not a half dragon");
                    return;
                }
                if (caster?.equipment?.Primary?.def?.IsMeleeWeapon == false)
                {
                    //Log.Message("Not a melee weapon");
                    return;
                }
                caster.IncreaseDragonRage();
            }
            catch (Exception e)
            {
                Log.Warning("Half-Dragons: Something went wrong " + e);
                return;
            }
        }
    }

    [HarmonyPatch(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply")]
    class Patch_DamageInfosToApply
    {
        [HarmonyPostfix]
        private static IEnumerable<DamageInfo> AddDragonRageDamageBuff(IEnumerable<DamageInfo> __result, Verb_MeleeAttackDamage __instance)
        {
            DamageInfo primaryAttack = default(DamageInfo);
            bool hasPrimaryAttack = false;
            foreach(DamageInfo damage in __result)
            {
                if(damage.Def == __instance.verbProps.meleeDamageDef)
                {
                    primaryAttack = damage;
                    hasPrimaryAttack = true;
                    continue;
                }
                yield return damage;
            }
            if(!hasPrimaryAttack)
            {
                yield break;
            }
            //Log.Message("previous amount: " + primaryAttack.Amount + " penetration " + primaryAttack.ArmorPenetrationInt);
            try
            {
                if (!(__instance.Caster is Pawn caster))
                {
                    yield break;
                }
                if (!caster.IsEligableForDragonRage())
                {
                    //Log.Message("Not a half dragon");
                    yield break;
                }
                if (caster?.equipment?.Primary?.def?.IsMeleeWeapon == false)
                {
                    //Log.Message("Not a melee weapon");
                    yield break;
                }
                if (!caster.HasDragonRage())
                {
                    //Log.Message("Pawn does not have dragon rage");
                    yield break;
                }
                Hediff dragonRage = caster.GetDragonRageHediff();
                float dragonRageMultiplier = 1 + dragonRage.Severity;
                float newAmount = primaryAttack.Amount * dragonRageMultiplier;
                float newPenetration = primaryAttack.ArmorPenetrationInt * dragonRageMultiplier;
                primaryAttack = new DamageInfo(
                    primaryAttack.Def,
                    newAmount,
                    newPenetration,
                    primaryAttack.Angle,
                    primaryAttack.Instigator,
                    primaryAttack.HitPart,
                    primaryAttack.Weapon,
                    primaryAttack.Category,
                    primaryAttack.IntendedTarget,
                    primaryAttack.InstigatorGuilty,
                    primaryAttack.SpawnFilth);
            }
            catch (Exception e)
            {
                Log.Warning("Half-Dragons: Something went wrong " + e);
                yield break;
            }
            //Log.Message("after amount " + primaryAttack.Amount + " penetration " + primaryAttack.ArmorPenetrationInt);
            yield return primaryAttack;
        }
    }
}