using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Nature;
public class WaterDeficiency : Deficiency
{
    /// <summary>
    /// Рівень кислотності води (pH).
    /// </summary>
    /// <example>7.2</example>
    public double PH { get; set; }

    /// <summary>
    /// Концентрація розчиненого кисню у воді (мг/л).
    /// </summary>
    /// <example>8.5</example>
    public double DissolvedOxygen { get; set; }

    /// <summary>
    /// Концентрація свинцю у воді (мг/л).
    /// </summary>
    /// <example>0.01</example>
    public double LeadConcentration { get; set; }

    /// <summary>
    /// Концентрація ртуті у воді (мг/л).
    /// </summary>
    /// <example>0.0005</example>
    public double MercuryConcentration { get; set; }

    /// <summary>
    /// Концентрація нітратів у воді (мг/л).
    /// </summary>
    /// <example>45</example>
    public double NitrateConcentration { get; set; }

    /// <summary>
    /// Концентрація пестицидів у воді (мг/л).
    /// </summary>
    /// <example>0.002</example>
    public double PesticidesContent { get; set; }

    /// <summary>
    /// Кількість мікроорганізмів у воді (КУО/мл).
    /// </summary>
    /// <example>500</example>
    public double MicrobialActivity { get; set; }

    /// <summary>
    /// Рівень радіації у воді (Bk/l).
    /// </summary>
    /// <example>3</example>
    public double RadiationLevel { get; set; }

    /// <summary>
    /// Хімічне споживання кисню (ХСК) (мг/л).
    /// </summary>
    /// <example>20</example>
    public double ChemicalOxygenDemand { get; set; }

    /// <summary>
    /// Біохімічне споживання кисню (БСК) (мг/л).
    /// </summary>
    /// <example>5</example>
    public double BiologicalOxygenDemand { get; set; }

    /// <summary>
    /// Концентрація фосфатів у воді (мг/л).
    /// </summary>
    /// <example>0.5</example>
    public double PhosphateConcentration { get; set; }

    /// <summary>
    /// Концентрація кадмію у воді (мг/л).
    /// </summary>
    /// <example>0.001</example>
    public double CadmiumConcentration { get; set; }

    /// <summary>
    /// Загальні розчинені тверді речовини у воді (мг/л).
    /// </summary>
    /// <example>400</example>
    public double TotalDissolvedSolids { get; set; }

    /// <summary>
    /// Електрична провідність води (мкСм/см).
    /// </summary>
    /// <example>500</example>
    public double ElectricalConductivity { get; set; }

    /// <summary>
    /// Мікробне навантаження у воді (КУО/мл).
    /// </summary>
    /// <example>1500</example>
    public double MicrobialLoad { get; set; }

    [ForeignKey(nameof(Creator))]
    public Guid CreatorId { get; set; }

    [ForeignKey(nameof(ResponsibleUser))]
    public Guid? ResponsibleUserId { get; set; }

    [ForeignKey(nameof(Location))]
    public Guid LocationId { get; set; }
}
