using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Redemption.Base;
using Redemption.Buffs.Debuffs;
using Redemption.Buffs.NPCBuffs;
using Redemption.Globals;
using Redemption.Globals.NPC;
using Redemption.Items.Placeable.Banners;
using Redemption.Items.Placeable.Tiles;
using Redemption.Projectiles.Hostile;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Redemption.BaseExtension;

namespace Redemption.NPCs.PreHM
{
    public class AncientGladestoneGolem : ModNPC
    {
        public enum ActionState
        {
            Idle,
            Wander,
            Threatened,
            PillarAttack,
            PillarJump
        }

        public ActionState AIState
        {
            get => (ActionState)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }

        public ref float AITimer => ref NPC.ai[1];

        public ref float TimerRand => ref NPC.ai[2];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;

            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    ModContent.BuffType<InfestedDebuff>(),
                    ModContent.BuffType<NecroticGougeDebuff>(),
                    ModContent.BuffType<ViralityDebuff>(),
                    ModContent.BuffType<DirtyWoundDebuff>()
                }
            });

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Velocity = 1f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 80;
            NPC.damage = 35;
            NPC.friendly = false;
            NPC.defense = 20;
            NPC.lifeMax = 125;
            NPC.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.value = 5000;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AncientGladestoneGolemBanner>();
            NPC.RedemptionGuard().GuardPoints = NPC.lifeMax / 4;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                for (int i = 0; i < 10; i++)
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Stone, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
                for (int i = 0; i < 8; i++)
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Redemption/AncientGladestoneGolemGore" + (i + 1)).Type, 1);
            }
            Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Stone, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
            if (AIState is ActionState.Idle or ActionState.Wander)
            {
                SoundEngine.PlaySound(SoundID.Zombie63, NPC.position);
                AITimer = 0;
                AIState = ActionState.Threatened;
            }
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            bool vDmg = false;
            if (NPC.RedemptionGuard().GuardPoints >= 0)
            {
                NPC.RedemptionGuard().GuardHit(NPC, ref vDmg, ref damage, ref knockback, SoundID.DD2_WitherBeastCrystalImpact, .1f);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, (float)damage, knockback, hitDirection, 0, 0, 0);
                if (NPC.RedemptionGuard().GuardPoints >= 0)
                    return vDmg;
            }
            NPC.RedemptionGuard().GuardBreakCheck(NPC, DustID.Stone, CustomSounds.GuardBreak, 20, 2, 10);

            damage *= 2;
            return true;
        }
        public override bool? CanFallThroughPlatforms() => NPC.Redemption().fallDownPlatform;
        public NPC npcTarget;
        public Vector2 moveTo;
        public int runCooldown;
        public override void OnSpawn(IEntitySource source)
        {
            TimerRand = Main.rand.Next(120, 280);
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            RedeNPC globalNPC = NPC.Redemption();
            NPC.TargetClosest();
            NPC.LookByVelocity();

            switch (AIState)
            {
                case ActionState.Idle:
                    if (NPC.velocity.Y == 0)
                        NPC.velocity.X *= 0.5f;
                    AITimer++;
                    if (AITimer >= TimerRand)
                    {
                        moveTo = NPC.FindGround(15);
                        AITimer = 0;
                        TimerRand = Main.rand.Next(120, 260);
                        AIState = ActionState.Wander;
                    }

                    if (NPC.Sight(player, 800, true, true))
                    {
                        SoundEngine.PlaySound(SoundID.Zombie63, NPC.position);
                        globalNPC.attacker = player;
                        moveTo = NPC.FindGround(15);
                        AITimer = 0;
                        AIState = ActionState.Threatened;
                    }
                    if (NPC.lavaWet && Main.rand.NextBool(250) && NPC.velocity.Y == 0)
                    {
                        AITimer = 0;
                        NPC.frame.Y = 0;
                        AIState = ActionState.PillarJump;
                    }
                    break;

                case ActionState.Wander:
                    if (NPC.Sight(player, 800, true, true))
                    {
                        SoundEngine.PlaySound(SoundID.Zombie63, NPC.position);
                        globalNPC.attacker = player;
                        moveTo = NPC.FindGround(15);
                        AITimer = 0;
                        AIState = ActionState.Threatened;
                    }

                    AITimer++;
                    if (AITimer >= TimerRand || NPC.Center.X + 20 > moveTo.X * 16 && NPC.Center.X - 20 < moveTo.X * 16)
                    {
                        AITimer = 0;
                        TimerRand = Main.rand.Next(120, 280);
                        AIState = ActionState.Idle;
                    }

                    NPC.PlatformFallCheck(ref NPC.Redemption().fallDownPlatform, 30);
                    RedeHelper.HorizontallyMove(NPC, moveTo * 16, 0.1f, 1, 10, 2, NPC.Center.Y > player.Center.Y);
                    break;

                case ActionState.Threatened:
                    if (NPC.ThreatenedCheck(ref runCooldown))
                    {
                        runCooldown = 0;
                        AIState = ActionState.Wander;
                    }

                    if (!NPC.Sight(globalNPC.attacker, 800, false, true))
                        runCooldown++;
                    else if (runCooldown > 0)
                        runCooldown--;

                    NPC.PlatformFallCheck(ref NPC.Redemption().fallDownPlatform, 30);
                    RedeHelper.HorizontallyMove(NPC, globalNPC.attacker.Center, 0.1f, 3, 10, 1, NPC.Center.Y > globalNPC.attacker.Center.Y);

                    NPC.DamageHostileAttackers(0, 7);

                    if (Main.rand.NextBool(100) && NPC.velocity.Y == 0)
                    {
                        int tilePosY = BaseWorldGen.GetFirstTileFloor((int)globalNPC.attacker.Center.X / 16, (int)globalNPC.attacker.Center.Y / 16);
                        int dist = (tilePosY * 16) - (int)globalNPC.attacker.Center.Y;

                        NPC.frame.Y = 0;

                        if (NPC.DistanceSQ(globalNPC.attacker.Center) < 300 * 300 && dist < 140 && globalNPC.attacker.active)
                            AIState = ActionState.PillarAttack;
                        else
                            AIState = ActionState.PillarJump;
                    }
                    break;

                case ActionState.PillarAttack:
                    if (NPC.ThreatenedCheck(ref runCooldown))
                        AIState = ActionState.Wander;

                    NPC.velocity.X = 0;
                    break;

                case ActionState.PillarJump:
                    if (NPC.ThreatenedCheck(ref runCooldown))
                        AIState = ActionState.Wander;
                    break;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                RedeNPC globalNPC = NPC.Redemption();
                NPC.frame.Width = TextureAssets.Npc[NPC.type].Width() / 2;
                switch (AIState)
                {
                    case ActionState.PillarAttack:
                        NPC.rotation = NPC.velocity.X * 0.05f;
                        NPC.frame.X = NPC.frame.Width;
                        NPC.frameCounter++;
                        if (NPC.frameCounter > 5)
                        {
                            NPC.frame.Y += frameHeight;
                            NPC.frameCounter = 0;
                            if (NPC.frame.Y == frameHeight)
                            {
                                SoundEngine.PlaySound(SoundID.Zombie64, NPC.position);
                                int tilePosY = BaseWorldGen.GetFirstTileFloor((int)globalNPC.attacker.Center.X / 16, (int)globalNPC.attacker.Center.Y / 16);
                                NPC.Shoot(new Vector2(globalNPC.attacker.Center.X, (tilePosY * 16) + 55), ModContent.ProjectileType<AncientGladestonePillar>(), NPC.damage, Vector2.Zero, false, SoundID.Item1);
                            }
                            if (NPC.frame.Y == 7 * frameHeight)
                            {
                                Player player = Main.player[NPC.target];
                                SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                                Main.LocalPlayer.RedemptionScreen().ScreenShakeOrigin = NPC.Center;
                                Main.LocalPlayer.RedemptionScreen().ScreenShakeIntensity += 6;
                            }
                            if (NPC.frame.Y > 9 * frameHeight)
                                AIState = ActionState.Threatened;
                        }
                        return;
                    case ActionState.PillarJump:
                        NPC.rotation = NPC.velocity.X * 0.05f;
                        NPC.frame.X = NPC.frame.Width;
                        if (NPC.frame.Y < 6 * frameHeight) { NPC.velocity.X = 0; }
                        NPC.frameCounter++;
                        if (NPC.frameCounter > 5)
                        {
                            NPC.frame.Y += frameHeight;
                            NPC.frameCounter = 0;
                            if (NPC.frame.Y == frameHeight)
                            {
                                SoundEngine.PlaySound(SoundID.Zombie64, NPC.position);
                                int tilePosY = BaseWorldGen.GetFirstTileFloor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                                NPC.Shoot(new Vector2(NPC.Center.X, (tilePosY * 16) + 55), ModContent.ProjectileType<AncientGladestonePillar>(), NPC.damage, Vector2.Zero, false, SoundID.Item1);
                            }
                            if (NPC.frame.Y == 6 * frameHeight)
                                NPC.velocity.X += NPC.spriteDirection == 1 ? Main.rand.Next(2, 7) : Main.rand.Next(-7, -2);
                            if (NPC.frame.Y == 7 * frameHeight)
                            {
                                Main.LocalPlayer.RedemptionScreen().ScreenShakeOrigin = NPC.Center;
                                Main.LocalPlayer.RedemptionScreen().ScreenShakeIntensity += 6;
                            }
                            if (NPC.frame.Y > 9 * frameHeight)
                                AIState = ActionState.Threatened;
                        }
                        return;
                }
                NPC.frame.X = 0;
                if (NPC.collideY || NPC.velocity.Y == 0)
                {
                    NPC.rotation = 0;
                    if (NPC.velocity.X == 0)
                        NPC.frame.Y = 0;
                    else
                    {
                        NPC.frameCounter += NPC.velocity.X * 0.5f;
                        if (NPC.frameCounter is >= 3 or <= -3)
                        {
                            NPC.frameCounter = 0;
                            NPC.frame.Y += frameHeight;
                            if (NPC.frame.Y > 11 * frameHeight)
                                NPC.frame.Y = 0;
                        }
                    }
                }
                else
                {
                    NPC.rotation = NPC.velocity.X * 0.05f;
                    if (NPC.velocity.Y < 0)
                        NPC.frame.Y = 3 * frameHeight;
                    else
                        NPC.frame.Y = 10 * frameHeight;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GathicGladestone>(), 1, 4, 14));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientDirt>(), 1, 2, 8));
            npcLoot.Add(ItemDropRule.Common(ItemID.NightVisionHelmet, 30));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float baseChance = SpawnCondition.Cavern.Chance;
            float multiplier = TileLists.AncientTileArray.Contains(Framing.GetTileSafely(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY).TileType) ? .03f : 0.006f;

            return baseChance * multiplier;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                new FlavorTextBestiaryInfoElement(
                    "An ancient relic of the far past. These golems were once infused with a human's soul to come to life, now they roam aimlessly within ruined structures.")
            });
        }
    }
}