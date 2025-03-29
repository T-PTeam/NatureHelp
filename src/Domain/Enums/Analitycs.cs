namespace Domain.Enums;

public enum EReportTopic
{
    Water,
    Soil
}

public enum EFileExtension
{
    Word,
    Excel,
    Pdf
}

public enum ResearchType
{
    SoilChemicalAnalysis, // Хімічний аналіз ґрунту
    SoilProfileStudy, // Вивчення ґрунтових профілів
    GeochemicalResearch, // Геохімічні дослідження ґрунту
    WaterPhysicalChemicalAnalysis, // Фізико-хімічний аналіз води
    WaterBiologicalMonitoring, // Біологічний моніторинг води
    HydromorphologicalAnalysis // Гідроморфологічний аналіз води
}