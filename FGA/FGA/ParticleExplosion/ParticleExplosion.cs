﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA
{
    class ParticleExplosion
    {
        //Particle arrays and vertex buffer
        VertexPositionTexture[] verts;
        Vector3[] vertexDirectionArray;
        Color[] vertexColorArray;
        VertexBuffer particleVertexBuffer;

        //Position
        Vector3 position;

        //Life
        int lifeLeft;

        //Rounds and particle counts
        int numParticlesPerRound;
        int maxParticles;
        static Random rnd = new Random();
        int roundTime;
        int timeSinceLastRound = 0;

        //Vertex and graphics info
        GraphicsDevice graphicsDevice;

        //Settings
        ParticleSettings particleSettings;

        //Effect
        Effect particleEffect;

        //Textures
        Texture2D particleColorsTexture;

        //Array indices
        int endOfLiveParticlesIndex = 0;
        int endOfDeadParticlesIndex = 0;

        //Drawing settings
        private Matrix view;
        private Matrix projection;

        public bool IsDead
        {
            get { return endOfDeadParticlesIndex == maxParticles; }
        }

        public ParticleExplosion(GraphicsDevice graphicsDevice, Vector3 position,
            int lifeLeft, int roundTime, int numParticlesPerRound, int maxParticles,
            Texture2D particleColorsTexture, ParticleSettings particleSettings,
            Effect particleEffect)
        {
            this.position = position;
            this.lifeLeft = lifeLeft;
            this.numParticlesPerRound = numParticlesPerRound;
            this.maxParticles = maxParticles;
            this.roundTime = roundTime;
            this.graphicsDevice = graphicsDevice;
            this.particleSettings = particleSettings;
            this.particleEffect = particleEffect;
            this.particleColorsTexture = particleColorsTexture;

            view = Matrix.CreateLookAt(new Vector3(0, 0, 50), Vector3.Zero, Vector3.Up);

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)graphicsDevice.Viewport.Width /
                (float)graphicsDevice.Viewport.Height,
                1, 3000);

            InitializeParticleVertices();
        }

        private void InitializeParticleVertices()
        {
            //Instantiate all particle arrays
            verts = new VertexPositionTexture[maxParticles * 4];
            vertexDirectionArray = new Vector3[maxParticles];
            vertexColorArray = new Color[maxParticles];

            //Get color data from colors texture
            Color[] colors = new Color[particleColorsTexture.Width *
                particleColorsTexture.Height];
            particleColorsTexture.GetData(colors);

            //Loop until max particles
            for (int i = 0; i < maxParticles; i++)
            {
                float size = (float)rnd.NextDouble() * particleSettings.maxSize;

                //Set position, direction and size of particle
                verts[i * 4] = new VertexPositionTexture(position, new Vector2(0, 0));
                verts[(i * 4) + 1] = new VertexPositionTexture(new Vector3(position.X,
                    position.Y + size, position.Z), new Vector2(0, 1));
                verts[(i * 4) + 2] = new VertexPositionTexture(new Vector3(position.X + size,
                    position.Y, position.Z), new Vector2(1, 0));
                verts[(i * 4) + 3] = new VertexPositionTexture(new Vector3(position.X + size,
                    position.Y + size, position.Z), new Vector2(1, 1));

                //Create a random velocity/direction
                Vector3 direction = new Vector3(
                    (float)rnd.NextDouble() * 2 - 1,
                    (float)rnd.NextDouble() * 2 - 1,
                    (float)rnd.NextDouble() * 2 - 1);
                direction.Normalize();

                //Multiply by Next double to make sure that
                //all particles move at random speed
                direction *= (float)rnd.NextDouble();

                //Set direction of particle
                vertexDirectionArray[i] = direction;

                //Set color of particle by getting a random color from the texture
                vertexColorArray[i] = colors[(
                    rnd.Next(0, particleColorsTexture.Height) * particleColorsTexture.Width) +
                    rnd.Next(0, particleColorsTexture.Width)];
            }

            //Instantiate vertex buffer
            particleVertexBuffer = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
        }

        public void Update(GameTime gameTime)
        {
            //Decrement life left until it's gone
            if (lifeLeft > 0)
                lifeLeft -= gameTime.ElapsedGameTime.Milliseconds;

            //Time for new round?
            timeSinceLastRound += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastRound > roundTime)
            {
                //New round - add and remove particles
                timeSinceLastRound -= roundTime;

                //Incremend end of live particles index each
                //round until end of list is reached
                if (endOfLiveParticlesIndex < maxParticles)
                {
                    endOfLiveParticlesIndex += numParticlesPerRound;
                    if (endOfLiveParticlesIndex > maxParticles)
                        endOfLiveParticlesIndex = maxParticles;
                }
                if (lifeLeft <= 0)
                {
                    //Increment end of dead particles each
                    //round until of of list is reached
                    if (endOfDeadParticlesIndex < maxParticles)
                    {
                        endOfDeadParticlesIndex += numParticlesPerRound;
                        if (endOfDeadParticlesIndex > maxParticles)
                            endOfDeadParticlesIndex = maxParticles;
                    }
                }
            }

            //Update positions of all live particles
            for (int i = endOfDeadParticlesIndex;
                i < endOfLiveParticlesIndex; i++)
            {
                verts[i * 4].Position += vertexDirectionArray[i];
                verts[(i * 4) + 1].Position += vertexDirectionArray[i];
                verts[(i * 4) + 2].Position += vertexDirectionArray[i];
                verts[(i * 4) + 3].Position += vertexDirectionArray[i];
            }
        }

        public void Draw()
        {
            graphicsDevice.SetVertexBuffer(particleVertexBuffer);

            //Only draw if there are live particles
            if (endOfLiveParticlesIndex - endOfDeadParticlesIndex > 0)
            {
                for (int i = endOfDeadParticlesIndex; i < endOfLiveParticlesIndex; i++)
                {
                    particleEffect.Parameters["WorldViewProjection"].SetValue(
                        view * projection);
                    particleEffect.Parameters["particleColor"].SetValue(
                        vertexColorArray[i].ToVector4());

                    //Draw particles
                    foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                            PrimitiveType.TriangleStrip, verts, i * 4, 2);
                    }
                }
            }
        }
    }
}
