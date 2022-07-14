using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Redemption.NPCs.Space
{
    public class WallDatalog : ModNPC
    {
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
                2 => "[c/e0ffff:Data Log #933]\n" +
                    "[c/e0ffff:With the resources from Nabu III, I was able to construct a new cryo-chamber that was essentially a Sleep Mode with a timer. I set it to shut me down and power me back on in 3 days, however I have made a terrible discovery. I was still fully conscious, even with my body powered off, even with my mind having no energy to realistically function, I was still fully aware of my surroundings, in an infinite void of nothingness. I could not move my body. No way to repower myself, no way to escape the confines of the chamber, all I could do was wait 3 days. But as if it couldn't get any worse, the haunting visions of sleep paralysis began. I'm out of it now, but all I feel is hopelessness and overwhelming dread. Alas, I will persist.]",
                3 => "[c/e0ffff:Data Log #184999]\n" +
                    "[c/e0ffff:I can't walk straight. I can't talk properly. This phantom pain makes my body recoil, my voice shake in anguish. I can't continue on like this, at least not without assistance. Which is why I have been working on a predictive AI for myself. This AI should predict my movement and speech based on signals in my mind. This will allow me to continue moving and communicating verbally, but would not help my pain.]",
                4 => "[c/e0ffff:Data Log #18500]\n" +
                    "[c/e0ffff:I have incorporated the predictive AI into my body. The AI is inaccurate and unstable for now, but it uses the self-learning algorithm of an Android, and in due time, I will eventually appear to be cured of this pain from the outside. But on the inside, I remain uncured.]",
                5 => "[c/e0ffff:Data Log #2042280]\n" +
                    "[c/e0ffff:I made a rash decision that almost ended in my death. My robotic body had broken and in that moment, just for a minute, I had a state of pure lucidity. I could've sworn I had been cured, but it was short-lived and my mind fogged once more. I repaired my body and created a new one, named Prototype Goukisan.]",
                6 => "[c/e0ffff:Data Log #2042281]\n" +
                    "[c/e0ffff:I entered my new vessel, Prototype Goukisan, but I felt a sense of unfamiliarity with myself and quickly left it. My first vessel, Prototype Multium, has a strange sense of... nostalgia to it. Despite being inside it for what is basically my entire life, it reminds me of when I was still human, a feeling any other vessel lacked.]",
                7 => "[c/e0ffff:Data Log #5385431]\n" +
                    "[c/e0ffff:I entered the engine room to find a robot, just like myself. It had black plating and green highlights. I was about to shoot, but the robot spoke to me. This was the first time I understood another being's language without needing to translate it. His name was Xehito. He was a mercenary hired by the space pirates and had no care for their lives, so we exchanged a few words and are now - what would be considered - friends, I guess? I've never had a friend I didn't construct myself before. It's an interesting feeling.]",
                8 => "[c/e0ffff:Data Log #8022208]\n" +
                    "[c/e0ffff:An epiphany has struck me during my time observing the universe. I have a new goal in mind - a new purpose. But I must learn more about the universe before I can hope to achieve it. I will call this project: 'Operation Dusk's End'.]",
                9 => "[c/e0ffff:Data Log #9145620]\n" +
                    "[c/e0ffff:I discovered a new planet and it's been quickly made apparent that it has intelligent life. Very intelligent life. An intermission was broadcasted to the main room demanding the purpose of my sudden arrival. I will beam down to see the planet's leader. I've already scanned it beforehand, looks to be a spacefaring empire with many soldiers. This is certainly interesting.]",
                10 => "[c/e0ffff:Data Log #9145621]\n" +
                    "[c/e0ffff:Xehito stayed on the empire's planet to collect information while I headed off to explore a neighboring planet. It looked war-stricken. I stumbled upon a ruined city, flattened by high-power explosives. There is life here, humanoid beings with ragged clothing and depressed looks. When asked, they told me this destruction was the doing of the neighboring planet's empire. I roamed the wastelands, observed the near-dead residents, and heard their pleads for help. I have no reason to help them. They had done nothing for me. But, they had done nothing to deserve this either. I will decide my actions tomorrow.]",
                11 => "[c/e0ffff:Data Log #9145622]\n" +
                    "[c/e0ffff:I have ejected thousands of hologram drones across the planet's outer atmosphere and projected the image of the planet how it is now - barren and dull. I've sent Androids down to help clear rubble, landmines, undetonated warheads, and collect materials for my plans. The residents are suspicious of me. It's understandable though, beings from other planets have not been kind to them. I don't understand why I'm helping them, but it makes me feel something strange.]",
                12 => "[c/e0ffff:Data Log #9145629]\n" +
                    "[c/e0ffff:In 8 days, my army and I have rebuilt skyscrapers, planted seeds and saplings created from the remains of another planet, and collected ice from asteroids to pour into the oceans. The world feels alive once more, the residents of this planet must be relieved as they witness the rebirth of their home. The hologram drones are still projecting the image of the planet as it was when I got here, so any outsiders won't notice my deeds. My next goal will be the annihilation of the neighboring planet's empire.]",
                13 => "[c/e0ffff:Data Log #9145630]\n" +
                    "[c/e0ffff:I tested my new weaponry on the empire's planet. Xehito came back to the SoS so he wouldn't get hit. It's a dual-beam that shoots from both edges of my crescent moon-shaped spaceship. The planet was devastated, with the empire along with it. There were leftovers in the form of spaceships that were out of range of the planet's explosion, but Xehito and I made quick work of them. The retribution of the empire fills me with satisfaction, and during my time helping the other planet, my feelings of pain had dulled. But, now it returns once more.]",
                14 => "[c/e0ffff:Data Log #170001202]\n" +
                    "[c/e0ffff:Xehito had to leave for certain reasons, but whatever. I gave him a parting gift, it was a memory chip, one that held all my memories. He will find me again when the time was right.]",
                15 => "[c/e0ffff:Data Log #365000663]\n" +
                    "[c/e0ffff:I remain lost in space. However, I set up a system of scanners that are capable of collecting the data of every planet using signals that would travel indefinitely, in the hopes that I may recognize the data of one of them and use it to lead me into the right direction. It'll take many decades for the signals to travel across space, but in the long term, this might save me.]",
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