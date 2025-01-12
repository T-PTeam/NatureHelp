import { IPoint } from "./IPoint";
import { ProductType } from "./ProductType.enum";

export interface IGasStation {
  id: number,
  name: string,
  latitude: number,
  longitude: number
  description?: string,
  productType: ProductType,
  workTimeFrom: number,
  workTimeTo: number
}
