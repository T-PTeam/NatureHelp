using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Nature;
public class WaterDeficiency : Deficiency
{
    /// <summary>
    /// Рівень кислотності води (pH).
    /// </summary>
    /// <example>7.2</example>
    [Range(0, 14, ErrorMessage = "pH повинен бути в діапазоні від 0 до 14.")]
    public double PH { get; set; }

    /// <summary>
    /// Концентрація розчиненого кисню у воді (мг/л).
    /// </summary>
    /// <example>8.5</example>
    [Range(0, 20, ErrorMessage = "Розчинений кисень повинен бути в діапазоні від 0 до 20 мг/л.")]
    public double DissolvedOxygen { get; set; }

    /// <summary>
    /// Концентрація свинцю у воді (мг/л).
    /// </summary>
    /// <example>0.01</example>
    [Range(0, 0.01, ErrorMessage = "Концентрація свинцю повинна бути в межах 0 - 0.01 мг/л.")]
    public double LeadConcentration { get; set; }

    /// <summary>
    /// Концентрація ртуті у воді (мг/л).
    /// </summary>
    /// <example>0.0005</example>
    [Range(0, 0.001, ErrorMessage = "Концентрація ртуті повинна бути в межах 0 - 0.001 мг/л.")]
    public double MercuryConcentration { get; set; }

    /// <summary>
    /// Концентрація нітратів у воді (мг/л).
    /// </summary>
    /// <example>45</example>
    [Range(0, 50, ErrorMessage = "Концентрація нітратів повинна бути в межах 0 - 50 мг/л.")]
    public double NitrateConcentration { get; set; }

    /// <summary>
    /// Концентрація пестицидів у воді (мг/л).
    /// </summary>
    /// <example>0.002</example>
    [Range(0, 0.005, ErrorMessage = "Концентрація пестицидів повинна бути в межах 0 - 0.005 мг/л.")]
    public double PesticidesContent { get; set; }

    /// <summary>
    /// Кількість мікроорганізмів у воді (КУО/мл).
    /// </summary>
    /// <example>500</example>
    [Range(0, 1000, ErrorMessage = "Мікробна активність повинна бути в межах 0 - 1000 КУО/мл.")]
    public double MicrobialActivity { get; set; }

    /// <summary>
    /// Рівень радіації у воді (Bk/l).
    /// </summary>
    /// <example>3</example>
    [Range(0, 10, ErrorMessage = "Рівень радіації повинен бути в межах 0 - 10 Bk/l.")]
    public double RadiationLevel { get; set; }

    /// <summary>
    /// Хімічне споживання кисню (ХСК) (мг/л).
    /// </summary>
    /// <example>20</example>
    [Range(0, 1000, ErrorMessage = "ХСК повинен бути в межах 0 - 1000 мг/л.")]
    public double ChemicalOxygenDemand { get; set; }

    /// <summary>
    /// Біохімічне споживання кисню (БСК) (мг/л).
    /// </summary>
    /// <example>5</example>
    [Range(0, 50, ErrorMessage = "БСК повинен бути в межах 0 - 50 мг/л.")]
    public double BiologicalOxygenDemand { get; set; }

    /// <summary>
    /// Концентрація фосфатів у воді (мг/л).
    /// </summary>
    /// <example>0.5</example>
    [Range(0, 2, ErrorMessage = "Концентрація фосфатів повинна бути в межах 0 - 2 мг/л.")]
    public double PhosphateConcentration { get; set; }

    /// <summary>
    /// Концентрація кадмію у воді (мг/л).
    /// </summary>
    /// <example>0.001</example>
    [Range(0, 0.005, ErrorMessage = "Концентрація кадмію повинна бути в межах 0 - 0.005 мг/л.")]
    public double CadmiumConcentration { get; set; }

    /// <summary>
    /// Загальні розчинені тверді речовини у воді (мг/л).
    /// </summary>
    /// <example>400</example>
    [Range(0, 1500, ErrorMessage = "Загальні розчинені тверді речовини повинні бути в межах 0 - 1500 мг/л.")]
    public double TotalDissolvedSolids { get; set; }

    /// <summary>
    /// Електрична провідність води (мкСм/см).
    /// </summary>
    /// <example>500</example>
    [Range(0, 2500, ErrorMessage = "Електропровідність повинна бути в межах 0 - 2500 мкСм/см.")]
    public double ElectricalConductivity { get; set; }

    /// <summary>
    /// Мікробне навантаження у воді (КУО/мл).
    /// </summary>
    /// <example>1500</example>
    [Range(0, 2000, ErrorMessage = "Мікробне навантаження повинно бути в межах 0 - 2000 КУО/мл.")]
    public double MicrobialLoad { get; set; }
}
