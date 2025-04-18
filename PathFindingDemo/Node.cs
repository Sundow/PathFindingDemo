namespace PathFindingDemo
{
    internal struct Node(int x, int y) : IEquatable<Node>
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public override readonly string ToString() => $"({X}, {Y})";

        public readonly bool Equals(Node other) => X == other.X && Y == other.Y;

        public static bool operator ==(Node left, Node right) => left.Equals(right);
        public static bool operator !=(Node left, Node right) => !left.Equals(right);

        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        public override readonly bool Equals(object? obj) => obj is Node node && Equals(node);
    }
}
