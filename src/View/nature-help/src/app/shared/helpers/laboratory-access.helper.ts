import { ILaboratory } from "@/modules/laboratories/models/ILaboratory";

export function canEditLaboratory(
  lab: Pick<ILaboratory, "id" | "createdBy"> & { createdById?: string },
  userId: string | null,
  role?: string | null,
  laboratoryIds?: string[] | null,
): boolean {
  if (!userId) {
    return false;
  }

  if (role === "superadmin") {
    return true;
  }

  const storedLaboratoryIds = sessionStorage.getItem("laboratoryIds");
  const parsedLaboratoryIds = storedLaboratoryIds ? (JSON.parse(storedLaboratoryIds) as string[]) : [];
  const primaryLaboratoryId = sessionStorage.getItem("laboratoryId");
  const accessibleLaboratoryIds = laboratoryIds ?? [
    ...parsedLaboratoryIds,
    ...(primaryLaboratoryId ? [primaryLaboratoryId] : []),
  ];

  if (accessibleLaboratoryIds.includes(lab.id)) {
    return true;
  }

  const createdByRaw = lab.createdBy;
  const createdById = typeof createdByRaw === "string" ? createdByRaw : createdByRaw?.id || lab.createdById || "";

  return createdById === userId;
}
