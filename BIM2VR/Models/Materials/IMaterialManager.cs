using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace BIM2VR.Models.Materials
{
    public interface IMaterialManager
    {
        Dictionary<Guid, MaterialModel> Materials { get; }
        MaterialModel this[Guid id] { get; }
        void Add(MaterialModel material);
        void Replace(MaterialModel materialOld, MaterialModel materialNew);
        bool IsExist(MaterialModel material);
        bool IsExist(Guid id);
        void Clear();
    }
}
