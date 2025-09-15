import { LabResearchersPipe } from "./lab-researchers.pipe";
import { IUser } from "@/models/IUser";

describe("LabResearchersPipe", () => {
  let pipe: LabResearchersPipe;
  const researchers: IUser[] = [
    {
      firstName: "Alice",
      lastName: "Johnson",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
    {
      firstName: "Bob",
      lastName: "Smith",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
    {
      firstName: "Charlie",
      lastName: "Brown",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
    {
      firstName: "David",
      lastName: "Clark",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
    {
      firstName: "Eve",
      lastName: "Davis",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
    {
      firstName: "Frank",
      lastName: "Wright",
      id: "",
      email: "",
      password: "",
      passwordHash: "",
      role: 0,
      organizationId: null,
    },
  ];

  beforeEach(() => {
    pipe = new LabResearchersPipe();
  });

  it("should return researchers as a formatted string", () => {
    expect(pipe.transform(researchers.slice(0, 3))).to.equal("Alice Johnson, Bob Smith, Charlie Brown");
  });

  it("should truncate list when more than 5 researchers", () => {
    expect(pipe.transform(researchers)).to.equal(
      "Alice Johnson, Bob Smith, Charlie Brown, David Clark, Eve Davis and other 1 researchers",
    );
  });

  it("should return an empty string for an empty array", () => {
    expect(pipe.transform([])).to.equal("");
  });
});
