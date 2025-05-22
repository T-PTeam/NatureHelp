describe("Leaflet Map Markers", () => {
  it("should display at least one marker on the map", () => {
    cy.visit("/");

    cy.get(".leaflet-container", { timeout: 5000 }).should("exist");

    cy.get("path.leaflet-interactive").should("have.length.at.least", 1);
  });
});
