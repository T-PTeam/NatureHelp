import { RoleStringPipe } from "./role-string.pipe";
import { RoleCodeStrings } from "../constants/user-constants";

describe("RoleStringPipe", () => {
  let pipe: RoleStringPipe;

  beforeEach(() => {
    pipe = new RoleStringPipe();
  });

  it("should return correct role names", () => {
    expect(pipe.transform(1)).to.equal(RoleCodeStrings[1]);
    expect(pipe.transform(2)).to.equal(RoleCodeStrings[2]);
    expect(pipe.transform(3)).to.equal(RoleCodeStrings[3]);
    expect(pipe.transform(4)).to.equal(RoleCodeStrings[4]);
  });

  it("should return Guest for unknown role codes", () => {
    expect(pipe.transform(99)).to.equal("Guest");
    expect(pipe.transform(-1)).to.equal("Guest");
  });
});
