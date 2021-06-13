using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.Sound;
using HarmonyLib;
using HarmonyMod;
using UnityEngine;
using Verse;

namespace Oof
{
	public class Need_Adrenaline : Need
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00009014 File Offset: 0x00007214
		

		// Token: 0x060000FD RID: 253 RVA: 0x00009031 File Offset: 0x00007231
		public Need_Adrenaline(Pawn pawn)
		{
			this.pawn = pawn;
		}
		public float Pope = Rand.Range(0.7f, 1.1f);
		public float RegenFloat = Rand.Range(0.1f, 0.2f) / 100;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00009044 File Offset: 0x00007244
		public override float MaxLevel
		{
			get
			{
				
				return Pope;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009072 File Offset: 0x00007272
		public override void SetInitialLevel()
		{
			this.CurLevel = this.MaxLevel;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009084 File Offset: 0x00007284
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			bool flag = this.threshPercents == null;
			if (flag)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			bool flag2 = this.MaxLevel > 1f;
			if (flag2)
			{
				float num = 1f / this.MaxLevel;
				this.threshPercents.Add(num * 0.5f);
				this.threshPercents.Add(num * 0.2f);
				for (int i = 0; i < (int)Math.Floor((double)this.MaxLevel); i++)
				{
					this.threshPercents.Add(num + num * (float)i);
				}
			}
			else
			{
				this.threshPercents.Add(0.5f);
				this.threshPercents.Add(0.2f);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		public InjuriesComp injurcomp
		{
			get
			{
				return this.pawn.TryGetComp<InjuriesComp>();
			}
		}
		public override void NeedInterval()
		{
			bool dool = this.pawn.TryGetComp<InjuriesComp>() != null;
			if (!this.pawn.IsPrisoner)
			{
				this.CurLevel += this.RegenFloat;

			}
		
			
		}
		



	}
	
}
