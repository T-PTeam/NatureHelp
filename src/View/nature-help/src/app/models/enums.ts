export enum EDeficiencyType {
    Water,
    Soil,
}

export enum EDangerState {
    Moderate,
    Dangerous,
    Critical,
}

export enum EAuthType {
    Login = "login",
    Register = "register",
    AddNewToOrganization = "add-new-to-org",
    AddMultipleToOrganization = "add-multiple-to-org",
}

export enum ERole {
    SuperAdmin,
    Owner,
    Manager,
    Supervisor,
    Researcher,
}

export enum ResearchType
{
    SoilChemicalAnalysis, // Хімічний аналіз ґрунту
    SoilProfileStudy, // Вивчення ґрунтових профілів
    GeochemicalResearch, // Геохімічні дослідження ґрунту
    WaterPhysicalChemicalAnalysis, // Фізико-хімічний аналіз води
    WaterBiologicalMonitoring, // Біологічний моніторинг води
    HydromorphologicalAnalysis // Гідроморфологічний аналіз води
}