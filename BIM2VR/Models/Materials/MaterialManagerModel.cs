using System;
using System.Collections.Generic;

namespace BIM2VR.Models.Materials
{
    public class MaterialManagerModel : IMaterialManager
    {
        public Dictionary<Guid, MaterialModel> Materials { get; protected set; }

        public MaterialModel this[Guid id]
        {
            get
            {
                if (Materials.TryGetValue(id, out var material))
                {
                    return material;
                }

                return null;
            }
        }

        public void Replace(MaterialModel materialOld, MaterialModel materialNew)
        {
            if (IsExist(materialOld))
            {
                Materials[materialOld.Id] = materialNew;
            }
        }

        public bool IsExist(MaterialModel material)
        {
            return IsExist(material.Id);
        }

        public bool IsExist(Guid id)
        {
            return Materials.ContainsKey(id);
        }

        public void Clear()
        {
            Materials.Clear();
            Materials = new Dictionary<Guid, MaterialModel>();
        }

        public void Add(MaterialModel material)
        {
            if (!IsExist(material))
            {
                material.Name = $"Material-{Materials.Count + 1}";
                Materials.Add(material.Id, material);
            }
        }

        public MaterialManagerModel()
        {
            Materials = new Dictionary<Guid, MaterialModel>();
        }
    }
}
