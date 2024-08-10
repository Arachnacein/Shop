using System.ComponentModel;
using System.Runtime.Serialization;


namespace WarehouseManager.Models
{
    public enum UnitEnum
    {
        [Description("Litr")]
        Liter = 1,
        [Description("Kilogram")]
        Kg = 2,
        [Description("Metr")]
        meter = 3,
        [Description("Sztuka")]
        piece = 4
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