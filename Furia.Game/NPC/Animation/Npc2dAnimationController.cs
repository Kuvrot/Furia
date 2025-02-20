﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Vortice.Vulkan;
using Stride.Rendering.Sprites;
using Stride.Graphics;
using SharpDX.Direct3D11;

namespace Furia.NPC.Animation
{
    public class Npc2dAnimationController : SyncScript
    {
        public byte animationSpeed = 12; //Frames per second
        public byte idleStartFrame = 0, idleEndFrame = 0;
        public byte attackStartFrame = 0 , attackEndFrame = 0;
        public byte walkStartFrame = 0, walkEndFrame = 0;
        public byte deathStartFrame = 0, deathEndFrame = 0;

        private byte currentStartFrame = 0, currentEndFrame = 0;
        private float clock = 0;

        //Animation state machine
        /*
        -1 = death 
        0 = idle
        1 = walk
        2 = attack 
        */
        public sbyte animationState = 0;

        //Components
        private SpriteComponent spriteComponent;
        private SpriteFromSheet spriteSheet;
        private DateTime dateTime;

        public override void Start()
        {
            //This is how you are supposed to set sprite frames https://doc.stride3d.net/4.0/en/manual/sprites/use-sprites.html
            spriteComponent = Entity.Get<SpriteComponent>();
            spriteSheet = spriteComponent.SpriteProvider as SpriteFromSheet;
            dateTime = DateTime.Now;
        }

        public override void Update()
        {
            try
            {
                switch (animationState)
                {
                    default: PlayIdleAnimation(); break;
                    case -1: PlayDeathAnimation(); break;
                    case 0: PlayIdleAnimation(); break;
                    case 1: PlayWalkAnimation(); break;
                    case 2: PlayAttackAnimation(); break;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void PlayIdleAnimation()
        {
            currentStartFrame = idleStartFrame;
            currentEndFrame = idleEndFrame;
            PlayFrames();
        }

        private void PlayWalkAnimation()
        {
            currentStartFrame = walkStartFrame;
            currentEndFrame = walkEndFrame;
            PlayFrames();
        }

        private void PlayAttackAnimation()
        {
            currentStartFrame = attackStartFrame;
            currentEndFrame = attackEndFrame;
            PlayFrames();
        }

        private void PlayDeathAnimation() 
        {
            currentStartFrame = deathStartFrame;
            currentEndFrame = deathEndFrame;
            PlayFrames();
        }

        //Plays the frames
        public void PlayFrames ()
        {
            if (Counter())
            {
                if (spriteSheet.CurrentFrame < currentEndFrame)
                {
                    spriteSheet.CurrentFrame += 1;
                }
                else
                {
                    spriteSheet.CurrentFrame = currentStartFrame;
                }
            } 
        }

        public void SetAnimationState(sbyte state)
        {
            animationState = state;
        }

        public sbyte GetAnimationState()
        {
            return animationState;
        }

        private bool Counter ()
        {
            if (clock >=  animationSpeed * 0.01)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
