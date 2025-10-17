using System;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public static class DirectionTool
    {
        public static Direction GetOpposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new NotImplementedException($"There is no case for Direction {direction}");
            }
        }

        public static bool IsSide(Direction dir1, Direction dir2)
        {
            if (dir1 == dir2 || dir1 == GetOpposite(dir2))
            {
                return false;
            }
            return true;
        }

        public static bool IsOpposite(Direction dir1, Direction dir2)
        {
            if(dir1 == GetOpposite(dir2))
            {
                return true;
            }

            return false;
        }

        public static Vector2Int DirectionToVector2(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Right:
                    return Vector2Int.right;
                default:
                    throw new NotImplementedException($"There is no case for Direction {direction}");
            }
        }

        public static Direction TurnLeft(Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                Direction.Down => Direction.Right,
                Direction.Right => Direction.Up,
                _ => dir
            };
        }

        public static Direction TurnRight(Direction dir)
        {
            return dir switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => dir
            };
        }
    }
}
