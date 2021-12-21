using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace DAL.Enums.Internal
{
    public enum Devices
    {
        [Description("Mobile Utm")]
        Mobile,
        [Description("Client Utm")]
        Web
    }
}