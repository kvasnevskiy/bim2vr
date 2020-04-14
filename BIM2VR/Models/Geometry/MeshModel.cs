using System;
using System.Linq;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

namespace BIM2VR.Models.Geometry
{
    public class MeshModel
    {
        public double Width => Positions.Max(item => item.X) - Positions.Min(item => item.X);
        public double Depth => Positions.Max(item => item.Y) - Positions.Min(item => item.Y);
        public double Height => Positions.Max(item => item.Z) - Positions.Min(item => item.Z);

        public Vector3Collection Positions { get; set; }
        public Vector3Collection Normals { get; set; }
        public Vector2Collection TextureCoordinates { get; set; }
        public IntCollection TriangleIndices { get; set; }

        #region Constructors

        public MeshModel(Vector3Collection positions, Vector3Collection normals, IntCollection triangleIndices, Vector2Collection textureCoordinates)
        {
            Positions = positions;
            Normals = normals;
            TriangleIndices = triangleIndices;
            TextureCoordinates = textureCoordinates;
        }

        public MeshModel(Vector3Collection positions, Vector3Collection normals, IntCollection triangleIndices, bool calculateTextureCoordinates)
        {
            Positions = positions;
            Normals = normals;
            TriangleIndices = triangleIndices;

            if (calculateTextureCoordinates)
            {
                CalculateTextureCoordinates();
            }
        }

        #endregion

        protected void CalculateTextureCoordinates()
        {
            var uvCollection = new Vector2[Positions.Count];

            for (var i = 0; i < TriangleIndices.Count; i += 3)
            {
                var a = Positions[TriangleIndices[i]];
                var b = Positions[TriangleIndices[i + 1]];
                var c = Positions[TriangleIndices[i + 2]];
                var side1 = b - a;
                var side2 = c - a;
                var n = Vector3.Cross(side1, side2);
                n = new Vector3(Math.Abs(n.X), Math.Abs(n.Normalized().Y), Math.Abs(n.Normalized().Z));

                if (n.X > n.Y && n.X > n.Z)
                {
                    uvCollection[TriangleIndices[i]] = new Vector2(Positions[TriangleIndices[i]].Z, Positions[TriangleIndices[i]].Y);
                    uvCollection[TriangleIndices[i + 1]] = new Vector2(Positions[TriangleIndices[i + 1]].Z, Positions[TriangleIndices[i + 1]].Y);
                    uvCollection[TriangleIndices[i + 2]] = new Vector2(Positions[TriangleIndices[i + 2]].Z, Positions[TriangleIndices[i + 2]].Y);
                }
                else if (n.Y > n.X && n.Y > n.Z)
                {
                    uvCollection[TriangleIndices[i]] = new Vector2(Positions[TriangleIndices[i]].X, Positions[TriangleIndices[i]].Z);
                    uvCollection[TriangleIndices[i + 1]] = new Vector2(Positions[TriangleIndices[i + 1]].X, Positions[TriangleIndices[i + 1]].Z);
                    uvCollection[TriangleIndices[i + 2]] = new Vector2(Positions[TriangleIndices[i + 2]].X, Positions[TriangleIndices[i + 2]].Z);
                }
                else if (n.Z > n.X && n.Z > n.Y)
                {
                    uvCollection[TriangleIndices[i]] = new Vector2(Positions[TriangleIndices[i]].X, Positions[TriangleIndices[i]].Y);
                    uvCollection[TriangleIndices[i + 1]] = new Vector2(Positions[TriangleIndices[i + 1]].X, Positions[TriangleIndices[i + 1]].Y);
                    uvCollection[TriangleIndices[i + 2]] = new Vector2(Positions[TriangleIndices[i + 2]].X, Positions[TriangleIndices[i + 2]].Y);
                }
            }

            TextureCoordinates = new Vector2Collection(uvCollection);
        }
    }
}
