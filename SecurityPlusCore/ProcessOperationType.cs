using System.ComponentModel;

namespace SecurityPlusCore
{
    public enum ProcessOperationType : byte
    {
        [Description("CREATE")]
        Create,

        [Description("CLOSE")]
        Close
    }
}
