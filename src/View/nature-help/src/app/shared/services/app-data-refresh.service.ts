import { Injectable } from "@angular/core";

import { LabsAPIService } from "@/modules/laboratories/services/labs-api.service";
import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";

@Injectable({
  providedIn: "root",
})
export class AppDataRefreshService {
  constructor(
    private waterAPIService: WaterAPIService,
    private soilAPIService: SoilAPIService,
    private labsAPIService: LabsAPIService,
  ) {}

  refreshAll(): void {
    this.waterAPIService.reloadCurrentData();
    this.soilAPIService.reloadCurrentData();
    this.labsAPIService.reloadCurrentData();
  }
}
