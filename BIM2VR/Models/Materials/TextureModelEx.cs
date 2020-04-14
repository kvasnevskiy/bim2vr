using System;
using HelixToolkit.Wpf.SharpDX;

namespace BIM2VR.Models.Materials
{
    public class TextureModelEx
    {
        public Guid Id { get; set; }
        public string TexturePath { get; set; }
        public TextureModel Texture { get; set; }

        public TextureModelEx(TextureModel texture, string texturePath)
        {
            Id = Guid.NewGuid();
            Texture = texture;
            TexturePath = texturePath;
        }

        public static implicit operator TextureModel(TextureModelEx textureModelEx)
        {
            return textureModelEx.Texture;
        }
    }
}
