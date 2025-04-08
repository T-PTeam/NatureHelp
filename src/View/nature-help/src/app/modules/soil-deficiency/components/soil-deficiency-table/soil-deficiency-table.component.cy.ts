import { SoilDeficiencyTable } from "@/modules/soil-deficiency/components/soil-deficiency-table/soil-deficiency-table.component";
import { mount } from "cypress/angular";

describe("LabsTableComponent", () => {
  it("mounts with component", () => {
    mount(SoilDeficiencyTable);
  });
});
