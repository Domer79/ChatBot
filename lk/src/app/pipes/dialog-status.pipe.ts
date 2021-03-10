import {Pipe, PipeTransform} from '@angular/core';
import MessageDialog, {DialogStatus} from "../contracts/message-dialog";

@Pipe({
  name: 'dialogStatus'
})
export class DialogStatusPipe implements PipeTransform {

  transform(value: MessageDialog, exportType?: string): string {
    let result = '';

    switch (value.dialogStatus){
      case DialogStatus.Started:
        result = exportType == 'color' ? "started" : "Открыт";
        break;
      case DialogStatus.Active:
        result = exportType == 'color' ? "active" : "В работе";
        break;
      case DialogStatus.Started | DialogStatus.Active:
        result = exportType == 'color' ? "active" : "Открыт";
        break;
      case DialogStatus.Closed:
        result = exportType == 'color' ? "inactive" : "Закрыт";
        break;
      case DialogStatus.Rejected:
        result = exportType == 'color' ? "inactive" : "Отклонен";
        break;
      case DialogStatus.Sleep:
        result = exportType == 'color' ? "inactive" : "Закрыт";
        break;
      default:
        throw new Error("Неизвестный статус диалога");
    }

    if (value.offline){
      if (exportType == 'color'){
        if (value.dialogStatus & (DialogStatus.Closed | DialogStatus.Rejected)){
          return 'inactive';
        }
        return 'offline';
      }
      else{
        result = `${result} Оффлайн`;
      }
    }

    return result;
  }

}
