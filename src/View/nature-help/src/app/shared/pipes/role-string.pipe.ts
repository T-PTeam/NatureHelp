import { Pipe, PipeTransform } from '@angular/core';
import { RoleCodeStrings } from '../constants/user-constants';

@Pipe({
  name: 'roleString',
  standalone: false,
})
export class RoleStringPipe implements PipeTransform {

  transform(roleCode: number): string {
    switch(roleCode){
      case 0: return RoleCodeStrings[0];
      case 1: return RoleCodeStrings[1];
      case 2: return RoleCodeStrings[2];
      case 3: return RoleCodeStrings[3];
      case 4: return RoleCodeStrings[4];
      default: return "Guest";
    }
  }

}
