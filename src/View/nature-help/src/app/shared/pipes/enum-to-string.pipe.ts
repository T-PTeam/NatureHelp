import { EDangerState } from '@/models/enums';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToString',
  standalone: false,
})
export class EnumToStringPipe implements PipeTransform {
  transform(value: EDangerState): string {
    return EDangerState[value];
  }
}
