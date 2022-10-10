using CombatExtended;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;
using Verse;

namespace HandLoading
{
    public class othersomethingsets : ModSettings
    {


        public bool Open;

        public bool Silly;

        public int factionammos;


        public override void ExposeData()
        {

            Scribe_Values.Look<int>(ref factionammos, "factionammus");
            Scribe_Values.Look<bool>(ref Open, "Open");
            base.ExposeData();
        }
    }
    public class HandeLoading : Mod
    {
        public static othersomethingsets settings;


        public HandeLoading(ModContentPack content) : base(content)
        {


            settings = GetSettings<othersomethingsets>();


        }
        public override void DoSettingsWindowContents(Rect inRect)
        {

            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.GapLine(60);
            listingStandard.Label("Faction hand loads max amount");
            settings.factionammos = (int)listingStandard.Slider(settings.factionammos, 0, 30);
            listingStandard.Gap(5);
            listingStandard.Label(settings.factionammos.ToString());
            listingStandard.GapLine(60);


            string workingDirectory = Environment.CurrentDirectory;
            string actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569";


            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/Hand-Loading"))
            {

                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/Hand-Loading";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/2548930569"))
            {


                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/2548930569";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569"))
            {

                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer"))
            {

                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer";
            }

            string[] stufsf = Directory.GetDirectories(actualpathtomod + "/");




            listingStandard.Gap(60);
            listingStandard.CheckboxLabeled("Create seconday mod folder used as container for hand loading saving", ref settings.Open, "dew it");
            listingStandard.TextEntry("IF YOU CHOOSE TO DO IT REMEMBER TO ADD THE CONTAINER TO YOUR MODLIST");
            listingStandard.TextEntry("container is created after first ammo is created with the settings on");
            listingStandard.Gap(60);
            listingStandard.CheckboxLabeled("Allow all guns to be rebareled to any caliber", ref settings.Silly, "");
           
           
              
          
            listingStandard.Gap(120);

           

            if (listingStandard.ButtonText("Remove all ammos"))
            {


                Log.Message("2");
                List<ThingDef> filestoyeet = new List<ThingDef>();
                //Log.Error(ThingDefOf.Door.fileName);
                //filestoyeet.Add(ThingDefOf.AncientBed);

                string[] stuff = Directory.GetDirectories(actualpathtomod + "/");
                foreach (string s in stuff)
                {
                    string[] vs = Directory.GetDirectories(s + "/");
                    vs.ToList().RemoveAll(OO => OO.Contains("Textures"));
                    vs.ToList().RemoveAll(OO => OO.Contains("Source"));
                    vs.ToList().RemoveAll(OO => OO.Contains("Sounds"));
                    foreach (string fathouse in vs)
                    {
                        //Log.Message(fathouse);
                        string[] again = Directory.GetFiles(fathouse + "/");
                        foreach (string cos in again)
                        {
                            Log.Message(cos);
                            File.Delete(cos);
                        }
                    }
                    //Log.Message(s);
                }
                Log.Message(stuff.ToString());

            }

            


            listingStandard.Gap(20);


            foreach (string a in stufsf)
            {
                string[] vs = Directory.GetDirectories(a + "/");
                vs.ToList().RemoveAll(OO => OO.Contains("Textures"));
                foreach (string fathouse in vs)
                {
                    //Log.Message(fathouse);
                    string[] again = Directory.GetFiles(fathouse + "/");
                    foreach (string cos in again)
                    {
                        //Log.Message(cos);
                        if (ghjkl == null)
                        {
                            ghjkl = new List<string>();
                        }
                        if (!cos.Contains("sln"))
                        {
                            ghjkl.Add(cos);
                        }

                        //File.Delete(cos);
                    }
                }
            }
            





            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public List<string> ghjkl;
        public override string SettingsCategory()
        {
            return "Hand loading";
        }
    }
    public class SaverComp : WorldComponent
    {
        public bool myExampleBool = true;



        public SaverComp(World world) : base(world)
        {
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref abc, "abc");
            Log.Message(abc.ToString());

        }
        public override void FinalizeInit()
        {

            foreach (RecipeDef recdef in DefDatabase<RecipeDef>.AllDefs.ToList().FindAll(o => (o.modContentPack?.Name ?? "a") == "Hand loads [Beta]" && (!o.HasModExtension<BootlegExtension>())))
            {
                //Log.Message(recdef.ToString());
                if (recdef.recipeUsers == null)
                {
                    recdef.recipeUsers = new List<ThingDef>();
                }
                if (recdef.recipeUsers != null)
                {
                    recdef.recipeUsers.Add(CE_ThingDefOf.AmmoBench);
                }


            }
            foreach (RecipeDef recdef in DefDatabase<RecipeDef>.AllDefs.ToList().FindAll(o => (o.modContentPack?.Name ?? "a") == "Hand loads [Beta]" && (o.HasModExtension<BootlegExtension>())))
            {
                //Log.Message(recdef.ToString());
                if (recdef.recipeUsers == null)
                {
                    recdef.recipeUsers = new List<ThingDef>();
                }
                if (recdef.recipeUsers != null)
                {
                    recdef.recipeUsers.Add(AmmoClassesDefOf.CraftingSpot);
                }


            }

            foreach (RecipeDef recdef in DefDatabase<RecipeDef>.AllDefs.ToList().FindAll(o => (o.modContentPack?.Name ?? "a") == "Hand Loads Saved Ammo Data Container"))
            {
                //Log.Message(recdef.ToString());
                if (recdef.recipeUsers == null)
                {
                    recdef.recipeUsers = new List<ThingDef>();
                }
                if (recdef.recipeUsers != null)
                {
                    recdef.recipeUsers.Add(CE_ThingDefOf.AmmoBench);
                }


            }


            base.FinalizeInit();




        }

        public othersomethingsets sets;
        public void SaveAmmodef(AmmoDef ammo = null, AmmoCategoryDef ammoCategory = null, ThingDef projectile = null, AmmoSetDef ammoset = null, RecipeDef recpe = null, float someamount = 0f, ThingDef damImtired = null, float re = 0f, bool amifrag = false, bool amItrash = false, bool npcammo = false, bool amIthermo = false)
        {
            sets = HandeLoading.settings;
            abc++;





            string workingDirectory = Environment.CurrentDirectory;
            Log.Error(Directory.GetParent(workingDirectory).Parent.FullName);
            if (sets.Open && !(Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer")))
            {
                Log.Message("Creating container directories");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs/ammos");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs/bullets");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs/categories");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs/recpe");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Patches");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Patches/codemade");
                Directory.CreateDirectory(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/About");
                XmlDocument docsaver = new XmlDocument();
                docsaver.LoadXml("<ModMetaData><name>Hand Loads Saved Ammo Data Container</name><author>Caulaflower</author><packageId>Caulaflower.HLContainer.CustomAmmoContainer</packageId><supportedVersions><li>1.2</li><li>1.3</li></supportedVersions><url>Link</url><description>Container with your created ammo.</description></ModMetaData>"); //Your string here

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writersaver = new XmlTextWriter(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/About/" + "About" + ".xml", null);
                abc++;
                writersaver.Formatting = Formatting.Indented;
                docsaver.Save(writersaver);
            }
            string actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569/Defs";

            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/Hand-Loading"))
            {
                Log.Message("saved ammo in github version files");
                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/Hand-Loading/Defs";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/2548930569"))
            {
                Log.Message("saved ammo in rimpy version files");

                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/2548930569/Defs";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569/Defs"))
            {
                Log.Message("saved ammo in steam version files");
                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/workshop/content/294100/2548930569/Defs";
            }
            if (Directory.Exists(Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer"))
            {
                Log.Message("saved ammo in container files. The messages are fucked, this is definitely the actual one");
                actualpathtomod = Directory.GetParent(workingDirectory).Parent.FullName + "/common/Rimworld/Mods/HLContainer/Defs";
            }






            if (ammoCategory != null)
            {
                Log.Message("saving ammo category");
                XmlDocument doc2 = new XmlDocument();
                doc2.LoadXml(
                    "<Defs><CombatExtended.AmmoCategoryDef><defName>" + ammoCategory.defName + "</defName><label>" + ammoCategory.label + "</label><labelShort>" + ammoCategory.label + "</labelShort><description>" + ammo.description + "</description></CombatExtended.AmmoCategoryDef></Defs>"); //Your string here

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer2 = new XmlTextWriter(actualpathtomod + "/categories/category" + abc.ToString() + ".xml", null);
                abc++;
                writer2.Formatting = Formatting.Indented;
                doc2.Save(writer2);

            }
            if (ammo != null)
            {
                Log.Message("saving ammo");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(
                    "<Defs><ThingDef Class='CombatExtended.AmmoDef'><defName>" + ammo.defName + "</defName>"
                    + "<thingClass>" + ammo.thingClass + "</thingClass>"
                    + "<label>" + ammo.label + "</label>"
                    + "<tickerType>" + ammo.tickerType + "</tickerType>"
                    + "<thingCategories><li>ResourcesRaw</li><li>HandsLoadedAmmo</li></thingCategories>"
                    + "<ammoClass>" + ammoCategory.defName + "</ammoClass>"
                    + "<modExtensions><li Class='HandLoading.modextmisc'>" + "<upset>" + ammo.AmmoSetDefs.First().defName + "</upset><prjder>" + projectile.defName + "</prjder></li></modExtensions>"
                    + "<comps><li Class='HandLoading.damagercproprs'><dammpoints>" + re.ToString() + "</dammpoints></li><li><compClass>HandLoading.HaulComp</compClass></li></comps>"
                    + "<graphicData>" + "<texPath>" + ammo.graphic.path + "</texPath>"
                    + "<graphicClass>" + ammo.graphicData.graphicClass.ToString() + "</graphicClass>" + "</graphicData>"
                    + "<selectable>true</selectable><alwaysHaulable>true</alwaysHaulable><stackLimit>50000</stackLimit>" +
                    "<drawGUIOverlay>true</drawGUIOverlay><statBases>" + "<Bulk>" + ammo.statBases.Find(a => a.stat == CE_StatDefOf.Bulk).value.ToString() + "</Bulk>" + "<Mass>" + ammo.statBases.Find(a => a.stat == StatDefOf.Mass).value.ToString() + "</Mass>" + "</statBases></ThingDef></Defs>"); //Your string here


                XmlTextWriter writer = new XmlTextWriter(actualpathtomod + "/ammos/ammo" + abc.ToString() + ".xml", null);
                doc.Save(writer);
            }





            if (projectile != null)
            {

                ProjectilePropertiesCE projpropsCE = projectile.projectile as ProjectilePropertiesCE;
                Log.Error("saving projectile");
                XmlDocument doc3 = new XmlDocument();
                if (!amifrag)
                {
                    if ((projpropsCE.secondaryDamage?.Count ?? 0) >= 1)
                    {
                        Log.Message("amogussy");
                        doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + projectile.graphicData.texPath + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'>" + "<damageDef>Bullet</damageDef>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + projectile.GetModExtension<BulletModExtension>().FixedDamage.ToString() + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp>" + "<armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "<secondaryDamage><li><def>" + projpropsCE.secondaryDamage.First().def + "</def><amount>" + projpropsCE.secondaryDamage.First().amount + "</amount></li></secondaryDamage></projectile>" + "</ThingDef></Defs>"); //Your string here
                    }
                    else
                    {
                        if (projectile.comps?.Any(K => K is CompProperties_Fragments) ?? false)
                        {
                            Log.Error("test test test");
                            CompProperties_Fragments kurwa = projectile.comps.Find(G => G is CompProperties_Fragments) as CompProperties_Fragments;
                            if (kurwa != null)
                            {
                                Log.Message("test");
                                doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + projectile.graphicData.texPath + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'>" + "<damageDef>Bullet</damageDef>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + projectile.GetModExtension<BulletModExtension>().FixedDamage.ToString() + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp>" + "<armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "</projectile>" + "<comps><li Class='CombatExtended.CompProperties_Fragments'><fragments>" +
                                 "<" + kurwa.fragments.First().thingDef.defName + ">" + kurwa.fragments.First().count + "</" + kurwa.fragments.First().thingDef.defName + ">" + "</fragments></li></comps>" + "</ThingDef></Defs>"); //Your string here
                            }


                        }
                        else
                        {
                            if (projpropsCE.pelletCount >= 2)
                            {
                                Log.Message("amogus 3");
                                doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + "Things/Ammo/Cannon/HE" + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'><spreadMult>" + projpropsCE.spreadMult.ToString() + "</spreadMult>" + "<damageDef>Bullet</damageDef>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + projectile.GetModExtension<BulletModExtension>().FixedDamage.ToString() + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp><pelletCount>" + projpropsCE.pelletCount.ToString() + "</pelletCount><armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "</projectile>" + "</ThingDef></Defs>"); //Your string here
                            }
                            else
                            {
                                Log.Message("amogus");
                                doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + projectile.graphicData.texPath + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'>" + "<damageDef>Bullet</damageDef>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + (projectile.GetModExtension<BulletModExtension>()?.FixedDamage.ToString() ?? "5") + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp>" + "<armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "</projectile>" + "</ThingDef></Defs>"); //Your string here
                            }

                        }

                    }
                }

                if (amifrag)
                {
                    if (projpropsCE == null)
                    {
                        Log.Message("how is this even possible");
                    }
                    doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + projectile.graphicData.texPath + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'>" + "<damageDef>Bullet</damageDef>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + "5" + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp>" + "<armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "</projectile>" + "</ThingDef></Defs>"); //Your string here
                }
                if (amIthermo | projpropsCE.damageDef.defName == "Thermobaric")
                {
                    Log.Warning("test test, works");
                    doc3.LoadXml("<Defs><ThingDef><defName>" + projectile.defName + "</defName>" + "<tickerType>Normal</tickerType>" + "<graphicData>" + "<texPath>" + projectile.graphicData.texPath + "</texPath>" + "<graphicClass>Graphic_Single</graphicClass>" + "</graphicData>" + "<thingClass>CombatExtended.BulletCE</thingClass>" + "<label>" + projectile.label + "</label>" + "<projectile Class='CombatExtended.ProjectilePropertiesCE'>" + "<damageDef>Thermobaric</damageDef>" + "<explosionRadius>" + projpropsCE.explosionRadius + "</explosionRadius>" + "<speed>" + projpropsCE.speed.ToString() + "</speed>" + "<dropsCasings>true</dropsCasings>" + "<damageAmountBase>" + projectile.GetModExtension<HandLoading.BulletModExtension>().FixedDamage.ToString() + "</damageAmountBase>" + "<armorPenetrationSharp>" + projpropsCE.armorPenetrationSharp.ToString() + "</armorPenetrationSharp>" + "<armorPenetrationBlunt>" + projpropsCE.armorPenetrationBlunt.ToString() + "</armorPenetrationBlunt>" + "</projectile>" + "</ThingDef></Defs>"); //Your string here
                }


                XmlTextWriter writer3 = new XmlTextWriter(actualpathtomod + "/bullets/projectile" + abc.ToString() + ".xml", null);

                writer3.Formatting = Formatting.Indented;
                doc3.Save(writer3);
            }




            






            if (ammo != null && projectile != null && ammoset != null && !npcammo && false)
            {
                Log.Message("saving patch");
                XmlDocument doc4 = new XmlDocument();
                doc4.LoadXml("<Patch><Operation Class='PatchOperationAdd'><xpath>/Defs/CombatExtended.AmmoSetDef[defName='" + ammoset.defName + "']/ammoTypes" + "</xpath><value>" + "<" + ammo.defName + ">" + projectile.defName + "</" + ammo.defName + ">" + "</value></Operation></Patch>"); //Your string here

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer4 = new XmlTextWriter(actualpathtomod + "/../Patches/codemade/patchnumber" + Rand.Range(0, 56709723).ToString() + ".xml", null);

                writer4.Formatting = Formatting.Indented;
                doc4.Save(writer4);
            }
            if (ammo != null && projectile != null && ammoset != null && npcammo && false)
            {
                Log.Message("saving patch");
                XmlDocument doc4 = new XmlDocument();
                doc4.LoadXml("<Patch><Operation Class='PatchOperationAdd'><xpath>/Defs/CombatExtended.AmmoSetDef[defName='" + ammoset.defName + "']/ammoTypes" + "</xpath><value>" + "<" + ammo.defName + ">" + projectile.defName + "</" + ammo.defName + ">" + "</value></Operation></Patch>"); //Your string here

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer4 = new XmlTextWriter(actualpathtomod + "/../Patches/codemade/patchnumber" + Rand.Range(0, 56709723).ToString() + "identifier" + ".xml", null);

                writer4.Formatting = Formatting.Indented;
                doc4.Save(writer4);
            }


            if (recpe != null)
            {
                Log.Message("saving recipe");
                XmlDocument doc5 = new XmlDocument();
                ProjectilePropertiesCE projpropsCEr = projectile.projectile as ProjectilePropertiesCE;
                //&& (recpe.HasModExtension<BootlegExtension>())
                if (projpropsCEr.secondaryDamage.Count >= 1)
                {
                   
                    doc5.LoadXml("<Defs><RecipeDef ParentName=" + '"' + "AmmoRecipeBase" + '"' + "><defName>" + recpe.defName + "</defName>" + "<label>" + "Make " + ammo.label + "(" + recpe.products.First().count.ToString() + ")" + "</label>" + "<description>Make custom ammo</description>" + "<jobString>Making custom ammunition</jobString>" + "<ingredients><li><filter><thingDefs><li>" + recpe.ingredients.Find(a => a.filter.Allows(CE_ThingDefOf.FSX) | a.filter.Allows(AmmoClassesDefOf.Prometheum)).filter.AnyAllowedDef.defName + "</li>" + "</thingDefs>" + "</filter><count>" + "2" + "</count>" + "</li><li><filter><thingDefs><li>" + recpe.ingredients.First().filter.AnyAllowedDef.defName + "</li>" + "</thingDefs>" + "</filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count>" + "</li>" + "<li><filter>" + "<thingDefs><li>" + recpe.ingredients.Find(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() != null).filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + someamount + "</count></li>" + "<li><filter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count></li>" + "</ingredients>" + "<fixedIngredientFilter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></fixedIngredientFilter><products>" + "<" + ammo.defName + ">" + recpe.products.First().count.ToString() + "</" + ammo.defName + ">" + "</products><workAmount>" + recpe.workAmount + "</workAmount></RecipeDef></Defs>"); //Your string here
                    
                }
                else
                {
                    if (recpe.HasModExtension<BootlegExtension>())
                    {
                        //recpe.modExtensions
                        doc5.LoadXml("<Defs><RecipeDef ParentName=" + '"' + "AmmoRecipeBase" + '"' + "><modExtensions>" + "<li Class='HandLoading.BootlegExtension' />" + "</modExtensions><defName>" + recpe.defName + "</defName>" + "<label>" + "Make " + ammo.label + "(" + recpe.products.First().count.ToString() + ")" + "</label>" + "<description>Make custom ammo</description>" + "<jobString>Making custom ammunition</jobString>" + "<ingredients><li><filter><thingDefs><li>" + recpe.ingredients.First().filter.AnyAllowedDef.defName + "</li>" + "</thingDefs>" + "</filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count>" + "</li>" + "<li><filter>" + "<thingDefs><li>" + recpe.ingredients.Find(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() != null).filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + someamount + "</count></li>" + "<li><filter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count></li>" + "</ingredients>" + "<fixedIngredientFilter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></fixedIngredientFilter><products>" + "<" + ammo.defName + ">" + recpe.products.First().count.ToString() + "</" + ammo.defName + ">" + "</products><workAmount>" + recpe.workAmount + "</workAmount></RecipeDef></Defs>"); //Your string here
                    }
                    else 
                    {
                        doc5.LoadXml("<Defs><RecipeDef ParentName=" + '"' + "AmmoRecipeBase" + '"' + "><defName>" + recpe.defName + "</defName>" + "<label>" + "Make " + ammo.label + "(" + recpe.products.First().count.ToString() + ")" + "</label>" + "<description>Make custom ammo</description>" + "<jobString>Making custom ammunition</jobString>" + "<ingredients><li><filter><thingDefs><li>" + recpe.ingredients.First().filter.AnyAllowedDef.defName + "</li>" + "</thingDefs>" + "</filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count>" + "</li>" + "<li><filter>" + "<thingDefs><li>" + recpe.ingredients.Find(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() != null).filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + someamount + "</count></li>" + "<li><filter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></filter><count>" + recpe.ingredients.First().GetBaseCount() + "</count></li>" + "</ingredients>" + "<fixedIngredientFilter><thingDefs><li>" + recpe.ingredients.FindAll(a => ThingMaker.MakeThing(a.filter.AnyAllowedDef).TryGetComp<PowderComp>() == null && !a.filter.Allows(CE_ThingDefOf.FSX) && !a.filter.Allows(AmmoClassesDefOf.Prometheum)).Last().filter.AnyAllowedDef.defName + "</li></thingDefs></fixedIngredientFilter><products>" + "<" + ammo.defName + ">" + recpe.products.First().count.ToString() + "</" + ammo.defName + ">" + "</products><workAmount>" + recpe.workAmount + "</workAmount></RecipeDef></Defs>"); //Your string here
                    }
                   
                }


                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer5 = new XmlTextWriter(actualpathtomod + "/recpe/recipe" + Rand.Range(0, 56709723).ToString() + ".xml", null);

                writer5.Formatting = Formatting.Indented;
                doc5.Save(writer5);
            }



        }



        public int abc;
        public string ammolabel;
        public Type ammotype;
        public string defnameofammo;
        public IEnumerable<AmmoDef> ammodefmade;
    }

    [StaticConstructorOnStartup]
    public class ActualPatching
    {
        static ActualPatching()
        {
            foreach (AmmoDef amder in DefDatabase<AmmoDef>.AllDefsListForReading.FindAll(t => t.modExtensions != null && t.HasModExtension<modextmisc>()))
            {
                //Log.Message(amder.label);
                amder.GetModExtension<modextmisc>().upset.ammoTypes.Add(new AmmoLink {ammo = amder, projectile = amder.GetModExtension<modextmisc>().prjder});
            }
        }
    }

    public class modextmisc : DefModExtension
    {
        public AmmoSetDef upset;

        public ThingDef prjder;
    }

    [StaticConstructorOnStartup]
    public class FuckedPatchRemover
    {
        static FuckedPatchRemover()
        {
            List<PatchOperation> patches = Verse.LoadedModManager.RunningModsListForReading.Find(F => F.Name == "Hand loads [Beta]").Patches.ToList();
            Log.Message(Verse.LoadedModManager.RunningModsListForReading.Find(F => F.Name == "Hand loads [Beta]").RootDir);
            foreach (PatchOperationAdd patch in patches)
            {
                string tring = System.IO.File.ReadAllText(patch.sourceFile);
                tring = tring.Trim();
                var s = tring;
                string[] subs = s.Split('/');
                foreach (string sub in subs)
                {
                    if (sub.Contains("value"))
                    {
                        string amogus = sub;
                        string[] sub2 = sub.Split('<');
                        //Log.Message($"Substring: {amogus}");
                        foreach (string sub44 in sub2)
                        {

                            if(sub44.Contains("True") && sub44.Contains("False"))
                            {
                                
                                string[] sub3 = sub44.Split('>');
                                foreach (string sub444 in sub3)
                                {
                                    //Log.Message($"{sub444}");
                                    if (DefDatabase<AmmoDef>.AllDefsListForReading.Any(pp => pp.defName == sub444) | DefDatabase<ThingDef>.AllDefsListForReading.Any(pp => pp.defName == sub444))
                                    {
                                        //Log.Error("exists");
                                    }
                                    else
                                    {
                                        
                                        Log.Error("doesn't exist: " + patch.sourceFile);
                                        File.Delete(patch.sourceFile);
                                    }
                                }
                                   
                            }
                            
                            

                        }
                     
                    }


                }
                //Log.Message(tring);
            }
            
        }
    }
}
