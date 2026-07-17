using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Tooling
{
    public struct InventoryToolCache : IComponent
    {
        public HashSet<ToolTypeConfig> _toolTypes;

        public bool HasTool(ToolTypeConfig toolType)
        {
            var hasTool = toolType == null;

            if (!hasTool && _toolTypes != null)
            {
                hasTool = _toolTypes.Contains(toolType);
            }

            return hasTool;
        }
    }
}
