using System.ComponentModel;
using System.Runtime.Serialization;

namespace UI.Enums
{
    public enum UnitEnum
    {
        [Description("Litr")]
        Liter = 1,
        [Description("Kilogram")]
        Kg = 2,
        [Description("Metr")]
        meter = 3
    }

    public enum ProductTypeEnum
    {
        [Description("Ciekły")]
        Liquid = 1,
        [Description("Sypki")]
        Loose = 2,
        [Description("Stały")]
        Solid = 3
    }
}