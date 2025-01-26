import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: false,
})
export class FilterPipe implements PipeTransform {

  transform(value: IWaterDeficiency[], searchValue: string): IWaterDeficiency[] {
    if (!searchValue) return value;

    return value.filter((v: IWaterDeficiency) =>
      v.title.toLowerCase().indexOf(searchValue.toLowerCase()) > -1)
  }

}
