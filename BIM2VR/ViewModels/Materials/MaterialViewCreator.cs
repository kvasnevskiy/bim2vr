using System;
using BIM2VR.Models.Materials;

namespace BIM2VR.ViewModels.Materials
{
    public static class MaterialViewCreator
    {
        public static MaterialViewModel Create(MaterialModel model)
        {
            try
            {
                var baseViewModelType = typeof(MaterialViewModel);
                var viewModelType = Type.GetType($"{baseViewModelType.Namespace}.{model.Type}MaterialViewModel");

                if (viewModelType != null)
                {
                    var viewModelConstructor = viewModelType.GetConstructor(new[] { typeof(MaterialModel) });
                    if (viewModelConstructor != null)
                    {
                        return viewModelConstructor.Invoke(new object[] { model }) as MaterialViewModel;
                    }
                    else
                    {
                        throw new InvalidOperationException("Can't find viewModelConstructor!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Can't find viewModelType!");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static MaterialViewModel ChangeType(MaterialViewModel currentViewModel, MaterialType newType)
        {
            try
            {
                var baseModelType = typeof(MaterialModel);
                var modelType = Type.GetType($"{baseModelType.Namespace}.{newType}MaterialModel");

                if (modelType != null)
                {
                    var modelConstructor = modelType.GetConstructor(new[] { typeof(Guid), typeof(string) });
                    if (modelConstructor != null)
                    {
                        var model = modelConstructor.Invoke(new object[] { currentViewModel.Model.Id, currentViewModel.Name }) as MaterialModel;
                        var setOwnersMethod = modelType.GetMethod("SetOwners");
                        setOwnersMethod?.Invoke(model, new object[] { currentViewModel.Model.Owners });

                        var baseViewModelType = typeof(MaterialViewModel);
                        var viewModel = Create(model);
                        var isChangedProperty = baseViewModelType.GetProperty("IsChanged");
                        isChangedProperty?.SetValue(viewModel, true);
                        return viewModel;
                    }
                    else
                    {
                        throw new InvalidOperationException("Can't find modelConstructor!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Can't find modelType!");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
