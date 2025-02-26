/* Original source Farseer Physics Engine:
 * Copyright (c) 2014 Ian Qvist, http://farseerphysics.codeplex.com
 * Microsoft Permissive License (Ms-PL) v1.1
 */

/*
* Farseer Physics Engine:
* Copyright (c) 2012 Ian Qvist
* 
* Original source Box2D:
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Samples.Testbed.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace tainicom.Aether.Physics2D.Samples.Testbed.Tests
{
    public class DistanceTest : Test
    {
        private float _angleB;
        private PolygonShape _polygonA;
        private PolygonShape _polygonB;
        private Vector2 _positionB = Vector2.Zero;
        private Transform _transformA;
        private Transform _transformB;

        private DistanceTest()
        {
            {
                _transformA = Transform.Identity;
                _transformA.p = new Vector2(0.0f, -0.2f);
                Vertices vertices = PolygonTools.CreateRectangle(10.0f, 0.2f);
                _polygonA = new PolygonShape(vertices, 0);
            }

            {
                _positionB = new Vector2(12.017401f, 0.13678508f);
                _angleB = -0.0109265f;
                _transformB = new Transform(_positionB, _angleB);

                Vertices vertices = PolygonTools.CreateRectangle(2.0f, 0.1f);
                _polygonB = new PolygonShape(vertices, 0);
            }
        }

        public override void Update(GameSettings settings, GameTime gameTime)
        {
            base.Update(settings, gameTime);

            DistanceInput input;
            input.ProxyA = new DistanceProxy(_polygonA, 0);
            input.ProxyB = new DistanceProxy(_polygonB, 0);
            input.TransformA = _transformA;
            input.TransformB = _transformB;
            input.UseRadii = true;
            SimplexCache cache;
            cache.Count = 0;
            DistanceOutput output;
            Distance.ComputeDistance(out output, out cache, input);

            DrawString("Distance = " + output.Distance);
            DrawString("Iterations = " + output.Iterations);
            
            DebugView.BeginCustomDraw(ref GameInstance.Projection, ref GameInstance.View);
            {
                Color color = new Color(0.9f, 0.9f, 0.9f);
                Vector2[] v = new Vector2[Settings.MaxPolygonVertices];
                for (int i = 0; i < _polygonA.Vertices.Count; ++i)
                {
                    v[i] = Transform.Multiply(_polygonA.Vertices[i], ref _transformA);
                }
                DebugView.DrawPolygon(v, _polygonA.Vertices.Count, color);

                for (int i = 0; i < _polygonB.Vertices.Count; ++i)
                {
                    v[i] = Transform.Multiply(_polygonB.Vertices[i], ref _transformB);
                }
                DebugView.DrawPolygon(v, _polygonB.Vertices.Count, color);
            }

            Vector2 x1 = output.PointA;
            Vector2 x2 = output.PointB;

            DebugView.DrawPoint(x1, 0.5f, new Color(1.0f, 0.0f, 0.0f));
            DebugView.DrawPoint(x2, 0.5f, new Color(1.0f, 0.0f, 0.0f));

            DebugView.DrawSegment(x1, x2, new Color(1.0f, 1.0f, 0.0f));
            DebugView.EndCustomDraw();
        }

        public override void Keyboard(InputState input)
        {
            if (input.IsKeyPressed(Keys.A))
                _positionB.X -= 0.1f;
            if (input.IsKeyPressed(Keys.D))
                _positionB.X += 0.1f;
            if (input.IsKeyPressed(Keys.S))
                _positionB.Y -= 0.1f;
            if (input.IsKeyPressed(Keys.W))
                _positionB.Y += 0.1f;
            if (input.IsKeyPressed(Keys.Q))
                _angleB += 0.1f * MathHelper.Pi;
            if (input.IsKeyPressed(Keys.E))
                _angleB -= 0.1f * MathHelper.Pi;

            _transformB = new Transform(_positionB, _angleB);
            
            base.Keyboard(input);
        }

        internal static Test Create()
        {
            return new DistanceTest();
        }
    }
}