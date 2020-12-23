export default class Helper{
  public static getTimeStatus(time: Date): string{
    const now = new Date();
    // @ts-ignore
    let elapsed = now - time;
    elapsed = Math.round(elapsed / 1000 / 60);
    if (elapsed < 5) {
      return 'только что';
    }

    if (elapsed < 1440) {
      return 'сегодня';
    }

    if (elapsed < 2880) {
      return 'Вчера';
    }

    return '';
  }

  public static guidEmpty(): string{
    return '00000000-0000-0000-0000-000000000000';
  }
}
