import { mount } from "cypress/angular";

import { LabsTableComponent } from "./labs-table.component";

describe("LabsTableComponent", () => {
    it("mounts with component", () => {
        mount(LabsTableComponent);
    });
});
