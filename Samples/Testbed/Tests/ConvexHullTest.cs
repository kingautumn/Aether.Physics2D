/* Original source Farseer Physics Engine:
 * Copyright (c) 2014 Ian Qvist, http://farseerphysics.codeplex.com
 * Microsoft Permissive License (Ms-PL) v1.1
 */

using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Samples.Testbed.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace tainicom.Aether.Physics2D.Samples.Testbed.Tests
{
    public class ConvexHullTest : Test
    {
        private int _count = Settings.MaxPolygonVertices;
        private Vector2[] _points = new Vector2[Settings.MaxPolygonVertices];
        private bool _auto;

        private ConvexHullTest()
        {
            Generate();
            _auto = false;
        }

        private void Generate()
        {
            Vector2 lowerBound = new Vector2(-8.0f, -8.0f);
            Vector2 upperBound = new Vector2(8.0f, 8.0f);

            for (int i = 0; i < _count; ++i)
            {
                float x = 10.0f * Rand.RandomFloat();
                float y = 10.0f * Rand.RandomFloat();

                // Clamp onto a square to help create collinearities.
                // This will stress the convex hull algorithm.
                Vector2 v = new Vector2(x, y);
                v = MathUtils.Clamp(v, lowerBound, upperBound);
                _points[i] = v;
            }
        }

        public override void Keyboard(InputState input)
        {
            if (input.IsKeyPressed(Keys.A))
                _auto = !_auto;

            if (input.IsKeyPressed(Keys.G))
                Generate();


            base.Keyboard(input);
        }

        public override void Update(GameSettings settings, GameTime gameTime)
        {
            base.Update(settings, gameTime);

            PolygonShape shape = new PolygonShape(new Vertices(_points), 0f);

            DrawString("Press g to generate a new random convex hull");

            DebugView.BeginCustomDraw(ref GameInstance.Projection, ref GameInstance.View);
            DebugView.DrawPolygon(shape.Vertices.ToArray(), shape.Vertices.Count, new Color(0.9f, 0.9f, 0.9f));

            for (int i = 0; i < _count; ++i)
            {
                DebugView.DrawPoint(_points[i], 0.1f, new Color(0.9f, 0.5f, 0.5f));
                Vector2 position = GameInstance.ConvertWorldToScreen(_points[i]);
                DebugView.DrawString((int)position.X, (int)position.Y, i.ToString());
            }

            DebugView.EndCustomDraw();

            if (_auto)
            {
                Generate();
            }
        }

        public static Test Create()
        {
            return new ConvexHullTest();
        }
    }
}