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

using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Joints;
using tainicom.Aether.Physics2D.Samples.Testbed.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace tainicom.Aether.Physics2D.Samples.Testbed.Tests
{
    public class PrismaticTest : Test
    {
        private PrismaticJoint _joint;

        private PrismaticTest()
        {
            Body ground = World.CreateEdge(new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));

            PolygonShape shape = new PolygonShape(5);
            shape.Vertices = PolygonTools.CreateRectangle(2.0f, 0.5f);

            Body body = World.CreateBody();
            body.BodyType = BodyType.Dynamic;
            body.Position = new Vector2(-10.0f, 10.0f);
            body.Rotation = 0.5f * MathHelper.Pi;
            body.CreateFixture(shape);

            // Bouncy limit
            Vector2 axis = new Vector2(2.0f, 1.0f);
            axis.Normalize();
            _joint = new PrismaticJoint(ground, body, Vector2.Zero, Vector2.Zero, axis, true);

            // Non-bouncy limit
            //_joint = new PrismaticJoint(ground, body2, body2.Position, new Vector2(-10.0f, 10.0f), new Vector2(1.0f, 0.0f));

            _joint.MotorSpeed = 5.0f;
            _joint.MaxMotorForce = 1000.0f;
            _joint.MotorEnabled = true;
            _joint.LowerLimit = -10.0f;
            _joint.UpperLimit = 20.0f;
            _joint.LimitEnabled = true;

            World.Add(_joint);
        }

        public override void Keyboard(InputState input)
        {
            if (input.IsKeyPressed(Keys.L))
                _joint.LimitEnabled = !_joint.LimitEnabled;

            if (input.IsKeyPressed(Keys.M))
                _joint.MotorEnabled = !_joint.MotorEnabled;

            if (input.IsKeyPressed(Keys.S))
                _joint.MotorSpeed = -_joint.MotorSpeed;

            base.Keyboard(input);
        }

        public override void Update(GameSettings settings, GameTime gameTime)
        {
            base.Update(settings, gameTime);
            DrawString("Keys: (l) limits, (m) motors, (s) speed");
        }

        internal static Test Create()
        {
            return new PrismaticTest();
        }
    }
}