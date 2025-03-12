import { SoilDeficiencyList } from "@/modules/soil-deficiency/components/soil-deficiency-list/soil-deficiency-list.component";
import { mount } from "cypress/angular";

describe("LabsTableComponent", () => {
    it("mounts with component", () => {
        mount(SoilDeficiencyList);
    });
});
