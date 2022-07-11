using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Redemption.Globals;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Redemption.Backgrounds
{
	public class SpaceBgStyle : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
        public override int ChooseFarTexture()
        {
            return -1;
        }
        public override int ChooseMiddleTexture()
        {
            return -1;
        }
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return -1;
        }
    }
}