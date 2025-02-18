import { EDangerState } from '@/models/enums';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToString',
  standalone: false,
})
export class EnumToStringPipe implements PipeTransform {
  transform(value: any, type: string): string {
    if (type === typeof EDangerState){
      return EDangerState[value];
    }

    return value;
  }
}
