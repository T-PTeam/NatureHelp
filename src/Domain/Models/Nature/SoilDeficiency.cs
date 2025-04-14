namespace Domain.Models.Nature;

public class SoilDeficiency : Deficiency
{
    /// <summary>
    /// Рівень кислотності ґрунту (pH).
    /// </summary>
    /// <example>6.8</example>
    public double PH { get; set; }

    /// <summary>
    /// Вміст органічних речовин у ґрунті (%).
    /// </summary>
    /// <example>3.5</example>
    public double OrganicMatter { get; set; }

    /// <summary>
    /// Концентрація свинцю в ґрунті (мг/кг).
    /// </summary>
    /// <example>200</example>
    public double LeadConcentration { get; set; }

    /// <summary>
    /// Концентрація кадмію в ґрунті (мг/кг).
    /// </summary>
    /// <example>2.5</example>
    public double CadmiumConcentration { get; set; }

    /// <summary>
    /// Концентрація ртуті в ґрунті (мг/кг).
    /// </summary>
    /// <example>0.8</example>
    public double MercuryConcentration { get; set; }

    /// <summary>
    /// Концентрація пестицидів у ґрунті (мг/кг).
    /// </summary>
    /// <example>1.2</example>
    public double PesticidesContent { get; set; }

    /// <summary>
    /// Вміст нітратів у ґрунті (мг/кг).
    /// </summary>
    /// <example>50</example>
    public double NitratesConcentration { get; set; }

    /// <summary>
    /// Концентрація важких металів (мідь, цинк тощо) у ґрунті (мг/кг).
    /// </summary>
    /// <example>150</example>
    public double HeavyMetalsConcentration { get; set; }

    /// <summary>
    /// Електропровідність ґрунту (мСм/см), яка вказує на рівень солоності.
    /// </summary>
    /// <example>0.75</example>
    public double ElectricalConductivity { get; set; }

    /// <summary>
    /// Мікробіологічна активність у ґрунті (КУО/г).
    /// </summary>
    /// <example>3000</example>
    public double MicrobialActivity { get; set; }

    /// <summary>
    /// Дата проведення аналізу ґрунту.
    /// </summary>
    /// <example>2025-01-24</example>
    public DateTime AnalysisDate { get; set; }
}
