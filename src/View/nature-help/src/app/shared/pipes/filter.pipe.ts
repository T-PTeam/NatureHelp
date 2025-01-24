import { IWDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { ISDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: false,
})
export class FilterPipe implements PipeTransform {

  transform(value: IWDeficiency[], searchValue: string): IWDeficiency[] {
    if (!searchValue) return value;

    return value.filter((v: IWDeficiency) =>
      v.title.toLowerCase().indexOf(searchValue.toLowerCase()) > -1)
  }

}
