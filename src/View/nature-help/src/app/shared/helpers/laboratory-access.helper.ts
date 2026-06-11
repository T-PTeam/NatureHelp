import { ILaboratory } from "@/modules/laboratories/models/ILaboratory";

export function canEditLaboratory(
  lab: Pick<ILaboratory, "id" | "createdBy"> & { createdById?: string },
  userId: string | null,
  role?: string | null,
  laboratoryId?: string | null,
): boolean {
  if (!userId) {
    return false;
  }

  if (role === "superadmin") {
    return true;
  }

  const userLaboratoryId = laboratoryId ?? sessionStorage.getItem("laboratoryId");
  if (userLaboratoryId && userLaboratoryId === lab.id) {
    return true;
  }

  const createdByRaw = lab.createdBy;
  const createdById = typeof createdByRaw === "string" ? createdByRaw : createdByRaw?.id || lab.createdById || "";

  return createdById === userId;
}
