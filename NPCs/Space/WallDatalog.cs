using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Redemption.NPCs.Space
{
    public class WallDatalog : ModNPC
    {
        public override string Texture => "Redemption/Items/Lore/Datalog";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData { ImmuneToAllBuffsThatAreNotWhips = true });
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 30;
            NPC.lifeMax = 1000;
            NPC.immortal = true;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
        }
        public override bool CheckActive() => false;
        public override bool CanChat() => true;
        public override string GetChat()
        {
            return NPC.ai[0] switch
            {
                1 => "[c/e0ffff:Data Log #466111]\n" +
                    "[c/e0ffff:Continuing with this memory chip experiment, I have removed chunks of memories from the chips and tested it on the Androids. All had the same effect, except one. The Android did not scream nor show signs of discomfort. Peculiar, for the only memories that I removed from the chip were the ones of me being human. Perhaps it's just a coincidence, but after numerous repeats of the experiment, the Android's pain is suggested to be directly tied to those memories. I considered removing those memories from myself, but something in me is against it. My instincts want me to remember.]",
                _ => "[c/e0ffff:Data Log #466110]\n" +
                    "[c/e0ffff:I have successfully created a memory chip to store all data my mind currently contains - not counting the data stored in the memory database of the SoS. I have experimented by injecting it into an empty Android. The idea of these chips is to allow me to construct new vessels for myself to occupy. When the Android was powered on, it screamed and flailed until I turned it back off. Very interesting, this suggests the cause of the phantom pain is within my mind directly. I will modify the chip and proceed with this experiment.]",
            };
        }
        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > 1 * frameHeight)
                    NPC.frame.Y = 0 * frameHeight;
            }
        }
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, NPC.Opacity * 0.2f, NPC.Opacity * 0.3f, NPC.Opacity * 0.6f);
        }
    }
}