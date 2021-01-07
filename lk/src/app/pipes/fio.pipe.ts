import { Pipe, PipeTransform } from '@angular/core';
import Fio from "../../abstracts/Fio";
import User from "../contracts/user";

@Pipe({
  name: 'userFio'
})
export class FioPipe implements PipeTransform {

  transform(fio: User): string {
    return `${fio.lastName} ${fio.firstName} ${fio.middleName}`;
  }

}
