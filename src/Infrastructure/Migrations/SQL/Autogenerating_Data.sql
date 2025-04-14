START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b1111111-1111-1111-1111-111111111111', 'Addis Ababa', 'Ethiopia', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.63465Z', 50.450099999999999, 30.523399999999999, 10.0);
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b2222222-2222-2222-2222-222222222222', 'Mumbai', 'India', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634713Z', 49.8429, 24.031600000000001, 10.0);
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b3333333-3333-3333-3333-333333333333', 'Phoenix', 'USA', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634714Z', 46.482500000000002, 30.732600000000001, 10.0);
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b4444444-4444-4444-4444-444444444444', 'Sydney', 'Australia', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634714Z', 50.450099999999999, 30.523399999999999, 10.0);
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b5555555-5555-5555-5555-555555555555', 'Beijing', 'China', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634714Z', 49.993499999999997, 36.229199999999999, 10.0);
    INSERT INTO "NatLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "Latitude", "Longitude", "RadiusAffected")
    VALUES ('b6666666-6666-6666-6666-666666666666', 'Uzhhorod', 'Ukraine', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634716Z', 48.464700000000001, 35.0456, 10.0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('11111111-1111-1111-1111-111111111111', 'Kyiv', 'Ukraine', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.48066Z', 'Shevchenkivsky', 'Kyiv');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('22222222-2222-2222-2222-222222222222', 'New York', 'USA', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480752Z', 'Manhattan', 'New York');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('33333333-3333-3333-3333-333333333333', 'Berlin', 'Germany', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480753Z', 'Mitte', 'Berlin');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('44444444-4444-4444-4444-444444444444', 'Kyiv', 'Ukraine', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480753Z', 'Kyiv City District', 'Kyiv Oblast');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('55555555-5555-5555-5555-555555555555', 'New York', 'USA', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480753Z', 'Manhattan District', 'New York State');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('66666666-6666-6666-6666-666666666666', 'Berlin', 'Germany', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480753Z', 'Mitte District', 'Berlin');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('77777777-7777-7777-7777-777777777777', 'Rio de Janeiro', 'Brazil', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480754Z', 'Central District', 'Rio de Janeiro State');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('88888888-8888-8888-8888-888888888888', 'Paris', 'France', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480754Z', 'Paris City District', 'Île-de-France');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('99999999-9999-9999-9999-999999999999', 'Tokyo', 'Japan', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480754Z', 'Tokyo Metropolis District', 'Kantō');
    INSERT INTO "OrgLocations" ("Id", "City", "Country", "CreatedBy", "CreatedOn", "District", "Region")
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Rome', 'Italy', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480755Z', 'Rome City District', 'Lazio');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "Laboratories" ("Id", "CreatedBy", "CreatedOn", "LocationId", "Title")
    VALUES ('dddddddd-dddd-dddd-dddd-dddddddddddd', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480866Z', '11111111-1111-1111-1111-111111111111', 'Biomedical Research Lab');
    INSERT INTO "Laboratories" ("Id", "CreatedBy", "CreatedOn", "LocationId", "Title")
    VALUES ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.4809Z', '33333333-3333-3333-3333-333333333333', 'AI and Machine Learning Lab');
    INSERT INTO "Laboratories" ("Id", "CreatedBy", "CreatedOn", "LocationId", "Title")
    VALUES ('ffffffff-ffff-ffff-ffff-ffffffffffff', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480901Z', '22222222-2222-2222-2222-222222222222', 'Genetics and Biotechnology Lab');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "Organizations" ("Id", "CreatedBy", "CreatedOn", "LocationId", "Title")
    VALUES ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480802Z', '11111111-1111-1111-1111-111111111111', 'Global Research Institute');
    INSERT INTO "Organizations" ("Id", "CreatedBy", "CreatedOn", "LocationId", "Title")
    VALUES ('cccccccc-cccc-cccc-cccc-cccccccccccc', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480841Z', '33333333-3333-3333-3333-333333333333', 'International Tech Hub');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "Users" ("Id", "AccessToken", "AccessTokenExpireTime", "AddressId", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "LaboratoryId", "LastName", "OrganizationId", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime", "Role")
    VALUES ('11112222-3333-4444-5555-666677778888', NULL, NULL, '44444444-4444-4444-4444-444444444444', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.480964Z', TIMESTAMPTZ '1985-05-19T21:00:00Z', 'valentyn@example.com', 'Valentyn', 'dddddddd-dddd-dddd-dddd-dddddddddddd', 'Riabinchak', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'AQAAAAIAAYagAAAAECguO79y3aAyVPpzpWncaB4IYu9PYjpnVFccaS8craV/lS2/wsFIdGgP3zt57jcgng==', '+380501234567', NULL, NULL, 3);
    INSERT INTO "Users" ("Id", "AccessToken", "AccessTokenExpireTime", "AddressId", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "LaboratoryId", "LastName", "OrganizationId", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime", "Role")
    VALUES ('11223344-5566-7788-99aa-bbccddeeff00', NULL, NULL, '66666666-6666-6666-6666-666666666666', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.585563Z', TIMESTAMPTZ '1980-03-09T22:00:00Z', 'igor@example.com', 'Igor', 'ffffffff-ffff-ffff-ffff-ffffffffffff', 'Zaitsev', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 'AQAAAAIAAYagAAAAEKxFyghqrxHSumgKLFEzw7dG6LzDHXmxeuQErcXaVxRD8l7pFWl/gJI94vUXdtBUHw==', '+49 17612345678', NULL, NULL, 3);
    INSERT INTO "Users" ("Id", "AccessToken", "AccessTokenExpireTime", "AddressId", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "LaboratoryId", "LastName", "OrganizationId", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpireTime", "Role")
    VALUES ('99990000-aaaa-bbbb-cccc-ddddeeeeffff', NULL, NULL, '55555555-5555-5555-5555-555555555555', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.536467Z', TIMESTAMPTZ '1990-07-14T21:00:00Z', 'igorzayets@example.com', 'Valentyn', 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Riabinchak', 'cccccccc-cccc-cccc-cccc-cccccccccccc', 'AQAAAAIAAYagAAAAEAvDOvE1RJIgnTiRC1b1t8ovIg71oxhDmkd+tdUk85PBDMsoLY1lk5hiNFi2OI54yw==', '+380631234567', NULL, NULL, 3);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "Reports" ("Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic")
    VALUES ('a1111111-1111-1111-1111-111111111111', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634506Z', 'Genetic research data goes here...', '11112222-3333-4444-5555-666677778888', 'Annual Genetic Study', 1);
    INSERT INTO "Reports" ("Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic")
    VALUES ('a2222222-2222-2222-2222-222222222222', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634614Z', 'Performance analysis data goes here...', '99990000-aaaa-bbbb-cccc-ddddeeeeffff', 'AI Algorithm Performance', 1);
    INSERT INTO "Reports" ("Id", "CreatedBy", "CreatedOn", "Data", "ReporterId", "Title", "Topic")
    VALUES ('a3333333-3333-3333-3333-333333333333', '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634615Z', 'Pandemic analysis data goes here...', '11223344-5566-7788-99aa-bbccddeeff00', 'Global Pandemic Analysis', 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "SoilDeficiencies" ("Id", "AnalysisDate", "CadmiumConcentration", "CreatedBy", "CreatedOn", "CreatorId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "ResponsibleUserId", "Title", "Type")
    VALUES ('d1111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2025-01-14T22:00:00Z', 1.2, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.635059Z', '99990000-aaaa-bbbb-cccc-ddddeeeeffff', '', 0, 0.69999999999999996, 120.0, 150.0, 'b4444444-4444-4444-4444-444444444444', 0.59999999999999998, 3200.0, 45.0, 3.7999999999999998, 6.5, 0.80000000000000004, '11223344-5566-7788-99aa-bbccddeeff00', 'First Soil def', 0);
    INSERT INTO "SoilDeficiencies" ("Id", "AnalysisDate", "CadmiumConcentration", "CreatedBy", "CreatedOn", "CreatorId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "ResponsibleUserId", "Title", "Type")
    VALUES ('d2222222-2222-2222-2222-222222222222', TIMESTAMPTZ '2025-01-17T22:00:00Z', 2.5, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.635279Z', '99990000-aaaa-bbbb-cccc-ddddeeeeffff', '', 2, 0.90000000000000002, 200.0, 250.0, 'b5555555-5555-5555-5555-555555555555', 1.1000000000000001, 1500.0, 60.0, 2.5, 5.9000000000000004, 1.5, '11223344-5566-7788-99aa-bbccddeeff00', 'Second Soil def', 0);
    INSERT INTO "SoilDeficiencies" ("Id", "AnalysisDate", "CadmiumConcentration", "CreatedBy", "CreatedOn", "CreatorId", "Description", "EDangerState", "ElectricalConductivity", "HeavyMetalsConcentration", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "NitratesConcentration", "OrganicMatter", "PH", "PesticidesContent", "ResponsibleUserId", "Title", "Type")
    VALUES ('d3333333-3333-3333-3333-333333333333', TIMESTAMPTZ '2025-01-19T22:00:00Z', 0.80000000000000004, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.63528Z', '99990000-aaaa-bbbb-cccc-ddddeeeeffff', '', 1, 0.5, 50.0, 80.0, 'b6666666-6666-6666-6666-666666666666', 0.29999999999999999, 4000.0, 30.0, 4.0999999999999996, 7.2000000000000002, 0.5, '11223344-5566-7788-99aa-bbccddeeff00', 'Third Soil def', 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "WaterDeficiencies" ("Id", "BiologicalOxygenDemand", "CadmiumConcentration", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "CreatorId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "Type")
    VALUES ('c1111111-1111-1111-1111-111111111111', 4.5, 0.029999999999999999, 0.0, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.634771Z', '11112222-3333-4444-5555-666677778888', '', 6.7999999999999998, 0, 1.2, 0.14999999999999999, 'b1111111-1111-1111-1111-111111111111', 0.02, 0.0, 1500.0, 20.0, 7.2000000000000002, 0.10000000000000001, 2.1000000000000001, 0.0, '11112222-3333-4444-5555-666677778888', 'First Water def', 500.0, 0);
    INSERT INTO "WaterDeficiencies" ("Id", "BiologicalOxygenDemand", "CadmiumConcentration", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "CreatorId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "Type")
    VALUES ('c2222222-2222-2222-2222-222222222222', 8.0, 0.14999999999999999, 0.0, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.635025Z', '11223344-5566-7788-99aa-bbccddeeff00', '', 4.0, 2, 2.5, 0.5, 'b2222222-2222-2222-2222-222222222222', 0.10000000000000001, 0.0, 4000.0, 50.0, 6.5, 0.80000000000000004, 5.5, 0.0, '99990000-aaaa-bbbb-cccc-ddddeeeeffff', 'Second Water def', 800.0, 0);
    INSERT INTO "WaterDeficiencies" ("Id", "BiologicalOxygenDemand", "CadmiumConcentration", "ChemicalOxygenDemand", "CreatedBy", "CreatedOn", "CreatorId", "Description", "DissolvedOxygen", "EDangerState", "ElectricalConductivity", "LeadConcentration", "LocationId", "MercuryConcentration", "MicrobialActivity", "MicrobialLoad", "NitrateConcentration", "PH", "PesticidesContent", "PhosphateConcentration", "RadiationLevel", "ResponsibleUserId", "Title", "TotalDissolvedSolids", "Type")
    VALUES ('c3333333-3333-3333-3333-333333333333', 2.0, 0.01, 0.0, '00000000-0000-0000-0000-000000000000', TIMESTAMPTZ '2025-03-19T09:06:35.635026Z', '99990000-aaaa-bbbb-cccc-ddddeeeeffff', '', 7.5, 1, 0.90000000000000002, 0.050000000000000003, 'b3333333-3333-3333-3333-333333333333', 0.0050000000000000001, 0.0, 800.0, 10.0, 8.0, 0.050000000000000003, 1.0, 0.0, '11223344-5566-7788-99aa-bbccddeeff00', 'Third Water def', 350.0, 0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250319090636_Autogenerating_Data') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250319090636_Autogenerating_Data', '9.0.3');
    END IF;
END $EF$;
COMMIT;

