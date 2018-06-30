using System;
using System.Reflection;
using GlobalEnums;
using Modding;
 
namespace GodKillers
{
    /// <summary>
    /// The main mod class
    /// </summary>
    /// <remarks>This configuration has settings that are save specific</remarks>
    public class GodKillers : Mod
    {
 
        private int _NailDamageTracker;
 
        public int defaultNailDamage;    
 
        private UnityEngine.Vector3 defaultSlashScale;
 
        private UnityEngine.Vector3 defaultAltScale;
 
        private UnityEngine.Vector3 defaultUpScale;
 
        private UnityEngine.Vector3 defaultDownScale;
 
        public int counter;
 
        public double QSAttackDuration;
 
        public double QSCooldown;
 
        public int Punishment;
 
        /// <summary>
        /// Represents this Mod's instance.
        /// </summary>
        internal static GodKillers Instance;
 
        /// <summary>
        /// Fetches the Mod Version From AssemblyInfo.AssemblyVersion
        /// </summary>
        /// <returns>Mod's Version</returns>
        public override string GetVersion() => "1.0.2";
 
 
 
        /// <summary>
        /// Called after the class has been constructed.
        /// </summary>
        public override void Initialize()
        {
            //Assign the Instance to the instantiated mod.
            Instance = this;
 
            Log("Initializing");
 
            //Here we are hooking into the AttackHook so we can modify the damage for the attack.
            ModHooks.Instance.AttackHook += OnAttack;
 
            Modding.ModHooks.Instance.CharmUpdateHook += CharmUpdate;
 
            ModHooks.Instance.AfterAttackHook += OnAfterAttack;
 
            ModHooks.Instance.LanguageGetHook += Description;
            //UnityEngine.GameObject.Find("Charms").LocateMyFSM("UI Charms").ChangeState("Royal?", "CANCEL", "Equipped");
 
            Log("Initialized");
        }
 
 
        /// <summary>
        /// Calculates Crits on attack
        /// </summary>
        /// <remarks>
        /// This checks if we have FoTF. If we do Damage is calculated based on lost masks. otherwise we revert back to normal
        /// </remarks>
        /// <param name="dir"></param>
        public void OnAttack(AttackDirection dir)
        {
            Log("Attacking");
 
            defaultNailDamage = 5 + 4 * PlayerData.instance.GetInt("nailSmithUpgrades");
 
            if (PlayerData.instance.GetInt("nailDamage") != defaultNailDamage)
            {
                PlayerData.instance.SetInt("nailDamage", defaultNailDamage);
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            }
 
            if (PlayerData.instance.GetBool("equippedCharm_32"))
            {
 
                PlayerData.instance.SetInt("nailDamage",
 
                        ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.55)));
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            }
 
            if (PlayerData.instance.GetBool("equippedCharm_13") && PlayerData.instance.GetBool("equippedCharm_18"))
            {
                PlayerData.instance.overcharmed = true;
 
                PlayerData.instance.SetInt("nailDamage",
 
                        ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.65)));
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            }
 
            else if (PlayerData.instance.GetBool("equippedCharm_13"))
            {
                PlayerData.instance.overcharmed = false;
 
                PlayerData.instance.SetInt("nailDamage",
 
                        ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.8)));
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            }
 
            else if (PlayerData.instance.GetBool("equippedCharm_18"))
            {
                PlayerData.instance.overcharmed = false;
 
                PlayerData.instance.SetInt("nailDamage",
 
                        ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.9)));
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
            }
 
            if (PlayerData.instance.GetBool("equippedCharm_6") && PlayerData.instance.GetBool("equippedCharm_25"))
            {
                Log("Super Attack");
 
                {
                    PlayerData.instance.overcharmed = true;
 
                    if (PlayerData.instance.GetInt("health") < (PlayerData.instance.GetInt("maxHealth")) - 1 && PlayerData.instance.GetInt("health") > 2)
                    {
 
                        PlayerData.instance.SetInt("nailDamage",
 
                        ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.5)));
 
                        PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                        _NailDamageTracker = PlayerData.instance.GetInt("nailDamage");
 
                        Log("Set Nail Damage to to " + _NailDamageTracker);
                    }
 
                    else if (PlayerData.instance.GetInt("health") > (PlayerData.instance.GetInt("maxHealth")) - 1)
                    {
 
                        PlayerData.instance.SetInt("nailDamage",
 
                        (int)(PlayerData.instance.GetInt("nailDamage") * (double)0.15 * (1 + 2.0 * ((PlayerData.instance.GetInt("maxHealth") - ((PlayerData.instance.GetInt("maxHealth") - 1)) / (double)PlayerData.instance.GetInt("maxHealth"))))));
 
                        PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                    }
 
                    else if (PlayerData.instance.GetInt("health") < 3)
                    {
 
                        PlayerData.instance.SetInt("nailDamage",
 
                        (int)(PlayerData.instance.GetInt("nailDamage") * (double)0.15 * (1 + 2.0 * ((PlayerData.instance.GetInt("maxHealth") - ((PlayerData.instance.GetInt("maxHealth") - 1)) / (double)PlayerData.instance.GetInt("maxHealth"))))));
 
                        PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                        _NailDamageTracker = PlayerData.instance.GetInt("nailDamage");
 
                        Log("Set Nail Damage to to " + _NailDamageTracker);
                    }
 
 
                }
            }
            else if (PlayerData.instance.GetBool("equippedCharm_6"))
            {
                Log("FotF Attack");
 
                PlayerData.instance.SetInt("nailDamage",
 
                    (int)(PlayerData.instance.GetInt("nailDamage") * (1 + 2.0 * ((PlayerData.instance.GetInt("maxHealth") - PlayerData.instance.GetInt("health")) / (double)PlayerData.instance.GetInt("maxHealth")))));
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                _NailDamageTracker = PlayerData.instance.GetInt("nailDamage");
 
                Log("Set Nail Damage to to " + _NailDamageTracker);
 
            }
            else if (PlayerData.instance.GetBool("equippedCharm_25"))
            {
                Log("Fragile Attack");
 
                if (PlayerData.instance.GetInt("health") == (PlayerData.instance.GetInt("maxHealth")))
                {
 
                    PlayerData.instance.overcharmed = true;
 
                    PlayerData.instance.SetInt("nailDamage",
 
                    ((int)(PlayerData.instance.GetInt("nailDamage") * (double)2.0)));
 
                    PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                }
 
                else if (PlayerData.instance.GetInt("health") != (PlayerData.instance.GetInt("maxHealth")))
                {
 
                    PlayerData.instance.overcharmed = false;
 
                    PlayerData.instance.SetInt("nailDamage",
 
                    ((int)(PlayerData.instance.GetInt("nailDamage") * (double)0.75 - ((PlayerData.instance.GetInt("maxHealth") - PlayerData.instance.GetInt("health"))))));
 
                    PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                    _NailDamageTracker = PlayerData.instance.GetInt("nailDamage");
 
                    Log("Set Nail Damage to to " + _NailDamageTracker);
                }
                else if (!PlayerData.instance.GetBool("equippedCharm_6") && !PlayerData.instance.GetBool("equippedCharm_25"))
                {
                    PlayerData.instance.SetInt("nailDamage", defaultNailDamage);
                }
            }
        }
 
        string Description(string key, string sheet)
        {
            string ret = Language.Language.GetInternal(key, sheet);
            if (sheet == "UI")
            {
                switch (key)
                {
                    case "CHARM_DESC_13":
                        ret = @"Freely given by the Mantis Tribe to those they respect.
 
Turns the nail into a Lance, greatly increasing its range, but decreasing its strength. Allows the bearer to strike foes from further away.";
                        break;
 
                    case "CHARM_DESC_18":
                        ret = @"Turns the nail into a Lance, increasing its range, but slightly decreasing its strength.";
                        break;
 
                    case "CHARM_DESC_25":
                        ret = @"Strengthens the bearer, greatly increasing the damage they deal to enemies when at full health,
But once the bearer loses health they will lose the power as well, only gaining it back after focusing.
 
This charm is fragile, and will break if its bearer is killed.";
                        break;
 
                    case "CHARM_DESC_25_G":
                        ret = @"Strengthens the bearer, greatly increasing the damage they deal to enemies when at full health,
But once the bearer loses health they will lose the power as well, only gaining it back after focusing.
 
This charm is ubreakable.";
                        break;
 
                    case "CHARM_DESC_32":
                        ret = @"Born from imperfect, discarded weapons that have fused together. The weapons still long to be used.
The Bearer will attack much more rapidly, but the attacks will be weaker";
                        break;
 
                    case "CHARM_NAME_18":
                        ret = @"Lance Nail";
                        break;
 
                    case "CHARM_NAME_13":
                        ret = @"Lance of Pride";
                        break;
                }
            }
            return ret;
        }
        private static FieldInfo whatever = typeof(HeroController).GetField("slashComponent", BindingFlags.NonPublic | BindingFlags.Instance);
 
        void CharmUpdate(PlayerData data, HeroController controller)
        {
 
            HeroController.instance.ATTACK_DURATION = 0.41f;
 
            HeroController.instance.ATTACK_COOLDOWN_TIME = 0.36f;
 
            NailSlash Slash = HeroController.instance.normalSlash;
 
            NailSlash AltSlash = HeroController.instance.alternateSlash;
 
            NailSlash SlashUp = HeroController.instance.upSlash;
 
            NailSlash SlashDown = HeroController.instance.downSlash;
 
            if (counter == 0)
            {
 
                defaultSlashScale = Slash.scale;
 
                defaultAltScale = AltSlash.scale;
 
                defaultUpScale = SlashUp.scale;
 
                defaultDownScale = SlashDown.scale;
            }
            counter++;
            Slash.scale = defaultSlashScale;
 
            AltSlash.scale = defaultAltScale;
 
            SlashUp.scale = defaultUpScale;
 
            SlashDown.scale = defaultDownScale;
 
            HeroController.instance.ATTACK_DURATION_CH = 0.25f;
 
            HeroController.instance.ATTACK_COOLDOWN_TIME_CH = 0.25f;
 
            if (PlayerData.instance.GetBool("equippedCharm_13") && PlayerData.instance.GetBool("equippedCharm_18"))
            {
                PlayerData.instance.overcharmed = true;
 
                Slash.scale = UnityEngine.Vector3.Scale(Slash.scale, new UnityEngine.Vector3(1.6f, 0.35f, 1));
 
                AltSlash.scale = UnityEngine.Vector3.Scale(AltSlash.scale, new UnityEngine.Vector3(1.6f, 0.35f, 1));
 
                SlashUp.scale = UnityEngine.Vector3.Scale(SlashUp.scale, new UnityEngine.Vector3(0.35f, 1.6f, 1));
 
                SlashDown.scale = UnityEngine.Vector3.Scale(SlashDown.scale, new UnityEngine.Vector3(0.35f, 1.6f, 1));
            }
 
            else if (PlayerData.instance.GetBool("equippedCharm_13"))
            {
                PlayerData.instance.overcharmed = false;
 
                Slash.scale = UnityEngine.Vector3.Scale(Slash.scale, new UnityEngine.Vector3(1.4f, 0.56f, 1));
 
                AltSlash.scale = UnityEngine.Vector3.Scale(AltSlash.scale, new UnityEngine.Vector3(1.4f, 0.56f, 1));
 
                SlashUp.scale = UnityEngine.Vector3.Scale(SlashUp.scale, new UnityEngine.Vector3(0.56f, 1.4f, 1));
 
                SlashDown.scale = UnityEngine.Vector3.Scale(SlashDown.scale, new UnityEngine.Vector3(0.56f, 1.4f, 1));
            }
 
            else if (PlayerData.instance.GetBool("equippedCharm_18"))
            {
                PlayerData.instance.overcharmed = false;
 
                Slash.scale = UnityEngine.Vector3.Scale(Slash.scale, new UnityEngine.Vector3(1.3f, 0.73f, 1));
 
                AltSlash.scale = UnityEngine.Vector3.Scale(AltSlash.scale, new UnityEngine.Vector3(1.3f, 0.73f, 1));
 
                SlashUp.scale = UnityEngine.Vector3.Scale(SlashUp.scale, new UnityEngine.Vector3(0.73f, 1.3f, 1));
 
                SlashDown.scale = UnityEngine.Vector3.Scale(SlashDown.scale, new UnityEngine.Vector3(0.73f, 1.3f, 1));
            }
            else if (!PlayerData.instance.GetBool("equippedCharm_13") && !PlayerData.instance.GetBool("equippedCharm_18"))
            {
                PlayerData.instance.SetInt("nailDamage", defaultNailDamage);
            }
            if (PlayerData.instance.GetBool("equippedCharm_32"))
            {
                HeroController.instance.ATTACK_DURATION_CH = 0.15f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME_CH = 0.05f;
            }
            else if (!PlayerData.instance.GetBool("equippedCharm_32"))
            {
                HeroController.instance.ATTACK_DURATION_CH = 0.41f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME_CH = 0.36f;
            }
        }
 
        private void OnAfterAttack(AttackDirection dir)
        {
            if (Punishment != 0)
            {
                Punishment = 0;
 
            }
            if (PlayerData.instance.GetBool("equippedCharm_6"))
            {
                Punishment += 1;
            }
            if (PlayerData.instance.GetBool("equippedCharm_25"))
            {
                Punishment += 1;
            }
            if (PlayerData.instance.GetBool("equippedCharm_13"))
            {
                Punishment += 1;
            }
            if (PlayerData.instance.GetBool("equippedCharm_18"))
            {
                Punishment += 1;
            }
            if (PlayerData.instance.GetBool("equippedCharm_32"))
            {
                Punishment += 1;
            }
            if (Punishment == 4)
            {
                PlayerData.instance.overcharmed = true;
               
                HeroController.instance.ATTACK_DURATION = 0.51f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME = 0.45f;
 
                HeroController.instance.ATTACK_DURATION_CH = 0.25f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME_CH = 0.2f;
            }
            if (Punishment >= 6)
            {
                PlayerData.instance.overcharmed = true;
 
                PlayerData.instance.SetInt("nailDamage",
 
                        1);
 
                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
 
                HeroController.instance.ATTACK_DURATION = 0.82f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME = 0.36f;
 
                HeroController.instance.ATTACK_DURATION_CH = 0.75f;
 
                HeroController.instance.ATTACK_COOLDOWN_TIME_CH = 0.36f;
 
            }
        }
    }
}
