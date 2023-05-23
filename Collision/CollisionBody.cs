using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AnotherLib.Collision
{
    public abstract class CollisionBody
    {
        public Vector2 position;
        public Rectangle hitbox;
        public Point hitboxOffset;
        public int hitboxWidth = 0;
        public int hitboxHeight = 0;
        public int bodyID = 0;      //All entities use this to store their proper dictionary IDs

        /// <summary>
        /// An array of what this object can collide with.
        /// </summary>
        public virtual CollisionType[] colliderTypes { get; }

        /// <summary>
        /// The type of the collision of this object.
        /// </summary>
        public virtual CollisionType collisionType { get; }

        /// <summary>
        /// Whether or not this object should draw under other draws.
        /// </summary>
        public virtual bool ForceDrawBottom { get; } = false;

        /// <summary>
        /// Whether or not this object should draw on top of all other draws.
        /// </summary>
        public virtual bool ForceDrawTop { get; } = false;

        public bool[] tileCollisionDirection = new bool[4];
        public readonly int CollisionDirection_Top = 0;        //To make it usable in all inheritants without needing to reference this class
        public readonly int CollisionDirection_Bottom = 1;
        public readonly int CollisionDirection_Left = 2;
        public readonly int CollisionDirection_Right = 3;

        public enum CollisionType
        {
            None,
            Player,
            Enemies,
            FriendlyProjectiles,
            EnemyProjectiles,
            MapObjects,
            Traps
        }

        public enum BodyType
        {
            Enemy,
            Projectile
        }


        public virtual void Initialize()
        { }

        public virtual void Update()
        { }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }

        /// <summary>
        /// Checks the collision types of this object to compare for collisionType.
        /// </summary>
        /// <param name="collisionType">The collision type to look for.</param>
        /// <returns>Whether or not this object can collide with a certain collision type.</returns>
        public bool CanCollideWith(CollisionType collisionType)
        {
            for (int c = 0; c < colliderTypes.Length; c++)
            {
                if (colliderTypes[c] == collisionType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Detects collisions between the object this method is called on and the objects in the list.
        /// </summary>
        /// <param name="possibleIntersectors">The list of collision bodies to compare against.</param>
        public void DetectCollisions(List<CollisionBody> possibleIntersectors)
        {
            CollisionBody[] possibleIntersectorsCopy = possibleIntersectors.ToArray();
            foreach (CollisionBody intersector in possibleIntersectorsCopy)
            {
                if (hitbox.Intersects(intersector.hitbox) && CanCollideWith(intersector.collisionType))
                {
                    HandleAnyCollision();
                    HandleCollisions(intersector, intersector.collisionType);
                    break;
                }
            }
        }

        /// <summary>
        /// A method that gets called whenever a collision happens.
        /// </summary>
        /// <param name="collider"> The collider. </param>
        /// <param name="colliderType"> The collision type of the collider. </param>
        public virtual void HandleCollisions(CollisionBody collider, CollisionType colliderType)
        { }

        /// <summary>
        /// Detects rectangle-rectangle collisions with the input rectangle and the rectangles of the input list.
        /// </summary>
        /// <param name="collidingRect">The rectangle to check collisions for.</param>
        /// <param name="possibleIntersectors">The list of collision bodies to compare against.</param>
        public void DetectRectCollision(Rectangle collidingRect, List<CollisionBody> possibleIntersectors)
        {
            CollisionBody[] possibleIntersectorsCopy = possibleIntersectors.ToArray();
            foreach (CollisionBody intersector in possibleIntersectorsCopy)
            {
                if (collidingRect.Intersects(intersector.hitbox) && CanCollideWith(intersector.collisionType))
                {
                    HandleAnyCollision();
                    HandleRectCollisions(intersector, intersector.collisionType);
                    break;
                }
            }
        }

        /// <summary>
        /// A method that gets called whenever any collision happens.
        /// </summary>
        /// <param name="collider"></param>
        public virtual void HandleAnyCollision()
        { }

        /// <summary>
        /// A method that gets called whenever a specfiic rectangle collision happens.
        /// </summary>
        /// <param name="collider"> The collider. </param>
        /// <param name="colliderType"> The collision type of the collider. </param>
        public virtual void HandleRectCollisions(CollisionBody collider, CollisionType colliderType)
        { }

        /// <summary>
        /// A method that gets called whenever a tile collision happens.
        /// </summary>
        public virtual void HandleTileCollision()
        { }
    }
}
