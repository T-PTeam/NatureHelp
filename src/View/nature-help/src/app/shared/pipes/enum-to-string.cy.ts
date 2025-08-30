import { EnumToStringPipe } from "./enum-to-string.pipe";
import { EDangerState, EResearchType } from "@/models/enums";

describe("EnumToStringPipe", () => {
  let pipe: EnumToStringPipe;

  beforeEach(() => {
    pipe = new EnumToStringPipe();
  });

  it("should transform EDangerState correctly", () => {
    expect(pipe.transform(0, "EDangerState")).to.equal(EDangerState[0]);
    expect(pipe.transform(1, "EDangerState")).to.equal(EDangerState[1]);
  });

  it("should transform ResearchType correctly", () => {
    expect(pipe.transform("FieldStudy", "ResearchType")).to.equal("Field Study");
    expect(pipe.transform("LabExperiment", "ResearchType")).to.equal("Lab Experiment");
  });

  it("should return original value if type is unknown", () => {
    expect(pipe.transform("Unknown", "InvalidType")).to.equal("Unknown");
  });
});
