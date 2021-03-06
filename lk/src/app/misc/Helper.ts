import {TimeStatus} from "../../abstracts/message-type";
import {LinkType} from "../contracts/message-dialog";
import Message from "../../abstracts/message";
import {HttpResponse} from "@angular/common/http";

export default class Helper{
  public static getTimeStatus(time: Date): TimeStatus{
    const now = new Date();
    // @ts-ignore
    let elapsed = now - time;
    elapsed = Math.round(elapsed / 1000 / 60);
    if (elapsed < 5) {
      return TimeStatus.JustNow; // 'только что';
    }

    if (elapsed < 1440) {
      return TimeStatus.Today; // 'сегодня';
    }

    if (elapsed < 2880) {
      return TimeStatus.Yesterday; // 'Вчера';
    }

    return TimeStatus.None; // ''
  }

  public static guidEmpty(): string{
    return '00000000-0000-0000-0000-000000000000';
  }

  public static searchQueryFilter(query: string): string{
    let result: string = query;
    if (query.match(/(.*)(<br>)$/)){
      result = query.replace(/(.*)(<br>)$/, '$1');
    }

    return result;
  }

  public static existBr(str: string): any | null{
    return str.match(/(.*)(<br>)$/);
  }

  public static getLinkTypeDescriptions(): { value: LinkType, description: string }[] {
    const linkTypeDescriptions: { value: LinkType, description: string }[] = [];
    for(let value in LinkType){
      const status = Number.parseInt(value) as LinkType;
      if (isNaN(status)) continue;
      linkTypeDescriptions.push(this.getLinkTypeDescription(status));
    }

    return linkTypeDescriptions;
  }

  public static getLinkTypeDescription(value: LinkType): { value: LinkType, description: string } {
    const linkType = value * 1 as LinkType;
    switch(linkType){
      case LinkType.all: return { value: linkType, description: 'Все' };
      case LinkType.opened: return { value: linkType, description: 'Открыт' };
      case LinkType.offline: return { value: linkType, description: 'Офлайн' };
      case LinkType.closed: return { value: linkType, description: 'Закрыт' };
      case LinkType.worked: return { value: linkType, description: 'В работе' };
      case LinkType.rejected: return { value: linkType, description: 'Отклонен' };
      default: throw new Error(`Unknown link type ${value}`);
    }
  }

  public static objectIsEmpty(obj: any): boolean{
    if (!obj) return true;

    for(let property in obj){
      return false;
    }

    return true;
  }

  public static sortMessages(messages: Message[]): Message[]{
    return messages.sort((a:Message, b: Message) => {
      if (a.time < b.time)
        return -1;

      if (a.time > b.time)
        return 1;

      return 0;
    });
  }

  /**
   * https://stackoverflow.com/a/60455250
   * Method is use to download file.
   * @param fileName - File name
   * @param response - HttpResponse<Blob>
   * @param type - type of the document.
   */
  public static downLoadFile(fileName: string, response: HttpResponse<Blob>, type: string) {
    let binaryData = [];
    binaryData.push(response.body);
    let downloadLink = document.createElement('a');
    downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: 'blob' }));
    downloadLink.setAttribute('download', fileName);
    document.body.appendChild(downloadLink);
    downloadLink.click();  }
}
