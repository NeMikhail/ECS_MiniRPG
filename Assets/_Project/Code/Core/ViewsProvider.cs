using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ECSMiniRPG.Core
{
    public sealed class ViewsProvider : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<IView> _views = new List<IView>();

        public TView GetView<TView>() where TView : class, IView
        {
            TView view = null;

            if (!TryGetView(out view))
            {
                Debug.LogError($"View of type '{typeof(TView).Name}' is not assigned in ViewsProvider.");
            }

            return view;
        }

        public bool TryGetView<TView>(out TView view) where TView : class, IView
        {
            var hasView = false;
            view = null;

            for (var i = 0; i < _views.Count; i++)
            {
                if (_views[i] is TView typedView)
                {
                    view = typedView;
                    hasView = true;
                }
            }

            return hasView;
        }
    }
}
