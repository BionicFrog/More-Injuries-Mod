using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse.AI;
using Verse.Sound;
using HarmonyLib;
using HarmonyMod;
using Verse;

namespace Oof
{
    public class OofMod : Mod
    {
        public OofMod(ModContentPack content) : base(content)
        {
           
            

            var harmony = new Harmony("Caulaflower.Extended_Injuries.oof");
            try
            {
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error("Failed to apply 1 or more harmony patches! See error:");
                Log.Error(e.ToString());
            }
        }
    }
    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    public static class Patch_Thing_TakeDamage
    {
        public static bool Active = false;

        static void Postfix(Thing __instance, DamageWorker.DamageResult __result)
        {
            if (!Active)
                return;

            if (__instance is Pawn compHolder)
            {
                var comp = compHolder.GetComp<InjuriesComp>();
                comp.PostDamageFull(__result);
            }

            Active = false;
        }
    }
    public class InjuriesComp : ThingComp
    {
        public int someinteger = 1;
       
       
        public void NeedGetting(Need need)
        {
            pawns_need = need;
        }
        public InjuriesCompProps Props => (InjuriesCompProps)this.props;
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
           
            
            
            Oof.Patch_Thing_TakeDamage.Active = true;
            this.dinfofrompreapplydamage = dinfo;
            base.PostPreApplyDamage(dinfo, out absorbed);
            
        }
        public Need pawns_need;
        public override void Initialize(CompProperties props)
        {
            
            Pawn lowan = this.parent as Pawn;
           
            base.Initialize(props);
        }
        public void DumpAdrenaline(float DealtDamageChance)
        {
            Pawn lowan = this.parent as Pawn;
            
           
            if (Rand.Chance(DealtDamageChance))
            {
                if (!lowan.health.hediffSet.HasHediff(Caula_DefOf.adrenalinedump))
                {
                    lowan.health.AddHediff(HediffMaker.MakeHediff(Caula_DefOf.adrenalinedump, lowan));
                    Hediff AdrenalineOnPawn = lowan.health.hediffSet.GetFirstHediffOfDef(Caula_DefOf.adrenalinedump);
                    float bloat = Rand.Range(DealtDamageChance * - 10f, DealtDamageChance * 2);
                    AdrenalineOnPawn.Severity = bloat;
                   
                    lowan.needs.TryGetNeed(Caula_DefOf.TymonsMods_Adrenaline).CurLevel -= bloat;

                }
                else
                {
                    float bloat = Rand.Range(DealtDamageChance * -10f, DealtDamageChance * 2);
                    lowan.health.hediffSet.GetFirstHediffOfDef(Caula_DefOf.adrenalinedump).Severity += bloat;
                }
            }
            
        }
        public DamageInfo dinfofrompreapplydamage;
       

        public void PostDamageFull(DamageWorker.DamageResult damage)
        {
            
            if (damage != null)
            {
                Log.Error(damage.ToString());
            }
            if (damage.totalDamageDealt != 0f)
            {
                Log.Error(damage.totalDamageDealt.ToString());
            }
           
            Pawn myself = (Pawn)this.parent;
            if (damage.LastHitPart != null && damage != null) 
            {
                BodyPartRecord dahitpart = damage.LastHitPart;
            }
            if (!damage.diminished && damage != null)
            {
                if (damage.totalDamageDealt > 31)
                {
                    if (dinfofrompreapplydamage.Def == DamageDefOf.Bullet)
                    {
                        if (Rand.Chance(10f))
                        {
                            myself.health.AddHediff(HediffMaker.MakeHediff(Caula_DefOf.hemorrhagicstroke, myself));
                        }
                    }
                }
                
            }
           
            if (BodyPartTagDefOf.BreathingSource == null)
            {
                Log.Error("BreathingSource tag is null?");
            }
            if (Caula_DefOf.Shock == null)
            {
                Log.Error("Shock is null?");
            }
            if (Caula_DefOf.LungCollapse == null)
            {
                Log.Error("Lung Collapse is null?");
            }
            
            
            
            if (damage != null)
            {
                DumpAdrenaline(damage.totalDamageDealt);
            }    
            if (damage == null)
            {
                Log.Message("damage is null");
            }
           
          
            if (myself != null && Props.Concussion != null && damage != null)
            {
                if (myself.HitPoints < myself.MaxHitPoints / 2)
                {
                    if (myself.health.hediffSet.GetInjuredParts().Count() > 8)
                    {
                        if (myself.health.hediffSet.HasHediff(HediffDefOf.BloodLoss))
                        {
                            if (myself.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss).Severity > 0.20f)
                            {
                                myself.health.AddHediff(HediffMaker.MakeHediff(Props.Shock, myself));
                            }
                           
                        }
                    }
                }
                
                if (damage.LastHitPart.groups.Contains(BodyPartGroupDefOf.FullHead))
                {
                   
                    if (Rand.Chance(30 + damage.totalDamageDealt))
                    {
                        if (!myself.health.hediffSet.HasHediff(Props.Concussion))
                        {
                            myself.health.AddHediff(HediffMaker.MakeHediff(Props.Concussion, myself) );
                        }
                    }
                }
                
               
                    
                    
                
            }

        }

    }

    public class InjuriesCompProps : CompProperties
    {
        public HediffDef Concussion;
        public HediffDef Shock;
        public NeedDef polak;

        public InjuriesCompProps()
        {
            this.compClass = typeof(Oof.InjuriesComp);
        }

        public InjuriesCompProps(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }

    }

    [DefOf]
    public static class Caula_DefOf
    {
        
        


        public static HediffDef Shock;
        public static HediffDef Concussion;
        public static HediffDef adrenalinedump;
        public static HediffDef LungCollapse;
        public static NeedDef TymonsMods_Adrenaline;
        public static HediffDef hemorrhagicstroke;
        public static BodyPartGroupDef Arms;
        



    }
}

