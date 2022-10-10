using CombatExtended;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace HandLoading
{
	[DefOf]
	public static class AmmoClassesDefOf
	{
		public static AmmoCategoryDef MusketBall;

		public static ThingDef Prometheum;

		public static DamageDef Thermobaric;

		public static ResearchProjectDef CE_AdvancedAmmo;

		public static ThingDef IndustrialPowder2b;

		public static StatDef PowderPower;

		public static AmmoDef Ammo_556x45mmNATO_FMJ;

		public static ThingDef Bullet_12Gauge_Buck;

		public static ThingDef Fragment_Small;

		public static ThingDef Fragment_Large;

		public static ThingDef testshit;

		public static ResearchProjectDef emploading;

		public static ResearchProjectDef apone;

		public static ThingDef CraftingSpot;

		public static ResearchProjectDef aptwo;

		public static RecipeDef MakeAmmo_556x45mmNATO_FMJ;

		public static AmmoCategoryDef FullMetalJacket;

		public static ThingDef UnfinishedAmmo;

		public static ThingDef IndustrialPowder;

		static AmmoClassesDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AmmoClassesDefOf));
		}
	}

	public class HandLoadingWindow : Window
	{
		public string shing;

		public ThingDef fragc1;

		public void makefrags(ThingDef material, ThingDef powder)
		{
			fragc1 = new ThingDef { };
			FieldInfo[] fields = typeof(ThingDef).GetFields(
					   BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo dd in fields)
			{

				dd.SetValue(fragc1, dd.GetValue(AmmoClassesDefOf.Fragment_Large));
			}
			fragc1.projectile = new ProjectilePropertiesCE();
			FieldInfo[] fieldsproj = typeof(ProjectileProperties).GetFields(
					   BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo dd in fieldsproj)
			{
				//Log.Message(dd.Name);
				dd.SetValue(fragc1.projectile, dd.GetValue(AmmoClassesDefOf.Fragment_Large.projectile));
			}
			float powderpower = powder.statBases.Find(oo => oo.stat.defName == "PowderPower").value;
			//Log.Message(powderpower.ToString());
			ProjectilePropertiesCE atak = (ProjectilePropertiesCE)fragc1.projectile;
			atak.armorPenetrationSharp = 1 * material.statBases.Find(pp => pp.stat == StatDefOf.StuffPower_Armor_Sharp).value * powderpower * material.statBases.Find(pp => pp.stat == StatDefOf.Mass).value;
			atak.armorPenetrationBlunt = 10 * material.statBases.Find(pp => pp.stat == StatDefOf.Mass).value * powderpower;
			Log.Message(atak.armorPenetrationSharp.ToString() + "ap sharp");
			Log.Message(atak.armorPenetrationBlunt.ToString() + "ap blunt");
			fragc1.label = material + " fragments";
			fragc1.defName = "truten" + Rand.Range(0, 670).ToString() + "trzmiel" + Rand.Range(0, 670).ToString() + "bak";

			Find.World.GetComponent<SaverComp>().SaveAmmodef(projectile: fragc1, amifrag: true);
		}

		public void CalPel()
		{
			if (actualshape.ismulti && SelectedProjectile != null)
			{
				string test = SelectedProjectile.defName.Substring(7, 2);

				if (PelCount == 0)
				{


				}
				PelCount = (int)(Math.Round(Convert.ToInt32(test) / 18.5) * 21);
				pelcal = 6.1f;
				Log.Message(test);
			}

		}
		private static readonly Vector2 Test = new Vector2(100f, 140f);

		public RecipeShapeDef actualshape;

		private int pint = 0;

		public int PelCount = 0;

		public int bigshrabint;

		public AmmoSetDef ammoclass_selected;

		public List<AmmoSetDef> calibers;

		public AmmoDef ammodef_selected;

		public CompProperties_Fragments kurwakurwakurwa;

		public string AmmoDefLabel;

		public float ArmorPenBluntCalculated;

		public float ArmorPenSharpCalculated;

		public float CalculatedSpeed;

		public float pelcal;

		public ThingDef casematerial;

		public float hardness_material_multiplier;

		public AmmoDef MadeAmmoDef;

		public ThingDef MadeProjectile;

		public ThingDef projectile_material;

		public string ProjectileShape;

		public ThingDef SelectedProjectile;

		public Texture sometextureidk;

		public List<ThingDef> ThingDefsToAddToDatabase;

		public float ChargeAmount = 1f;
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 610f);
			}
		}
		public Pawn desigboy;

		public HandLoadingWindow(IntVec3 pos, Map mpa, Pawn designer)
		{

			desigboy = designer;
			mpaa = mpa;
			postition = pos;
		}
		public IntVec3 postition;
		public Map mpaa;

		public float skillmult
		{
			get
			{
				float abc = 1;
				float shooty = (desigboy.skills.skills.Find(TT => TT.def == SkillDefOf.Intellectual).Level) / 10.0f;
				float crafty = (desigboy.skills.skills.Find(TT => TT.def == SkillDefOf.Crafting).Level) / 10.0f;
				float idktbh = (shooty + crafty);
				abc = idktbh;
				//Log.Message("skillmult: " + idktbh.ToString());
				return abc;
			}
		}

		public void CalculateAP()
		{
			float ShapeDoubleAP = 1f;
			switch (ProjectileShape)
			{
				case "Hollow point":
					ShapeDoubleAP = 0.5f;
					break;

				case "Armor piercing":
					ShapeDoubleAP = 2f;
					break;

				case "Full Metal Jacket":
					ShapeDoubleAP = 1f;
					break;

				case "Duplex":
					ShapeDoubleAP = 0.70f;
					break;

				case "Sabot":
					ShapeDoubleAP = 3.5f;
					break;

			}
			ProjectilePropertiesCE propsCE = SelectedProjectile?.projectile as ProjectilePropertiesCE;
			float Penmult2 = new float { };
			Penmult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value;
			if (Penmult2 == 0f)
			{
				//Log.Error("propelant most likely has no PowderPower statbase");
			}


			if (ProjectileShape == "Buckshot")
			{
				ArmorPenSharpCalculated = (float)(2.25 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmult);
			}
			else if (ProjectileShape == "Flechette")
			{
				ArmorPenSharpCalculated = (float)(3.10 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmult);
			}
			else
			{
				ArmorPenSharpCalculated = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * hardness_material_multiplier) * Penmult2 * this.ChargeAmount * skillmult);
			}
			Log.Message(ArmorPenSharpCalculated.ToString());

		}
		public void CalculateAP2()
		{
			float ShapeDoubleAP = actualshape.penmult;

			ProjectilePropertiesCE propsCE = SelectedProjectile?.projectile as ProjectilePropertiesCE;
			float Penmult2 = new float { };
			Penmult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value;
			if (Penmult2 == 0f)
			{
				//Log.Error("propelant most likely has no PowderPower statbase");
			}


			if (ProjectileShape == "Buckshot")
			{
				ArmorPenSharpCalculated = (float)(2.25 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmult);
			}
			else if (ProjectileShape == "Flechette")
			{
				ArmorPenSharpCalculated = (float)(3.10 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmult);
			}
			else
			{
				if (actualshape.penusestatpawn)
				{
					ArmorPenSharpCalculated = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * (hardness_material_multiplier * 0.85)) * Penmult2 * (this.ChargeAmount / 2) * (skillmult * 0.75f));
				}
				else
				{
					ArmorPenSharpCalculated = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * (hardness_material_multiplier * 0.85)) * Penmult2 * (this.ChargeAmount / 2));
				}
				
			}
			Log.Message(ArmorPenSharpCalculated.ToString());

		}

		public void CalculateAPBlunt()
		{
			if (ProjectileShape == "Buckshot")
			{
				ArmorPenBluntCalculated = ArmorPenSharpCalculated * 8f;
			}
			else
			{
				ArmorPenBluntCalculated = ArmorPenSharpCalculated * 6f * projectile_material.statBases.Find(tt33 => tt33.stat == StatDefOf.Mass).value;
			}

			Log.Message(ArmorPenBluntCalculated.ToString());
		}
		public ThingDef propelant;

		public string fillant;

		public int smolshrabcunt = 3;

		public int cunt = 0;
		public bool isexplosiveprojectile;

		public ThingDef fillantdef;

		public float maxcharge
		{
			get
			{
				if (AmmoClassesDefOf.apone.IsFinished && AmmoClassesDefOf.aptwo.IsFinished == false)
				{
					return 2f;
				}
				if (AmmoClassesDefOf.aptwo.IsFinished)
				{
					return 3f;
				}
				else
				{
					return 1f;
				}
			}
		}

		//Rect for choose caliber
		public Rect rectDesigner(Rect inRect)
		{

			Rect rectDesign = new Rect(inRect);
			rectDesign.width = 100f;
			rectDesign.height = 100f;
			rectDesign = rectDesign.CenteredOnXIn(inRect);
			rectDesign = rectDesign.CenteredOnYIn(inRect);
			rectDesign.x += -320f;
			rectDesign.y += -200f;
			return rectDesign;
		}
		//Action for the choose caliber button
		public void ChooseCaliberAction()
		{
			var options = new List<FloatMenuOption>
			{
			};
			List<AmmoDef> shlist = new List<AmmoDef>();
			if (!isexplosiveprojectile)
			{
				shlist = DefDatabase<AmmoDef>.AllDefs.ToList().FindAll(deffer => deffer.Users.Count > 0 && deffer.ammoClass == AmmoClassesDefOf.FullMetalJacket | deffer.ammoClass == AmmoClassesDefOf.MusketBall | deffer.ammoClass.defName == "Slug");
			}
			else
			{
				shlist = DefDatabase<AmmoDef>.AllDefs.ToList().FindAll(deffer => deffer.Users.Count > 0 && deffer.ammoClass.defName == "GrenadeHE");
				shlist.Add(DefDatabase<AmmoDef>.AllDefs.ToList().Find(gaugeshell => gaugeshell.defName == "Ammo_12Gauge_Buck"));
				// && kurwakurwakurwa.fragments.Find(ghjb => ghjb.thingDef == AmmoClassesDefOf.Fragment_Large) != null

				//shlist.Add(DefDatabase<AmmoDef>.AllDefs.ToList().Find(LKHKF => LKHKF.defName == "Ammo_30x29mmGrenade_HE"));
			}


			foreach (AmmoDef ammodef in shlist)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(ammodef.label, delegate
				{
					this.ammodef_selected = ammodef;
					this.ammoclass_selected = ammodef.AmmoSetDefs.Find(dod => dod.ammoTypes != null);
					this.SelectedProjectile = ammoclass_selected?.ammoTypes?.Find(A => A.ammo == ammodef_selected).projectile;
					calibers = ammodef.AmmoSetDefs;
					Log.Message($"Chosen ammo def: {SelectedProjectile}, {ammodef_selected}.");
					sometextureidk = ammodef_selected.uiIcon;
					if (ammodef.ammoClass.defName == "BuckShot" | ammodef.ammoClass.defName == "GrenadeHE")
					{
						if (actualshape != null && actualshape.ismulti)
						{
							if (actualshape.isfrag == false)
							{
								CalPel();
							}

						}

					}

				});

				options.Add(floatmenuoption);
			}
			Find.WindowStack.Add(new FloatMenu(options));
		}

		//Rect for choose projectile material
		public Rect Rect51(Rect inRect)
		{
			Rect rect5 = new Rect(inRect);
			rect5.width = 100f;
			rect5.height = 100f;
			rect5 = rect5.CenteredOnXIn(inRect);
			rect5 = rect5.CenteredOnYIn(inRect);
			rect5.x += -320f;
			rect5.y += -100f;
			return rect5;
		}

		//Action for choose projectile material
		public void ProjClick()
		{
			var options2 = new List<FloatMenuOption>
			{
			};

			List<ThingDef> somelist = DefDatabase<ThingDef>.AllDefs.ToList().Where<ThingDef>(L => L.stuffProps?.categories?.Contains(StuffCategoryDefOf.Metallic) is true).ToList();

			foreach (ThingDef szteel in somelist)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(szteel.label, delegate
				{
					projectile_material = szteel;
					hardness_material_multiplier = projectile_material.stuffProps.statFactors.Find(A => A.stat == StatDefOf.MaxHitPoints).value;
				});

				options2.Add(floatmenuoption);
			}
			Find.WindowStack.Add(new FloatMenu(options2));
		}

		//Rect for casing button
		public Rect rect_case(Rect inRect)
		{
			Rect rectCasing = new Rect(inRect);
			rectCasing.width = 100f;
			rectCasing.height = 100f;
			rectCasing = rectCasing.CenteredOnXIn(inRect);
			rectCasing = rectCasing.CenteredOnYIn(inRect);
			rectCasing.x += -320f;
			rectCasing.y += 100f;
			return rectCasing;
		}

		//Action for casing button
		public void action_case()
		{
			var options2 = new List<FloatMenuOption>
			{
			};

			List<ThingDef> somelist = DefDatabase<ThingDef>.AllDefs.ToList().Where(L => L.stuffProps?.categories?.Contains(StuffCategoryDefOf.Metallic) == true).ToList();
			somelist.Add(ThingDefOf.Chemfuel);
			if ((actualshape?.isbootleq ?? true))
			{
				Log.Message("bee");
				somelist = new List<ThingDef>();
				somelist.Add(ThingDefOf.Steel);
			}
			foreach (ThingDef szteel in somelist)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(szteel.label, delegate
				{
					casematerial = szteel;
				});

				options2.Add(floatmenuoption);
			}
			Find.WindowStack.Add(new FloatMenu(options2));
		}

		//Rect for power
		public Rect rect_powder(Rect inRect)
		{
			Rect rectPowder = new Rect(inRect);
			rectPowder.width = 100f;
			rectPowder.height = 100f;
			rectPowder = rectPowder.CenteredOnXIn(inRect);
			rectPowder = rectPowder.CenteredOnYIn(inRect);
			rectPowder.x += -320f;
			rectPowder.y += -0f;
			return rectPowder;
		}

		//action for powder button
		public void action_powder()
		{
			var options3 = new List<FloatMenuOption>
			{
			};
			StatDef state = DefDatabase<StatDef>.AllDefs.ToList().Find(loop => loop.defName == "PowderPower");
			Log.Message(state?.defName ?? "co kurwa");
			List<ThingDef> somelister = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(l => l.comps.Any(L => L is PowderCompProps));
			if (somelister?.Count == 0)
			{
				somelister = DefDatabase<ThingDef>.AllDefs.ToList().FindAll(l => l.statBases.Any(koof => koof.stat == state));
			}


			foreach (ThingDef szteel in somelister)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(szteel.label, delegate
				{
					propelant = szteel;

				});

				options3.Add(floatmenuoption);
			}
			Find.WindowStack.Add(new FloatMenu(options3));
		}

		//Rect for fillant
		public Rect rect_fillant(Rect inRect)
		{
			Rect rectFillant = new Rect(inRect);
			rectFillant.width = 100f;
			rectFillant.height = 40f;
			rectFillant = rectFillant.CenteredOnXIn(inRect);
			rectFillant = rectFillant.CenteredOnYIn(inRect);
			rectFillant.x += 340f;
			rectFillant.y += 115f;
			return rectFillant;
		}

		//action for fillant button
		public void action_fillant()
		{
			var options3 = new List<FloatMenuOption>
			{
			};

			List<String> somelister = new List<string>();
			somelister.Add("nothing");
			somelister.Add("prometheum");
			somelister.Add("FSX");
			if (AmmoClassesDefOf.emploading.IsFinished)
			{
				somelister.Add("EMP devices");
			}


			foreach (String szteel in somelister)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption("Fill with: " + szteel, delegate
				{

					fillant = szteel;
				});

				options3.Add(floatmenuoption);
			}
			Find.WindowStack.Add(new FloatMenu(options3));
		}

		//Rect for pellet caliber increase
		public Rect rect_pelcal(Rect inRect)
		{
			Rect rectplusbigfragment = new Rect(inRect);
			rectplusbigfragment.width = 33f;
			rectplusbigfragment.height = 33f;
			rectplusbigfragment = rectplusbigfragment.CenteredOnXIn(inRect);
			rectplusbigfragment = rectplusbigfragment.CenteredOnYIn(inRect);
			rectplusbigfragment.x += 307f;
			rectplusbigfragment.y += -25f;
			return rectplusbigfragment;
		}

		//Rect for pellet caliber decrease
		public Rect rect_pelcal2(Rect inRect)
		{
			Rect rectfragsmalltwo = new Rect(inRect);
			rectfragsmalltwo.width = 33f;
			rectfragsmalltwo.height = 33f;
			rectfragsmalltwo = rectfragsmalltwo.CenteredOnXIn(inRect);
			rectfragsmalltwo = rectfragsmalltwo.CenteredOnYIn(inRect);
			rectfragsmalltwo.x += 373f;
			rectfragsmalltwo.y += -25f;
			return rectfragsmalltwo;
		}

		//Rect for text for pellet caliber
		public Rect rect_caltext(Rect inRect)
		{
			Rect chargetext6969 = new Rect(inRect);
			chargetext6969.width = 35f;
			chargetext6969.height = 33f;
			chargetext6969 = chargetext6969.CenteredOnXIn(inRect);
			chargetext6969 = chargetext6969.CenteredOnYIn(inRect);
			chargetext6969.x += 340f;
			chargetext6969.y += -25f;
			return chargetext6969;
		}

		//Rect for string saying "pellet size"
		public Rect rect_caltexttrue(Rect inRect)
		{
			Rect chargetext69696 = new Rect(inRect);
			chargetext69696.width = 101f;
			chargetext69696.height = 33f;
			chargetext69696 = chargetext69696.CenteredOnXIn(inRect);
			chargetext69696 = chargetext69696.CenteredOnYIn(inRect);
			chargetext69696.x += 340f;
			chargetext69696.y += -60f;
			return chargetext69696;
		}

		//the stats shown mid designing
		//fyi it's long af
		public void ass_pain(Rect inRect)
		{
			Rect rect227 = new Rect(inRect);
			rect227.width = 100f;
			rect227.height = 100f;
			rect227 = rect227.CenteredOnXIn(inRect);
			rect227 = rect227.CenteredOnYIn(inRect);
			//rect27.x += 320f;
			rect227.y += 200f;

			Rect rect2272 = new Rect(inRect);
			rect2272.width = 100f;
			rect2272.height = 100f;
			rect2272 = rect2272.CenteredOnXIn(inRect);
			rect2272 = rect2272.CenteredOnYIn(inRect);
			//rect27.x += 320f;
			rect2272.y += 250f;
			Widgets.Label(rect227, "stats:");
			var skillmultrelative = skillmult;
			if (!actualshape?.dmgusestatpawn ?? false)
			{
				skillmultrelative = 1f;
			}
			float DamageMult = 1f;
			float aproxdmg = 0f;
			if (actualshape == null && ProjectileShape != null)
			{
				if (ProjectileShape == "Hollow point")
				{
					DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) * 2;


				}
				if (ProjectileShape == "Armor piercing")
				{
					DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;


				}
				if (ProjectileShape == "Full Metal Jacket")
				{
					DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f);


				}
				if (ProjectileShape == "Duplex")
				{
					DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;


				}
				if (ProjectileShape == "Sabot")
				{
					DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;


				}
			}
			if (actualshape != null)
			{
				DamageMult = actualshape.damagemult;
			}
			if (ProjectileShape != null && ProjectileShape == "Buckshot")
			{
				aproxdmg = (float)Math.Round(8 * ChargeAmount * (SelectedProjectile?.projectile.GetDamageAmount(1) ?? 0f) * (pelcal / 10f) * skillmultrelative);
			}
			else if (ProjectileShape != null && ProjectileShape == "Flechette")
			{
				aproxdmg = (float)Math.Round(12 * ChargeAmount * (SelectedProjectile?.projectile.GetDamageAmount(1) ?? 0f) * (pelcal / 10f) * skillmultrelative);
			}
			else
			{
				//Log.Message(SelectedProjectile?.projectile.GetDamageAmount(1).ToString());
				aproxdmg = (float)Math.Round(DamageMult * (SelectedProjectile?.projectile.GetDamageAmount(1) ?? 0f) * (ChargeAmount) * (propelant?.statBases.Find(pp => pp.stat.defName == "PowderPower").value ?? 69f) * skillmultrelative);
			}

			Widgets.Label(rect2272, "damage: " + aproxdmg);

			Rect rect2273 = new Rect(inRect);
			rect2273.width = 100f;
			rect2273.height = 100f;
			rect2273 = rect2273.CenteredOnXIn(inRect);
			rect2273 = rect2273.CenteredOnYIn(inRect);
			rect2273.x += 100f;
			rect2273.y += 250f;
			float aproxpen = 0f;
			var skillmultrelative2 = skillmult;
			if (!actualshape?.penusestatpawn ?? false)
			{
				skillmultrelative2 = 1f;
			}

			if (SelectedProjectile != null && actualshape != null)
			{
				float ShapeDoubleAP = actualshape.penmult;

				ProjectilePropertiesCE propsCE = SelectedProjectile?.projectile as ProjectilePropertiesCE;
				float Penmult2 = new float { };
				Penmult2 = propelant?.statBases.Find(abc => abc.stat.defName == "PowderPower").value ?? 0f;
				if (Penmult2 == 0f)
				{
					//Log.Error("propelant most likely has no PowderPower statbase");
				}


				if (ProjectileShape == "Buckshot")
				{
					aproxpen = (float)(2.25 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmultrelative2);
				}
				else if (ProjectileShape == "Flechette")
				{
					aproxpen = (float)(3.10 * (hardness_material_multiplier) * (Penmult2) * this.ChargeAmount * (pelcal / 10) * skillmultrelative2);
				}
				else
				{
					aproxpen = (float)((propsCE?.armorPenetrationSharp ?? 1) + (ShapeDoubleAP * hardness_material_multiplier) * Penmult2 * this.ChargeAmount * skillmultrelative2);
				}
			}
			Widgets.Label(rect2273, "sharp armor penetration: " + aproxpen);

			Rect rect3273 = new Rect(inRect);
			rect3273.width = 170f;
			rect3273.height = 100f;
			rect3273 = rect3273.CenteredOnXIn(inRect);
			rect3273 = rect3273.CenteredOnYIn(inRect);
			rect3273.x += -150f;
			rect3273.y += 250f;

			Widgets.Label(rect3273, "Pawn skill multiplier: " + skillmult.ToString() + "(" + ((desigboy.skills.skills.Find(TT => TT.def == SkillDefOf.Intellectual).Level) / 10.0f).ToString() + " int + " + ((desigboy.skills.skills.Find(TT => TT.def == SkillDefOf.Crafting).Level) / 10.0f).ToString() + " craft) " + desigboy.Name.ToStringShort + " used: " + (actualshape?.penusestatpawn ?? false) + "(pen) " + (actualshape?.dmgusestatpawn ?? false) + "(dmg)");

			Rect rect2373 = new Rect(inRect);
			rect2373.width = 100f;
			rect2373.height = 100f;
			rect2373 = rect2373.CenteredOnXIn(inRect);
			rect2373 = rect2373.CenteredOnYIn(inRect);
			rect2373.x += 200f;
			rect2373.y += 250f;

			Widgets.Label(rect2373, "blunt armor penetration: " + aproxpen * 8);
		}

		//action for projectile shape button
		public void button_shape()
		{

			var options3 = new List<FloatMenuOption>
			{
			};

			List<String> somelister = new List<String>();
			List<RecipeShapeDef> shapes = new List<RecipeShapeDef>();
			if (!isexplosiveprojectile)
			{
				foreach (RecipeShapeDef recshapedef in DefDatabase<RecipeShapeDef>.AllDefs.ToList().FindAll(TT => TT.needed.IsFinished && TT.ismulti == false))
				{
					shapes.Add(recshapedef);
				}

				//somelister.Add("Hollow point");
				//somelister.Add("Armor piercing");
				//somelister.Add("Full Metal Jacket");
				//somelister.Add("Duplex");
				//somelister.Add("Sabot");
			}
			else
			{
				foreach (RecipeShapeDef recshapedef in DefDatabase<RecipeShapeDef>.AllDefs.ToList().FindAll(TT => TT.needed.IsFinished && TT.ismulti))
				{
					shapes.Add(recshapedef);
				}
				//somelister.Add("fragmentation");

			}
			foreach (var szteel in shapes)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(szteel.label, delegate
				{
					actualshape = szteel;
				});
				options3.Add(floatmenuoption);
			}
			foreach (var idfk in somelister)
			{
				FloatMenuOption floatmenuoption = new FloatMenuOption(idfk, delegate
				{
					ProjectileShape = idfk;
				});
				options3.Add(floatmenuoption);
			}





			Find.WindowStack.Add(new FloatMenu(options3));
		}

		//rect for that button
		public Rect rect_shape(Rect inRect)
		{
			Rect rect4 = new Rect(inRect);
			rect4.width = 100f;
			rect4.height = 100f;
			rect4 = rect4.CenteredOnXIn(inRect);
			rect4 = rect4.CenteredOnYIn(inRect);
			rect4.x += -320f;
			rect4.y += 200f;
			return rect4;
		}

		//rect for finish button
		public Rect rect_finish(Rect inRect)
		{
			Rect rect6 = new Rect(inRect);
			rect6.width = 100f;
			rect6.height = 100f;
			rect6 = rect6.CenteredOnXIn(inRect);
			rect6 = rect6.CenteredOnYIn(inRect);
			rect6.x += 340f;
			rect6.y += 200f;
			return rect6;
		}

		//action for finish button. probably longer that the one for stats
		public void finish_action()
		{
			++pint;
			if (actualshape == null)
			{
				CalculateAP();
			}
			else
			{
				CalculateAP2();
			}
			if (actualshape.pelletcount > 1)
			{
				ProjectilePropertiesCE meth = MadeProjectile.projectile as ProjectilePropertiesCE;
				meth.pelletCount = actualshape.pelletcount;
			}



			if (actualshape?.isfrag ?? false)
			{
				makefrags(projectile_material, propelant);
			}

			if (actualshape.defName == "pronershape")
			{
				ThingDef poopboner = DefDatabase<ThingDef>.AllDefs.ToList().Find(tt33 => tt33.defName == "vcghjkilokjbhvg");
				MadeProjectile = new ThingDef() { tickerType = TickerType.Normal, graphic = poopboner.graphic, label = projectile_material.label + " " + ammoclass_selected.label + " " + ProjectileShape, defName = $"GeneratedDef_Projectile{pint}" + "a" + Rand.Range(0, 256790333).ToString() + "v", graphicData = poopboner.graphicData, projectile = new ProjectilePropertiesCE() { damageDef = DamageDefOf.Bullet, armorPenetrationSharp = ArmorPenSharpCalculated, armorPenetrationBlunt = ArmorPenSharpCalculated * 6 * projectile_material.statBases.Find(ree => ree.stat == StatDefOf.Mass).value, speed = 150f, flyOverhead = false, ai_IsIncendiary = false, dropsCasings = true, pelletCount = 1, stoppingPower = 2.5f }, thingClass = AmmoClassesDefOf.testshit.thingClass };
			}
			else
			{
				MadeProjectile = new ThingDef() { tickerType = TickerType.Normal, graphic = SelectedProjectile.graphic, label = projectile_material.label + " " + ammoclass_selected.label + " " + ProjectileShape, defName = $"GeneratedDef_Projectile{pint}" + "a" + Rand.Range(0, 256790333).ToString() + "v", graphicData = SelectedProjectile?.graphicData, projectile = new ProjectilePropertiesCE() { damageDef = DamageDefOf.Bullet, armorPenetrationSharp = ArmorPenSharpCalculated, armorPenetrationBlunt = ArmorPenSharpCalculated * 6 * projectile_material.statBases.Find(ree => ree.stat == StatDefOf.Mass).value, speed = 150f, flyOverhead = false, ai_IsIncendiary = false, dropsCasings = true, pelletCount = 1, stoppingPower = 2.5f }, thingClass = AmmoClassesDefOf.testshit.thingClass };
			}

			MadeAmmoDef = new AmmoDef()
			{
				thingClass = AmmoClassesDefOf.Ammo_556x45mmNATO_FMJ.thingClass,
				label = projectile_material.label + " " + ammoclass_selected.label + "  " + ProjectileShape,
				defName = $"GeneratedDef_Ammo{pint}" + "b" + Rand.Range(0, 694201314).ToString() + "c",
				graphicData = ammodef_selected?.graphicData,
				cookOffProjectile = MadeProjectile,
				projectile = MadeProjectile.projectile,

				ammoClass = new AmmoCategoryDef
				{
					defName = $"GeneratedDef_Ammoclass{pint}" + Rand.Range(0, 256923213).ToString() + "a",
					label = projectile_material.label + " " + ammoclass_selected.label,
					description = projectile_material.label + " " + ammoclass_selected.label + " " + casematerial.label + " cased",
					labelShort = projectile_material.label + " " + ammoclass_selected.label
				}
			};


			foreach (AmmoSetDef caliber in calibers)
			{
				caliber.ammoTypes.Add(new AmmoLink() { ammo = MadeAmmoDef, projectile = MadeProjectile });
				foreach (AmmoLink amlink in ammoclass_selected.ammoTypes)
				{
					//Log.Error(caliber.ToString());
				}
			}
			if (ProjectileShape == "Duplex")
			{
				ProjectilePropertiesCE arab = MadeProjectile.projectile as ProjectilePropertiesCE;
				arab.pelletCount = 2;
				arab.spreadMult = 3.4f;
			}
			DefGenerator.AddImpliedDef(MadeAmmoDef);
			DefGenerator.AddImpliedDef(MadeProjectile);
			List<ThingDefCountClass> products = new List<ThingDefCountClass>();
			products.Add(new ThingDefCountClass
			{
				thingDef = MadeAmmoDef,
				count = 500
			});
			ThingFilter filter = new ThingFilter();
			filter.AllowedThingDefs.ToList().Add(projectile_material);
			filter.SetAllow(projectile_material, true);

			ThingFilter filter2 = new ThingFilter();
			filter2.AllowedThingDefs.ToList().Add(casematerial);
			filter2.SetAllow(casematerial, true);
			//Log.Error(filter.AllowedDefCount.ToString());
			if (actualshape == null)
			{
				CalculateDamage();
			}
			else
			{
				CalculateDamage2();
			}
			List<IngredientCount> ingredientss = new List<IngredientCount>();
			IngredientCount ingredient1 = new IngredientCount() { filter = filter };
			IngredientCount ingredient2 = new IngredientCount() { filter = filter2 };
			ingredient1.SetBaseCount(DefDatabase<RecipeDef>.AllDefs.ToList().Find(A => A.defName == "Make" + ammodef_selected.defName).ingredients.Find(B => B.GetBaseCount() != 0).GetBaseCount() / 2);
			ingredientss.Add(ingredient1);
			ingredient2.SetBaseCount(DefDatabase<RecipeDef>.AllDefs.ToList().Find(A => A.defName == "Make" + ammodef_selected.defName).ingredients.Find(B => B.GetBaseCount() != 0).GetBaseCount() / 2);
			ingredientss.Add(ingredient2);

			ThingFilter filter3 = new ThingFilter();
			filter3.AllowedThingDefs.ToList().Add(propelant);
			filter3.SetAllow(propelant, true);
			IngredientCount ingredient3 = new IngredientCount() { filter = filter3 };
			if (actualshape.ThermoBar)
			{
				ingredient3.SetBaseCount(ingredient3.GetBaseCount() * 3);
			}
			ingredient3.SetBaseCount(ChargeAmount);
			ingredientss.Add(ingredient3);

			List<ThingDef> users = new List<ThingDef>();
			//users.Add(AmmoClassesDefOf.CraftingSpot);
			users.Add(CE_ThingDefOf.AmmoBench);

			RecipeDef makeammorecipe = new RecipeDef
			{
				products = products,
				workSkill = SkillDefOf.Crafting,
				fixedIngredientFilter = filter,
				ingredients = ingredientss,
				workAmount = (DefDatabase<RecipeDef>.AllDefs.ToList().Find(A => A.defName == "Make" + ammodef_selected.defName).workAmount * actualshape.workmult),
				jobString = "Making ammo",
				effectWorking = EffecterDefOf.Clean,
				label = "Make" + " " + MadeAmmoDef.label,
				workSpeedStat = StatDefOf.GeneralLaborSpeed,
				defName = "Make" + MadeAmmoDef.defName,
				recipeUsers = users,
				unfinishedThingDef = AmmoClassesDefOf.UnfinishedAmmo,
				defaultIngredientFilter = filter,




			};
			RecipeDefGenerator.ImpliedRecipeDefs().ToList().Add(makeammorecipe);
			List<Building> benchesidk = mpaa.listerBuildings.AllBuildingsColonistOfDef(CE_ThingDefOf.AmmoBench).ToList();
			if (benchesidk.Count >= 1)
			{
				benchesidk.RandomElement().def.AllRecipes.Add(makeammorecipe);
				foreach (Building a in benchesidk)
				{
					//Log.Error(makeammorecipe.defName);

				}
			}

			//Log.Error(mpaa.ToString());
			//Log.Error(postition.ToString());
			MadeAmmoDef.selectable = true;
			MadeAmmoDef.stackLimit = 500;

			//Log.Error(MadeAmmoDef.statBases.Find(ABC => ABC.stat.defName == "Mass").value.ToString());
			MadeAmmoDef.statBases = new List<StatModifier>();
			MadeAmmoDef.statBases.RemoveAll(OO => OO.stat == StatDefOf.Mass);
			MadeAmmoDef.statBases.Add(new StatModifier
			{
				stat = StatDefOf.Mass,
				value = ammodef_selected.statBases.Find(rtar => rtar.stat == StatDefOf.Mass).value * (casematerial.statBases.Find(rtar => rtar.stat == StatDefOf.Mass).value)
			});
			Log.Message("mass 0: " + ammodef_selected.statBases.Find(LL => LL.stat == StatDefOf.Mass).value.ToString());
			Log.Message("mass: " + MadeAmmoDef.statBases.Find(LL => LL.stat == StatDefOf.Mass).value.ToString());
			foreach (StatModifier amongsussybaka in MadeAmmoDef.statBases)
			{
				//Log.Error(amongsussybaka.value.ToString());
			}
			float athink = 0f;

			athink = (ChargeAmount * propelant.statBases.Find(Poof => Poof.stat.defName == "PowderPower").value) / 10;



			Log.Message(athink.ToString() + "testing");

			MadeAmmoDef.comps.Add(new damagercproprs { dammpoints = athink });
			MadeAmmoDef.useHitPoints = true;
			MadeAmmoDef.tickerType = TickerType.Normal;
			MadeAmmoDef.comps.Add(new CompProperties_Forbiddable { });
			MadeAmmoDef.category = ThingCategory.Item;
			MadeAmmoDef.altitudeLayer = AltitudeLayer.Item;
			MadeAmmoDef.statBases = ammodef_selected.statBases;
			MadeAmmoDef.alwaysHaulable = true;
			MadeAmmoDef.drawGUIOverlay = true;
			this.Close();
			List<DefModExtension> defModExtensions = new List<DefModExtension>();
			defModExtensions.Add(new BulletModExtension { });
			MadeProjectile.modExtensions = defModExtensions;
			MadeProjectile.GetModExtension<BulletModExtension>().FixedDamage = DamageCalculated;
			MadeAmmoDef.description = "Actual damage is: " + DamageCalculated.ToString();
			ProjectilePropertiesCE projpropce = MadeProjectile.projectile as ProjectilePropertiesCE;
			List<SecondaryDamage> secdmgs = new List<SecondaryDamage>();
			if (casematerial == ThingDefOf.Chemfuel)
			{
				MadeAmmoDef.label = "polymer cased " + projectile_material.label + " " + "projectile " + " " + ammoclass_selected.label + " " + ProjectileShape;
				MadeAmmoDef.ammoClass.description = "polymer cased " + projectile_material.label + " " + ammoclass_selected.label;
			}

			CalculateSecDmg(fillant, makeammorecipe, secdmgs);

			if (fillantdef != null)
			{

				projpropce.secondaryDamage = secdmgs;
			}
			if ((actualshape?.isfrag ?? false) && ((actualshape?.defName ?? "thermo_shape") != "thermo_shape"))
			{
				MakeFrags();
				List<DefModExtension> defModExtensionss = new List<DefModExtension>();
				defModExtensionss.Add(new BulletModExtension { });
				smolshrab.modExtensions = defModExtensions;
				smolshrab.GetModExtension<BulletModExtension>().FixedDamage = 10;
				//MadeProjectile.thingClass = typeof(ProjectileCE_Explosive);
				ProjectilePropertiesCE ProjCE = MadeProjectile.projectile as ProjectilePropertiesCE;
				ProjectilePropertiesCE selectedProjCE = SelectedProjectile.projectile as ProjectilePropertiesCE;
				//ProjCE.explosionRadius = selectedProjCE.explosionRadius;
				projpropce.secondaryDamage = new List<SecondaryDamage>();
				CompProperties_ExplosiveCE proper = new CompProperties_ExplosiveCE { damageAmountBase = selectedProjCE.GetDamageAmount(1), explosiveDamageType = DamageDefOf.Bomb, explosiveRadius = selectedProjCE.explosionRadius };
				List<ThingDefCountClass> frags = new List<ThingDefCountClass>();
				frags.Add(new ThingDefCountClass { thingDef = fragc1, count = smolshrabcunt });
				if (bigshrabint >= 1)
				{
					frags.Add(new ThingDefCountClass { thingDef = fragc1, count = bigshrabint });
				}

				CompProperties_Fragments proper2 = new CompProperties_Fragments { fragments = frags };
				MadeProjectile.comps.Add(proper);
				MadeProjectile.comps.Add(proper2);
				MadeAmmoDef.statBases.Find(gvhj => gvhj.stat == StatDefOf.Mass).value += ((smolshrabcunt * 0.03f) + (bigshrabint * 0.06f));


			}
			if (actualshape?.ismulti ?? false && actualshape.isfrag == false)
			{
				ProjectilePropertiesCE prohectyl = MadeProjectile.projectile as ProjectilePropertiesCE;
				MadeProjectile.graphic = AmmoClassesDefOf.Bullet_12Gauge_Buck.graphic;
				prohectyl.pelletCount = PelCount;
				prohectyl.spreadMult = actualshape.spreadmult * (PelCount / 10f);
			}


			if (SelectedProjectile.projectile.flyOverhead)
			{
				ProjectilePropertiesCE projectile = (ProjectilePropertiesCE)MadeProjectile.projectile;
				projectile.speed = 0;
				projectile.flyOverhead = true;

			}
			bool flag = ProjectileShape == "Buckshot";
			if (ProjectileShape == "Buckshot" | ProjectileShape == "Flechette")
			{
				makeammorecipe.products.First().count = 200;
			}
			if (actualshape.isfrag)
			{
				makeammorecipe.products.First().count = 5;
			}
			List<ThingCategoryDef> thingCategories = new List<ThingCategoryDef>();
			thingCategories.Add(DefDatabase<ThingCategoryDef>.AllDefs.ToList().Find(G => G.defName == "HandsLoadedAmmo"));
			DefDatabase<ThingCategoryDef>.AllDefs.ToList().Find(G => G.defName == "HandsLoadedAmmo").childThingDefs.Add(MadeAmmoDef);
			//DefDatabase<ThingCategoryDef>.AllDefs.ToList().Find(G => G.defName == "HandsLoadedAmmo").
			MadeAmmoDef.thingCategories = thingCategories;
			foreach (ThingDef thing in DefDatabase<ThingCategoryDef>.AllDefs.ToList().Find(G => G.defName == "HandsLoadedAmmo").childThingDefs)
			{
				//Log.Error(thing.label);
			}
			MadeAmmoDef.comps.Add(new CompProperties { compClass = typeof(HaulComp) });
			MadeAmmoDef.alwaysHaulable = true;
			DefDatabase<ThingDef>.Add(MadeAmmoDef);
			if ((shing?.Length ?? 0) > 0)
			{
				MadeAmmoDef.label = shing;
				MadeAmmoDef.ammoClass.label = shing;
				MadeProjectile.label = "bullet " + shing;
			}
			makeammorecipe.label = "Make " + MadeAmmoDef.label;
			ProjectilePropertiesCE proce = MadeProjectile.projectile as ProjectilePropertiesCE;
			if (proce.pelletCount < 1)
			{
				proce.pelletCount = 1;
			}
			if (actualshape.isfrag)
			{
				proce.pelletCount = 1;
			}
			if (actualshape.isbootleq)
			{
				Log.Error("dudus");
				AmmoClassesDefOf.CraftingSpot.AllRecipes.Add(makeammorecipe);
				makeammorecipe.modExtensions = new List<DefModExtension>();
				makeammorecipe.modExtensions.Add(new BootlegExtension { isthisreallyneedtbh = false });
			}
			tryfix();
			MadeAmmoDef.alwaysHaulable = true;

			if (actualshape.ThermoBar)
			{
				var projce = (ProjectilePropertiesCE)MadeProjectile.projectile;
				projce.explosionRadius = Math.Max((DamageCalculated / 60f), 2f);
				projce.armorPenetrationBlunt = 0f;
				projce.armorPenetrationSharp = 0f;
				projce.applyDamageToExplosionCellsNeighbors = true;
				projce.damageDef = AmmoClassesDefOf.Thermobaric;
			}


			MadeProjectile.projectile.speed = (float)Math.Round(SelectedProjectile.projectile.speed * (ChargeAmount * propelant.statBases.Find(pp => pp.stat == AmmoClassesDefOf.PowderPower).value));

			var clor = Color.red;

			MadeAmmoDef.uiIconColor = clor;
			MadeAmmoDef.graphicData.color = clor;
			MadeAmmoDef.graphic.color = clor;

			Find.World.GetComponent<SaverComp>().SaveAmmodef(MadeAmmoDef, MadeAmmoDef.ammoClass, MadeProjectile, ammoclass_selected, makeammorecipe, ChargeAmount, smolshrab, athink, actualshape.isbootleq, actualshape.ThermoBar);




			List<Zone> bone = new List<Zone>();
			//bone = Find.CurrentMap.zoneManager.AllZones.FindAll(OOF => OOF is Zone_Stockpile); 
			foreach (Zone_Stockpile zonee in bone)
			{
				Zone_Stockpile cockpile = zonee as Zone_Stockpile;
				cockpile.settings.filter.AllowedThingDefs.AddItem(MadeAmmoDef);
				cockpile.settings.filter.SetAllow(MadeAmmoDef, true);
				List<ThingDef> things = new List<ThingDef>();
				things.Add(MadeAmmoDef);
				cockpile.settings.filter = new ThingFilter { };
				cockpile.settings.filter.AllowedThingDefs.AddItem(MadeAmmoDef);
				cockpile.settings.filter.SetAllow(MadeAmmoDef, true);
				//Log.Message(cockpile.settings.filter.Allows(MadeAmmoDef).ToString() + "a,p");
				//Log.Error(cockpile.settings.filter.AllowedThingDefs.ToList().Find(o => o == MadeAmmoDef).ToString() + cockpile.label);
			}



			//cockpile.settings.filter.


		}

		//void for all of multi shot shit. super long
		public void frag_shit(Rect inRect)
		{
			if (actualshape != null && actualshape.ismulti == true && actualshape.isfrag == false)
			{

				if (Widgets.ButtonText(rect_pelcal(inRect), "+"))
				{
					//this action is so small I don't think a public void is needed
					if (PelCount > 1)
					{
						pelcal += 0.50f;
						PelCount -= 1;
					}




				}


				if (Widgets.ButtonText(rect_pelcal2(inRect), "-"))
				{

					//same here as before
					if (pelcal >= 1)
					{
						pelcal -= 0.50f;
						PelCount += 1;
					}




				}


				if (actualshape != null && actualshape.isfrag == false)
				{
					Widgets.TextArea(rect_caltext(inRect), this.pelcal.ToString());
				}



				if (actualshape != null && actualshape.isfrag == false)
				{
					Widgets.TextArea(rect_caltexttrue(inRect), "Pellet size");
				}


				Rect chargetext6969b = new Rect(inRect);
				chargetext6969b.width = 35f;
				chargetext6969b.height = 33f;
				chargetext6969b = chargetext6969b.CenteredOnXIn(inRect);
				chargetext6969b = chargetext6969b.CenteredOnYIn(inRect);
				chargetext6969b.x += 340f;
				chargetext6969b.y += -100f;
				if (actualshape != null && actualshape.isfrag == false)
				{
					Widgets.TextArea(chargetext6969b, this.PelCount.ToString());
				}



				Rect chargetext69696a = new Rect(inRect);
				chargetext69696a.width = 101f;
				chargetext69696a.height = 33f;
				chargetext69696a = chargetext69696a.CenteredOnXIn(inRect);
				chargetext69696a = chargetext69696a.CenteredOnYIn(inRect);
				chargetext69696a.x += 340f;
				chargetext69696a.y += -140f;
				if (actualshape != null && actualshape.isfrag == false)
				{
					Widgets.TextArea(chargetext69696a, "Pellet count");
				}


			}

			if (actualshape?.isfrag ?? false)
			{
				Rect rectplusbigfragment = new Rect(inRect);
				rectplusbigfragment.width = 33f;
				rectplusbigfragment.height = 33f;
				rectplusbigfragment = rectplusbigfragment.CenteredOnXIn(inRect);
				rectplusbigfragment = rectplusbigfragment.CenteredOnYIn(inRect);
				rectplusbigfragment.x += 307f;
				rectplusbigfragment.y += -25f;
				if (Widgets.ButtonText(rectplusbigfragment, "+"))
				{

					cunt++;
					//Log.Error(cunt.ToString());
				}
				Rect rectfragsmalltwo = new Rect(inRect);
				rectfragsmalltwo.width = 33f;
				rectfragsmalltwo.height = 33f;
				rectfragsmalltwo = rectfragsmalltwo.CenteredOnXIn(inRect);
				rectfragsmalltwo = rectfragsmalltwo.CenteredOnYIn(inRect);
				rectfragsmalltwo.x += 373f;
				rectfragsmalltwo.y += -25f;
				if (Widgets.ButtonText(rectfragsmalltwo, "-"))
				{
					if (cunt >= 1)
					{
						cunt -= 1;
					}

				}
				Rect chargetext6969 = new Rect(inRect);
				chargetext6969.width = 35f;
				chargetext6969.height = 33f;
				chargetext6969 = chargetext6969.CenteredOnXIn(inRect);
				chargetext6969 = chargetext6969.CenteredOnYIn(inRect);
				chargetext6969.x += 340f;
				chargetext6969.y += -25f;
				Widgets.TextArea(chargetext6969, cunt.ToString());
				Rect chargetext69696 = new Rect(inRect);
				chargetext69696.width = 101f;
				chargetext69696.height = 33f;
				chargetext69696 = chargetext69696.CenteredOnXIn(inRect);
				chargetext69696 = chargetext69696.CenteredOnYIn(inRect);
				chargetext69696.x += 340f;
				chargetext69696.y += -60f;
				Widgets.TextArea(chargetext69696, "Small fragments amount");

				if (SelectedProjectile != null)
				{
					kurwakurwakurwa = SelectedProjectile.comps.Find(g => g is CompProperties_Fragments) as CompProperties_Fragments;
				}

				if (SelectedProjectile != null)
				{


					Rect rectplusbigbigfragment = new Rect(inRect);
					rectplusbigbigfragment.width = 33f;
					rectplusbigbigfragment.height = 33f;
					rectplusbigbigfragment = rectplusbigbigfragment.CenteredOnXIn(inRect);
					rectplusbigbigfragment = rectplusbigbigfragment.CenteredOnYIn(inRect);
					rectplusbigbigfragment.x += 307f;
					rectplusbigbigfragment.y += -100f;
					if (Widgets.ButtonText(rectplusbigbigfragment, "+"))
					{
						var peni = (CompProperties_Fragments)SelectedProjectile?.comps.Find(G => G is CompProperties_Fragments);

						if (SelectedProjectile != null)
						{
							if (bigshrabint <= (peni?.fragments.Find(D => D.thingDef == AmmoClassesDefOf.Fragment_Large)?.count ?? -1) + 5)
							{
								bigshrabint += 1;
							}
						}



					}
					Rect rectfragbigtwo = new Rect(inRect);
					rectfragbigtwo.width = 33f;
					rectfragbigtwo.height = 33f;
					rectfragbigtwo = rectfragbigtwo.CenteredOnXIn(inRect);
					rectfragbigtwo = rectfragbigtwo.CenteredOnYIn(inRect);
					rectfragbigtwo.x += 373f;
					rectfragbigtwo.y += -100f;
					if (Widgets.ButtonText(rectfragbigtwo, "-"))
					{
						if (bigshrabint >= 1)
						{
							bigshrabint -= 1;
						}

					}
					Rect efdwsx = new Rect(inRect);
					efdwsx.width = 35f;
					efdwsx.height = 33f;
					efdwsx = efdwsx.CenteredOnXIn(inRect);
					efdwsx = efdwsx.CenteredOnYIn(inRect);
					efdwsx.x += 340f;
					efdwsx.y += -100f;
					Widgets.TextArea(efdwsx, bigshrabint.ToString());
					Rect sdfecw = new Rect(inRect);
					sdfecw.width = 101f;
					sdfecw.height = 33f;
					sdfecw = sdfecw.CenteredOnXIn(inRect);
					sdfecw = sdfecw.CenteredOnYIn(inRect);
					sdfecw.x += 340f;
					sdfecw.y += -135f;
					Widgets.TextArea(sdfecw, "Big fragments amount");
				}
			}
		}
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect1 = new Rect(inRect);
			rect1.width = HandLoadingWindow.Test.x + 16f;
			rect1.height = HandLoadingWindow.Test.y + 16f;
			rect1 = rect1.CenteredOnXIn(inRect);
			rect1 = rect1.CenteredOnYIn(inRect);
			rect1.x += 16f;
			rect1.y += 16f;
			Rect position = new Rect(rect1.xMin + (rect1.width - HandLoadingWindow.Test.x) / 2f - 10f, rect1.yMin + 20f, HandLoadingWindow.Test.x, HandLoadingWindow.Test.y);

			if (pint != 1)
			{
				ammodef_selected = AmmoClassesDefOf.Ammo_556x45mmNATO_FMJ;

				++pint;
			}


			///no idea tf is this
			Rect rect3 = new Rect(inRect);
			rect3.width = 20f;
			rect3.height = 20f;
			rect3 = rect3.CenteredOnXIn(inRect);
			rect3 = rect3.CenteredOnYIn(inRect);
			rect3.x += 16f;
			rect3.y += 16f;
			

			
			///choose caliber button
			if (Widgets.ButtonText(rectDesigner(inRect), "choose caliber"))
			{
				ChooseCaliberAction();
			}
			

			///this shit is the ammo shown in the middle, I think.
			if (MadeAmmoDef != null)
			{
				sometextureidk = MadeAmmoDef.uiIcon;
			}


			///choose proj. mat button
			if (Widgets.ButtonText(Rect51(inRect), "Choose Bullet's projectile material"))
			{
				ProjClick();
			}


		
			///casing mat button
			if (Widgets.ButtonText(rect_case(inRect), "Choose Bullet's case material"))
			{
				action_case();
			}



			//propellant button
			if (Widgets.ButtonText(rect_powder(inRect), "Choose propellant"))
			{
				action_powder();
			}


			///that's the rect and label for the little toggle for shells in upper right corner
			/// don't think that need to be split into it's own public void and rect
			Rect newshit = new Rect(inRect);
			newshit.width = 80f;
			newshit.height = 80f;
			newshit = newshit.CenteredOnXIn(inRect);
			newshit = newshit.CenteredOnYIn(inRect);
			newshit.x += 345f;
			newshit.y += -240f;
			Widgets.CheckboxLabeled(newshit, "hand load grenades and shells", ref this.isexplosiveprojectile);


			Rect rect69420 = new Rect(inRect);
			rect69420.width = 100f;
			rect69420.height = 100f;
			rect69420 = rect69420.CenteredOnXIn(inRect);
			rect69420 = rect69420.CenteredOnYIn(inRect);
			rect69420.x += -340f;
			rect69420.y += 240f;

		
			
			if (AmmoClassesDefOf.CE_AdvancedAmmo.IsFinished && !isexplosiveprojectile && !(actualshape?.isbootleq ?? true))
			{
				if (Widgets.ButtonText(rect_fillant(inRect), "Choose fillant"))
				{
					action_fillant();
				}
			}

			//text for charge amount rect
			Rect rect69252 = new Rect(inRect);
			rect69252.width = 80f;
			rect69252.height = 40f;
			rect69252 = rect69252.CenteredOnXIn(inRect);
			rect69252 = rect69252.CenteredOnYIn(inRect);
			rect69252.x += 343f;
			rect69252.y += 70f;

			//widget for the charge amount
			Widgets.TextArea(rect69252, "Charge amount above");

			//buttons for upping and decreasing the charge amount
			Rect rectcharge = new Rect(inRect);
			rectcharge.width = 33f;
			rectcharge.height = 33f;
			rectcharge = rectcharge.CenteredOnXIn(inRect);
			rectcharge = rectcharge.CenteredOnYIn(inRect);
			rectcharge.x += 307f;
			rectcharge.y += 25f;
			if (Widgets.ButtonText(rectcharge, "+"))
			{
				if (ChargeAmount <= maxcharge)
				{
					ChargeAmount += 0.10f;
				}

			}
			Rect rectchargeminus = new Rect(inRect);
			rectchargeminus.width = 33f;
			rectchargeminus.height = 33f;
			rectchargeminus = rectchargeminus.CenteredOnXIn(inRect);
			rectchargeminus = rectchargeminus.CenteredOnYIn(inRect);
			rectchargeminus.x += 373f;
			rectchargeminus.y += 25f;
			if (Widgets.ButtonText(rectchargeminus, "-"))
			{
				if (ChargeAmount >= 0.5)
				{
					ChargeAmount -= 0.10f;
				}

			}

			//rect and button for charge amount
			Rect chargetext = new Rect(inRect);
			chargetext.width = 35f;
			chargetext.height = 33f;
			chargetext = chargetext.CenteredOnXIn(inRect);
			chargetext = chargetext.CenteredOnYIn(inRect);
			chargetext.x += 340f;
			chargetext.y += 25f;
			Widgets.TextArea(chargetext, ChargeAmount.ToString());


			//Stuff for choosing fragment amount and pellet stats when loading fragmentation or multi shot projectiles
			frag_shit(inRect);



			if (Widgets.ButtonText(rect_shape(inRect), "Choose projectile type"))
			{
				button_shape();
			}


			//no idea.
			Rect rect7 = new Rect(inRect);
			rect7.width = 80f;
			rect7.height = 80f;
			rect7 = rect7.CenteredOnXIn(inRect);
			rect7 = rect7.CenteredOnYIn(inRect);
			rect7.x += 320f;
			rect7.y += -200f;



			//no idea.
			Rect rect27 = new Rect(inRect);
			rect27.width = 80f;
			rect27.height = 80f;
			rect27 = rect27.CenteredOnXIn(inRect);
			rect27 = rect27.CenteredOnYIn(inRect);
			//rect27.x += 320f;
			rect27.y += -90f;
			shing = Widgets.TextField(rect27, shing);


			//again no idea, probably color slider shit which I removed
			Rect rect28 = new Rect(inRect);
			rect28.width = 80f;
			rect28.height = 80f;
			rect28 = rect28.CenteredOnXIn(inRect);
			rect28 = rect28.CenteredOnYIn(inRect);
			//rect27.x += 320f;
			rect28.y += -45f;
			//Widgets.Label(rect28 ,"Color sliders");


			//fuckery for the stats shown
			ass_pain(inRect);

		

			
			List<ThingDef> list12345 = new List<ThingDef>();
			list12345.Add(CE_ThingDefOf.AmmoBench);

			if (Widgets.ButtonText(rect_finish(inRect), "Finish"))
			{
				finish_action();
			}




			Rect rect69 = new Rect(inRect);
			rect69.width = 100f;
			rect69.height = 100f;
			rect69 = rect69.CenteredOnXIn(inRect);
			rect69 = rect69.CenteredOnYIn(inRect);
			rect69.x += 0f;
			rect69.y += 0f;
			//colour = GUI.VerticalSlider(rect69, colour, 0, 255);
			Color col = new Color { r = colour, b = 0, a = 0, g = 0 };

			//tests for coloring ammo. doesn't work in current state
			if (MadeAmmoDef != null)
			{

				MadeAmmoDef.colorGenerator = new ColorGenerator_Single { color = col };
				MadeAmmoDef.uiIconColor = col;
				GUI.DrawTexture(position, MadeAmmoDef.uiIcon);
			}
			else
			{
				//ammo graphic shown in the middle
				GUI.DrawTexture(position, sometextureidk);
			}


			//upper label
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 32f), "Design bullets");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 32f;
			Rect rect2 = new Rect(32f, num, 240f, 24f);
		}
		public DamageDef secondarydamagedef;
		public bool usefillatn;

		public void tryfix()
		{
			FieldInfo[] projectus = typeof(ProjectileProperties).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			projectus.ToList().Find(tt33 => tt33.Name == "damageAmountBase").SetValue(MadeProjectile.projectile, (int)Math.Round(DamageCalculated));
		}
		public void CalculateDamage()
		{


			int DamageMult = new int();
			if (ProjectileShape == "Hollow point")
			{
				DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) * 2;

				Log.Message(DamageMult.ToString());
			}
			if (ProjectileShape == "Armor piercing")
			{
				DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;

				Log.Message(DamageMult.ToString());
			}
			if (ProjectileShape == "Full Metal Jacket")
			{
				DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f);

				Log.Message(DamageMult.ToString());
			}
			if (ProjectileShape == "Duplex")
			{
				DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;

				Log.Message(DamageMult.ToString());
			}
			if (ProjectileShape == "Sabot")
			{
				DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) / 2;

				Log.Message(DamageMult.ToString());
			}
			if (projectile_material.label == "Korwinium" && ammodef_selected.ammoClass == AmmoClassesDefOf.MusketBall)
			{
				DamageMult += 240;
				Log.Error(DamageMult.ToString());
			}
			float Damagemult2 = new float();

			//Log.Error(propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value.ToString());
			Damagemult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value;
			if (Damagemult2 == 0f)
			{
				//Log.Error("propelant most likely has no PowderPower statbase");
			}
			if (ProjectileShape == "Buckshot")
			{
				DamageCalculated = (float)Math.Round(8 * ChargeAmount * Damagemult2 * (pelcal / 10f) * skillmult);
			}
			else if (ProjectileShape == "Flechette")
			{
				DamageCalculated = (float)Math.Round(12 * ChargeAmount * Damagemult2 * (pelcal / 10f) * skillmult);
			}
			else
			{
				DamageCalculated = (float)Math.Round(DamageMult * Damagemult2 * (ChargeAmount) * skillmult);
			}



		}
		public void CalculateDamage2()
		{


			float DamageMult = 1f;

			DamageMult = SelectedProjectile.projectile.GetDamageAmount(1f) * actualshape.damagemult;

			float Damagemult2 = new float();

			//Log.Error(propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value.ToString());
			Damagemult2 = propelant.statBases.Find(abc => abc.stat.defName == "PowderPower").value;
			if (Damagemult2 == 0f)
			{
				//Log.Error("propelant most likely has no PowderPower statbase");
			}
			if (ProjectileShape == "Buckshot")
			{
				DamageCalculated = (float)Math.Round(8 * ChargeAmount * Damagemult2 * (pelcal / 10f) * skillmult);
			}
			else if (ProjectileShape == "Flechette")
			{
				DamageCalculated = (float)Math.Round(12 * ChargeAmount * Damagemult2 * (pelcal / 10f) * skillmult);
			}
			else
			{
				if (actualshape.dmgusestatpawn)
				{
					DamageCalculated = (float)Math.Round(DamageMult * Damagemult2 * (ChargeAmount / 2) * (skillmult * 0.75f));
				}
				else
				{
					DamageCalculated = (float)Math.Round(DamageMult * Damagemult2 * (ChargeAmount / 2) * 1);
				}
				
			}
			if (ChargeAmount < 2.01f && ChargeAmount > 1.20f)
			{
				MadeAmmoDef.label += " +P";
				MadeAmmoDef.ammoClass.label += " +P";
			}
			if (ChargeAmount > 2.01f)
			{
				MadeAmmoDef.label += " +P+";
				MadeAmmoDef.ammoClass.label += " +P+";
			}


		}

		public void CalculateSecDmg(string tring, RecipeDef recipe, List<SecondaryDamage> secondaries)
		{
			switch (tring)
			{
				case "FSX":
					fillantdef = CE_ThingDefOf.FSX;
					secondaries.Add(new SecondaryDamage { amount = (int)MadeProjectile.GetModExtension<BulletModExtension>().FixedDamage / 2, chance = 1f, def = DamageDefOf.Bomb });
					ThingFilter filteR = new ThingFilter { };
					filteR.AllowedThingDefs.ToList().Add(CE_ThingDefOf.FSX);
					filteR.SetAllow(CE_ThingDefOf.FSX, true);
					IngredientCount ingrcount = new IngredientCount { filter = filteR };
					ingrcount.SetBaseCount(2);
					recipe.ingredients.Add(ingrcount);
					MadeProjectile.label += " (HE)";
					break;
				case "prometheum":
					secondaries.Add(new SecondaryDamage { amount = (int)MadeProjectile.GetModExtension<BulletModExtension>().FixedDamage / 2, chance = 1f, def = CE_DamageDefOf.Flame_Secondary });
					fillantdef = AmmoClassesDefOf.Prometheum;
					ThingFilter filteRr = new ThingFilter { };
					filteRr.AllowedThingDefs.ToList().Add(AmmoClassesDefOf.Prometheum);
					filteRr.SetAllow(AmmoClassesDefOf.Prometheum, true);
					IngredientCount ingrcount2 = new IngredientCount { filter = filteRr };
					ingrcount2.SetBaseCount(2);
					recipe.ingredients.Add(ingrcount2);
					MadeProjectile.label += " (API)";
					break;
				case "EMP devices":
					secondaries.Add(new SecondaryDamage { amount = (int)MadeProjectile.GetModExtension<BulletModExtension>().FixedDamage / 4, chance = 1f, def = CE_DamageDefOf.Electrical });
					fillantdef = ThingDefOf.ComponentIndustrial;
					ThingFilter filteRrR = new ThingFilter { };
					filteRrR.AllowedThingDefs.ToList().Add(ThingDefOf.ComponentIndustrial);
					filteRrR.SetAllow(ThingDefOf.ComponentIndustrial, true);
					IngredientCount ingrcount2e = new IngredientCount { filter = filteRrR };
					ingrcount2e.SetBaseCount(2);
					recipe.ingredients.Add(ingrcount2e);
					MadeProjectile.label += " (EMP)";
					break;
				case "nothing":
					fillantdef = null;
					break;
			}

		}

		public ThingDef bigshrap;
		public ThingDef smolshrab;
		public float colour;
		public void MakeFrags()
		{
			ThingDef shrab = new ThingDef { thingClass = typeof(HandLoading.Bullet), tickerType = TickerType.Normal, graphic = AmmoClassesDefOf.Fragment_Small.graphic, graphicData = AmmoClassesDefOf.Fragment_Small.graphicData, uiIcon = AmmoClassesDefOf.Fragment_Small.uiIcon, projectile = new ProjectilePropertiesCE() { flyOverhead = false, pelletCount = 1, alwaysFreeIntercept = true, speed = AmmoClassesDefOf.Fragment_Small.projectile.speed * ChargeAmount, damageDef = DamageDefOf.Bullet, armorPenetrationSharp = 1 * hardness_material_multiplier * ChargeAmount, armorPenetrationBlunt = 12 * hardness_material_multiplier * ChargeAmount }, defName = Rand.Range(0, 6896).ToString() + "d" + Rand.Range(0, 231) + "ghj", label = "custom shrabel" };
			smolshrab = shrab;
			ProjectilePropertiesCE projproceagain = shrab.projectile as ProjectilePropertiesCE;
			//projproceagain.gravityFactor = 15;
			projproceagain.speed = 20f;

		}

		public float DamageCalculated;
	}

	public class PowderComp : ThingComp
	{
		public float Power => Props.Power;
		public PowderCompProps Props => (PowderCompProps)this.props;

		public override void Initialize(CompProperties props)
		{
			Log.Message("Initialized.");

		}
	}

	public class PowderCompProps : CompProperties
	{
		public float Power;
		//public float power = null;

		public PowderCompProps()
		{
			this.compClass = typeof(PowderComp);
		}

		public PowderCompProps(Type compClass) : base(compClass)
		{
			this.compClass = compClass;
		}
	}
}
