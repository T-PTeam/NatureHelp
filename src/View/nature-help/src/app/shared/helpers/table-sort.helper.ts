import { HttpParams } from "@angular/common/http";

import { ITableSort } from "../models/ITableSort";

export function toggleSort(current: ITableSort | null, field: string): ITableSort {
  if (current?.sortBy === field) {
    return {
      sortBy: field,
      sortDirection: current.sortDirection === "asc" ? "desc" : "asc",
    };
  }

  return { sortBy: field, sortDirection: "asc" };
}

export function sortIndicator(sort: ITableSort | null, field: string): string {
  if (sort?.sortBy !== field) {
    return "";
  }

  return sort.sortDirection === "asc" ? " ↑" : " ↓";
}

export function appendSortParams(params: HttpParams, sort: ITableSort | null): HttpParams {
  if (!sort?.sortBy) {
    return params;
  }

  return params.set("sortBy", sort.sortBy).set("sortDirection", sort.sortDirection);
}
