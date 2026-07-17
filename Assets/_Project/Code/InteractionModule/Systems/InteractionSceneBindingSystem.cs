using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InteractionModule.Components;
using ECSMiniRPG.InteractionModule.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.InteractionModule.Systems
{
    public struct InteractionSceneBindingSystem : ISystem
    {
        public void Init()
        {
            var views = Object.FindObjectsByType<InteractableView>(FindObjectsSortMode.None);

            for (var i = 0; i < views.Length; i++)
            {
                CreateEntity(views[i]);
            }
        }

        private void CreateEntity(InteractableView view)
        {
            var entity = GameWorld.NewEntity<Default>();

            entity.Set(
                new InteractionViewRef
                {
                    _value = view
                },
                new LongActionTransformRef
                {
                    _value = view.transform
                }
            );
            entity.Set<InteractableTag>();
        }
    }
}
