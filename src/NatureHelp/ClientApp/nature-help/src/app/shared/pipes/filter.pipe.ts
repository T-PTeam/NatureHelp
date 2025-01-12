import { Pipe, PipeTransform } from '@angular/core';
import { IGasStation } from '../../models/IGasStation';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value: IGasStation[], searchValue: string): IGasStation[] {
    if (!searchValue) return value;

    return value.filter((v: IGasStation) =>
      v.name.toLowerCase().indexOf(searchValue.toLowerCase()) > -1)
  }

}
