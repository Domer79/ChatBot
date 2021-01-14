import {Pipe, PipeTransform} from '@angular/core';
import Helper from '../misc/Helper';
import {TimeStatus} from '../misc/message-type';

@Pipe({
  name: 'chatDate'
})
export class ChatDatePipe implements PipeTransform {

  transform(value: Date): string {
    const valueDate = new Date(value);
    if (Helper.getTimeStatus(valueDate) === TimeStatus.None){
      debugger;
      return `${this.getTimeStatus(valueDate)} ${valueDate.toLocaleDateString('ru-Ru')}`;
    }

    return `${this.getTimeStatus(valueDate)} ${valueDate.toLocaleTimeString('ru-Ru')}`;
  }

  public getTimeStatus(value: Date): string{
    let statusName = '';
    switch (Helper.getTimeStatus(value)) {
      case TimeStatus.None:
        return statusName = '';
      case TimeStatus.JustNow:
        return statusName = 'только что';
      case TimeStatus.Today:
        return statusName = 'сегодня';
      case TimeStatus.Yesterday:
        return statusName = 'вчера';
    }
  }

}
