import {TimeStatus} from "../../abstracts/message-type";

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
}
