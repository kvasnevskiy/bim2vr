using System;
using BIM2VR.Models.Materials;

namespace BIM2VR.Models.Visualizer.Data.TextureSource
{
    public class TextureSourceData
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public TextureDetailsData TextureDetails { get; set; }

        public TextureSourceData(TextureModelEx textureModelEx)
        {
            Id = textureModelEx.Id;
            Path = textureModelEx.TexturePath;
            TextureDetails = new TextureDetailsData();
        }
    }
}
