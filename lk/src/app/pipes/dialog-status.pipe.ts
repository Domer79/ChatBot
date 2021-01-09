import {Pipe, PipeTransform} from '@angular/core';
import {DialogStatus} from "../contracts/message-dialog";

@Pipe({
  name: 'dialogStatus'
})
export class DialogStatusPipe implements PipeTransform {

  transform(value: DialogStatus, exportType?: string): string {
    switch (value){
      case DialogStatus.Started:
        return exportType == 'color' ? "active" : "Открыт";
      case DialogStatus.Active:
        return exportType == 'color' ? "active" : "В работе";
      case DialogStatus.Started | DialogStatus.Active:
        return exportType == 'color' ? "active" : "Открыт";
      case DialogStatus.Closed:
        return exportType == 'color' ? "inactive" : "Закрыт";
      case DialogStatus.Rejected:
        return exportType == 'color' ? "inactive" : "Отклонен";
      case DialogStatus.Sleep:
        return exportType == 'color' ? "offline" : "Оффлайн";
      default:
        throw new Error("Неизвестный статус диалога");
    }
  }

}
